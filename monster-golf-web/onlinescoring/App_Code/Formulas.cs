using System;
using System.Configuration;
using System.Data;

namespace MonsterGolfOnline
{
    public class IndividualResult
    {
        public string Name = "";
        public int HCP = 0;
        public int Gross = 0;
        public int StrokeControl = 0;
    }
    /// <summary>
    /// Summary description for Formulas.
    /// </summary>
    public class Formulas
    {
        public Formulas()
        {
        }
        public static int NoScoreDefault = -99;

        public static int Handicap(double hcpIndex, int slope)
        {
            double dhcp = (hcpIndex * slope) / 113;
            return (int)Math.Round(dhcp, 0, MidpointRounding.AwayFromZero);
        }
        public static int NetScore(int playerhandicap, int holehandicap, int grossscore)
        {
            int strokesforhole = playerhandicap % 18 >= holehandicap ? 1 : 0;
            strokesforhole += playerhandicap / 18;
            int netscore = grossscore - strokesforhole;
            return netscore;
        }
        public static int ParPoints(int playerhandicap, int score, int parforhole)
        {
            int parpoints = 0;
            if (ConfigurationManager.AppSettings["minusinparpoints"] != null &&
                 ConfigurationManager.AppSettings["minusinparpoints"] == "true")
                parpoints = -1;

            if ((parforhole - score) > -2)
                parpoints = parforhole - score + 2;
            return parpoints;
        }

        public static int ParPoints(int score, int parforhole,
           System.Collections.Specialized.NameValueCollection formula)
        {
            int overunder = score - parforhole;
            int parpoints = 99;
            if (formula.Get(overunder.ToString()) != null)
            {
                // in the formula
                parpoints = int.Parse(formula.Get(overunder.ToString()));
            }
            else
            {
                // return the least amount of par points
                for (int i = 0; i < formula.Count; i++)
                {
                    if (int.Parse(formula[i]) < parpoints)
                        parpoints = int.Parse(formula[i]);
                }
            }
            return parpoints;
        }

        public enum ScoringType
        {
            Gross,
            Net,
            GrossParPoints,
            NetParPoints
        };

        public enum BestBallOption
        {
            Individual,
            BestOne,
            BestTwo,
            BestThree,
            All
        };

        public static BestBallOption GetBestBallOption(string holepar, string informat, string round)
        {
            BestBallOption bbo = BestBallOption.Individual;
            string format = informat;
            if (round.Length > 0)
            {
                if (format.Contains("rnd" + round + "=["))
                {
                    string rndInd = "rnd" + round + "=[";
                    int start = format.IndexOf(rndInd);
                    start += rndInd.Length;
                    int end = format.IndexOf("]", start);
                    format = format.Substring(start, end - start);
                }
            }

            if (format.Contains("{" + holepar + "=all}"))
                bbo = BestBallOption.All;
            else if (format.Contains("{" + holepar + "=best1}"))
                bbo = BestBallOption.BestOne;
            else if (format.Contains("{" + holepar + "=best2}"))
                bbo = BestBallOption.BestTwo;
            else if (format.Contains("{" + holepar + "=best3}"))
                bbo = BestBallOption.BestThree;
            else if (format.Contains("{best1}"))
                bbo = BestBallOption.BestOne;
            else if (format.Contains("{best2}"))
                bbo = BestBallOption.BestTwo;
            else if (format.Contains("{best3}"))
                bbo = BestBallOption.BestThree;
            else if (format.Contains("{all}"))
                bbo = BestBallOption.All;
            return bbo;
        }

        private static string GetScoringFormat(string scoringOption)
        {
            // default to gross formula
            if (scoringOption.Length == 0)
                return "{gross}";

            DB DB = new DB();
            DataSet dsFormula = DB.GetDataSet("select FormatFormula, MinPlayers, MaxPlayers from mg_tourneyScoringFormats where formatid=" + scoringOption);
            DB.Close();
            string format = dsFormula.Tables[0].Rows[0]["FormatFormula"].ToString();
            return format;
        }

        public static ScoringType GetScoringType(string scoringOption)
        {
            return GetScoringType(GetScoringFormat(scoringOption), scoringOption);
        }

        public static ScoringType GetScoringType(string format, string scoringOption)
        {
            ScoringType st = ScoringType.Gross;
            if (format.Contains("{netpp}"))
                st = ScoringType.NetParPoints;
            else if (format.Contains("{net}"))
                st = ScoringType.Net;
            else if (format.Contains("{grosspp}"))
                st = ScoringType.GrossParPoints;
            return st;
        }
        public static int GetBestScore(ScoringType st, int currentbest, int score)
        {
            if (currentbest == NoScoreDefault ||
                ((st == ScoringType.GrossParPoints || st == ScoringType.NetParPoints) && currentbest < score) ||
                ((st == ScoringType.Gross || st == ScoringType.Net) && currentbest > score)
                )
            {
                return score;
            }
            return currentbest;
        }
        public static DataTable CalculateScores(Tournament tourney, Team[] teamlist, string scoringOption, int throwOutWorst, bool includeLast, bool sideBets, int maxNumGolfers)
        {
            DB DB = new DB();
            // get the scoring options
            string format = GetScoringFormat(scoringOption);
            ScoringType st = GetScoringType(format, scoringOption);
            bool addbestall345 = format.Contains("{bestall345}");

            GolfCourse course = null;

            // result table
            DataTable results = new DataTable();

            results.Columns.Add("TeamID");
            results.Columns.Add("Flight");
            results.Columns.Add("Name");
            results.Columns.Add("Round");
            results.Columns.Add("Image");
            for (int i = 1; i < 19; i++)
            {
                results.Columns.Add(i.ToString(), typeof(int));
                if (i == 9)
                    results.Columns.Add("Out", typeof(int));
            }
            results.Columns.Add("In", typeof(int));
            if (addbestall345)
            {
                results.Columns.Add("BestPar3", typeof(int));
                results.Columns.Add("BestPar4", typeof(int));
                results.Columns.Add("BestPar5", typeof(int));
            }
            results.Columns.Add("Total", typeof(int));
            if (addbestall345)
            {
                results.Columns.Add("TotalPlusBest", typeof(int));
            }
            results.Columns.Add("ToPar", typeof(int));
            results.Columns.Add("Overall", typeof(int));
            if (addbestall345)
            {
                results.Columns.Add("OverallPlusBest", typeof(int));
            }
            results.Columns.Add("Overall ToPar");

            for (int x = 0; x < maxNumGolfers; x++)
            {
                results.Columns.Add("Player" + x.ToString());
                results.Columns.Add("PlayerGross" + x.ToString());
                results.Columns.Add("PlayerHCP" + x.ToString());
                results.Columns.Add("PlayerNet" + x.ToString());
                results.Columns.Add("PlayerSC" + x.ToString());
            }

            System.Collections.Specialized.NameValueCollection ppformula = new System.Collections.Specialized.NameValueCollection();
            if (st == ScoringType.GrossParPoints || st == ScoringType.NetParPoints)
            {
                if (format.Contains("{pp:"))
                {
                    int start = format.IndexOf("{pp:");
                    int end = format.IndexOf("}", start);
                    string[] formulavals = format.Substring(start + 4, end - (start + 4)).Split(',');
                    for (int i = 0; i < formulavals.Length; i++)
                        ppformula.Add(formulavals[i].Split('=')[0], formulavals[i].Split('=')[1]);
                }
            }

            // get generic best ball option to determine team filter
            BestBallOption bboPar3 = GetBestBallOption("par3", format, "");
            BestBallOption bboPar4 = GetBestBallOption("par4", format, "");
            BestBallOption bboPar5 = GetBestBallOption("par5", format, "");

            string teamfilter = "";
            if (bboPar3 == BestBallOption.Individual && bboPar4 == BestBallOption.Individual && bboPar5 == BestBallOption.Individual)
            {
                teamfilter = "UserID";
            }
            else
            {
                teamfilter = "TeamID";
            }

            string teamssql = "select " + teamfilter + ", NumGolfers = Count(UserID) from mg_tourneyTeamPlayers where TournamentID=" +
               tourney.TournamentID.ToString() + " Group By " + teamfilter;

            DataSet dsTeams = DB.GetDataSet(teamssql);

            //DataSet dsScores = DB.GetDataSet(
            //   "SELECT tp.TeamID, t.TeamName, t.Flight, tp.TeeNumber, tp.Handicap AS TPHandicap, ts.*, u.LastName, u.FirstName, u.Image FROM " +
            //   "(((mg_tourneyTeamPlayers AS tp " +
            //   "INNER JOIN mg_tourneyTeams AS t on t.TeamID = tp.TeamID) " +
            //   "INNER JOIN mg_tourneyUsers AS u on u.userid = tp.UserID) " +
            //   "LEFT OUTER JOIN mg_TourneyScores AS ts on tp.UserID = ts.UserID AND tp.TournamentID = ts.TournamentID) " +
            //   "WHERE tp.TournamentID = " + tourney.TournamentID.ToString() + " ORDER BY ts.RoundNumber, tp.TeamID, u.LastName, u.FirstName");
            string q = "SELECT tp.TeamID, tp.UserId AS TPUserId, t.TeamName, t.Flight, tp.TeeNumber, tp.Handicap AS TPHandicap, ts.*, u.LastName, u.FirstName, u.Image" +
                " FROM mg_tourneyTeamPlayers AS tp" +
                " INNER JOIN mg_tourneyTeams AS t on t.TeamID = tp.TeamID AND " + (sideBets ? "SideBet = 1" : "TourneyTeam = 1") +
                " INNER JOIN mg_tourneyUsers AS u on u.userid = tp.UserID" +
                " LEFT OUTER JOIN mg_TourneyScores AS ts on u.WebID = ts.UserID AND tp.TournamentID = ts.TourneyId" +
                " WHERE tp.TournamentID = " + tourney.TournamentID.ToString() + " ORDER BY ts.RoundNum, tp.TeamID, u.LastName, u.FirstName";
            
            DataSet dsScores = DB.GetDataSet(q);

            DataRow curResultRow = null;
            int overalltotal = 0;
            int overalltotalplusbest = 0;
            int overallpar = 0;
            int overallstartrow = 0;
            int overallendrow = 0;
            DataTable teamTable = dsTeams.Tables[0];
            for (int x = 0; x < teamTable.Rows.Count; x++)
            {
                overallstartrow = results.Rows.Count;
                overallendrow = overallstartrow;
                int numGolfers = maxNumGolfers;
                DataRow teamRow = teamTable.Rows[x];
                if (!teamRow.IsNull("NumGolfers"))
                {
                    if (!int.TryParse(teamRow["NumGolfers"].ToString(), out numGolfers))
                    {
                        numGolfers = maxNumGolfers;
                    }
                }

                for (int y = 1; y <= tourney.NumberOfRounds; y++)
                {
                    dsScores.Tables[0].DefaultView.RowFilter = (teamfilter == "UserID" ? "TPUserID" : teamfilter) + "=" + teamRow[teamfilter].ToString() +
                       " and RoundNum = " + y.ToString();
                    int currentCourseID = tourney.GetCourseID(y);

                    // reset bestball options based on round
                    bboPar3 = GetBestBallOption("par3", format, y.ToString());
                    bboPar4 = GetBestBallOption("par4", format, y.ToString());
                    bboPar5 = GetBestBallOption("par5", format, y.ToString());

                    string teamName = "";
                    string images = "";
                    for (int f = 0; f < dsScores.Tables[0].DefaultView.Count; f++)
                    {
                        if (images != "") images += ";";
                        images += dsScores.Tables[0].DefaultView[f]["LastName"].ToString().Replace(" ", "") + dsScores.Tables[0].DefaultView[f]["FirstName"].ToString().Replace(" ", "") + ".png";
                        if (teamName.Length > 0) teamName += " - ";
                        teamName += dsScores.Tables[0].DefaultView[f]["LastName"].ToString();
                    }

                    if (dsScores.Tables[0].DefaultView.Count > 0)
                    {
                        curResultRow = results.NewRow();
                        overallendrow++;

                        results.Rows.Add(curResultRow);
                        if (teamfilter == "UserID")
                            curResultRow["Name"] = dsScores.Tables[0].DefaultView[0]["LastName"].ToString() + ", " +
                                dsScores.Tables[0].DefaultView[0]["FirstName"].ToString();
                        else
                            curResultRow["Name"] = teamName;
                        curResultRow["Flight"] = dsScores.Tables[0].DefaultView[0]["Flight"].ToString();
                        curResultRow["Round"] = dsScores.Tables[0].DefaultView[0]["RoundNum"].ToString();
                        curResultRow["TeamID"] = dsScores.Tables[0].DefaultView[0][(teamfilter == "UserID" ? "TPUserID" : teamfilter)].ToString();
                        // images of all the players
                        //for (int i = 0; i < dsScores.Tables[0].DefaultView.Count; i++)
                        //{
                        //   images += (images.Length > 0) ? ";" : "";
                        //   images += dsScores.Tables[0].DefaultView[i]["Image"].ToString();
                        //}
                        curResultRow["Image"] = images;

                        // reload the course if not loaded, courseid has changed due to a new round, or the tee number
                        // for the golfer has changed
                        if (course == null || course.ID != currentCourseID ||
                           course.TeeID != int.Parse(dsScores.Tables[0].DefaultView[0]["TeeNumber"].ToString()))
                            course = tourney.Course(tourney.GetCourseID(y), int.Parse(dsScores.Tables[0].DefaultView[0]["TeeNumber"].ToString()));

                        int total = 0;
                        int totalpar = 0;
                        int best5 = NoScoreDefault;
                        int best4 = NoScoreDefault;
                        int best3 = NoScoreDefault;

                        System.Collections.Generic.List<IndividualResult> indResults = new System.Collections.Generic.List<IndividualResult>();
                        for (int ng = 0; ng < numGolfers; ng++)
                        {
                            indResults.Add(new IndividualResult());
                        }

                        for (int j = 1; j <= 18; j++)
                        {
                            // scoring option based on holes par
                            BestBallOption bbo = (course.Hole(j).Par == 3) ?
                                  bboPar3 :
                                  ((course.Hole(j).Par == 4) ? bboPar4 : bboPar5);

                            // array for all scores of all players on the hole
                            int[] scores = new int[5];
                            for (int z = 0; z < 5; z++) scores[z] = NoScoreDefault;
                            int[] pars = new int[5];
                            for (int z = 0; z < 5; z++) pars[z] = NoScoreDefault;

                            // get the scores of all the players
                            int score;
                            for (int i = 0; i < dsScores.Tables[0].DefaultView.Count; i++)
                            {
                                // reload the course if the tee number for the golfer has changed
                                if (course.TeeID != int.Parse(dsScores.Tables[0].DefaultView[i]["TeeNumber"].ToString()))
                                    course = tourney.Course(tourney.GetCourseID(y), int.Parse(dsScores.Tables[0].DefaultView[i]["TeeNumber"].ToString()));

                                // get the score
                                if (dsScores.Tables[0].DefaultView[i]["Hole" + j.ToString()] == DBNull.Value) score = NoScoreDefault;
                                else score = int.Parse(dsScores.Tables[0].DefaultView[i]["Hole" + j.ToString()].ToString());
                                int hcp = 0;
                                string hcpToUse = "";
                                // set the handicap from the scoring table 
                                //if not there use the handicap set up for the tournament
                                if (dsScores.Tables[0].DefaultView[i]["HCP"] == DBNull.Value)
                                    hcpToUse = dsScores.Tables[0].DefaultView[i]["TPHandicap"].ToString();
                                else
                                    hcpToUse = dsScores.Tables[0].DefaultView[i]["HCP"].ToString();

                                if (score > 0)
                                {
                                    indResults[i].Gross += score;
                                    hcp = Handicap(double.Parse(hcpToUse), course.Slope);

                                    if (hcp < 10)
                                    {
                                        if ((score - course.Hole(j).Par) > 2) indResults[i].StrokeControl += (course.Hole(j).Par + 2);
                                        else indResults[i].StrokeControl += score;
                                    }
                                    else if (hcp >= 10 && hcp < 20)
                                    {
                                        if (score > 7) indResults[i].StrokeControl += 7;
                                        else indResults[i].StrokeControl += score;
                                    }
                                    else if (hcp >= 20 && hcp < 30)
                                    {
                                        if (score > 8) indResults[i].StrokeControl += 8;
                                        else indResults[i].StrokeControl += score;
                                    }
                                    else if (hcp >= 30 && hcp < 40)
                                    {
                                        if (score > 9) indResults[i].StrokeControl += 9;
                                        else indResults[i].StrokeControl += score;
                                    }
                                    else
                                    {
                                        if (score > 10) indResults[i].StrokeControl += 10;
                                        else indResults[i].StrokeControl += score;
                                    }

                                    switch (st)
                                    {
                                        case ScoringType.GrossParPoints:
                                            score = ParPoints(score, course.Hole(j).Par, ppformula);
                                            break;
                                        case ScoringType.NetParPoints:
                                            score = NetScore(hcp, course.Hole(j).Handicap, score);
                                            score = ParPoints(score, course.Hole(j).Par, ppformula);
                                            break;
                                        case ScoringType.Net:
                                            score = NetScore(hcp, course.Hole(j).Handicap, score);
                                            break;
                                    }
                                    scores[i] = score;
                                    // add to the total par
                                    pars[i] = course.Hole(j).Par;
                                }
                                if (j == 1)
                                {
                                    if (i < indResults.Count)
                                    {
                                        indResults[i].Name = dsScores.Tables[0].DefaultView[i]["FirstName"].ToString() + " " +
                                            dsScores.Tables[0].DefaultView[i]["LastName"].ToString();
                                        indResults[i].HCP = hcp;
                                    }
                                }
                            }

                            // get best scores based on option
                            score = 0;
                            if (bbo == BestBallOption.All || bbo == BestBallOption.Individual)
                            {
                                for (int z = 0; z < 5; z++)
                                {
                                    score += ((scores[z] == NoScoreDefault) ? 0 : scores[z]);
                                    totalpar += ((pars[z] == NoScoreDefault) ? 0 : pars[z]);
                                }
                            }
                            else
                            {
                                if (addbestall345)
                                {
                                    for (int z = 0; z < 5; z++)
                                    {
                                        score += ((scores[z] == NoScoreDefault) ? 0 : scores[z]);
                                    }
                                    if (course.Hole(j).Par == 3)
                                    {
                                        best3 = GetBestScore(st, best3, score);
                                    }
                                    else if (course.Hole(j).Par == 5)
                                    {
                                        best5 = GetBestScore(st, best5, score);
                                    }
                                    else
                                    {
                                        best4 = GetBestScore(st, best4, score);
                                    }
                                    score = 0;
                                }
                                int nget = 1;
                                switch (bbo)
                                {
                                    case BestBallOption.BestOne:
                                        nget = 1;
                                        break;
                                    case BestBallOption.BestTwo:
                                        nget = 2;
                                        break;
                                    case BestBallOption.BestThree:
                                        nget = 3;
                                        break;
                                }

                                // get the best scores based on the number of holes that count

                                for (int m = 0; m < nget; m++)
                                {
                                    int bestpos = -1;
                                    for (int z = 0; z < 5; z++)
                                    {
                                        if (scores[z] != NoScoreDefault)
                                        {
                                            if (st == ScoringType.GrossParPoints || st == ScoringType.NetParPoints)
                                            {
                                                if (bestpos == -1 || scores[z] > scores[bestpos])
                                                {
                                                    bestpos = z;
                                                }
                                            }
                                            else
                                            {
                                                if (bestpos == -1 || scores[z] < scores[bestpos])
                                                {
                                                    bestpos = z;
                                                }
                                            }
                                        }
                                    }
                                    if (bestpos > -1 && scores[bestpos] != NoScoreDefault)
                                    {
                                        score += scores[bestpos];
                                        scores[bestpos] = NoScoreDefault;
                                        totalpar += pars[bestpos];
                                    }
                                }
                            }

                            curResultRow[j.ToString()] = score;
                            total += score;
                            if (j == 9)
                            {
                                curResultRow["Out"] = total;
                            }
                            if (j == 18)
                            {
                                curResultRow["In"] = (total - int.Parse(curResultRow["Out"].ToString()));
                            }
                        }
                        if (addbestall345)
                        {
                            curResultRow["BestPar3"] = best3;
                            curResultRow["BestPar4"] = best4;
                            curResultRow["BestPar5"] = best5;
                            curResultRow["TotalPlusBest"] = total + best3 + best4 + best5;
                            overalltotalplusbest += total + best3 + best4 + best5;
                        }
                        curResultRow["Total"] = total;
                        curResultRow["ToPar"] = totalpar;

                        for (int resI = 0; resI < indResults.Count; resI++)
                        {
                            curResultRow["Player" + resI.ToString()] = indResults[resI].Name;
                            curResultRow["PlayerHCP" + resI.ToString()] = indResults[resI].HCP;
                            curResultRow["PlayerGross" + resI.ToString()] = indResults[resI].Gross;
                            curResultRow["PlayerSC" + resI.ToString()] = indResults[resI].StrokeControl;
                            curResultRow["PlayerNet" + resI.ToString()] = indResults[resI].Gross - indResults[resI].HCP;
                        }
                        overalltotal += total;
                        overallpar += totalpar;
                    }
                }

                // add the totals
                if (throwOutWorst > 0)
                {
                    // reset the totals
                    overalltotal = 0;
                    overalltotalplusbest = 0;
                    overallpar = 0;

                    // throw out the score if not wanted
                    int[] scoresToKeep = new int[tourney.NumberOfRounds];
                    int[] parsToKeep = new int[tourney.NumberOfRounds];

                    int scorePos = 0;
                    for (int y = overallstartrow; y < overallendrow; y++)
                    {
                        scoresToKeep[scorePos] = int.Parse(results.Rows[y]["Total"].ToString());
                        parsToKeep[scorePos] = int.Parse(results.Rows[y]["ToPar"].ToString());
                        scorePos++;
                    }

                    // zero out the unwanted scores
                    int checkAgainst = (includeLast) ? scoresToKeep.Length - 1 : scoresToKeep.Length;
                    for (int y = 0; y < scoresToKeep.Length; y++)
                    {
                        if (y < checkAgainst)
                        {
                            int betterThan = 0;
                            for (int z = 0; z < checkAgainst; z++)
                            {
                                if (st == ScoringType.GrossParPoints || st == ScoringType.NetParPoints)
                                {
                                    if (scoresToKeep[y] > scoresToKeep[z] || scoresToKeep[z] == 0)
                                        betterThan++;
                                }
                                else
                                {
                                    if (scoresToKeep[y] < scoresToKeep[z] || scoresToKeep[z] == 0)
                                        betterThan++;
                                }
                            }
                            if (betterThan < throwOutWorst)
                            {
                                scoresToKeep[y] = 0;
                                parsToKeep[y] = 0;
                            }
                        }

                        // add the scores to the overall
                        overalltotal += scoresToKeep[y];
                        overallpar += parsToKeep[y];
                    }

                    // set the Overalls in the results
                    for (int y = overallstartrow; y < overallendrow; y++)
                    {
                        results.Rows[y]["Overall"] = overalltotal;
                        if (addbestall345)
                        {
                            results.Rows[y]["OverallPlusBest"] = overalltotalplusbest;
                        }
                        results.Rows[y]["Overall ToPar"] = overallpar.ToString();
                    }

                }
                else
                {
                    for (int y = overallstartrow; y < overallendrow; y++)
                    {
                        results.Rows[y]["Overall"] = overalltotal;
                        if (addbestall345)
                        {
                            results.Rows[y]["OverallPlusBest"] = overalltotalplusbest;
                        }
                        results.Rows[y]["Overall ToPar"] = overallpar.ToString();
                    }
                }

                overalltotal = 0;
                overalltotalplusbest = 0;
                overallpar = 0;
            }
            DB.Close();
            return results;
        }
    }
}
