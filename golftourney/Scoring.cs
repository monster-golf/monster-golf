using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace MonsterGolf
{
	/// <summary>
	/// Summary description for Scoring.
	/// </summary>
	public class Scoring : System.Windows.Forms.Form
	{
      private System.Windows.Forms.Label[] GolferName;
      private System.Windows.Forms.TextBox[] Tee;
      private System.Windows.Forms.TextBox[,] Holes;
      private System.Windows.Forms.Label[] Front9;
      private System.Windows.Forms.Label[] Back9;
      private System.Windows.Forms.Label[] HCP;
      private System.Windows.Forms.Label[] Total;
      private System.Windows.Forms.Label[] LabelHoles;
      private System.Windows.Forms.Label[,] LabelPars;
      private System.Windows.Forms.Label[,] LabelHCPs;
      private System.Windows.Forms.Label LabelGolfer = null;
      private System.Windows.Forms.Label LabelHCP = null;
      private System.Windows.Forms.Label LabelIn = null;
      private System.Windows.Forms.Label LabelOut = null;
      private System.Windows.Forms.Label LabelTotal = null;
      private System.Windows.Forms.Label LabelCourse = null;
      private System.Windows.Forms.Label LabelTourney = null;
      private System.Windows.Forms.Label LabelTee = null;
      private Tournament m_tournament = null;
      private FormHelper formHelp =  null;
      private System.Windows.Forms.ComboBox GolfersList;
      private System.Windows.Forms.Button ClearScores;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.ComboBox Rounds;
      private System.Windows.Forms.Button ButtonSave;
      private int currentRow = 0;
      private int _currentCourseID = -1;
      private System.Windows.Forms.Button buttonResults;
      private System.Windows.Forms.Button buttonCopyTo;
      private ScrollScores scrollScores1;
      private bool _fillingScores = false;
      private int _scoreRows = 30;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Scoring(int TournamentID)
		{
			//
			// Required for Windows Form Designer support
			//
         formHelp = new FormHelper();
			InitializeComponent();
         m_tournament = new Tournament(TournamentID);
         DataSet golfers = m_tournament.GolfersDataSet();
         if (m_tournament.NumberOfCourses < 1 || m_tournament.NumberOfRounds < 1 ||
            golfers == null || golfers.Tables[0].Rows.Count < 1)
         {
            MessageBox.Show("Courses and Golfers must be entered in order to do scoring.");
            this.Close();
            return;
         }
         this.Text = m_tournament.Name + " - Scoring";
         this.FillTournament();
         this.DrawHoleLabels();
         this.DrawHoles();
         int numRounds = m_tournament.NumberOfRounds;
         ListItem rounditem;
         int roundnum;
         for (int x=0; x<numRounds; x++) {
            roundnum = x+1;
            rounditem = new ListItem(x.ToString(), roundnum.ToString());
            Rounds.Items.Add(rounditem);
         }
         Rounds.SelectedIndex = 0;
         //scrollScores1.ScoringType = Formulas.ScoringType.Gross;
         //scrollScores1.Results = Formulas.CalculateScores(m_tournament, "", 0, false);
		}
      public void FillTournament() 
      {
         this.SuspendLayout();
         ListItem blank = new ListItem("", "-- Select --");
         GolfersList.Sorted = true;
         GolfersList.Items.Add(blank);
         GolfersList.SelectedIndex = 0;

         ListItem listteam;

         DataSet ds = m_tournament.GolfersDataSet();
         ds.Tables[0].DefaultView.Sort = "TeamID";
         string curTeam = "-1";
         string lastTeam = "-1";
         string teamName = "";
         for (int i=0; i < ds.Tables[0].DefaultView.Count; i++ )
         {
            curTeam = ds.Tables[0].DefaultView[i]["TeamID"].ToString();
            
            if (curTeam != lastTeam && lastTeam != "-1")
            {
               listteam = new ListItem(lastTeam, teamName);
               GolfersList.Items.Add(listteam);
               teamName = "";
            }

            if (teamName != "") teamName += " - ";
            teamName += ds.Tables[0].DefaultView[i]["LastName"].ToString() + ", " + ds.Tables[0].DefaultView[i]["FirstName"].ToString();
            
            lastTeam = curTeam;
         }

         if (lastTeam != "-1")
         {
            listteam = new ListItem(lastTeam, teamName);
            GolfersList.Items.Add(listteam);
         }

         ds.Tables[0].DefaultView.Sort = "";

         formHelp.CreateLabel(ref this.LabelTourney, (8 * m_tournament.Name.Length) + 5, 20, 5, 5, ContentAlignment.MiddleLeft, Color.Transparent, m_tournament.Name);
         this.Controls.Add(this.LabelTourney);
         this.ResumeLayout(false);
      }

      private void RemoveControl(string controlname)
      {
         for ( int i=0; i<Controls.Count; i++ )
         {
            if ( this.Controls[i].Name ==  controlname )
            {
               this.Controls.Remove(this.Controls[i]);
               break;
            }
         }
      }

      private void RemoveCourse(int currCourseID) 
      {
         if ( currCourseID < 0 )
            return;

         GolfCourse currCourse = null;
         int numTees = m_tournament.NumberOfTees(currCourseID); 

         for (int x = 0; x<numTees; x++) 
         {
            currCourse = m_tournament.Course(currCourseID,x);
            if (x==0) 
            {
               RemoveControl("Course" + currCourse.Name);
            }
            
            for (int y=0; y<20; y++) 
            {
               RemoveControl("LabelHcp" + x.ToString() + y.ToString());
               RemoveControl("LabelPar" + x.ToString() + y.ToString());
             
               if (y==19)  
               {
                  RemoveControl("LabelHcp" + x.ToString() + (y+1).ToString());
                  RemoveControl("LabelPar" + x.ToString() + (y+1).ToString());
               }
            }
         }
      }

      public void FillCourse(int currCourseID) 
      {
         this.SuspendLayout();
         if ( this._currentCourseID != currCourseID )
            RemoveCourse(this._currentCourseID);
         int numTees = m_tournament.NumberOfTees(currCourseID); 
         LabelHCPs = new Label[numTees,21];
         LabelPars = new Label[numTees,21];
         GolfCourse currCourse = null;
         ContentAlignment align = ContentAlignment.MiddleCenter;
         Color yellow = Color.Yellow;
         Color green =  Color.YellowGreen;
         Color currColor = Color.Black;
         int toppoint = 0;
         int leftpoint = 0;
         int teetext = 0;

         for (int x = 0; x<numTees; x++) 
         {
            if (x%2 == 1)
               currColor = yellow;
            else
               currColor = green;
            toppoint = 80 + (_scoreRows * 20) + (x * 40);
            leftpoint = 0;

            currCourse = m_tournament.Course(currCourseID,x);
            if (x==0) 
            {
                formHelp.CreateLabel(ref this.LabelCourse, (8 * currCourse.Name.Length) + 5, 20, (8 * m_tournament.Name.Length) + 15, 5, ContentAlignment.MiddleLeft, Color.Transparent, currCourse.Name, "Course" + currCourse.Name);
                this.Controls.Add(this.LabelCourse);
            }
            
            for (int y=0; y<20; y++) 
            {
               if (y==0) 
               {
                  teetext = currCourse.TeeID + 1;
                  formHelp.CreateLabel(ref LabelHCPs[x,y], 260, 20, 5, toppoint, ContentAlignment.MiddleLeft, currColor, "Tee " + teetext.ToString() + " Handicaps", "LabelHcp" + x.ToString() + y.ToString() );
                  formHelp.CreateLabel(ref LabelPars[x, y], 260, 20, 5, toppoint + 20, ContentAlignment.MiddleLeft, currColor, "Tee " + teetext.ToString() + " Pars", "LabelPar" + x.ToString() + y.ToString());
                  this.Controls.Add(LabelHCPs[x,y]);
                  this.Controls.Add(LabelPars[x,y]);
               }
               else if (y==19)  
               {
                  formHelp.CreateLabel(ref this.LabelHCPs[x,y], 25, 20, 445, toppoint, align, currColor, "", "LabelHcp" + x.ToString() + y.ToString());
                  formHelp.CreateLabel(ref this.LabelPars[x, y], 25, 20, 445, toppoint + 20, align, currColor, "", "LabelPar" + x.ToString() + y.ToString());
                  formHelp.CreateLabel(ref this.LabelHCPs[x,y+1], 60, 20, 650, toppoint, align, currColor, "", "LabelHcp" + x.ToString() + (y+1).ToString());
                  formHelp.CreateLabel(ref this.LabelPars[x, y + 1], 60, 20, 650, toppoint + 20, align, currColor, "", "LabelPar" + x.ToString() + (y + 1).ToString());
                  this.Controls.Add(LabelHCPs[x,y]);
                  this.Controls.Add(LabelPars[x,y]);
                  LabelHCPs[x,y+1].Text = currCourse.Rating.ToString() + " - " + currCourse.Slope.ToString();
                  this.Controls.Add(LabelHCPs[x,y+1]);
                  this.Controls.Add(LabelPars[x,y+1]);
               }
               else 
               {
                  leftpoint = 265 + ((y-1)*20);
                  if (y >= 10) leftpoint += 25;
                  formHelp.CreateLabel(ref this.LabelHCPs[x,y], 20, 20, leftpoint, toppoint, align, currColor, currCourse.Hole(y).Handicap.ToString(), "LabelHcp" + x.ToString() + y.ToString());
                  formHelp.CreateLabel(ref this.LabelPars[x,y], 20, 20, leftpoint, toppoint+20, align, currColor, currCourse.Hole(y).Par.ToString(), "LabelPar" + x.ToString() + y.ToString());
                  this.Controls.Add(LabelHCPs[x,y]);
                  this.Controls.Add(LabelPars[x,y]);
               }
            }
         }
         this._currentCourseID = currCourseID;
         this.ResumeLayout(false);
      }
      private void DrawHoleLabels() 
      {
         this.SuspendLayout();
         this.LabelHoles = new System.Windows.Forms.Label[18];
         int leftpoint;
         int text = 0;
         ContentAlignment align = ContentAlignment.MiddleCenter;
         Color color = Color.YellowGreen;

         for (int x=0; x<18; x++) 
         {
            leftpoint = 265 + (x*20);
            if (x >= 9) leftpoint += 25;
            text = x+1;
            formHelp.CreateLabel(ref this.LabelHoles[x], 20, 20, leftpoint, 60, align, color, text.ToString());
            this.Controls.Add(this.LabelHoles[x]);
         }

         formHelp.CreateLabel(ref this.LabelGolfer, 200, 20, 5, 60, ContentAlignment.MiddleLeft, color, "Golfer");
         this.Controls.Add(this.LabelGolfer);
         formHelp.CreateLabel(ref this.LabelTee, 30, 20, 205, 60, ContentAlignment.MiddleCenter, color, "Tee");
         this.Controls.Add(this.LabelTee);
         formHelp.CreateLabel(ref this.LabelHCP, 30, 20, 235, 60, align, color, "HCP");
         this.Controls.Add(this.LabelHCP);
         formHelp.CreateLabel(ref this.LabelOut, 25, 20, 445, 60, align, color, "Out");
         this.Controls.Add(this.LabelOut);
         formHelp.CreateLabel(ref this.LabelIn, 25, 20, 650, 60, align, color, "In");
         this.Controls.Add(this.LabelIn);
         formHelp.CreateLabel(ref this.LabelTotal, 35, 20, 675, 60, align, color, "Total");
         this.Controls.Add(this.LabelTotal);
         this.ResumeLayout(false);
      }
      public void FillScores(int row, int[] scores) 
      {
         _fillingScores = true;
         for (int x=0; x<18; x++) 
         {
            if (scores[x] != 0)
               Holes[row,x].Text = scores[x].ToString();
            else
               Holes[row,x].Text = "";
         }
         _fillingScores = false;
      }
      public void LoadTeam(string teamidstr) 
      {
         int teamid = System.Convert.ToInt32(teamidstr);
         Team team = m_tournament.GetTeam(teamid);
         bool setfocus = true;
         foreach (Golfer golfer in team.Golfers()) 
         {
            LoadGolfer(golfer, setfocus);
            setfocus = false;
         }
      }
      public void LoadGolfer(int id, bool setfocus) 
      {
         Golfer golfer = new Golfer(id);
         LoadGolfer(golfer, setfocus);
      }

      private bool _fillGolfers = false;
      public void LoadGolfer(Golfer golfer, bool setfocus) 
      {
          _fillGolfers = true;
         bool inlist = false;
         for (int y = 0; y < _scoreRows; y++)
         {
             if (GolferName[y].Text == "")
             {
                 break;
             }
             if (GolferName[y].Name == golfer.ID.ToString())
             {
                 if (setfocus)
                     Holes[y, 0].Focus();
                 inlist = true;
                 break;
             }
         }

         if (!inlist) 
         {
            golfer.TournamentID = m_tournament.TournamentID;
            golfer.RoundNumber = int.Parse(((ListItem)Rounds.Items[Rounds.SelectedIndex]).Name);

            int courseid = m_tournament.GetCourseID(golfer.RoundNumber);

            golfer.LoadGolfersScore();
            GolferName[currentRow].Text = golfer.FirstName + " " + golfer.LastName;
            GolferName[currentRow].Name = golfer.ID.ToString();
            Tee[currentRow].Text = (golfer.Tee + 1).ToString();
            HCP[currentRow].Name = golfer.HcpIndex.ToString();
            HCP[currentRow].Text = Formulas.Handicap(golfer.HcpIndex, m_tournament.Course(courseid,golfer.Tee).Slope).ToString();
            FillScores(currentRow, golfer.Scores);
            if (setfocus)
               Holes[currentRow,0].Focus();
            currentRow += 1;
            if (currentRow == _scoreRows)
               currentRow = 0;
         }
         _fillGolfers = false;
      }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

      private void DrawHoles() 
      {
         this.SuspendLayout();
         int toppoint = 0;
         this.GolferName = new System.Windows.Forms.Label[_scoreRows];
         this.Tee = new System.Windows.Forms.TextBox[_scoreRows];
         this.HCP = new System.Windows.Forms.Label[_scoreRows];
         this.Front9 = new System.Windows.Forms.Label[_scoreRows];
         this.Back9 = new System.Windows.Forms.Label[_scoreRows];
         this.Total = new System.Windows.Forms.Label[_scoreRows];
         Color color = Color.Beige;

         for (int x=0; x<_scoreRows; x++) 
         {
            toppoint = 80+(x*20);
            
            formHelp.CreateLabel(ref this.GolferName[x], 200, 20, 5, toppoint, ContentAlignment.MiddleLeft, color, "");
            this.Controls.Add(this.GolferName[x]);
            formHelp.CreateNumberBox(ref this.Tee[x], "Tee[" + x + "]", -1, 205, toppoint, 20, Color.White, HorizontalAlignment.Center, "");
            this.Tee[x].LostFocus +=new EventHandler(Tee_LostFocus);
            this.Tee[x].Width = 30;
            //this.Tee[x].BorderStyle = BorderStyle.None;

            this.Controls.Add(this.Tee[x]);
            formHelp.CreateLabel(ref this.HCP[x], 30, 20, 235, toppoint, ContentAlignment.MiddleCenter, color, "");
            this.Controls.Add(this.HCP[x]);
            formHelp.CreateLabel(ref this.Front9[x], 25, 20, 445, toppoint, ContentAlignment.MiddleCenter, color, "");
            this.Controls.Add(this.Front9[x]);
            formHelp.CreateLabel(ref this.Back9[x], 25, 20, 650, toppoint, ContentAlignment.MiddleCenter, color, "");
            this.Controls.Add(this.Back9[x]);
            formHelp.CreateLabel(ref this.Total[x], 35, 20, 675, toppoint, ContentAlignment.MiddleCenter, color, "");
            this.Controls.Add(this.Total[x]);
         }

         this.Holes = new TextBox[_scoreRows,18];
         int leftpoint = 0;
         for (int y=0; y<_scoreRows; y++) 
         {
            leftpoint = 0;
            toppoint = 80 + (y * 20);

            for (int x=0; x<18; x++) 
            {
               leftpoint = 265 + (x*20);
               if (x >= 9) leftpoint += 25;
              formHelp.CreateNumberBox(ref this.Holes[y, x], "Holes[" + y + "," + x + "]", (y * 18) + x + 1, leftpoint, toppoint, 20, Color.White, HorizontalAlignment.Center, "");
               this.Holes[y,x].TextChanged +=new EventHandler(Scoring_TextChanged);
               this.Holes[y,x].GotFocus +=new EventHandler(Scoring_GotFocus);
               this.Holes[y,x].MouseUp +=new MouseEventHandler(Scoring_MouseUp);
               this.Controls.Add(this.Holes[y,x]);
            }
         }
         this.ResumeLayout(false);
      }
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Scoring));
            this.ButtonSave = new System.Windows.Forms.Button();
            this.GolfersList = new System.Windows.Forms.ComboBox();
            this.ClearScores = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Rounds = new System.Windows.Forms.ComboBox();
            this.buttonResults = new System.Windows.Forms.Button();
            this.buttonCopyTo = new System.Windows.Forms.Button();
            this.scrollScores1 = new MonsterGolf.ScrollScores();
            this.SuspendLayout();
            // 
            // ButtonSave
            // 
            this.ButtonSave.BackColor = System.Drawing.Color.Firebrick;
            this.ButtonSave.ForeColor = System.Drawing.Color.White;
            this.ButtonSave.Location = new System.Drawing.Point(471, 31);
            this.ButtonSave.Name = "ButtonSave";
            this.ButtonSave.Size = new System.Drawing.Size(80, 23);
            this.ButtonSave.TabIndex = 0;
            this.ButtonSave.TabStop = false;
            this.ButtonSave.Text = "Save Scores";
            this.ButtonSave.UseVisualStyleBackColor = false;
            this.ButtonSave.Click += new System.EventHandler(this.ButtonSave_Click);
            // 
            // GolfersList
            // 
            this.GolfersList.Location = new System.Drawing.Point(3, 31);
            this.GolfersList.MaxDropDownItems = 20;
            this.GolfersList.Name = "GolfersList";
            this.GolfersList.Size = new System.Drawing.Size(352, 21);
            this.GolfersList.TabIndex = 0;
            this.GolfersList.TabStop = false;
            this.GolfersList.SelectedIndexChanged += new System.EventHandler(this.GolfersList_SelectedIndexChanged);
            // 
            // ClearScores
            // 
            this.ClearScores.Location = new System.Drawing.Point(557, 31);
            this.ClearScores.Name = "ClearScores";
            this.ClearScores.Size = new System.Drawing.Size(80, 23);
            this.ClearScores.TabIndex = 0;
            this.ClearScores.TabStop = false;
            this.ClearScores.Text = "Clear Scores";
            this.ClearScores.Click += new System.EventHandler(this.ClearScores_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(371, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Round";
            // 
            // Rounds
            // 
            this.Rounds.Location = new System.Drawing.Point(414, 31);
            this.Rounds.Name = "Rounds";
            this.Rounds.Size = new System.Drawing.Size(40, 21);
            this.Rounds.TabIndex = 0;
            this.Rounds.TabStop = false;
            this.Rounds.SelectedIndexChanged += new System.EventHandler(this.Rounds_SelectedIndexChanged);
            // 
            // buttonResults
            // 
            this.buttonResults.Location = new System.Drawing.Point(557, 2);
            this.buttonResults.Name = "buttonResults";
            this.buttonResults.Size = new System.Drawing.Size(80, 23);
            this.buttonResults.TabIndex = 0;
            this.buttonResults.TabStop = false;
            this.buttonResults.Text = "View Results";
            this.buttonResults.Click += new System.EventHandler(this.buttonResults_Click);
            // 
            // buttonCopyTo
            // 
            this.buttonCopyTo.Location = new System.Drawing.Point(471, 2);
            this.buttonCopyTo.Name = "buttonCopyTo";
            this.buttonCopyTo.Size = new System.Drawing.Size(80, 23);
            this.buttonCopyTo.TabIndex = 0;
            this.buttonCopyTo.TabStop = false;
            this.buttonCopyTo.Text = "Copy To";
            this.buttonCopyTo.Click += new System.EventHandler(this.buttonCopyTo_Click);
            // 
            // scrollScores1
            // 
            this.scrollScores1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.scrollScores1.BackColor = System.Drawing.Color.LightGray;
            this.scrollScores1.Location = new System.Drawing.Point(3, 220);
            this.scrollScores1.Name = "scrollScores1";
            this.scrollScores1.ScrollPlayers = false;
            this.scrollScores1.Size = new System.Drawing.Size(743, 42);
            this.scrollScores1.TabIndex = 0;
            this.scrollScores1.TabStop = false;
            this.scrollScores1.Visible = false;
            // 
            // Scoring
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(748, 634);
            this.Controls.Add(this.buttonCopyTo);
            this.Controls.Add(this.buttonResults);
            this.Controls.Add(this.Rounds);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ClearScores);
            this.Controls.Add(this.GolfersList);
            this.Controls.Add(this.ButtonSave);
            this.Controls.Add(this.scrollScores1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Scoring";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Monster Golf Tournament Scoring";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Resize += new System.EventHandler(this.OnSize);
            this.ResumeLayout(false);

      }
      #endregion

      private void UpdateScores(int rowindex) 
      {
         int score = 0;
         int total = 0;
         int front = 0;
         int back = 0;
         for (int x=0; x<18; x++) 
         {
            if (Holes[rowindex,x].Text != "")  
            {
               score = int.Parse(Holes[rowindex,x].Text);
               total += score;
               if (x < 9)
                  front += score;
               else
                  back += score;
            }
         }
         
         this.Front9[rowindex].Text =  front.ToString();
         this.Back9[rowindex].Text = back.ToString();
         this.Total[rowindex].Text = total.ToString();
      }

      private void Scoring_TextChanged(object sender, EventArgs e)
      {
         TextBox thistext = (TextBox)sender;
         try 
         {
            int currScore = int.Parse(thistext.Text);
         
            if (currScore != 1) 
            {
               string[] name = thistext.Name.Split(',');
               name[0] = name[0].Replace("Holes[",  "");
               UpdateScores(int.Parse(name[0]));
               // leaving the 18 hole save to the database
               if (name[1].StartsWith("17") && !_fillingScores) 
                  this.ButtonSave_Click(null, null);

               if (!_fillGolfers)
               {
                   this.SelectNextControl(thistext, true, true, true, true);
               }
            }
         } 
         catch 
         {
            thistext.Text = "";
            string[] name = thistext.Name.Split(',');
            name[0] = name[0].Replace("Holes[",  "");
            this.Front9[int.Parse(name[0])].Text = "";
            this.Back9[int.Parse(name[0])].Text = "";
            this.Total[int.Parse(name[0])].Text = "";
            this.Cursor = Cursors.Default;
            // do nothing for invalid score;
         }
      }

      private void ButtonSave_Click(object sender, System.EventArgs e)
      {
         this.Cursor = Cursors.WaitCursor;
         Golfer golfer;
         int[] scores = new int[18];

         for (int y=0; y<_scoreRows; y++) 
         {
            if (GolferName[y].Text != "") 
            {
               golfer = new Golfer(int.Parse(GolferName[y].Name));
               golfer.Tee = int.Parse(Tee[y].Text) - 1;
               golfer.RoundNumber = int.Parse(((ListItem)Rounds.Items[Rounds.SelectedIndex]).Name);
               golfer.TournamentID = m_tournament.TournamentID;
               // OVERRIDE the handicap with the handicap for this round. 
               // DO THIS AFTER golfer.TournamentID which also sets the handicap
               golfer.HcpIndex = double.Parse(HCP[y].Name);
               int currScore = 0;
               for (int x=0; x<18; x++) 
               {
                  try 
                  {
                     currScore = int.Parse(Holes[y,x].Text);
                     scores[x] = currScore;
                  }
                  catch 
                  {
                     scores[x] = 0;
                  }
               }
               golfer.Scores = scores;
               golfer.SaveScores();
            }
         }
         this.Cursor = Cursors.Default;
         //scrollScores1.Results = Formulas.CalculateScores(this.m_tournament, "", 0, false);
      }

      private void GolfersList_SelectedIndexChanged(object sender, System.EventArgs e)
      {
         this.Cursor = Cursors.WaitCursor;
         ListItem selected = (ListItem)GolfersList.Items[GolfersList.SelectedIndex];
         if (selected.ID != "") 
         {
            int teamid = -1;
            try 
            {
               teamid = int.Parse(selected.ID);
               LoadTeam(selected.ID);
            }
            catch 
            {
               MessageBox.Show("Unable to show the golfers for the selected team.");
               //LoadGolfer(int.Parse(selected.ID), true);
            }
         }
         this.Cursor = Cursors.Default;
      }

      private void ClearScores_Click(object sender, System.EventArgs e)
      {
         this.Cursor = Cursors.WaitCursor;
         for (int x=0; x<_scoreRows; x++)
         {
            GolferName[x].Name = "";
            GolferName[x].Text = "";
            HCP[x].Name = "";
            HCP[x].Text = "";
            Front9[x].Text = "";
            Back9[x].Text = "";
            Total[x].Text = "";
            Tee[x].Text = "";
            
            for (int y=0; y<18; y++) 
               Holes[x,y].Text = "";

            currentRow = 0;
         }
         this.Cursor = Cursors.Default;
      }

      private void Rounds_SelectedIndexChanged(object sender, System.EventArgs e)
      {
         this.Cursor = Cursors.WaitCursor;
         string round = ((ListItem)Rounds.Items[Rounds.SelectedIndex]).Name;
         if (round != "") 
         {
            this.GolfersList.SelectedIndex = 0;
            ClearScores_Click(null,  null);
            FillCourse(m_tournament.GetCourseID(int.Parse(round)));
         }
         this.Cursor = Cursors.Default;
      }

      private void buttonResults_Click(object sender, System.EventArgs e)
      {
         ButtonSave_Click(null,null);
         Results res = new Results(m_tournament.TournamentID);
         res.MdiParent = this.MdiParent;
         res.Show();
         res.Focus();
      }

      public Tournament Tournament
      {
         get { return m_tournament; }
      }

      private void buttonCopyTo_Click(object sender, System.EventArgs e)
      {
         CopyScores cs = new CopyScores(this);
         cs.MdiParent = this.MdiParent;
         cs.Show();
         cs.Focus();
      }

      public int SelectedRound()
      {
         int round = 1;
         if ( Rounds.SelectedIndex != -1 )
         {
            try
            {
               round = int.Parse(((ListItem)Rounds.Items[Rounds.SelectedIndex]).Name);
            } 
            catch {}
         }
         return round;
      }

      private void Scoring_GotFocus(object sender, EventArgs e)
      {
         TextBox thistext = (TextBox)sender;
         thistext.SelectAll();
      }

      private void Scoring_MouseUp(object sender, MouseEventArgs e)
      {
         TextBox thistext = (TextBox)sender;
         thistext.SelectAll();
      }

      private void Tee_LostFocus(object sender, EventArgs e)
      {
         this.Cursor = Cursors.WaitCursor;
         TextBox tb = (TextBox)sender;
         int index = -1;
         string id = tb.Name.Replace("Tee[", "");
         id = id.Replace("]", "");
         try 
         {
            index = int.Parse(id);
            int teeid = int.Parse(tb.Text);
            teeid -= 1;
            string round = ((ListItem)Rounds.Items[Rounds.SelectedIndex]).Name;
            int courseid = m_tournament.GetCourseID(int.Parse(round));
            GolfCourse course = m_tournament.Course(courseid,teeid);
            HCP[index].Text = Formulas.Handicap(double.Parse(HCP[index].Name), course.Slope).ToString();
         }
         catch 
         {
            //do nothing
         }

         if (index > -1) 
            Holes[index,0].Focus();

         this.Cursor = Cursors.Default;
      }

        private void OnSize(object sender, EventArgs e)
        {
         //   scrollScores1.Location = new Point(2, this.Bottom - scrollScores1.Height - 2);
         //   scrollScores1.Width = this.Width - 4;
        }
   }
}
