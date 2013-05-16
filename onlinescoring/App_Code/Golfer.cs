using System;
using System.Xml;
using System.Data;

namespace MonsterGolfOnline
{

    public class GolferList
    {
        public GolferList()
        {
        }
        public static DataSet GetAvailableGolfers()
        {
            DB DB = new DB();
            DataSet ds = DB.GetDataSet("select * from mg_tourneyusers order by lastname, firstname");
            DB.Close();
            return ds;
        }
    }

    /// <summary>
    /// Summary description for Golfer.
    /// </summary>

    public class Golfer
    {
        public string FirstName = "";
        public string LastName = "";
        public int ID = -1;
        private int m_tournamentid = -1;
        private int m_roundnumber = 0;
        private double m_hcpindex = 0.0;
        private int m_tee = 0;
        private int[] m_scores = new int[18];
        private DataRow m_golfer = null;
        private bool m_loaded = false;

        public Golfer()
        {
        }

        public Golfer(DataRow dr)
        {
            m_golfer = dr;
            LoadGolfer();
        }

        public Golfer(int golferID)
        {
            ID = golferID;

            DB DB = new DB();
            DataSet ds = DB.GetDataSet("select * from mg_tourneyusers where userid = " + golferID.ToString());
            DB.Close();
            if (ds.Tables[0].Rows.Count > 0)
                m_golfer = ds.Tables[0].Rows[0];

            // editing a golfer
            LoadGolfer();
            LoadHcp();
        }

        public void LoadGolfer()
        {
            bool create = false;
            try
            {
                if (m_golfer == null)
                {
                    create = true;
                }
                else
                {
                    ID = int.Parse(m_golfer["userid"].ToString());
                    FirstName = m_golfer["FirstName"].ToString();
                    LastName = m_golfer["LastName"].ToString();
                    HcpIndex = double.Parse(m_golfer["HcpIndex"].ToString());
                }
            }
            catch (Exception e)
            {
                string msg = e.Message;
                create = true;
            }
            if (create)
                CreateGolfer();
            m_loaded = true;
        }

        public void CreateGolfer()
        {
            DB DB = new DB();
            DB.Exec("insert into mg_tourneyusers (firstname, lastname, hcpindex) values ('" +
               FirstName.Replace("'", "''") + "','" + LastName.Replace("'", "''") + "'," +
               m_hcpindex.ToString() + ")");
            DataSet ds = DB.GetDataSet("select max(userid) as userid from mg_tourneyusers");
            DB.Close();
            this.ID = int.Parse(ds.Tables[0].Rows[0]["userid"].ToString());
            m_loaded = true;
        }

        public void CreateRoundsForTournament(int TournamentID, int TeeNumber, double HCPIndex, int NumberOfRounds)
        {
            m_tournamentid = TournamentID;
            m_hcpindex = HCPIndex;
            m_tee = TeeNumber;
            for (int x = 1; x <= NumberOfRounds; x++)
            {
                m_roundnumber = x;
                Save();
            }

        }

        private void LoadHcp()
        {
            DataSet ds = DetailsForTournament();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                HcpIndex = double.Parse(ds.Tables[0].Rows[0]["Handicap"].ToString());
            }
        }

        public DataRow GolferDataRow
        {
            get { return m_golfer; }
        }
        public double HcpIndex
        {
            get { return m_hcpindex; }
            set { m_hcpindex = value; }
        }

        public int TournamentID
        {
            get { return m_tournamentid; }
            set { m_tournamentid = value; LoadHcp(); }
        }
        public int RoundNumber
        {
            get { return m_roundnumber; }
            set
            {
                m_roundnumber = value;
            }
        }
        public int Tee
        {
            get { return m_tee; }
            set { m_tee = value; }
        }
        public int[] Scores
        {
            get { return m_scores; }
            set { m_scores = value; }
        }

        private DataSet DetailsForTournament()
        {
            if (m_tournamentid == -1 || ID == -1) return null;

            string sql = "select tp.* from mg_tourneyteamplayers tp inner join mg_tourneyteams t on t.teamid = tp.teamid where tp.userid = " +
                  this.ID.ToString() + " and t.tournamentid = " + m_tournamentid.ToString();
            DB DB = new DB();
            DataSet ds = DB.GetDataSet(sql);
            DB.Close();
            return ds;
        }

        public bool LoadGolfersScore()
        {
            bool bRet = false;
            if (m_tournamentid > -1 && m_roundnumber > 0)
            {
                DataSet ds = DetailsForTournament();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    m_tee = int.Parse(ds.Tables[0].Rows[0]["teenumber"].ToString());
                    m_hcpindex = double.Parse(ds.Tables[0].Rows[0]["handicap"].ToString());
                }

                string sql = "select * from mg_tourneyscores where tournamentid = " +
                   m_tournamentid.ToString() + " and roundnumber = " + m_roundnumber.ToString() +
                   " and userid = " + this.ID;
                DB DB = new DB();
                ds = DB.GetDataSet(sql);
                DB.Close();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 1; i < 19; i++)
                    {
                        m_scores[i - 1] = int.Parse(ds.Tables[0].Rows[0]["hole" + i.ToString()].ToString());
                    }
                    bRet = true;

                    // override the handicap with the handicap for the round's scores
                    if (ds.Tables[0].Rows[0]["handicap"] != DBNull.Value)
                        m_hcpindex = double.Parse(ds.Tables[0].Rows[0]["handicap"].ToString());
                }
            }
            return bRet;
        }

        public override string ToString()
        {
            return this.LastName + ", " + this.FirstName;
        }

        private void AddToScoringTable()
        {
            if (m_tournamentid != -1)
            {
                DB DB = new DB();
                DataSet scores = DB.GetDataSet("select * from mg_tourneyscores where userid = " + this.ID.ToString() +
                   " and tournamentid = " + m_tournamentid.ToString() +
                   " and roundnumber = " + m_roundnumber.ToString());
                if (scores == null || scores.Tables[0].Rows.Count == 0)
                {
                    DB.Exec("insert into mg_tourneyscores (userid, tournamentid, roundnumber, handicap) values (" +
                       this.ID.ToString() + "," + m_tournamentid.ToString() + "," +
                       m_roundnumber.ToString() + "," + HcpIndex + ")");
                }
                DB.Close();
            }
        }

        public void SaveScores()
        {
            if (m_tournamentid != -1)
            {
                AddToScoringTable();

                string sql = "update mg_tourneyscores set handicap=" + m_hcpindex.ToString() + ", ";
                for (int x = 1; x < 19; x++)
                {
                    sql += "hole" + x.ToString() + "=" + m_scores[x - 1] + ",";
                }
                if (sql.EndsWith(",")) sql = sql.Substring(0, sql.Length - 1);
                sql += " where userid = " + this.ID.ToString() + " and tournamentid = " + m_tournamentid.ToString() +
                   " and roundnumber = " + m_roundnumber.ToString();
                DB DB = new DB();
                DB.Exec(sql);

                sql = "update mg_tourneyteamplayers set TeeNumber=" + Tee.ToString();
                sql += " where userid = " + this.ID.ToString() + " and tournamentid = " + m_tournamentid.ToString();
                DB.Exec(sql);
                DB.Close();
            }
        }

        public bool Save()
        {
            if (!m_loaded && LastName.Trim() != "")
                LoadGolfer();

            if (m_tournamentid != -1)
            {
                AddToScoringTable();

                string sql;
                DB DB = new DB();
                DataSet ds = DetailsForTournament();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    sql = "update mg_tourneyteamplayers set handicap=" + HcpIndex.ToString() + ", TeeNumber=" + Tee.ToString() + " where ID = " + ds.Tables[0].Rows[0]["id"].ToString();
                    DB.Exec(sql);
                }

                // don't change the core handicap, only the tournament handicap should be done here
                // core handicap is done from the Golfers screen
                //sql = "update Users set firstname='" + FirstName.Replace("'", "''") + "', lastname = '" + LastName.Replace("'", "''") + "', hcpindex=" + HcpIndex.ToString() + " where UserID = " + this.ID;
                sql = "update mg_tourneyUsers set firstname='" + FirstName.Replace("'", "''") + "', lastname = '" + LastName.Replace("'", "''") + "' where UserID = " + this.ID;
                DB.Exec(sql);
                DB.Close();
            }
            return true;
        }
    }
}
