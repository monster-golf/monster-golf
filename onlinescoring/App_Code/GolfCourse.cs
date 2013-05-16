using System;
using System.Xml;
using System.Data;

namespace MonsterGolfOnline
{
    /// <summary>
    /// Summary description for Course.
    /// </summary>
    public class GolfCourse
    {
        private string m_coursename = "";
        private int m_coursepar = 0;
        private int m_teeid = 0;
        private double m_rating = -1;
        private int m_slope = -1;
        private GolfHole[] m_holes = null;
        private System.Data.DataRow m_golfcourse = null;
        public int ID = -1;
        private int m_numberoftees = -1;
        private int[] m_rounds;
        private double[] m_teerating;
        private int[] m_teeslope;

        public GolfCourse()
        {
        }
        public GolfCourse(System.Data.DataRow GolfCourse, int TeeID, int NumberOfTees)
        {
            m_teeid = TeeID;
            m_numberoftees = NumberOfTees;
            Load(GolfCourse);
        }
        public string Name
        {
            get { return m_coursename; }
            set { m_coursename = value; }
        }
        public int Par
        {
            get { return m_coursepar; }
            set { m_coursepar = value; }
        }

        public double[] TeeRating
        {
            get { return m_teerating; }
            set { m_teerating = value; }
        }

        public int[] TeeSlope
        {
            get { return m_teeslope; }
            set { m_teeslope = value; }
        }

        public int TeeID
        {
            get { return m_teeid; }
            set { m_teeid = value; }
        }
        public double Rating
        {
            get { return m_rating; }
            set { m_rating = value; }
        }
        public int Slope
        {
            get { return m_slope; }
            set { m_slope = value; }
        }
        public int NumberOfTees
        {
            get { return m_numberoftees; }
            set { m_numberoftees = value; }
        }
        public GolfHole Hole(int currentHole)
        {
            return m_holes[currentHole - 1];
        }

        public GolfHole[] Holes
        {
            set { m_holes = value; }
            get { return m_holes; }
        }

        private string GetTeeString(XmlNodeList nodelist)
        {
            string retStr = "";
            foreach (XmlNode node in nodelist)
            {
                if (int.Parse(node.Attributes.GetNamedItem("tee").Value) == m_teeid)
                {
                    retStr = node.InnerText;
                }
            }
            return retStr;
        }
        private double GetTeeDouble(XmlNodeList nodelist)
        {
            string getStr = GetTeeString(nodelist);
            if (getStr == "")
                return -1;
            else
                return double.Parse(getStr);
        }
        private int GetTeeInt(XmlNodeList nodelist)
        {
            string getStr = GetTeeString(nodelist);
            if (getStr == "")
                return -1;
            else
                return int.Parse(getStr);
        }
        public DataSet TeeData(int teeid)
        {
            DB DB = new DB();
            DataSet ds = DB.GetDataSet("select * from mg_TourneyCourseDetails where courseid = " + ID +
               " and teenumber = " + teeid.ToString());
            DB.Close();
            return ds;
        }
        private void Load(System.Data.DataRow course)
        {
            int x = 0;
            m_golfcourse = course;

            m_coursename = course["course"].ToString();
            ID = int.Parse(course["courseid"].ToString());
            m_holes = new GolfHole[18];

            DB DB = new DB();
            DataSet ds = DB.GetDataSet("select * from mg_TourneyCourseDetails where courseid = " + course["courseid"].ToString() +
               " and teenumber = " + this.TeeID.ToString());
            DB.Close();
            System.Data.DataRow details = ds.Tables[0].Rows[0];

            m_rating = double.Parse(details["Rating"].ToString());
            m_slope = int.Parse(details["Slope"].ToString());

            int number = -1;
            int hcp = -1;
            int par = -1;
            for (x = 0; x < 18; x++)
            {
                number = x + 1; // int.Parse(holes[x].Attributes["number"].Value);
                hcp = int.Parse(details["Handicap" + number.ToString()].ToString()); // GetTeeInt(holes[x].SelectNodes("HCP"));
                par = int.Parse(details["Par" + number.ToString()].ToString()); //GetTeeInt(holes[x].SelectNodes("PAR"));
                m_holes[number - 1] = new GolfHole(number, m_teeid, par, hcp);
                m_coursepar += par;
            }

            string[] rounds = course["Round"].ToString().Split(',');
            m_rounds = new int[rounds.Length];
            for (x = 0; x < rounds.Length; x++)
                m_rounds[x] = int.Parse(rounds[x]);
        }
        public int[] Rounds
        {
            get { return m_rounds; }
            set { m_rounds = value; }
        }
        public override string ToString()
        {
            return m_coursename;
        }

        public void Save(int tournamentID)
        {
            string rounds = "";
            for (int i = 0; i < m_rounds.Length; i++)
            {
                rounds += m_rounds[i].ToString() + ",";
            }
            if (rounds.EndsWith(",")) rounds = rounds.Substring(0, rounds.Length - 1);

            DB DB = new DB();
            if (this.ID < 0)
            {
                DataSet ds = DB.GetDataSet("select max(courseid)+1 as courseid from mg_tourneycourses");
                if (ds.Tables[0].Rows[0]["courseid"] == DBNull.Value)
                    this.ID = 1;
                else
                    this.ID = int.Parse(ds.Tables[0].Rows[0]["courseid"].ToString());
                DB.Exec("insert into mg_tourneycourses (courseid, tournamentid, round, course) values (" +
                   this.ID.ToString() + ", " + tournamentID + ", '" + rounds + "', '" + m_coursename.Replace("'", "''") + "')");
            }
            else
            {
                DB.Exec("update mg_tourneycourses set course = '" + m_coursename.Replace("'", "''") +
                   "', round = '" + rounds + "' where courseid = " + this.ID.ToString() +
                   " and tournamentid = " + tournamentID.ToString());
            }
            DB.Close();

            for (int x = 0; x < m_numberoftees; x++)
            {
                string sql = "";
                DataSet ds = DB.GetDataSet("select * from mg_TourneyCourseDetails where courseid = " + this.ID.ToString() +
                   " and teenumber = " + x.ToString());
                if (ds.Tables[0].Rows.Count == 0)
                {
                    sql = "insert into mg_TourneyCourseDetails (courseid, teenumber, slope, rating) values (" +
                       this.ID.ToString() + ", " + x.ToString() + ", " + m_teeslope[x].ToString() + ", " + m_teerating[x].ToString() + ")";
                    DB.Exec(sql);
                }

                sql = "update mg_TourneyCourseDetails set slope=" + m_teeslope[x].ToString() + ", rating=" + m_teerating[x].ToString();
                for (int i = 0; i < m_holes.Length; i++)
                {
                    if (m_holes[i].TeeID == x)
                    {
                        sql += ", Par" + m_holes[i].Number.ToString() + "=" + m_holes[i].Par.ToString();
                        sql += ", Handicap" + m_holes[i].Number.ToString() + "=" + m_holes[i].Handicap.ToString();
                    }
                }
                sql += " where courseid = " + this.ID.ToString() +
                   " and teenumber = " + x.ToString();
                DB.Exec(sql);
            }

        }

    }
}
