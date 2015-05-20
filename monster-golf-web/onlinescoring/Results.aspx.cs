using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Data;
using MonsterGolfOnline;

public partial class Results : System.Web.UI.Page
{
    private ListDictionary colorDictionary;
    private List<Color> _flightColors;
    private Tournament m_tourney;
    private int m_currentRound = -1;
    private string m_currentSort;
    private bool m_hasflights = true;
    private DataTable DTResults;
    private int _throwoutrounds = 0;
    private int m_golfersperteam = 1;
    Team[] teamlist = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        int tourneyId = 0;
        if (int.TryParse(Request.QueryString["t"], out tourneyId))
        {
            hdnTourneyId.Value = tourneyId.ToString();
            InitResults(tourneyId);

            if (!IsPostBack)
            {
                if (Request.QueryString["p"] == "1") pnlOptions.Visible = false;
                if (Request.QueryString["to"] == "1") chkTotalsOnly.Checked = true;
                if (Request.QueryString["ii"] == "1") chkIncludeIndividuals.Checked = true;
                if (Request.QueryString["hb"] == "1") chkHighlightBest.Checked = true;
                if (Request.QueryString["cf"] == "1") chkColorFlights.Checked = true;
                bool showdata = false;
                if (Request.QueryString["so"] != null)
                {
                    ListItem li = ddScoring.Items.FindByValue(Request.QueryString["so"]);
                    if (li != null)
                    {
                        ddScoring.SelectedItem.Selected = false;
                        li.Selected = true;
                    }
                }
                if (ddScoring.SelectedIndex > 0) showdata = true;
                if (Request.QueryString["r"] != null)
                {
                    ListItem li = ddRound.Items.FindByValue(Request.QueryString["r"]);
                    if (li != null)
                    {
                        ddRound.SelectedItem.Selected = false;
                        li.Selected = true;
                    }
                }
                if (showdata)
                {
                    DTResults = Formulas.CalculateScores(m_tourney, teamlist, ddScoring.SelectedValue, _throwoutrounds, false, chkSideBets.Checked);
                    SetDefaultSort();
                    OutputDataGrid();
                }
            }
            else
            {
                if (Request.Form["hdnSort"] != "")
                {
                    dataGridViewResultsSort();
                }
            }
        }
    }

    public void InitResults(int TournamentID)
    {
        if (Session["tourneyinit" + TournamentID] == null)
        {
            m_tourney = new Tournament(TournamentID);
            Session["tourneyinit" + TournamentID] = m_tourney;
        }
        else
        {
            m_tourney = (Tournament)Session["tourneyinit" + TournamentID];
        }
        if (Session["tourneyteams" + TournamentID] == null)
        {
            teamlist = m_tourney.Teams();
            Session["tourneyteams" + TournamentID] = teamlist;
        }
        else
        {
            teamlist = (Team[])Session["tourneyteams" + TournamentID];
        }

        txtGridHeader.Text = m_tourney.Name + " - Results";

        // set flights or not
        string[] flights = m_tourney.Flights();
        m_hasflights = (flights != null && flights.Length > 0);
        if (m_hasflights)
        {
            if (flights.Length == 1 && flights[0] == "(Unknown flight)")
                m_hasflights = false;
        }
        if (!m_hasflights)
        {
            chkColorFlights.Enabled = false;
        }

        if (m_hasflights)
        {
            colorDictionary = new System.Collections.Specialized.ListDictionary();
            int rowcount = 1;
            foreach (string flight in flights)
            {
                colorDictionary.Add(flight, "row" + rowcount.ToString());
                rowcount += 1;
                if (rowcount == 11)
                    rowcount = 1;
            }

            _flightColors = new List<Color>();
            _flightColors.Add(Color.White);
            _flightColors.Add(Color.Wheat);
            _flightColors.Add(Color.LightBlue);
            _flightColors.Add(Color.LightGoldenrodYellow);
            _flightColors.Add(Color.LightGray);
            _flightColors.Add(Color.LightSteelBlue);
            _flightColors.Add(Color.LightCoral);
            _flightColors.Add(Color.LightSeaGreen);
            _flightColors.Add(Color.WhiteSmoke);
        }

        AddTournamentToMenu();
    }

    public Tournament Tournament
    {
        get { return m_tourney; }
    }

    private void AddScoringOptions()
    {
        // golfers per team
        DB DB = new DB();
        DataSet ds = DB.GetDataSet("select * from mg_tourneyTeamPlayers where TournamentID=" +
            m_tourney.TournamentID.ToString() + " order by TeamID");
        string teamid = "-1";
        int golfersperteam = 0;
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            if (teamid != "-1" &&
                teamid != ds.Tables[0].Rows[i]["TeamID"].ToString())
            {
                break;
            }
            golfersperteam++;
            teamid = ds.Tables[0].Rows[i]["TeamID"].ToString();
        }
        m_golfersperteam = (golfersperteam == 0) ? 1 : golfersperteam;

        // scoring formats
        if (!IsPostBack)
        {
            ds = DB.GetDataSet("select * from mg_tourneyScoringFormats order by FormatName");
            ListItem li = new ListItem("-- select --", "-1");
            ddScoring.Items.Add(li);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int minplayers = int.Parse(ds.Tables[0].Rows[i]["MinPlayers"].ToString());
                int maxplayers = int.Parse(ds.Tables[0].Rows[i]["MaxPlayers"].ToString());
                if (m_golfersperteam >= minplayers)
                {
                    li = new ListItem(
                        ds.Tables[0].Rows[i]["FormatName"].ToString(),
                        ds.Tables[0].Rows[i]["FormatID"].ToString());
                    ddScoring.Items.Add(li);
                }
            }
            ddScoring.SelectedIndex = 0;
        }
        DB.Close();
    }

    public void AddTournamentToMenu()
    {
        // add the options
        AddScoringOptions();

        // add the rounds
        if (!IsPostBack)
        {
            int courseid = 0;
            ListItem li = new ListItem("All", "-1");
            ddRound.Items.Add(li);

            for (int roundnum = 1; roundnum <= m_tourney.NumberOfRounds; roundnum++)
            {
                courseid = m_tourney.GetCourseID(roundnum);
                li = new ListItem("Round " + roundnum.ToString() + " - " + m_tourney.Course(courseid, 0).Name, roundnum.ToString());
                ddRound.Items.Add(li);
            }
            ddRound.SelectedIndex = 0;
        }
    }
    private void CheckBestScore(int score, string columnName, bool highestisbest, Dictionary<string, int> bestscores, List<string> tiebestscores)
    {
        if (score > Formulas.NoScoreDefault)
        {
            bool allowTies = columnName == "Out" || columnName == "In" ||
                columnName == "Total" || columnName == "Overall" || columnName.Contains("Round") ||
                columnName.Contains("Gross") || columnName.Contains("Net") || columnName.Contains("Score");

            if (bestscores.ContainsKey(columnName))
            {
                bool bbest = false;
                if (highestisbest)
                {
                    if (score > bestscores[columnName])
                        bbest = true;
                }
                else
                {
                    if (score < bestscores[columnName])
                        bbest = true;
                }

                if (bbest)
                {
                    bestscores[columnName] = score;
                    if (tiebestscores.Contains(columnName))
                        tiebestscores.Remove(columnName);
                }
                else if (score == bestscores[columnName] && !allowTies)
                {
                    if (!tiebestscores.Contains(columnName))
                        tiebestscores.Add(columnName);
                }
            }
            else
            {
                bestscores.Add(columnName, score);
            }
        }
    }
    Dictionary<string, int> bestScore = new Dictionary<string, int>();
    Dictionary<string, int> bestNet = new Dictionary<string, int>();
    private void CheckBestPlayer(DataRowView drv, Dictionary<string, int> bestscores, List<string> tiebestscores)
    {
        int currRound = 1;
        if (drv.DataView.Table.Columns.Contains("Round"))
        {
            currRound = int.Parse(drv["Round"].ToString());
        }
        string colName = "";
        int score;

        for (int i = 0; i < m_golfersperteam; i++)
        {
            colName = "P" + (i + 1).ToString() + " R" + currRound.ToString() + " Score";
            score = 0;
            bool parse = false;
            if (drv.DataView.Table.Columns.Contains(colName))
            {
                parse = int.TryParse(drv[colName].ToString(), out score);
            }
            else
            {
                if (drv.DataView.Table.Columns.Contains("PlayerGross" + i.ToString()))
                {
                    parse = int.TryParse(drv["PlayerGross" + i.ToString()].ToString(), out score);
                }
            }
            if (parse)
            {
                CheckBestScore(score, colName, false, bestscores, tiebestscores);
                if (bestscores.ContainsKey(colName))
                {
                    if (!bestScore.ContainsKey("Round" + currRound.ToString())) bestScore.Add("Round" + currRound.ToString(), bestscores[colName]);
                    else if (score < bestScore["Round" + currRound.ToString()]) bestScore["Round" + currRound.ToString()] = bestscores[colName];
                }
            }
            colName = "P" + (i + 1).ToString() + " R" + currRound.ToString() + " Net";
            if (drv.DataView.Table.Columns.Contains(colName))
            {
                parse = int.TryParse(drv[colName].ToString(), out score);
            }
            else
            {
                if (drv.DataView.Table.Columns.Contains("PlayerNet" + i.ToString()))
                {
                    parse = int.TryParse(drv["PlayerNet" + i.ToString()].ToString(), out score);
                }
            }
            if (parse)
            {
                CheckBestScore(score, colName, false, bestscores, tiebestscores);
                if (bestscores.ContainsKey(colName))
                {
                    if (!bestNet.ContainsKey("Round" + currRound.ToString())) bestNet.Add("Round" + currRound.ToString(), bestscores[colName]);
                    else if (score < bestNet["Round" + currRound.ToString()]) bestNet["Round" + currRound.ToString()] = bestscores[colName];
                }
            }
        }
        for (int i = 0; i < m_golfersperteam; i++)
        {
            colName = "P" + (i + 1).ToString() + " R" + currRound.ToString() + " Score";
            if (bestscores.ContainsKey(colName) && bestScore.ContainsKey("Round" + currRound.ToString()))
            {
                if (bestscores[colName] != bestScore["Round" + currRound.ToString()]) bestscores.Remove(colName);
            }
            colName = "P" + (i + 1).ToString() + " R" + currRound.ToString() + " Net";
            if (bestscores.ContainsKey(colName) && bestNet.ContainsKey("Round" + currRound.ToString()))
            {
                if (bestscores[colName] != bestNet["Round" + currRound.ToString()]) bestscores.Remove(colName);
            }
        }
    }
    private Dictionary<string, int> Highlights(DataView dv)
    {
        // 18 holes, front, back, round total, Overall = 22
        Dictionary<string, int> bestscores = new Dictionary<string, int>();
        List<string> tiebestscores = new List<string>();
        bestScore = new Dictionary<string, int>();
        bestNet = new Dictionary<string, int>();

        if (chkHighlightBest.Checked)
        {
            ListItem li = (ListItem)ddScoring.SelectedItem;
            Formulas.ScoringType st = Formulas.GetScoringType(li.Value);
            bool highestisbest = false;
            if (st == Formulas.ScoringType.GrossParPoints || st == Formulas.ScoringType.NetParPoints)
                highestisbest = true;

            // store the best scores
            for (int i = 0; i < dv.Count; i++)
            {
                int currRound = 1;
                if (dv.Table.Columns.Contains("Round"))
                {
                    currRound = int.Parse(dv[i]["Round"].ToString());
                }
                CheckBestPlayer(dv[i], bestscores, tiebestscores);
                for (int j = 0; j < dv.Table.Columns.Count; j++)
                {
                    int score;
                    if (dv.Table.Columns[j].ColumnName != "Round" &&
                        dv.Table.Columns[j].ColumnName != "Flight" &&
                        dv.Table.Columns[j].ColumnName != "TeamID" &&
                        !dv.Table.Columns[j].ColumnName.Contains("Player") &&
                        int.TryParse(dv[i][dv.Table.Columns[j].ColumnName].ToString(), out score))
                    {
                        CheckBestScore(score, dv.Table.Columns[j].ColumnName, highestisbest, bestscores, tiebestscores);
                        if (dv.Table.Columns[j].ColumnName == "Total")
                        {
                            CheckBestScore(score, "Round " + currRound.ToString(), highestisbest, bestscores, tiebestscores);
                        }
                    }
                }
            }

            // clear the ties
            for (int i = 0; i < tiebestscores.Count; i++)
            {
                if (bestscores.ContainsKey(tiebestscores[i]))
                    bestscores.Remove(tiebestscores[i]);
            }
        }
        return bestscores;
    }

    private void SetDefaultSort()
    {
        ListItem li = (ListItem)ddScoring.SelectedItem;
        if (li.Value != "-1")
        {
            Formulas.ScoringType st = Formulas.GetScoringType(li.Value);
            m_currentSort = "Flight ASC, Overall";
            if (st == Formulas.ScoringType.GrossParPoints ||
               st == Formulas.ScoringType.NetParPoints)
                m_currentSort += " DESC";
            else
                m_currentSort += " ASC";
        }
    }
    private void OutputDataGrid()
    {
        if (DTResults == null)
        {
            dataGridViewResults.Visible = false;
            return;
        }

        dataGridViewResults.Visible = true;

        if (m_currentSort == null) SetDefaultSort();

        // filter the default view
        if (ddRound.SelectedIndex > 0)
        {
            DTResults.DefaultView.RowFilter = "Round = '" + ((ListItem)ddRound.SelectedItem).Value + "'";
        }
        else
        {
            DTResults.DefaultView.RowFilter = "";
        }

        // set up column mappings
        for (int x = 0; x < DTResults.Columns.Count; x++)
        {
            if (chkTotalsOnly.Checked)
            {
                string colName = DTResults.Columns[x].ColumnName;
                if (colName != "Name" && colName != "Flight" && colName != "Round" && colName != "Total" && colName != "Overall" && colName != "OverallPlusBest")
                    DTResults.Columns[x].ColumnMapping = System.Data.MappingType.Hidden;
            }
            else
            {
                DTResults.Columns[x].ColumnMapping = System.Data.MappingType.Element;
            }
        }

        DTResults.Columns["TeamID"].ColumnMapping = System.Data.MappingType.Hidden;
        DTResults.Columns["Image"].ColumnMapping = System.Data.MappingType.Hidden;
        DTResults.Columns["Overall ToPar"].ColumnMapping = System.Data.MappingType.Hidden;
        DTResults.Columns["ToPar"].ColumnMapping = System.Data.MappingType.Hidden;
        if (!m_hasflights)
            DTResults.Columns["Flight"].ColumnMapping = System.Data.MappingType.Hidden;

        dataGridViewResults.DataSource = null;

        if (chkTotalsOnly.Checked)
        {
            bool addplusbest = (DTResults.Columns.Contains("OverallPlusBest"));

            DataTable dt = new DataTable();
            dt.Columns.Add("TeamID");
            dt.Columns["TeamID"].ColumnMapping = System.Data.MappingType.Hidden;
            dt.Columns.Add("Flight");
            dt.Columns.Add("Name");
            if (!m_hasflights)
                dt.Columns["Flight"].ColumnMapping = System.Data.MappingType.Hidden;
            for (int i = 0; i < m_tourney.NumberOfRounds; i++)
            {
                string colname = "Round " + (i + 1).ToString();
                dt.Columns.Add(colname, typeof(int));
                if (addplusbest)
                {
                    dt.Columns.Add(colname + " Plus Best", typeof(int));
                }
                if (ddRound.SelectedIndex > 0)
                {
                    if (ddRound.SelectedIndex != (i + 1))
                    {
                        dt.Columns[colname].ColumnMapping = System.Data.MappingType.Hidden;
                        if (addplusbest)
                        {
                            dt.Columns[colname + " Plus Best"].ColumnMapping = System.Data.MappingType.Hidden;
                        }
                    }
                }
            }
            dt.Columns.Add("Overall", typeof(int));

            if (addplusbest)
            {
                dt.Columns.Add("Overall Plus Best", typeof(int));
            }

            if (ddRound.SelectedIndex > 0)
            {
                dt.Columns["Overall"].ColumnMapping = System.Data.MappingType.Hidden;
                if (addplusbest)
                {
                    dt.Columns["Overall Plus Best"].ColumnMapping = System.Data.MappingType.Hidden;
                }
            }
            
            if (chkIncludeIndividuals.Checked)
            {
                for (int g = 1; g <= m_golfersperteam; g++)
                {
                    for (int i = 0; i < m_tourney.NumberOfRounds; i++)
                    {
                        dt.Columns.Add("P" + g.ToString() + " R" + (i + 1).ToString(), typeof(string));
                        dt.Columns.Add("P" + g.ToString() + " R" + (i + 1).ToString() + " Score", typeof(int));
                        dt.Columns.Add("P" + g.ToString() + " R" + (i + 1).ToString() + " HCP", typeof(int));
                        dt.Columns.Add("P" + g.ToString() + " R" + (i + 1).ToString() + " Net", typeof(int));
                        dt.Columns.Add("P" + g.ToString() + " R" + (i + 1).ToString() + " SC", typeof(int));

                        if (ddRound.SelectedIndex > 0)
                        {
                            if (ddRound.SelectedIndex != (i + 1))
                            {
                                dt.Columns["P" + g.ToString() + " R" + (i + 1).ToString()].ColumnMapping = System.Data.MappingType.Hidden;
                                dt.Columns["P" + g.ToString() + " R" + (i + 1).ToString() + " Score"].ColumnMapping = System.Data.MappingType.Hidden;
                                dt.Columns["P" + g.ToString() + " R" + (i + 1).ToString() + " HCP"].ColumnMapping = System.Data.MappingType.Hidden;
                                dt.Columns["P" + g.ToString() + " R" + (i + 1).ToString() + " Net"].ColumnMapping = System.Data.MappingType.Hidden;
                                dt.Columns["P" + g.ToString() + " R" + (i + 1).ToString() + " SC"].ColumnMapping = System.Data.MappingType.Hidden;
                            }
                        }
                        else
                        {
                            if (i != 0) dt.Columns["P" + g.ToString() + " R" + (i + 1).ToString()].ColumnMapping = System.Data.MappingType.Hidden;
                        }
                    }
                }
            }
            string curTeam = "-1";
            DataRow row = null;
            for (int i = 0; i < DTResults.DefaultView.Count; i++)
            {
                if (curTeam != DTResults.DefaultView[i]["TeamID"].ToString())
                {
                    if (row != null)
                        dt.Rows.Add(row);
                    row = dt.NewRow();
                    curTeam = DTResults.DefaultView[i]["TeamID"].ToString();
                    row["TeamID"] = curTeam;
                }
                row["Name"] = DTResults.DefaultView[i]["Name"].ToString();
                row["Flight"] = DTResults.DefaultView[i]["Flight"].ToString();
                row["Round " + DTResults.DefaultView[i]["Round"].ToString()] = int.Parse(DTResults.DefaultView[i]["Total"].ToString());
                if (addplusbest)
                {
                    row["Round " + DTResults.DefaultView[i]["Round"].ToString() + " Plus Best"] = int.Parse(DTResults.DefaultView[i]["TotalPlusBest"].ToString());
                }
                row["Overall"] = int.Parse(DTResults.DefaultView[i]["Overall"].ToString());
                if (addplusbest)
                {
                    row["Overall Plus Best"] = int.Parse(DTResults.DefaultView[i]["OverallPlusBest"].ToString());
                }
                if (chkIncludeIndividuals.Checked)
                {
                    for (int g = 0; g < m_golfersperteam; g++)
                    {
                        row["P" + (g + 1).ToString() + " R" + DTResults.DefaultView[i]["Round"].ToString()] = DTResults.DefaultView[i]["Player" + g.ToString()];
                        row["P" + (g + 1).ToString() + " R" + DTResults.DefaultView[i]["Round"].ToString() + " Score"] = DTResults.DefaultView[i]["PlayerGross" + g.ToString()];
                        row["P" + (g + 1).ToString() + " R" + DTResults.DefaultView[i]["Round"].ToString() + " HCP"] = DTResults.DefaultView[i]["PlayerHCP" + g.ToString()];
                        row["P" + (g + 1).ToString() + " R" + DTResults.DefaultView[i]["Round"].ToString() + " Net"] = DTResults.DefaultView[i]["PlayerNet" + g.ToString()];
                        row["P" + (g + 1).ToString() + " R" + DTResults.DefaultView[i]["Round"].ToString() + " SC"] = DTResults.DefaultView[i]["PlayerSC" + g.ToString()];
                    }
                }
            }
            if (row != null)
                dt.Rows.Add(row);

            // sort the default view
            dt.DefaultView.Sort = m_currentSort;

            // add all the columns to be bound
            foreach (DataColumn col in dt.Columns)
            {
                BoundColumn bc = new BoundColumn();
                bc.DataField = col.ColumnName;
                bc.HeaderText = col.ColumnName;
                bc.SortExpression = col.ColumnName;
                bc.ItemStyle.CssClass = "ScoreCell";
                bc.HeaderStyle.CssClass = "HeaderScoreCell";
                if (col.ColumnName != "Name" && col.ColumnName != "Player" && col.ColumnName != "Player0" && col.ColumnName != "Player1" && col.ColumnName != "Player2" && col.ColumnName != "Player3")
                {
                    bc.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    bc.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                }
                dataGridViewResults.Columns.Add(bc);
                if (col.ColumnMapping == MappingType.Hidden) dataGridViewResults.Columns[dataGridViewResults.Columns.Count - 1].Visible = false;
            }

            dataGridViewResults.DataSource = dt.DefaultView;

            if (chkIncludeIndividuals.Checked)
            {
                for (int i = 0; i < m_tourney.NumberOfRounds; i++)
                {
                    for (int g = 1; g <= m_golfersperteam; g++)
                    {
                        if (((DataView)dataGridViewResults.DataSource).Table.Columns.Contains("P" + g.ToString() + " R" + (i + 1).ToString()))
                        {
                            int idx = ((DataView)dataGridViewResults.DataSource).Table.Columns.IndexOf("P" + g.ToString() + " R" + (i + 1).ToString());
                            dataGridViewResults.Columns[idx].HeaderText = "Player";
                            dataGridViewResults.Columns[idx].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                        }
                        if (((DataView)dataGridViewResults.DataSource).Table.Columns.Contains("P" + g.ToString() + " R" + (i + 1).ToString() + " Score"))
                        {
                            int idx = ((DataView)dataGridViewResults.DataSource).Table.Columns.IndexOf("P" + g.ToString() + " R" + (i + 1).ToString() + " Score");
                            dataGridViewResults.Columns[idx].HeaderText = "R" + (i + 1).ToString();
                            dataGridViewResults.Columns[idx].ItemStyle.Width = Unit.Pixel(30);
                            //dataGridViewResults.Columns[idx].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                        }
                        if (((DataView)dataGridViewResults.DataSource).Table.Columns.Contains("P" + g.ToString() + " R" + (i + 1).ToString() + " HCP"))
                        {
                            int idx = ((DataView)dataGridViewResults.DataSource).Table.Columns.IndexOf("P" + g.ToString() + " R" + (i + 1).ToString() + " HCP");
                            dataGridViewResults.Columns[idx].HeaderText = "Hcp";
                            dataGridViewResults.Columns[idx].ItemStyle.Width = Unit.Pixel(32);
                            //dataGridViewResults.Columns[idx].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                        }
                        if (((DataView)dataGridViewResults.DataSource).Table.Columns.Contains("P" + g.ToString() + " R" + (i + 1).ToString() + " Net"))
                        {
                            int idx = ((DataView)dataGridViewResults.DataSource).Table.Columns.IndexOf("P" + g.ToString() + " R" + (i + 1).ToString() + " Net");
                            dataGridViewResults.Columns[idx].HeaderText = "Net";
                            dataGridViewResults.Columns[idx].ItemStyle.Width = Unit.Pixel(30);
                            //dataGridViewResults.Columns[idx].ItemStyle.HorizontalAlign = HorizontalAlign.Center;                      
                        }
                        if (((DataView)dataGridViewResults.DataSource).Table.Columns.Contains("P" + g.ToString() + " R" + (i + 1).ToString() + " SC"))
                        {
                            int idx = ((DataView)dataGridViewResults.DataSource).Table.Columns.IndexOf("P" + g.ToString() + " R" + (i + 1).ToString() + " SC");
                            dataGridViewResults.Columns[idx].HeaderText = "SC";
                            dataGridViewResults.Columns[idx].ItemStyle.Width = Unit.Pixel(30);
                            //dataGridViewResults.Columns[idx].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                }
            }

            // set up sorting
            for (int i = 0; i < dataGridViewResults.Columns.Count; i++)
            {
                string dir = (m_currentSort != null && m_currentSort.Contains("ASC")) ? "DESC" : "ASC";
                dataGridViewResults.Columns[i].HeaderText = "<a class=\"GridSortLink\" href=\"javascript:Sort('" + dataGridViewResults.Columns[i].SortExpression + " " + dir + "')\">" +
                    dataGridViewResults.Columns[i].HeaderText + "</a>";
            }

            // bind after column settings are finished
            dataGridViewResults.DataBind();
        }
        else
        {
            // sort the default view
            DTResults.DefaultView.Sort = m_currentSort;

            // add all the columns to be bound
            foreach (DataColumn col in DTResults.DefaultView.Table.Columns)
            {
                BoundColumn bc = new BoundColumn();
                bc.DataField = col.ColumnName;
                bc.HeaderText = col.ColumnName;
                bc.ItemStyle.CssClass = "ScoreCell";
                bc.HeaderStyle.CssClass = "HeaderScoreCell";
                if (col.ColumnName != "Name" && col.ColumnName != "Player" && col.ColumnName != "Player0" && col.ColumnName != "Player1" && col.ColumnName != "Player2" && col.ColumnName != "Player3")
                {
                    bc.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    bc.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                }
                bc.SortExpression = col.ColumnName;
                dataGridViewResults.Columns.Add(bc);
                if (col.ColumnMapping == MappingType.Hidden) dataGridViewResults.Columns[dataGridViewResults.Columns.Count - 1].Visible = false;
            }

            dataGridViewResults.DataSource = DTResults.DefaultView;

            for (int g = 0; g < m_golfersperteam; g++)
            {
                if (((DataView)dataGridViewResults.DataSource).Table.Columns.Contains("Player" + g.ToString()))
                {
                    int idx = ((DataView)dataGridViewResults.DataSource).Table.Columns.IndexOf("Player" + g.ToString());
                    dataGridViewResults.Columns[idx].HeaderText = "Player";
                    dataGridViewResults.Columns[idx].Visible = chkIncludeIndividuals.Checked;
                }
                if (((DataView)dataGridViewResults.DataSource).Table.Columns.Contains("PlayerGross" + g.ToString()))
                {
                    int idx = ((DataView)dataGridViewResults.DataSource).Table.Columns.IndexOf("PlayerGross" + g.ToString());
                    dataGridViewResults.Columns[idx].HeaderText = "Gross";
                    dataGridViewResults.Columns[idx].Visible = chkIncludeIndividuals.Checked;
                    dataGridViewResults.Columns[idx].ItemStyle.Width = Unit.Pixel(40);
                    dataGridViewResults.Columns[idx].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                }
                if (((DataView)dataGridViewResults.DataSource).Table.Columns.Contains("PlayerHCP" + g.ToString()))
                {
                    int idx = ((DataView)dataGridViewResults.DataSource).Table.Columns.IndexOf("PlayerHCP" + g.ToString());
                    dataGridViewResults.Columns[idx].HeaderText = "Hcp";
                    dataGridViewResults.Columns[idx].Visible = chkIncludeIndividuals.Checked;
                    dataGridViewResults.Columns[idx].ItemStyle.Width = Unit.Pixel(32);
                    dataGridViewResults.Columns[idx].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                }
                if (((DataView)dataGridViewResults.DataSource).Table.Columns.Contains("PlayerNet" + g.ToString()))
                {
                    int idx = ((DataView)dataGridViewResults.DataSource).Table.Columns.IndexOf("PlayerNet" + g.ToString());
                    dataGridViewResults.Columns[idx].HeaderText = "Net";
                    dataGridViewResults.Columns[idx].Visible = chkIncludeIndividuals.Checked;
                    dataGridViewResults.Columns[idx].ItemStyle.Width = Unit.Pixel(30);
                    dataGridViewResults.Columns[idx].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                }
                if (((DataView)dataGridViewResults.DataSource).Table.Columns.Contains("PlayerSC" + g.ToString()))
                {
                    int idx = ((DataView)dataGridViewResults.DataSource).Table.Columns.IndexOf("PlayerSC" + g.ToString());
                    dataGridViewResults.Columns[idx].HeaderText = "SC";
                    dataGridViewResults.Columns[idx].Visible = chkIncludeIndividuals.Checked;
                    dataGridViewResults.Columns[idx].ItemStyle.Width = Unit.Pixel(30);
                    dataGridViewResults.Columns[idx].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                }
            }

            string dir = (m_currentSort == null || m_currentSort.Contains("DESC")) ? "DESC" : "ASC";
            // set up sorting
            for (int i = 0; i < dataGridViewResults.Columns.Count; i++)
            {
                string sortdir = dir;
                if (m_currentSort.Contains(dataGridViewResults.Columns[i].SortExpression)) sortdir = (dir == "DESC") ? "ASC" : "DESC";
                dataGridViewResults.Columns[i].HeaderText = "<a class=\"GridSortLink\" href=\"javascript:Sort('" + dataGridViewResults.Columns[i].SortExpression + " " + sortdir + "')\">" +
                    dataGridViewResults.Columns[i].HeaderText + "</a>";
            }

            // bind after column settings are finished
            dataGridViewResults.DataBind();
        }


        // default the score widths
        if (!chkTotalsOnly.Checked)
        {
            for (int i = 1; i < 19; i++)
            {
                dataGridViewResults.Columns[i].ItemStyle.Width = Unit.Pixel(28);
            }
            
            int idx = ((DataView)dataGridViewResults.DataSource).Table.Columns.IndexOf("Out");
            dataGridViewResults.Columns[idx].ItemStyle.Width = Unit.Pixel(30);
            //dataGridViewResults.Columns[idx].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            idx = ((DataView)dataGridViewResults.DataSource).Table.Columns.IndexOf("In");
            dataGridViewResults.Columns[idx].ItemStyle.Width = Unit.Pixel(30);
            //dataGridViewResults.Columns[idx].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            idx = ((DataView)dataGridViewResults.DataSource).Table.Columns.IndexOf("Total");
            dataGridViewResults.Columns[idx].ItemStyle.Width = Unit.Pixel(40);
            //dataGridViewResults.Columns[idx].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        }
        else
        {
            for (int i = 1; i <= m_tourney.NumberOfRounds; i++)
            {
                if (((DataView)dataGridViewResults.DataSource).Table.Columns.Contains("Round " + i.ToString()))
                {
                    int idx = ((DataView)dataGridViewResults.DataSource).Table.Columns.IndexOf("Round " + i.ToString());
                    dataGridViewResults.Columns[idx].ItemStyle.Width = Unit.Pixel(55);
                    //dataGridViewResults.Columns[idx].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                }
            }
        }
        if (((DataView)dataGridViewResults.DataSource).Table.Columns.Contains("Flight"))
        {
            int idx = ((DataView)dataGridViewResults.DataSource).Table.Columns.IndexOf("Flight");
            dataGridViewResults.Columns[idx].ItemStyle.Width = Unit.Pixel(40);
            dataGridViewResults.Columns[idx].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        }
        if (((DataView)dataGridViewResults.DataSource).Table.Columns.Contains("Overall"))
        {
            int idx = ((DataView)dataGridViewResults.DataSource).Table.Columns.IndexOf("Overall");
            dataGridViewResults.Columns[idx].ItemStyle.Width = Unit.Pixel(50);
            dataGridViewResults.Columns[idx].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        }

        // set the highlight and flight colors
        SetGridColors();
    }

    private void SetGridColors()
    {
        if (DTResults == null)
            return;

        System.Collections.Generic.Dictionary<string, int> bestscores = Highlights(DTResults.DefaultView);

        if (bestscores.Count > 0 || m_hasflights)
        {
            DataView view = ((DataView)dataGridViewResults.DataSource);
            for (int x = 0; x < view.Count; x++)
            {

                for (int i = 0; i < dataGridViewResults.Columns.Count; i++)
                {
                    if (m_hasflights && chkColorFlights.Checked)
                    {
                        int idx = ((DataView)dataGridViewResults.DataSource).Table.Columns.IndexOf("Flight");
                        dataGridViewResults.Items[x].BackColor = _flightColors[int.Parse(dataGridViewResults.Items[x].Cells[idx].Text)];
                    }

                    foreach (string key in bestscores.Keys)
                    {
                        if (dataGridViewResults.Columns[i].HeaderText.Contains(key))
                        {
                            int score = 0;
                            if (int.TryParse(dataGridViewResults.Items[x].Cells[i].Text, out score))
                            {
                                if (bestscores[key] == score)
                                {
                                    dataGridViewResults.Items[x].Cells[i].BackColor = Color.Red;
                                    dataGridViewResults.Items[x].Cells[i].ForeColor = Color.White;
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }
    }

    public void WriteHTML()
    {
        if (DTResults == null)
        {
            // clear the results
            return;
        }

        DataView currentGridResults = (DataView)dataGridViewResults.DataSource;

        System.Text.StringBuilder htmlstring = new System.Text.StringBuilder();
        System.Text.StringBuilder divscores = new System.Text.StringBuilder();

        htmlstring.Append("<html><style>body {background-color:#D3D3D3; font-family:verdana; font-size:12pt; } td { font-family:verdana; font-size:10pt; background-color: #ffffff; } .skin { background-color:red; font-weight:bold;} .rowheader { background-color:#394fbc; color:white; font-weight:bold; font-size:10pt; } .row1 { background-color:87cefa; } .row2 { background-color:fff8dc; } .row3 { background-color:e0eee0; } .row4 { background-color:e6e6fa; } .row5 { background-color:deb887; } .row6 { background-color:f0e68c; } .row7 { background-color:ffa500; } .row8 { background-color:ff1493; } .row9 { background-color:d8bfd8; } .row10 { background-color:dccdc; } table { background-color:black; } </style><body>");

        htmlstring.Append("<center><b>" + m_tourney.Name + "</b><br><br>");

        htmlstring.Append("<table cellpadding=2 cellspacing=1 border=0>\n");
        htmlstring.Append("<tr class=rowheader><td class=rowheader></td>");
        int holeid = 0;
        for (int x = 0; x < currentGridResults.Table.Columns.Count; x++)
        {
            if (currentGridResults.Table.Columns[x].ColumnMapping != System.Data.MappingType.Hidden)
            {
                string colName = currentGridResults.Table.Columns[x].ColumnName;
                if (chkTotalsOnly.Checked && colName == "Round")
                {
                    string roundTitles = "";
                    foreach (System.Data.DataRowView dr in currentGridResults)
                    {
                        if (!roundTitles.Contains(">Round " + dr[colName].ToString() + "<"))
                            roundTitles += "<td class=rowheader align=center><b>Round " + dr[colName].ToString() + "</b></td>";
                    }
                    htmlstring.Append(roundTitles);
                }
                else if (chkTotalsOnly.Checked && colName == "Total")
                {
                    // do nothing
                }
                else
                {
                    htmlstring.Append("<td class=rowheader align=center><b>" + colName + "</b></td>");
                }
            }
        }
        htmlstring.Append("</tr>\n");

        string rowclass = "row1";
        System.Collections.Generic.Dictionary<string, int> bestscores = Highlights(currentGridResults);
        int rownum = 0;
        int rowstring = 0;
        string cellclass = "";
        string cellalign = "";

        for (int i = 0; i < currentGridResults.Count; i++)
        {
            System.Data.DataRowView dr = currentGridResults[i];

            rowstring = rownum + 1;
            htmlstring.Append("<tr id='row" + rowstring.ToString() + "' class='");
            if (m_hasflights && dr["Flight"].ToString().Length > 0)
            {
                rowclass = colorDictionary[dr["Flight"].ToString()].ToString();
                htmlstring.Append(rowclass);
            }
            else
            {
                htmlstring.Append(rowclass);
                rowclass = (rowclass == "row1") ? "row2" : "row1";
            }
            htmlstring.Append("'>");

            htmlstring.Append("<td align=center class=" + rowclass + ">" + rowstring.ToString() + "</td>");

            object cell;
            for (int x = 0; x < currentGridResults.Table.Columns.Count; x++)
            {
                cellclass = "";
                cell = dr[currentGridResults.Table.Columns[x].ColumnName];

                if (currentGridResults.Table.Columns[x].ColumnMapping != System.Data.MappingType.Hidden)
                {
                    try
                    {
                        holeid = int.Parse(cell.ToString());
                        cellalign = " align=center";
                    }
                    catch
                    {
                        cellalign = "";
                    }

                    cellclass = " class=" + rowclass;

                    if (chkHighlightBest.Checked)
                    {
                        try
                        {
                            int score;
                            if (int.TryParse(dr[currentGridResults.Table.Columns[x].ColumnName].ToString(), out score))
                            {
                                if (bestscores.ContainsKey(currentGridResults.Table.Columns[x].ColumnName))
                                {
                                    if (bestscores[currentGridResults.Table.Columns[x].ColumnName] == score)
                                    {
                                        cellclass = " class=skin";
                                    }
                                }
                            }
                        }
                        catch
                        {
                            //do nothing.
                        }
                    }

                    if (chkTotalsOnly.Checked && currentGridResults.Table.Columns[x].ColumnName == "Round")
                    {
                        // don't show
                    }
                    else if (chkTotalsOnly.Checked && currentGridResults.Table.Columns[x].ColumnName == "Total")
                    {
                        // while the team ID is the same get the totals
                        string lastteamid = dr["TeamID"].ToString();
                        for (i = i; i < currentGridResults.Count; i++)
                        {
                            dr = currentGridResults[i];
                            cell = dr[currentGridResults.Table.Columns[x].ColumnName];
                            if (lastteamid == dr["TeamID"].ToString())
                            {
                                htmlstring.Append("<td" + cellclass + cellalign + ">" + cell.ToString() + "</td>");

                            }
                            else
                            {
                                i--;
                                break;
                            }

                        }
                    }
                    else
                    {
                        string mouseoverout = "";
                        if (currentGridResults.Table.Columns[x].ColumnName == "Name")
                        {
                            mouseoverout = " onmouseout=\"UnHighlight('row" + rowstring.ToString() + "');";
                            mouseoverout += " HideScores('" +
                                rowstring.ToString() + "');";
                            mouseoverout += "\" onmouseover=\"Highlight('row" + rowstring.ToString() + "');";
                            mouseoverout += " ShowScores('" +
                                rowstring.ToString() + "',event);";
                            mouseoverout += "\"";
                        }
                        htmlstring.Append("<td" + cellclass + cellalign + mouseoverout + ">" + cell.ToString());
                        if (currentGridResults.Table.Columns[x].ColumnName == "Name")
                            divscores.Append(AddImagesPopupScores(dr["TeamID"].ToString(), rowstring));
                        htmlstring.Append("</td>");
                    }
                }
            }
            htmlstring.Append("</tr>\n");
            rownum += 1;
        }
        htmlstring.Append("</table>");
        htmlstring.Append("</center>");

        // add the hidden scores
        htmlstring.Append(divscores.ToString());

        // finish the HTML
        htmlstring.Append("</body>\n");
        htmlstring.Append(@"<script>
                function ShowScores(which,e)
                {
                    document.getElementById('divScores' + which).style.left = e.clientX;
                    document.getElementById('divScores' + which).style.top = e.clientY;
                    document.getElementById('divScores' + which).style.display = ''; //'inline';
                }
                function HideScores(which)
                {
                    document.getElementById('divScores' + which).style.display = 'none';
                }
                function Highlight(which)
                {
                    document.getElementById(which).style.fontWeight = 'bold';
                }
                function UnHighlight(which)
                {
                    document.getElementById(which).style.fontWeight = 'normal';
                }
                </script>");
        htmlstring.Append("</html>");
        FillBrowser(htmlstring.ToString());
    }

    private string AddImagesPopupScores(string teamid, int id)
    {
        string divtable = "";
        string curFilter = DTResults.DefaultView.RowFilter;

        DTResults.DefaultView.RowFilter = "TeamID='" + teamid + "'";
        DataView view = DTResults.DefaultView;

        string display = "none";
        string fontstyle = "";
        //if (checkBoxScrollScores.Checked)
        //{
        //   display = "";
        //   fontstyle = " style=\"font-size: 14;\"";
        //}
        divtable = "<div id=\"divScores" + id + "\" style=\"position: absolute; display: " + display + "\"><br><br>";
        //divtable += "<table><tr>";
        //string[] images = view[0]["Image"].ToString().Split(';');
        //for (int j = 0; j < images.Length; j++)
        //{
        //   if (images[j].Trim().Length == 0)
        //   {
        //      images[j] = "nogirl.jpg";
        //   }
        //   divtable += "<td valign=\"bottom\"><img height=\"438\" width=\"287\" src=\"Images/" + images[j] + "\" /></td>";
        //}
        //divtable += "</tr></table>";

        if (chkTotalsOnly.Checked)
        {
            divtable += "<table><tr>";
            for (int j = 1; j <= 18; j++)
            {
                divtable += "<td align=\"center\"" + fontstyle + ">" + j.ToString() + "</td>";
                if (j == 9)
                    divtable += "<td align=\"center\"" + fontstyle + ">Out</td>";
                if (j == 18)
                    divtable += "<td align=\"center\"" + fontstyle + ">In</td>";

            }
            divtable += "<td" + fontstyle + ">Total</td>";
            divtable += "</tr>";

            for (int i = 0; i < view.Count; i++)
            {
                string curteamid = view[i]["TeamID"].ToString();
                if (curteamid != teamid)
                    break;
                divtable += "<tr>";
                for (int j = 1; j <= 18; j++)
                {
                    divtable += "<td align=\"center\" style=\"background-color: yellow;\"" + fontstyle + ">" + view[i][j.ToString()] + "</td>";
                    if (j == 9)
                        divtable += "<td align=\"center\"" + fontstyle + ">" + view[i]["Out"] + "</td>";
                    if (j == 18)
                        divtable += "<td align=\"center\"" + fontstyle + ">" + view[i]["In"] + "</td>";

                }
                divtable += "<td align=\"center\"" + fontstyle + "><b>" + view[i]["Total"] + "</b></td>";
                divtable += "</tr>";
            }
            divtable += "</table>";
        }
        divtable += "</div>";

        DTResults.DefaultView.RowFilter = curFilter;

        return divtable;
    }
    private void FillBrowser(string html)
    {
        string filname = Server.MapPath("~") + "\\Results-" + m_tourney.Name.Replace(" ", "") + DateTime.Now.ToString().Replace(" ", "").Replace("/", "").Replace(":", "") + ".html";
        System.IO.StreamWriter fs = System.IO.File.CreateText(filname);
        fs.Write(html);
        fs.Flush();
        fs.Close();
        // launch the web page here
        System.Diagnostics.Process p = new System.Diagnostics.Process();
        p.StartInfo.FileName = filname;
        p.StartInfo.CreateNoWindow = false;
        p.Start();
    }

    private void ShowScoring(string sort)
    {
        ListItem li = ddScoring.SelectedItem;
        if (li.Value != "-1")
        {
            DTResults = Formulas.CalculateScores(m_tourney, teamlist, li.Value, _throwoutrounds, false, chkSideBets.Checked);
        }
        else
        {
            DTResults = null;
        }
        if (sort == null) SetDefaultSort();
        else m_currentSort = sort;
        OutputDataGrid();
    }
    protected void ScoringOption_Click(object sender, System.EventArgs e)
    {
        ShowScoring(null);
    }

    protected void dataGridViewResultsSort()
    {
        ShowScoring(Request.Form["hdnSort"]);
    }

    protected void Round_Select(object sender, EventArgs e)
    {
        m_currentRound = int.Parse(((ListItem)((DropDownList)sender).SelectedItem).Value);
        OutputDataGrid();
    }

    protected void checkBoxTotalsOnly_CheckedChanged(object sender, EventArgs e)
    {
        OutputDataGrid();
    }

    protected void checkBoxHighlight_CheckedChanged(object sender, EventArgs e)
    {
        OutputDataGrid();
    }

    //protected void OnSort(object sender, DataGridViewCellMouseEventArgs e)
    //{
    //    // this doesn't work becuase it is using built in sorting for the datagrid
    //    // which will not always match the datasource columns.
    //    //m_currentSort = ((DataView)dataGridViewResults.DataSource).Table.Columns[e.ColumnIndex].Name;
    //    //if (dataGridViewResults.SortOrder == SortOrder.Ascending) m_currentSort += " ASC";
    //    //else m_currentSort += " DESC";
    //    SetGridColors();
    //    dataGridViewResults.ClearSelection();
    //}

    //protected void SortByFlightAndTotal(object sender, LinkLabelLinkClickedEventArgs e)
    //{
    //    SetDefaultSort();
    //    OutputDataGrid();
    //}

    protected void checkBoxColorFlights_CheckedChanged(object sender, EventArgs e)
    {
        if (!((CheckBox)sender).Checked)
            OutputDataGrid(); // refresh the entire grid
        else
            SetGridColors(); // just set the colors
    }


    protected void checkBoxIncludeIndvd_CheckChanged(object sender, EventArgs e)
    {
        OutputDataGrid();
    }

}