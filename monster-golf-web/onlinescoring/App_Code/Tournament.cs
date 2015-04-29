using System;
using System.Xml;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;

namespace MonsterGolfOnline
{
    public class Team
    {
        private string m_flight = "";
        private string m_name = "";
        private int m_id = -1;
        private int m_numscores = -1;
        private Golfer[] m_golfers;
        private int m_numgolfers;
        private int m_tournamentid = -1;

        public Team()
        {
            m_numgolfers = 0;
        }

        public Team(System.Data.DataRow teamRow)
        {
            m_numgolfers = 0;
            m_flight = teamRow["Flight"].ToString();
            //m_name = teamRow["TeamName"].ToString();
            m_id = int.Parse(teamRow["TeamID"].ToString());
            m_tournamentid = int.Parse(teamRow["TournamentID"].ToString());
            SetGolfers();
            //if (m_name == "")
            //{
            SetName();
            //}
        }

        private void SaveFlight()
        {
            DB DB = new DB();
            DB.Exec("update mg_tourneyteams set flight='" + m_flight.Replace("'", "''") + "' where teamid = " + this.ID.ToString());
            DB.Close();
        }

        private void SetName()
        {
            m_name = "";
            foreach (Golfer golfer in m_golfers)
            {
                if (golfer != null)
                {
                    if (m_name != "")
                        m_name += " - ";
                    m_name += golfer.LastName + ", " + golfer.FirstName;
                }
            }
        }

        public double TeamHandicap
        {
            get
            {
                if (m_golfers == null)
                    return 0;
                double hcp = 0;
                try
                {
                    for (int i = 0; i < m_golfers.Length; i++)
                        hcp += m_golfers[i].HcpIndex;
                }
                catch
                {
                    return 0;
                }
                return hcp;
            }
        }

        public Golfer[] Golfers()
        {
            return m_golfers;
        }

        private void SetGolfers()
        {
            DB DB = new DB();
            DataSet ds = DB.GetDataSet("select UserID from mg_tourneyteamplayers where teamid=" + this.m_id.ToString());
            if (ds != null)
            {
                NumberOfGolfers = ds.Tables[0].Rows.Count;

                for (int x = 0; x < NumberOfGolfers; x++)
                {
                    DataSet golferDS = DB.GetDataSet("select * from mg_tourneyUsers where UserID = " + ds.Tables[0].Rows[x]["UserID"].ToString() + " order by lastname, firstname");
                    if (golferDS.Tables[0].Rows.Count == 0)
                        NumberOfGolfers--;
                    else
                    {
                        m_golfers[x] = new Golfer(golferDS.Tables[0].Rows[0]);
                        m_golfers[x].TournamentID = m_tournamentid;
                    }
                }
            }
            DB.Close();
        }

        public void AddGolfer(int index, string firstname, string lastname)
        {
            Golfer g = new Golfer();
            g.FirstName = firstname;
            g.LastName = lastname;
            g.Save();
            AddGolfer(index, g);
        }

        public void AddGolfer(int index, Golfer golfer)
        {
            if (m_golfers == null)
            {
                m_golfers = new Golfer[index + 1];
            }
            else if (index >= m_golfers.Length)
            {
                Golfer[] temp = new Golfer[index + 1];
                int x = 0;
                foreach (Golfer thisgolfer in m_golfers)
                {
                    temp[x] = thisgolfer;
                    x += 1;
                }
                m_golfers = null;
                m_golfers = temp;
            }
            m_golfers[index] = golfer;
            m_numgolfers = m_golfers.Length;
            SetName();
        }

        public void RemoveGolfer(int golferID)
        {
            DB DB = new DB();
            DB.Exec("delete from mg_tourneyteamplayers where teamid = " + this.ID.ToString() +
               " and userid = " + golferID.ToString());
            m_numgolfers -= 1;
            if (m_numgolfers == 0)
            {
                DB.Exec("delete from mg_tourneyTeams where teamid = " + this.ID.ToString());
                m_golfers = null;
            }
            else
            {
                SetGolfers();
                SetName();
            }
            DB.Close();
        }

        public int NumberOfGolfers
        {
            get { return m_numgolfers; }
            set
            {
                m_numgolfers = value;
                m_golfers = new Golfer[m_numgolfers];
            }
        }
        public int ID { get { return m_id; } set { m_id = value; } }
        public int NumberOfScores { get { return m_numscores; } set { m_numscores = value; } }
        public string Name
        {
            get
            {
                if (m_name == "" && m_numgolfers > 0)
                {
                    SetName();
                }
                return m_name;
            }
            set { m_name = value; }
        }
        public string Flight { get { return m_flight; } set { m_flight = value; SaveFlight(); } }
        public override string ToString()
        {
            return m_name;
        }

    }
    /// <summary>
    /// Summary description for Tournament.
    /// </summary>
    public class Tournament
    {
        private System.Data.DataSet m_tournament;
        private int m_id = -1;
        private int m_numberofcourses = -1;
        private int m_numberofrounds = -1;
        private string m_tourneyname = "";
        private System.Data.DataSet m_golfers = null;
        private System.Data.DataSet m_teams = null;
        private GolfCourse[,] m_courses = null;
        private int m_maxnumberoftees = -1;
        //private bool m_scramble = false;

        public Tournament()
        {
        }
        public Tournament(int id)
        {
            Load(id);
        }
        public int TournamentID
        {
            get { return m_id; }
            set { m_id = value; }
        }
        public int NumberOfCourses
        {
            get { return m_numberofcourses; }
            set { m_numberofcourses = value; }
        }
        public int NumberOfRounds
        {
            get { return m_numberofrounds; }
            set { m_numberofrounds = value; }
        }
        public string Name
        {
            get { return m_tourneyname; }
            set { m_tourneyname = value; }
        }
        public GolfCourse Course(int currentCourseID, int teeid)
        {
            return m_courses[currentCourseID, teeid];
        }
        public int NumberOfTees(int currentCourseID)
        {
            return m_courses[currentCourseID, 0].NumberOfTees;
        }
        public int GetCourseID(int roundnumber)
        {
            int[] rounds;
            for (int x = 0; x <= m_courses.GetUpperBound(0); x++)
            {
                rounds = m_courses[x, 0].Rounds;
                foreach (int round in rounds)
                {
                    if (round == roundnumber)
                        return x;
                }
            }
            return -1;
        }
        public Team[] Teams()
        {
            int nteams = m_teams.Tables[0].Rows.Count;
            if (nteams > 0)
            {
                Team[] teams = new Team[nteams];
                for (int x = 0; x < m_teams.Tables[0].Rows.Count; x++)
                {
                    teams[x] = new Team(m_teams.Tables[0].Rows[x]);
                }
                return teams;
            }

            return null;
        }

        public string[] Flights()
        {
            DB DB = new DB();
            string[] flights = null;
            DataSet ds = DB.GetDataSet("select distinct isnull(flight,'') as flight from mg_tourneyteams where tournamentid = " + this.TournamentID.ToString());
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                flights = new string[ds.Tables[0].Rows.Count];
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["Flight"] != DBNull.Value && ds.Tables[0].Rows[i]["Flight"].ToString().Trim() != "")
                        flights[i] = ds.Tables[0].Rows[i]["Flight"].ToString();
                    else
                        flights[i] = "(Unknown flight)";
                }
            }
            DB.Close();
            return flights;
        }

        public void RemoveTeam(int teamID)
        {
            DB DB = new DB();
            DB.Exec("delete from mg_tourneyteams where teamid = " + teamID.ToString() + " and tournamentid=" + this.TournamentID.ToString());
            DB.Close();
            Save();
        }

        //public void RemoveFlight(string flight)
        //{
        //   // flight stuff might not be necessary any more
        //   DB.Exec("update teams set flight='' where flight like '" + flight.Replace("'", "''") + "' and tournamentid=" +
        //      this.TournamentID.ToString());
        //   Save();
        //}

        //public void AddFlight(string flight)
        //{
        //   // flight stuff might not be necessary any more
        //   Save();
        //}

        public void ModifyTeam(Team teamtoadd)
        {
            if (teamtoadd.ID == -1)
            {
                AddTeam(teamtoadd);
                return;
            }
            DB DB = new DB();
            DataSet ds = DB.GetDataSet("select * from mg_tourneyteams where tournamentid = " + this.TournamentID.ToString() + " and teamid = " + teamtoadd.ID.ToString());
            if (ds == null || ds.Tables[0].Rows.Count == 0)
            {
                teamtoadd.ID = -1;
                AddTeam(teamtoadd);
                return;
            }

            if (teamtoadd.NumberOfGolfers > 0)
            {
                if (teamtoadd.Golfers().Length > 1)
                {
                    string teamname = "";
                    for (int i = 0; i < teamtoadd.NumberOfGolfers; i++)
                    {
                        if (teamname != "") teamname += " - ";
                        teamname += teamtoadd.Golfers()[i].LastName;
                    }
                    teamtoadd.Name = teamname;
                }
                DB.Exec("update mg_tourneyteams set teamname='" + teamtoadd.Name.Replace("'", "''") + "', flight='" +
                  teamtoadd.Flight.Replace("'", "''") + "' where tournamentid=" + this.TournamentID.ToString() + " and teamid = " + teamtoadd.ID.ToString());
                foreach (Golfer golfer in teamtoadd.Golfers())
                {
                    AddGolfer(golfer.ID, golfer.FirstName, golfer.LastName, golfer.HcpIndex, teamtoadd.ID, false);
                }
                this.ReloadTeams();
                this.ReloadGolfers();
            }
            DB.Close();
        }

        private bool GolferInList(int golferID)
        {
            m_golfers.Tables[0].DefaultView.RowFilter = "userid=" + golferID.ToString();
            bool inlist = (m_golfers.Tables[0].DefaultView.Count > 0);
            m_golfers.Tables[0].DefaultView.RowFilter = "";
            return inlist;
        }

        public void RemoveGolfer(int golferID)
        {
            bool inteam = false;
            int[] teamids = this.GetTeamIDFromGolfer(golferID);
            if (teamids != null)
            {
                inteam = true;
                foreach (int teamid in teamids)
                {
                    Team team = this.GetTeam(teamid);
                    team.RemoveGolfer(golferID);
                    if (team.NumberOfGolfers > 0)
                        ModifyTeam(team);
                    else
                    {
                        this.ReloadGolfers();
                        this.ReloadTeams();
                    }
                }
            }

            if (!inteam)
                Save();
        }

        public void AddGolfer(int inGolferID, string firstname, string lastname, double handicap, int teamID, bool reload)
        {
            DB DB = new DB();
            int golferID = inGolferID;
            if (!GolferInList(golferID))
            {
                if (golferID == -1)
                {
                    DB.Exec("insert into mg_tourneyusers (firstname, lastname) value ('" +
                       firstname.Replace("'", "''") + "', '" + lastname.Replace("'", "''") + "')");
                    DataSet ds = DB.GetDataSet("select max(userid) as userid from users");
                    golferID = int.Parse(ds.Tables[0].Rows[0]["userid"].ToString());
                }
                else
                    DB.Exec("update mg_tourneyusers set firstname = '" + firstname.Replace("'", "''") +
                       "', lastname = '" + lastname.Replace("'", "''") +
                       "' where userid = " + golferID.ToString());
            }

            if (teamID > -1)
            {
                DataSet ds = DB.GetDataSet("select * from mg_tourneyteamplayers where tournamentid = " + TournamentID +
                   " and userid = " + golferID);
                if (ds.Tables[0].Rows.Count == 0)
                    DB.Exec("insert into mg_tourneyteamplayers (teamid, userid, tournamentid, Handicap) values (" + teamID.ToString() + ", " + golferID.ToString() + ", " + this.TournamentID.ToString() + ", " + handicap.ToString() + ")");
                else
                    DB.Exec("update mg_tourneyteamplayers set teamid = " + teamID.ToString() + ", Handicap = " + handicap.ToString() + " where userid= " + golferID + " and tournamentid = " + TournamentID.ToString());
            }
            if (reload)
                this.ReloadGolfers();
            DB.Close();
        }

        public void AddTeam(Team teamtoadd)
        {
            if (teamtoadd.ID != -1)
            {
                return;
            }
            Golfer[] golfers = teamtoadd.Golfers();
            if (golfers.Length > 1)
            {
                string teamname = "";
                for (int i = 0; i < teamtoadd.NumberOfGolfers; i++)
                {
                    if (teamname != "") teamname += " - ";
                    teamname += golfers[i].LastName;
                }
                teamtoadd.Name = teamname;
            }
            DB DB = new DB();
            DB.Exec("insert into mg_tourneyteams (teamname, flight, tournamentid) values ('" +
               teamtoadd.Name.Replace("'", "''") + "', '" + teamtoadd.Flight.Replace("'", "''") +
               "', " + this.TournamentID + ")");
            DataSet ds = DB.GetDataSet("select max(teamid) as teamid from mg_tourneyteams");
            DB.Close();
            teamtoadd.ID = int.Parse(ds.Tables[0].Rows[0]["teamid"].ToString());


            for (int i = 0; i < teamtoadd.NumberOfGolfers; i++)
            {
                AddGolfer(golfers[i].ID, golfers[i].FirstName, golfers[i].LastName, golfers[i].HcpIndex, teamtoadd.ID, false);
            }
            this.ReloadTeams();
            this.ReloadGolfers();
        }

        public void AddCourse(GolfCourse coursetoadd)
        {
            try
            {
                coursetoadd.Save(this.TournamentID);
                Save();
            }
            catch (Exception ex)
            {
                string error = "AddCourse failed - " + ex.Message;
            }
        }

        public void RemoveCourse(int courseid)
        {
            DB DB = new DB();
            DB.Exec("delete from mg_tourneycourses where tournamentid=" + this.TournamentID.ToString() + " and courseid=" + courseid.ToString());
            DB.Close();
            Save();
        }

        public DataSet GolfersDataSet()
        {
            return m_golfers;
        }

        public ListItem[] Golfers()
        {
            ListItem[] golfers = null;
            if (m_golfers != null && m_golfers.Tables[0].Rows.Count > 0)
            {
                golfers = new ListItem[m_golfers.Tables[0].Rows.Count];
                for (int x = 0; x < m_golfers.Tables[0].Rows.Count; x++)
                {
                    golfers[x] = new ListItem(m_golfers.Tables[0].Rows[x]["UserID"].ToString(),
                       m_golfers.Tables[0].Rows[x]["LastName"].ToString() + ", " + m_golfers.Tables[0].Rows[x]["FirstName"].ToString());
                }
            }
            return golfers;
        }

        public int[] GetTeamIDFromGolfer(int golferID)
        {
            this.m_golfers.Tables[0].DefaultView.RowFilter = "userid=" + golferID.ToString();
            if (this.m_golfers.Tables[0].DefaultView.Count == 0)
                return null;

            int[] theids = new int[this.m_golfers.Tables[0].DefaultView.Count];
            for (int i = 0; i < this.m_golfers.Tables[0].DefaultView.Count; i++)
            {
                theids[i] = int.Parse(this.m_golfers.Tables[0].DefaultView[i]["TeamID"].ToString());
            }
            return theids;
        }

        public Team GetTeam(int teamid)
        {
            DB DB = new DB();
            DataSet ds = DB.GetDataSet("select * from mg_tourneyteams where tournamentid = " +
               this.TournamentID.ToString() + " and teamid = " + teamid.ToString());
            DB.Close();
            if (ds.Tables[0].Rows.Count == 0)
                return new Team();

            return new Team(ds.Tables[0].Rows[0]);
        }

        public void ReloadTeams()
        {
            DB DB = new DB();
            m_teams = DB.GetDataSet("select * from mg_tourneyTeams where TournamentID=" + m_id.ToString());
            DB.Close();
        }

        public void ReloadGolfers()
        {
            DB DB = new DB();
            m_golfers = DB.GetDataSet(
               "SELECT Users.firstname, Users.lastname, Users.userid, TeamPlayers.Handicap, Teams.TeamID, Teams.Flight, Users.WebId, Teams.TeamName, Round(Sum(TeamPlayers2.Handicap),2) AS TeamHcp " +
               "FROM " +
               "((mg_tourneyTeamPlayers AS TeamPlayers  " +
               "INNER JOIN mg_tourneyTeams AS Teams ON TeamPlayers.TeamID = Teams.TeamID) " +
               "INNER JOIN mg_tourneyUsers AS Users ON TeamPlayers.UserID = Users.UserID) " +
               "INNER JOIN mg_tourneyTeamPlayers AS TeamPlayers2 ON TeamPlayers2.TeamID = Teams.TeamID AND TeamPlayers2.TournamentID = Teams.TournamentID " +
               "WHERE Teams.TournamentID = " + m_id.ToString() + " " +
               "GROUP BY Users.firstname, Users.lastname, Users.userid, TeamPlayers.Handicap, Teams.TeamID, Teams.Flight, Users.WebId, Teams.TeamName " +
               "ORDER BY Users.lastname, Users.firstname"
            );
            DB.Close();
        }

        private void Load(int id)
        {
            if (m_id != id || m_tournament == null)
            {
                m_id = id;
            }
            DB DB = new DB();
            m_tournament = DB.GetDataSet("select * from mg_Tourney where TournamentID=" + m_id.ToString());

            ReloadTeams();
            ReloadGolfers();
            m_tourneyname = m_tournament.Tables[0].Rows[0]["Slogan"].ToString();
            // AARON ??
            //m_scramble = false;
            //try 
            //{
            //   m_scramble = bool.Parse(m_tournament.DocumentElement.SelectSingleNode("@scramble").Value);
            //}
            //catch
            //{
            //   m_scramble = false;
            //}

            System.Data.DataSet rounds = DB.GetDataSet("select * from mg_tourneycourses where tournamentid = " + m_id.ToString());
            if (rounds != null)
            {
                for (int x = 0; x < rounds.Tables[0].Rows.Count; x++)
                {
                    string[] round = rounds.Tables[0].Rows[x]["round"].ToString().Split(',');
                    for (int i = 0; i < round.Length; i++)
                    {
                        if (m_numberofrounds < int.Parse(round[i]))
                            m_numberofrounds = int.Parse(round[i]);
                    }
                }
            }

            if (rounds != null)
            {
                m_numberofcourses = rounds.Tables[0].Rows.Count;

                for (int x = 0; x < m_numberofcourses; x++)
                {
                    System.Data.DataSet tees = DB.GetDataSet("select * from mg_TourneyCourseDetails where courseid = " + rounds.Tables[0].Rows[x]["courseid"].ToString());

                    if (x == 0)
                        m_courses = new GolfCourse[m_numberofcourses, tees.Tables[0].Rows.Count];

                    if (m_maxnumberoftees < tees.Tables[0].Rows.Count)
                        m_maxnumberoftees = tees.Tables[0].Rows.Count;

                    for (int y = 0; y < tees.Tables[0].Rows.Count; y++)
                    {
                        m_courses[x, y] = new GolfCourse(rounds.Tables[0].Rows[x], y, tees.Tables[0].Rows.Count);
                    }

                }
            }
            DB.Close();
        }
        public void Save()
        {
            if (m_id > -1)
            {
                string sql = "select * from mg_tourney where TournamentID=" + m_id.ToString();
                DB DB = new DB();
                System.Data.DataSet dst = DB.GetDataSet(sql);
                if (dst.Tables[0].Rows.Count > 0)
                {
                    sql = "update mg_tourney set Location='', slogan='" + m_tourneyname.Replace("'", "''") +
                       "', description='' where tournamentid=" + m_id.ToString();
                }
                else
                {
                    sql = "insert into mg_tourney (TournamentID, Location, Slogan, Description) values (" +
                       m_id.ToString() + ",'','" + m_tourneyname.Replace("'", "''") +
                       "', '')";
                }

                try
                {
                    DB.Exec(sql);
                }
                catch
                {
                }
                DB.Close();
                Load(m_id);
            }
        }
    }
}
