using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Data;

namespace MonsterGolf
{
	/// <summary>
	/// Summary description for Results.
	/// </summary>
   public class Results : System.Windows.Forms.Form
   {
      private const int SKINSTABLE = 0;
      private const int TEAMTABLE = 1;
      private System.Collections.Specialized.ListDictionary colorDictionary;
      private System.Windows.Forms.MainMenu FullMenu;
      private System.Windows.Forms.MenuItem TourneyMenu;
      private Tournament m_tourney;
      private int m_currentRound = 1;
      private int m_currentlyDisplayed;
      private bool m_skins;
      private bool m_skinsusehighscore = false;
      private string m_currentSort;
      private bool m_hasflights = true;
      private AxSHDocVw.AxWebBrowser ResultsBrowser;
      private System.Data.DataSet DSResults;
      private System.Windows.Forms.RadioButton sortName;
      private System.Windows.Forms.RadioButton sortFlight;
      private System.Windows.Forms.RadioButton sortTotal;
      private System.Windows.Forms.ComboBox sortHole;
      private System.Windows.Forms.ComboBox comboBoxShowRounds;
      private int m_numbestrounds = 0;
      private bool m_uselow = false;
      private int m_golfersperteam = 1;
      private System.Windows.Forms.Label labelShowRounds;
      private System.Windows.Forms.CheckBox checkBoxIncludeFinal;
      private System.Windows.Forms.CheckBox checkBoxDescending;
      private System.Windows.Forms.Label labelSort;
      private System.Windows.Forms.CheckBox checkBoxStrokeControl;
      private Button scrollNext;
      private Button scrollPrev;
      private Button scrollCycle;
      private Timer timerScroll;
      private IContainer components;

      public Results(int TournamentID)
      {
         //
         // Required for Windows Form Designer support
         //
         InitializeComponent();
         m_tourney = new Tournament(TournamentID);
         m_hasflights = (m_tourney.Flights().Length > 0);
         scrollPrev.Visible = false;
         scrollNext.Visible = false;
         scrollCycle.Visible = false;

         if (m_hasflights)
         {
            if (m_tourney.Flights().Length == 1 && m_tourney.Flights()[0] == "(Unknown flight)")
               m_hasflights = false;
         }
         DSResults.Tables.Add();
         DSResults.Tables[SKINSTABLE].Columns.Add("PlayerFile");
         DSResults.Tables[SKINSTABLE].Columns["PlayerFile"].ColumnMapping = System.Data.MappingType.Hidden;
         DSResults.Tables[SKINSTABLE].Columns.Add("Name");
         if (m_hasflights)
            DSResults.Tables[SKINSTABLE].Columns.Add("Flight");
         DSResults.Tables.Add();
         DSResults.Tables[TEAMTABLE].Columns.Add("TeamID");
         DSResults.Tables[TEAMTABLE].Columns["TeamID"].ColumnMapping = System.Data.MappingType.Hidden;
         DSResults.Tables[TEAMTABLE].Columns.Add("Name");
         if (m_hasflights) 
         {
            DSResults.Tables[TEAMTABLE].Columns.Add("Flight");
            colorDictionary = new System.Collections.Specialized.ListDictionary();
            int rowcount = 1;
            foreach (string flight in m_tourney.Flights()) 
            {
               colorDictionary.Add(flight, "row" + rowcount.ToString());
               rowcount += 1;
               if (rowcount == 11)
                  rowcount = 1;
            }
         }
         AddTournamentToMenu();

         this.comboBoxShowRounds.Items.Add( "All" );
         for ( int i=0; i<m_tourney.NumberOfRounds; i++ )
         {
            this.comboBoxShowRounds.Items.Add( (i+1).ToString() );
         }
      }

      public void AddGolfers() 
      {
         DSResults.Tables[SKINSTABLE].Rows.Clear();
         DSResults.Tables[TEAMTABLE].Rows.Clear();
         object[] init;

         string curTeam = "-1";
         string lastTeam = "-1";
         string teamName = "";
         string teamFlight = "";
         int teamNumberOfGolfers = 0;
         
         DataSet ds = m_tourney.GolfersDataSet();
         ds.Tables[0].DefaultView.Sort = "TeamID";
         for (int i = 0; i < ds.Tables[0].DefaultView.Count; i++)
         {
            // individual golfer table
            init = new object[DSResults.Tables[SKINSTABLE].Columns.Count];
            init[0] = ds.Tables[0].DefaultView[i]["UserID"].ToString();
            init[1] = ds.Tables[0].DefaultView[i]["LastName"].ToString() + ", " + ds.Tables[0].DefaultView[i]["FirstName"].ToString();
            if (m_hasflights)
               init[2] = "";
            DSResults.Tables[SKINSTABLE].Rows.Add(init);
            // team table
            curTeam = ds.Tables[0].DefaultView[i]["TeamID"].ToString();

            if (curTeam != lastTeam && lastTeam != "-1")
            {
               init = new object[DSResults.Tables[TEAMTABLE].Columns.Count];
               init[0] = lastTeam;
               init[1] = teamName;
               if (m_hasflights)
                  init[2] = teamFlight;
               DSResults.Tables[TEAMTABLE].Rows.Add(init);
               m_golfersperteam = teamNumberOfGolfers;
               teamName = "";
               teamFlight = "";
               teamNumberOfGolfers=0;
            }

            // team variables
            if (teamName != "") teamName += " - ";
            teamName += ds.Tables[0].DefaultView[i]["LastName"].ToString() + ", " + ds.Tables[0].DefaultView[i]["FirstName"].ToString();
            teamFlight = ds.Tables[0].DefaultView[i]["Flight"].ToString();
            lastTeam = curTeam;
            teamNumberOfGolfers++;
         }

         if (lastTeam != "-1")
         {
            init = new object[DSResults.Tables[TEAMTABLE].Columns.Count];
            init[0] = lastTeam;
            init[1] = teamName;
            if (m_hasflights)
               init[2] = teamFlight;
            DSResults.Tables[TEAMTABLE].Rows.Add(init);
         }


         //foreach (ListItem liGolfer in m_tourney.Golfers()) 
         //{
         //   golfer = new Golfer(int.Parse(liGolfer.ID));
         //   init = new object[DSResults.Tables[SKINSTABLE].Columns.Count];
         //   init[0] = golfer.ID.ToString();
         //   init[1] = golfer.FirstName + " " + golfer.LastName;
         //   if (m_hasflights)
         //      init[2] = "";
         //   DSResults.Tables[SKINSTABLE].Rows.Add(init);
         //}
         //foreach (Team team in m_tourney.Teams()) 
         //{
         //   init = new object[DSResults.Tables[TEAMTABLE].Columns.Count];
         //   init[0] = team.ID;
         //   init[1] = team;
         //   if (m_hasflights)
         //      init[2] = team.Flight;
         //   DSResults.Tables[TEAMTABLE].Rows.Add(init);
         //   m_golfersperteam = team.NumberOfGolfers;
         //}
         init = null;
      }
      private void AddScoringOptions(System.Windows.Forms.MenuItem RoundItem) 
      {
         System.Windows.Forms.MenuItem gross;
         System.Windows.Forms.MenuItem net;
         System.Windows.Forms.MenuItem grossparpoints;
         System.Windows.Forms.MenuItem netparpoints;
         System.Windows.Forms.MenuItem scrollscores;

         if (m_golfersperteam > 1) 
         {
            System.Windows.Forms.MenuItem bestball = new MenuItem("Best Ball");
            gross = new MenuItem("Gross", new System.EventHandler(this.BestBallGross_Click));
            bestball.MenuItems.Add(0, gross);
            net = new MenuItem("Net", new System.EventHandler(this.BestBallNet_Click));
            bestball.MenuItems.Add(1, net);
            grossparpoints = new MenuItem("Gross Par Points", new System.EventHandler(this.BestBallPPGross_Click));
            bestball.MenuItems.Add(2, grossparpoints);
            netparpoints = new MenuItem("Net Par Points", new System.EventHandler(this.BestBallPPNet_Click));
            bestball.MenuItems.Add(3, netparpoints);
            scrollscores = new MenuItem("Scroll Scores", new EventHandler(this.ScrollScores_Click));
            bestball.MenuItems.Add(4, scrollscores);
            RoundItem.MenuItems.Add(RoundItem.MenuItems.Count, bestball);
         }
         if (m_golfersperteam > 2) 
         {
            System.Windows.Forms.MenuItem twoball = new MenuItem("Two Ball");
            gross = new MenuItem("Gross",  new System.EventHandler(this.TwoBallGross_Click));
            twoball.MenuItems.Add(0, gross);
            net = new MenuItem("Net", new System.EventHandler(this.TwoBallNet_Click));
            twoball.MenuItems.Add(1, net);
            grossparpoints = new MenuItem("Gross Par Points", new System.EventHandler(this.TwoBallPPGross_Click));
            twoball.MenuItems.Add(2, grossparpoints);
            netparpoints = new MenuItem("Net Par Points", new System.EventHandler(this.TwoBallPPNet_Click));
            twoball.MenuItems.Add(3, netparpoints);
            RoundItem.MenuItems.Add(RoundItem.MenuItems.Count, twoball);
         }
         if (m_golfersperteam > 3) 
         {
            System.Windows.Forms.MenuItem threeball = new MenuItem("Three Ball");
            gross = new MenuItem("Gross",  new System.EventHandler(this.ThreeBallGross_Click));
            threeball.MenuItems.Add(0, gross);
            net = new MenuItem("Net", new System.EventHandler(this.ThreeBallNet_Click));
            threeball.MenuItems.Add(1, net);
            grossparpoints = new MenuItem("Gross Par Points", new System.EventHandler(this.ThreeBallPPGross_Click));
            threeball.MenuItems.Add(2, grossparpoints);
            netparpoints = new MenuItem("Net Par Points", new System.EventHandler(this.ThreeBallPPNet_Click));
            threeball.MenuItems.Add(3, netparpoints);
            RoundItem.MenuItems.Add(RoundItem.MenuItems.Count, threeball);
         }

         System.Windows.Forms.MenuItem allball = new MenuItem("AllBall");
         if (m_golfersperteam > 1) 
            allball.Text = "All Ball";
         else
            allball.Text = "Individual";
         gross = new MenuItem("Gross", new System.EventHandler(this.AllBallGross_Click));
         allball.MenuItems.Add(0, gross);
         net = new MenuItem("Net", new System.EventHandler(this.AllBallNet_Click));
         allball.MenuItems.Add(1, net);
         grossparpoints = new MenuItem("Gross Par Points", new System.EventHandler(this.AllBallPPGross_Click));
         allball.MenuItems.Add(2, grossparpoints);
         netparpoints = new MenuItem("Net Par Points", new System.EventHandler(this.AllBallPPNet_Click));
         allball.MenuItems.Add(3, netparpoints);
         RoundItem.MenuItems.Add(RoundItem.MenuItems.Count, allball);

         if ( RoundItem.Text != "All Rounds" )
         {
            System.Windows.Forms.MenuItem skins = new MenuItem("Skins/Individual");
            gross = new MenuItem("Gross",  new System.EventHandler(this.SkinsGross_Click));
            skins.MenuItems.Add(0, gross);
            net = new MenuItem("Net", new System.EventHandler(this.SkinsNet_Click));
            skins.MenuItems.Add(1, net);
            grossparpoints = new MenuItem("Gross Par Points", new System.EventHandler(this.SkinsPPGross_Click));
            skins.MenuItems.Add(2, grossparpoints);
            netparpoints = new MenuItem("Net Par Points", new System.EventHandler(this.SkinsPPNet_Click));
            skins.MenuItems.Add(3, netparpoints);
            RoundItem.MenuItems.Add(RoundItem.MenuItems.Count, skins);
         }
      }
      public void AddTournamentToMenu() 
      {
         int courseid = 0;
         AddGolfers();
         int lastround = 0;
         for (int roundnum=1; roundnum<=m_tourney.NumberOfRounds; roundnum++) 
         {
            courseid = m_tourney.GetCourseID(roundnum);
            System.Windows.Forms.MenuItem menuitem = new MenuItem("Round " + roundnum.ToString() + " - " + m_tourney.Course(courseid,0).Name);
            menuitem.Index = roundnum-1;
            menuitem.Select += new EventHandler(Round_Select);
            this.TourneyMenu.MenuItems.Add(menuitem);
            AddScoringOptions(menuitem);
            lastround = roundnum;
         }
         System.Windows.Forms.MenuItem allrnds = new MenuItem("All Rounds");
         allrnds.Index = lastround;
         allrnds.Select += new EventHandler(Round_Select);
         this.TourneyMenu.MenuItems.Add(allrnds);
         AddScoringOptions(allrnds);

         System.Windows.Forms.MenuItem clear = new MenuItem("Clear");
         clear.Index = lastround+1;
         clear.Click += new EventHandler(Clear_Select);
         this.TourneyMenu.MenuItems.Add(clear);
      }

      public void ClearSkinChecks() 
      {
         for (int i=0; i<this.TourneyMenu.MenuItems.Count; i++) 
         {
            for (int j=0; j<this.TourneyMenu.MenuItems[i].MenuItems.Count; j++) 
            {
               if (this.TourneyMenu.MenuItems[i].MenuItems[j].Text == "Skins/Individual") 
               {
                  for (int k=0; k<this.TourneyMenu.MenuItems[i].MenuItems[j].MenuItems.Count; k++) 
                  {
                     this.TourneyMenu.MenuItems[i].MenuItems[j].MenuItems[k].Checked = false;
                  }
               }
            }
         }
      }

      public void ClearBestBallChecks() 
      {
         for (int i=0; i<this.TourneyMenu.MenuItems.Count; i++) 
         {
            for (int j=0; j<this.TourneyMenu.MenuItems[i].MenuItems.Count; j++) 
            {
               if (this.TourneyMenu.MenuItems[i].MenuItems[j].Text != "Skins/Individual") 
               {
                  for (int k=0; k<this.TourneyMenu.MenuItems[i].MenuItems[j].MenuItems.Count; k++) 
                  {
                     this.TourneyMenu.MenuItems[i].MenuItems[j].MenuItems[k].Checked = false;
                  }
               }
            }
         }
      }

      public bool SameScoringSelections()
      {
         bool allsame = true;
         string current = "";
         for ( int i=0; i<this.TourneyMenu.MenuItems.Count; i++ )
         {
            for ( int k=0; k<this.TourneyMenu.MenuItems[i].MenuItems.Count; k++ )
            {
               for ( int j=0; j<this.TourneyMenu.MenuItems[i].MenuItems[k].MenuItems.Count; j++ )
               {
                  if ( this.TourneyMenu.MenuItems[i].MenuItems[k].MenuItems[j].Checked )
                  {
                     if ( current == "" )
                        current = this.TourneyMenu.MenuItems[i].MenuItems[k].MenuItems[j].Text;

                     if (current.IndexOf("Par Point") > -1)
                        m_uselow = false;
                     else
                        m_uselow = true;

                     if ( this.TourneyMenu.MenuItems[i].MenuItems[k].MenuItems[j].Text != current )
                     {
                        allsame = false;
                     }
                  }
                  if ( !allsame )
                     break;
               }
               if ( !allsame )
                  break;
            }

            if ( !allsame )
               break;
         }
         return allsame;
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

      #region Windows Form Designer generated code
      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.components = new System.ComponentModel.Container();
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Results));
         this.FullMenu = new System.Windows.Forms.MainMenu(this.components);
         this.TourneyMenu = new System.Windows.Forms.MenuItem();
         this.DSResults = new System.Data.DataSet();
         this.labelSort = new System.Windows.Forms.Label();
         this.ResultsBrowser = new AxSHDocVw.AxWebBrowser();
         this.sortName = new System.Windows.Forms.RadioButton();
         this.sortFlight = new System.Windows.Forms.RadioButton();
         this.sortTotal = new System.Windows.Forms.RadioButton();
         this.sortHole = new System.Windows.Forms.ComboBox();
         this.labelShowRounds = new System.Windows.Forms.Label();
         this.comboBoxShowRounds = new System.Windows.Forms.ComboBox();
         this.checkBoxIncludeFinal = new System.Windows.Forms.CheckBox();
         this.checkBoxDescending = new System.Windows.Forms.CheckBox();
         this.checkBoxStrokeControl = new System.Windows.Forms.CheckBox();
         this.scrollNext = new System.Windows.Forms.Button();
         this.scrollPrev = new System.Windows.Forms.Button();
         this.scrollCycle = new System.Windows.Forms.Button();
         this.timerScroll = new System.Windows.Forms.Timer(this.components);
         ((System.ComponentModel.ISupportInitialize)(this.DSResults)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.ResultsBrowser)).BeginInit();
         this.SuspendLayout();
         // 
         // FullMenu
         // 
         this.FullMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.TourneyMenu});
         // 
         // TourneyMenu
         // 
         this.TourneyMenu.Index = 0;
         this.TourneyMenu.Text = "Tournament Scores";
         this.TourneyMenu.Click += new System.EventHandler(this.TourneyMenu_Click);
         // 
         // DSResults
         // 
         this.DSResults.DataSetName = "NewDataSet";
         this.DSResults.Locale = new System.Globalization.CultureInfo("en-US");
         // 
         // labelSort
         // 
         this.labelSort.Location = new System.Drawing.Point(16, 8);
         this.labelSort.Name = "labelSort";
         this.labelSort.Size = new System.Drawing.Size(48, 16);
         this.labelSort.TabIndex = 4;
         this.labelSort.Text = "Sort by";
         this.labelSort.Visible = false;
         // 
         // ResultsBrowser
         // 
         this.ResultsBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                     | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.ResultsBrowser.Enabled = true;
         this.ResultsBrowser.Location = new System.Drawing.Point(16, 48);
         this.ResultsBrowser.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("ResultsBrowser.OcxState")));
         this.ResultsBrowser.Size = new System.Drawing.Size(662, 380);
         this.ResultsBrowser.TabIndex = 7;
         this.ResultsBrowser.Visible = false;
         // 
         // sortName
         // 
         this.sortName.Location = new System.Drawing.Point(64, 8);
         this.sortName.Name = "sortName";
         this.sortName.Size = new System.Drawing.Size(56, 16);
         this.sortName.TabIndex = 8;
         this.sortName.Text = "Name";
         this.sortName.Visible = false;
         this.sortName.CheckedChanged += new System.EventHandler(this.sortName_CheckedChanged);
         // 
         // sortTotal
         // 
         this.sortTotal.Location = new System.Drawing.Point(128, 8);
         this.sortTotal.Name = "sortTotal";
         this.sortTotal.Size = new System.Drawing.Size(48, 16);
         this.sortTotal.TabIndex = 9;
         this.sortTotal.Text = "Total";
         this.sortTotal.Visible = false;
         this.sortTotal.CheckedChanged += new System.EventHandler(this.sortTotal_CheckedChanged);
         // 
         // sortFlight
         // 
         this.sortFlight.Location = new System.Drawing.Point(182, 8);
         this.sortFlight.Name = "sortFlight";
         this.sortFlight.Size = new System.Drawing.Size(104, 16);
         this.sortFlight.TabIndex = 10;
         this.sortFlight.Text = "Flight and Total";
         this.sortFlight.Visible = false;
         this.sortFlight.CheckedChanged += new System.EventHandler(this.sortFlight_CheckedChanged);
         // 
         // sortHole
         // 
         this.sortHole.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "Out",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "In",
            "Total"});
         this.sortHole.Location = new System.Drawing.Point(120, 5);
         this.sortHole.Name = "sortHole";
         this.sortHole.Size = new System.Drawing.Size(49, 21);
         this.sortHole.TabIndex = 11;
         this.sortHole.Text = "Hole";
         this.sortHole.Visible = false;
         this.sortHole.SelectedIndexChanged += new System.EventHandler(this.sortHole_SelectedIndexChanged);
         // 
         // labelShowRounds
         // 
         this.labelShowRounds.Location = new System.Drawing.Point(392, 11);
         this.labelShowRounds.Name = "labelShowRounds";
         this.labelShowRounds.Size = new System.Drawing.Size(110, 14);
         this.labelShowRounds.TabIndex = 12;
         this.labelShowRounds.Text = "Show Best Round(s)";
         this.labelShowRounds.Visible = false;
         // 
         // comboBoxShowRounds
         // 
         this.comboBoxShowRounds.Location = new System.Drawing.Point(504, 8);
         this.comboBoxShowRounds.Name = "comboBoxShowRounds";
         this.comboBoxShowRounds.Size = new System.Drawing.Size(56, 21);
         this.comboBoxShowRounds.TabIndex = 13;
         this.comboBoxShowRounds.Visible = false;
         this.comboBoxShowRounds.SelectedIndexChanged += new System.EventHandler(this.comboBoxShowRounds_SelectedIndexChanged);
         // 
         // checkBoxIncludeFinal
         // 
         this.checkBoxIncludeFinal.Location = new System.Drawing.Point(568, 8);
         this.checkBoxIncludeFinal.Name = "checkBoxIncludeFinal";
         this.checkBoxIncludeFinal.Size = new System.Drawing.Size(128, 18);
         this.checkBoxIncludeFinal.TabIndex = 14;
         this.checkBoxIncludeFinal.Text = "Include Final Round";
         this.checkBoxIncludeFinal.Visible = false;
         this.checkBoxIncludeFinal.CheckedChanged += new System.EventHandler(this.checkBoxIncludeFinal_CheckedChanged);
         // 
         // checkBoxDescending
         // 
         this.checkBoxDescending.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
         this.checkBoxDescending.Location = new System.Drawing.Point(289, 8);
         this.checkBoxDescending.Name = "checkBoxDescending";
         this.checkBoxDescending.Size = new System.Drawing.Size(88, 23);
         this.checkBoxDescending.TabIndex = 16;
         this.checkBoxDescending.Text = "Descending";
         this.checkBoxDescending.TextAlign = System.Drawing.ContentAlignment.TopLeft;
         this.checkBoxDescending.Visible = false;
         this.checkBoxDescending.CheckedChanged += new System.EventHandler(this.checkBoxDescending_CheckedChanged);
         // 
         // checkBoxStrokeControl
         // 
         this.checkBoxStrokeControl.Location = new System.Drawing.Point(18, 24);
         this.checkBoxStrokeControl.Name = "checkBoxStrokeControl";
         this.checkBoxStrokeControl.Size = new System.Drawing.Size(128, 24);
         this.checkBoxStrokeControl.TabIndex = 17;
         this.checkBoxStrokeControl.Text = "Use Stroke Control?";
         // 
         // scrollNext
         // 
         this.scrollNext.Location = new System.Drawing.Point(620, 13);
         this.scrollNext.Name = "scrollNext";
         this.scrollNext.Size = new System.Drawing.Size(42, 26);
         this.scrollNext.TabIndex = 18;
         this.scrollNext.Text = "Next";
         this.scrollNext.UseVisualStyleBackColor = true;
         this.scrollNext.Click += new System.EventHandler(this.scrollNext_Click);
         // 
         // scrollPrev
         // 
         this.scrollPrev.Location = new System.Drawing.Point(524, 12);
         this.scrollPrev.Name = "scrollPrev";
         this.scrollPrev.Size = new System.Drawing.Size(42, 27);
         this.scrollPrev.TabIndex = 19;
         this.scrollPrev.Text = "Prev";
         this.scrollPrev.UseVisualStyleBackColor = true;
         this.scrollPrev.Click += new System.EventHandler(this.scrollPrev_Click);
         // 
         // scrollCycle
         // 
         this.scrollCycle.Location = new System.Drawing.Point(572, 13);
         this.scrollCycle.Name = "scrollCycle";
         this.scrollCycle.Size = new System.Drawing.Size(42, 26);
         this.scrollCycle.TabIndex = 20;
         this.scrollCycle.Text = "Cycle";
         this.scrollCycle.UseVisualStyleBackColor = true;
         this.scrollCycle.Click += new System.EventHandler(this.scrollCycle_Click);
         // 
         // timerScroll
         // 
         this.timerScroll.Interval = 5000;
         this.timerScroll.Tick += new System.EventHandler(this.timerScroll_Tick);
         // 
         // Results
         // 
         this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
         this.BackColor = System.Drawing.Color.LightGray;
         this.ClientSize = new System.Drawing.Size(692, 445);
         this.Controls.Add(this.scrollCycle);
         this.Controls.Add(this.scrollPrev);
         this.Controls.Add(this.scrollNext);
         this.Controls.Add(this.comboBoxShowRounds);
         this.Controls.Add(this.checkBoxDescending);
         this.Controls.Add(this.checkBoxIncludeFinal);
         this.Controls.Add(this.labelShowRounds);
         this.Controls.Add(this.sortHole);
         this.Controls.Add(this.sortTotal);
         this.Controls.Add(this.sortFlight);
         this.Controls.Add(this.sortName);
         this.Controls.Add(this.ResultsBrowser);
         this.Controls.Add(this.labelSort);
         this.Controls.Add(this.checkBoxStrokeControl);
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.Menu = this.FullMenu;
         this.Name = "Results";
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
         this.Text = "Results";
         ((System.ComponentModel.ISupportInitialize)(this.DSResults)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.ResultsBrowser)).EndInit();
         this.ResumeLayout(false);

      }
      #endregion

      private int[] SkinsHighlight() 
      {
         int[] skinrows = new int[18];
         int skincolumn=0;
         int lownumber = 100;
         int currscore = 0;
         int skinrow = -1;
         bool onelow = false;
         object cell;
         for (int x=0; x<18; x++) 
         {
            if (m_skinsusehighscore)
               lownumber = -1;
            else
               lownumber = 100;
            currscore = 0;
            skincolumn = x+1;
            onelow = false;
            foreach (System.Data.DataRowView dr in DSResults.Tables[SKINSTABLE].DefaultView) 
            {
               cell = dr[skincolumn.ToString()];
               if (cell != System.DBNull.Value) 
               {
                  currscore = int.Parse(cell.ToString());
                  if (m_skinsusehighscore) 
                  {
                     if (currscore > lownumber)
                        lownumber = currscore;
                  }
                  else 
                  {
                     if (currscore < lownumber)
                        lownumber = currscore;
                  }
               }
            }
            skinrow = -1;
            foreach (System.Data.DataRowView dr in DSResults.Tables[SKINSTABLE].DefaultView) 
            {
               skinrow += 1;
               cell = dr[skincolumn.ToString()];
               if (cell != System.DBNull.Value) 
               {
                  currscore = int.Parse(cell.ToString());
                  if (lownumber == currscore) 
                  {
                     if (!onelow) 
                     {
                        skinrows[x] = skinrow;
                        onelow = true;
                     }
                     else
                     {
                        skinrows[x] = -1;
                        break;
                     }
                  }
               }
            }
         }
         return skinrows;
      }
      private void WriteHTML() 
      {
         System.Text.StringBuilder htmlstring = new System.Text.StringBuilder();
         int tableid = TEAMTABLE;
         if (m_currentlyDisplayed == SKINSTABLE) 
            tableid = SKINSTABLE;
         if (m_currentSort == "") 
         {
            if (m_skins) 
               m_currentSort = "Name ASC";
            else
               m_currentSort = "Total ASC";
         }
         
         DSResults.Tables[tableid].DefaultView.Sort = m_currentSort;
         htmlstring.Append("<html><style>body {background-color:#D3D3D3; font-family:verdana; font-size:12pt; } td { font-family:verdana; font-size:10pt; background-color: #ffffff; } .skin { background-color:red; font-weight:bold;} .rowheader { background-color:#394fbc; color:white; font-weight:bold; font-size:10pt; } .row1 { background-color:87cefa; } .row2 { background-color:fff8dc; } .row3 { background-color:e0eee0; } .row4 { background-color:e6e6fa; } .row5 { background-color:deb887; } .row6 { background-color:f0e68c; } .row7 { background-color:ffa500; } .row8 { background-color:ff1493; } .row9 { background-color:d8bfd8; } .row10 { background-color:dccdc; } table { background-color:black; } </style><body>");
         if (m_skins)
            htmlstring.Append("<center><b>" + m_tourney.Name + " - Skins Round " + m_currentRound + " - " + m_tourney.Course(m_tourney.GetCourseID(m_currentRound), 0).Name + "</b><br><br>");
         else
            htmlstring.Append("<center><b>" + m_tourney.Name + "</b><br><br>");
         htmlstring.Append("<table cellpadding=2 cellspacing=1 border=0>\n");
         htmlstring.Append("<tr class=rowheader><td class=rowheader></td>");
         int holeid = 0;
         string width = "";
         for (int x=0; x<DSResults.Tables[tableid].Columns.Count; x++) 
         {
            if (DSResults.Tables[tableid].Columns[x].ColumnMapping != System.Data.MappingType.Hidden) 
            {
               try 
               {
                  holeid = int.Parse(DSResults.Tables[tableid].Columns[x].ColumnName);
                  width = " class=rowheader width=20 align=center";
               }
               catch 
               {
                  width = " class=rowheader align=center";
               }
               htmlstring.Append("<td" + width + "><b>" + DSResults.Tables[tableid].Columns[x].ColumnName + "</b></td>");
            }
         }
         htmlstring.Append("</tr>\n");

         string rowclass = "row1";
         int[] skinrows = new int[18];
         if (m_skins)
            skinrows = SkinsHighlight();
         int rownum = 0;
         int rowstring = 0;
         string cellclass = "";
         string cellalign = "";

         foreach (System.Data.DataRowView dr in DSResults.Tables[tableid].DefaultView) 
         {
            rowstring = rownum + 1;
            if (m_hasflights && !m_skins && dr["Flight"].ToString().Length > 0) 
            {
               rowclass = colorDictionary[dr["Flight"].ToString()].ToString();
               htmlstring.Append("<tr class=" + rowclass + ">");
            }
            else 
            {
               htmlstring.Append("<tr class=" + rowclass + ">");
               rowclass = (rowclass == "row1") ? "row2" : "row1";
            }

            htmlstring.Append("<td align=center class=" + rowclass + ">" + rowstring.ToString() + "</td>");
            
            object cell;
            for (int x=0; x<DSResults.Tables[tableid].Columns.Count; x++) 
            {
               cellclass = "";
               cell = dr[DSResults.Tables[tableid].Columns[x].ColumnName];
               
               if (DSResults.Tables[tableid].Columns[x].ColumnMapping != System.Data.MappingType.Hidden) 
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
                  
                  if (m_skins) 
                  {
                     try 
                     {
                        holeid = int.Parse(DSResults.Tables[tableid].Columns[x].ColumnName);
                        
                        if (skinrows[holeid-1] == rownum)
                           cellclass = " class=skin";
                     }
                     catch 
                     {
                        //do nothing.
                     }
                  }
                  htmlstring.Append("<td" + cellclass + cellalign + ">" + ((cell.ToString()=="0")?"-":cell.ToString()) + "</td>");
               }
            }
            htmlstring.Append("</tr>\n");
            rownum += 1;
         }
         htmlstring.Append("</table></center></body></html>\n");
         FillBrowser(htmlstring.ToString());
      }
      private void FillBrowser(string html)
      {
         string filname = Application.StartupPath + "\\Results" + this.m_tourney.TournamentID.ToString() + ".html";
         System.IO.StreamWriter fs = System.IO.File.CreateText(filname);
         fs.Write(html);
         fs.Flush();
         fs.Close();
         object refmiss = System.Reflection.Missing.Value;
         ResultsBrowser.Visible = true;
         ResultsBrowser.Navigate(filname, ref refmiss, ref refmiss, ref refmiss, ref refmiss);
      }
      private void RemoveColumn(string columnname, int tableid) 
      {
         if (DSResults.Tables[tableid].Columns.IndexOf(columnname) > -1)
            DSResults.Tables[tableid].Columns.Remove(columnname);
      }
      private void AddColumn(string columnname, int tableid, System.Type type) 
      {
         if (DSResults.Tables[tableid].Columns.IndexOf(columnname) < 0) 
            DSResults.Tables[tableid].Columns.Add(columnname, type);
      }
      private void AddTotalColumn(int tableid) 
      {
         if ( this.m_numbestrounds > 0 )
         {
            if ( !this.SameScoringSelections() )
            {
               this.m_numbestrounds = 0;
               this.comboBoxShowRounds.SelectedIndex = 0;
               MessageBox.Show("All scoring selections must be the same to total the best rounds.");
            }
         }

         //string sort = this.m_currentSort;
         DSResults.Tables[tableid].DefaultView.Sort = "";

         RemoveColumn("Total", tableid);
         AddColumn("Total", tableid, typeof(int));
         int total = 0;
         object cell;

         int[] totals = new int[DSResults.Tables[tableid].Columns.Count];

         foreach (System.Data.DataRow dr in DSResults.Tables[tableid].Rows) 
         {
            for (int x=0; x<totals.Length; x++ )
            {
               totals[x] = 0;
            }
            total = 0;
            for (int x=0; x<DSResults.Tables[tableid].Columns.Count; x++) 
            {
               cell = dr[DSResults.Tables[tableid].Columns[x].ColumnName];

               if (DSResults.Tables[tableid].Columns[x].ColumnMapping != System.Data.MappingType.Hidden
                  && DSResults.Tables[tableid].Columns[x].ColumnName != "Total" 
                  && DSResults.Tables[tableid].Columns[x].ColumnName != "Flight") 
               {
                  try 
                  {
                     totals[x] = int.Parse(cell.ToString());
                  }
                  catch 
                  {
                  }
               }
            }

            if ( this.m_numbestrounds > 0 )
            {
               int numbestrounds = this.m_numbestrounds;
               int totalrnds = totals.Length;
               if ( this.checkBoxIncludeFinal.Checked )
               {
                  // skip the final totals row that is the total column (thus -2)
                  total += totals[totalrnds-2];
                  numbestrounds--;
                  totalrnds-=2;
               }

               for ( int i = 0; i<numbestrounds; i++ )
               {
                  int best = 0;
                  if (m_uselow)
                     best = 200;
                  int bestpos = 0;
                  for (int x=0; x<totalrnds; x++ )
                  {
                     if (m_uselow) 
                     {
                        if ( totals[x] < best && totals[x] > 18)
                        {
                           best = totals[x];
                           bestpos = x;
                        }
                     }
                     else 
                     {
                        if ( totals[x] > best )
                        {
                           best = totals[x];
                           bestpos = x;
                        }
                     }
                  }
                  total += best;
                  totals[bestpos] = 0;
               }
            }
            else
            {
               for (int x=0; x<totals.Length; x++ )
               {
                  total += totals[x];
               }
            }
            
            dr["Total"] = total;
         }
      }
      private void BestBallTotals(string columname, bool parpoints, bool usehandi, int numberofballs) 
      {
         int round = m_currentRound;
         int[,] bestscores = new int[numberofballs,18];
         int x = 0;
         int totalscore = 0;
         int netscore;
         int hcp;
         int pp;
         int tempscore;
         int currscore;
         int tableid = TEAMTABLE;
         GolfCourse course;
         AddColumn(columname, tableid, typeof(int));

         DataSet dsScores = DB.GetDataSet(
            "SELECT tp.TeamID, tp.TeeNumber, tp.Handicap, ts.* FROM TeamPlayers tp " +
            "INNER JOIN TourneyScores ts on tp.UserID = ts.UserID AND tp.TournamentID = ts.TournamentID " +
            "WHERE tp.TournamentID = " + m_tourney.TournamentID.ToString());
         foreach (System.Data.DataRow dr in DSResults.Tables[tableid].Rows) 
         {
            totalscore = 0;
            for (int y=0; y<numberofballs; y++) 
            {
               for (int score=0; score<18; score++) 
               {
                  if (parpoints)
                     bestscores[y,score] = int.MinValue;
                  else
                     bestscores[y,score] = int.MaxValue;
               }
            }
            dsScores.Tables[0].DefaultView.RowFilter = 
               "TeamID = " + dr["TeamID"].ToString() + " AND RoundNumber = " + round.ToString();
            if (dsScores.Tables[0].DefaultView.Count > 0)
            {
               for (int i = 0; i < dsScores.Tables[0].DefaultView.Count; i++)
               {
                  x = 0;
                  course = m_tourney.Course(m_tourney.GetCourseID(round), int.Parse(dsScores.Tables[0].DefaultView[i]["TeeNumber"].ToString()));
                  hcp = Formulas.Handicap(double.Parse(dsScores.Tables[0].DefaultView[i]["Handicap"].ToString()), course.Slope);
                  for (int j = 1; j <= 18; j++)
                  {
                     int score = int.Parse(dsScores.Tables[0].DefaultView[i]["Hole" + j.ToString()].ToString());
                     if (checkBoxStrokeControl.Checked)
                        currscore = StrokeControl(hcp, course.Hole(x).Par, score);
                     else
                        currscore = score;

                     if (usehandi)
                        netscore = Formulas.NetScore(hcp, course.Hole(x + 1).Handicap, currscore);
                     else
                        netscore = currscore;
                     if (parpoints)
                     {
                        pp = Formulas.ParPoints(hcp, netscore, course.Hole(x + 1).Par);

                        for (int y = 0; y < numberofballs; y++)
                        {
                           if (pp > bestscores[y, x])
                           {
                              tempscore = pp;
                              pp = bestscores[y, x];
                              bestscores[y, x] = tempscore;
                           }
                        }
                     }
                     else
                     {
                        for (int y = 0; y < numberofballs; y++)
                        {
                           if (netscore < bestscores[y, x])
                           {
                              tempscore = netscore;
                              netscore = bestscores[y, x];
                              bestscores[y, x] = tempscore;
                           }
                        }
                     }
                     x += 1;
                  }
               }
               for (int y = 0; y < numberofballs; y++)
               {
                  for (int score = 0; score < 18; score++)
                  {
                     totalscore += bestscores[y, score];
                  }
               }
            }
            else
            {
               totalscore = 0;
            }
            dr[columname] = totalscore;
         }
         AddTotalColumn(tableid);
      }
      private void RemoveSkinHoles() 
      {
         for (int x=1; x<19; x++) 
            RemoveColumn(x.ToString(), SKINSTABLE);
      }
      private void RemoveBestBallColumns() 
      {
         for (int x=0; x<DSResults.Tables[TEAMTABLE].Columns.Count; x++) 
         {
            if (DSResults.Tables[TEAMTABLE].Columns[x].ColumnMapping != System.Data.MappingType.Hidden)
            {
               if (DSResults.Tables[TEAMTABLE].Columns[x].ColumnName.IndexOf(" Ball ") > -1)
                  RemoveColumn(DSResults.Tables[TEAMTABLE].Columns[x].ColumnName, TEAMTABLE);
            }
         }
      }
      private int StrokeControl(int hcp, int par, int score) 
      {
         int retscore = score;

         if (hcp > 39)
         {
            if (score > 10)
               retscore = 10;
         }
         else if (hcp > 29)
         {
            if (score > 9)
               retscore = 9;
         }
         else if (hcp > 19) 
         {
            if (score > 8)
               retscore = 8;
         }
         else if (hcp > 9)
         {
            if (score > 7)
               retscore = 7;
         }
         else 
         {
            if (score > (par+2))
               retscore = par+2;
         }

         return retscore;
      }
      private void SkinsDisplay(bool usehandi, bool parpoints) 
      {
         Golfer golfer;
         int round = m_currentRound;
         int currscore = 0;
         int hcp = 0;
         int x = 1;
         int front = 0;
         int back = 0;
         GolfCourse course;
         for (x=1; x<10; x++) 
            AddColumn(x.ToString(), SKINSTABLE, typeof(int));

         AddColumn("Out", SKINSTABLE, typeof(int));

         for (x=10; x<19; x++) 
            AddColumn(x.ToString(), SKINSTABLE, typeof(int));
         
         AddColumn("In", SKINSTABLE, typeof(int));
         AddColumn("Total", SKINSTABLE, typeof(int));

         foreach (System.Data.DataRow dr in DSResults.Tables[SKINSTABLE].Rows) 
         {
            x = 1;
            front = 0;
            back = 0;
            golfer = new Golfer(int.Parse(dr["PlayerFile"].ToString()));
            golfer.TournamentID = m_tourney.TournamentID;
            golfer.RoundNumber = round;
            course = m_tourney.Course(m_tourney.GetCourseID(round), golfer.Tee);
            
            if (golfer.LoadGolfersScore()) 
            {
               if (usehandi)
                  hcp = Formulas.Handicap(golfer.HcpIndex, course.Slope);
               
               foreach(int score in golfer.Scores) 
               {
                  if (checkBoxStrokeControl.Checked) 
                     currscore = StrokeControl(Formulas.Handicap(golfer.HcpIndex, course.Slope), course.Hole(x).Par, score);
                  else
                     currscore = score;

                  if (usehandi) 
                     currscore = Formulas.NetScore(hcp, course.Hole(x).Handicap, currscore);
                  if (parpoints)
                     currscore = Formulas.ParPoints(hcp, currscore, course.Hole(x).Par);
                  
                  dr[x.ToString()] = currscore;
                  if (x<10)
                     front += currscore;
                  else
                     back += currscore;
                  x += 1;
               } 
            }

            dr["Out"] = front;
            dr["In"] = back;
            dr["Total"] = front+back;
         }
      }
      private void AddOrRemove(ref MenuItem mi, string columnname, bool parpoints, bool handi, int numscores, bool skins) 
      {
         // make sure buttons are not visible when not on scroll page.
         scrollPrev.Visible = false;
         scrollNext.Visible = false;
         scrollCycle.Visible = false;

         if (mi.Checked) 
         {
            mi.Checked = false;
            if (skins) 
            {
               //RemoveSkinHoles();
               //m_skins = false;
            }
            else 
            {
               RemoveColumn(columnname + " Round " + m_currentRound.ToString(), TEAMTABLE);
               AddTotalColumn(TEAMTABLE);
            }
            WriteHTML();
         }
         else
         {
            mi.Checked = true;
            if (skins) 
            {
               if (m_currentlyDisplayed == TEAMTABLE) 
               {
                  ClearBestBallChecks();
                  m_currentSort = "";
               }
               RemoveBestBallColumns();
               m_skinsusehighscore = parpoints;
               SkinsDisplay(handi, parpoints);
            }
            else 
            {
               if (m_currentlyDisplayed == SKINSTABLE) 
               {
                  ClearSkinChecks();
                  m_currentSort = "";
               }
               BestBallTotals(columnname + " Round " + m_currentRound.ToString(), parpoints, handi, numscores);
            }
            sortName.Visible = true;
            labelSort.Visible = true;
            if ( m_tourney.NumberOfRounds > 1 && 
               System.Configuration.ConfigurationManager.AppSettings["showfilterbest"] != null &&
               System.Configuration.ConfigurationManager.AppSettings["showfilterbest"] == "true")
            {
               this.labelShowRounds.Visible = true;
               this.comboBoxShowRounds.Visible = true;
               this.checkBoxIncludeFinal.Visible = true;
            }
            if (skins) 
            {
               this.labelShowRounds.Visible = false;
               this.comboBoxShowRounds.Visible = false;
               this.checkBoxIncludeFinal.Visible = false;
            }
            this.checkBoxDescending.Visible = true;

            m_skins = skins;
            if (skins) // && m_currentlyDisplayed != SKINSTABLE) 
            {
               m_currentlyDisplayed = SKINSTABLE;
               sortFlight.Visible = false;
               sortTotal.Visible = false;
               sortHole.Visible = true;
               if (!sortName.Checked)
                  sortName.Checked = true;
               else
                  WriteHTML();
            }
            else if (!skins)
            {
               m_currentlyDisplayed = TEAMTABLE;
               sortTotal.Visible = true;
               sortHole.Visible = false;
               sortFlight.Visible = m_hasflights;
               WriteHTML();
            }
            else
               WriteHTML();
         }
      }

      private void AddOrRemoveCheckAll(ref MenuItem mi, string columnname, bool parpoints, bool handi, int numscores, bool skins) 
      {
         this.Cursor = Cursors.WaitCursor;
         if ( this.m_currentRound == m_tourney.NumberOfRounds+1 )
         {
            for (int roundnum=1; roundnum<=m_tourney.NumberOfRounds; roundnum++) 
            {
               this.m_currentRound = roundnum;
               AddOrRemove(ref mi, columnname, parpoints, handi, numscores, skins);
               if ( roundnum+1<=m_tourney.NumberOfRounds )
               {
                  // toggle menu check, AddOrRemove expects it
                  mi.Checked = (mi.Checked) ? false : true;
               }
            }
         }
         else
            AddOrRemove(ref mi, columnname, parpoints, handi, numscores, skins);

         this.Cursor = Cursors.Default;
      }
      private void BestBallGross_Click(object sender, System.EventArgs e)
      {
         MenuItem menusender = (MenuItem)sender;
         AddOrRemoveCheckAll(ref menusender, "Best Ball Gross", false, false, 1, false);
      }
      private void BestBallNet_Click(object sender, System.EventArgs e)
      {
         MenuItem menusender = (MenuItem)sender;
         AddOrRemoveCheckAll(ref menusender, "Best Ball Net", false, true, 1, false);
      }
      private void BestBallPPGross_Click(object sender, System.EventArgs e)
      {
         MenuItem menusender = (MenuItem)sender;
         AddOrRemoveCheckAll(ref menusender, "Best Ball Gross Par Points", true, false, 1, false);
      }
      private void BestBallPPNet_Click(object sender, System.EventArgs e)
      {
         MenuItem menusender = (MenuItem)sender;
         AddOrRemoveCheckAll(ref menusender, "Best Ball Net Par Points", true, true, 1, false);
      }
      private void TwoBallGross_Click(object sender, System.EventArgs e)
      {
         MenuItem menusender = (MenuItem)sender;
         AddOrRemoveCheckAll(ref menusender, "Two Ball Gross", false, false, 2, false);
      }
      private void TwoBallNet_Click(object sender, System.EventArgs e)
      {
         MenuItem menusender = (MenuItem)sender;
         AddOrRemoveCheckAll(ref menusender, "Two Ball Net", false, true, 2, false);
      }
      private void TwoBallPPGross_Click(object sender, System.EventArgs e)
      {
         MenuItem menusender = (MenuItem)sender;
         AddOrRemoveCheckAll(ref menusender, "Two Ball Gross Par Points", true, false, 2, false);
      }
      private void TwoBallPPNet_Click(object sender, System.EventArgs e)
      {
         MenuItem menusender = (MenuItem)sender;
         AddOrRemoveCheckAll(ref menusender, "Two Ball Net Par Points", true, true, 2, false);
      }
      private void ThreeBallGross_Click(object sender, System.EventArgs e)
      {
         MenuItem menusender = (MenuItem)sender;
         AddOrRemoveCheckAll(ref menusender, "Three Ball Gross", false, false, 3, false);
      }
      private void ThreeBallNet_Click(object sender, System.EventArgs e)
      {
         MenuItem menusender = (MenuItem)sender;
         AddOrRemoveCheckAll(ref menusender, "Three Ball Net", false, true, 3, false);
      }
      private void ThreeBallPPGross_Click(object sender, System.EventArgs e)
      {
         MenuItem menusender = (MenuItem)sender;
         AddOrRemoveCheckAll(ref menusender, "Three Ball Gross Par Points", true, false, 3, false);
      }
      private void ThreeBallPPNet_Click(object sender, System.EventArgs e)
      {
         MenuItem menusender = (MenuItem)sender;
         AddOrRemoveCheckAll(ref menusender, "Three Ball Net Par Points", true, true, 3, false);
      }
      private void AllBallGross_Click(object sender, System.EventArgs e)
      {
         MenuItem menusender = (MenuItem)sender;
         AddOrRemoveCheckAll(ref menusender, "All Ball Gross", false, false, m_golfersperteam, false);
      }
      private void AllBallNet_Click(object sender, System.EventArgs e)
      {
         MenuItem menusender = (MenuItem)sender;
         AddOrRemoveCheckAll(ref menusender, "All Ball Net", false, true, m_golfersperteam, false);
      }
      private void AllBallPPGross_Click(object sender, System.EventArgs e)
      {
         MenuItem menusender = (MenuItem)sender;
         AddOrRemoveCheckAll(ref menusender, "All Ball Gross Par Points", true, false, m_golfersperteam, false);
      }
      private void AllBallPPNet_Click(object sender, System.EventArgs e)
      {
         MenuItem menusender = (MenuItem)sender;
         AddOrRemoveCheckAll(ref menusender, "All Ball Net Par Points", true, true, m_golfersperteam, false);
      }
      private void SkinsGross_Click(object sender, System.EventArgs e)
      {
         MenuItem menusender = (MenuItem)sender;
         if (!menusender.Checked) 
         {
            menusender.Parent.MenuItems[1].Checked = false;
            menusender.Parent.MenuItems[2].Checked = false;
            menusender.Parent.MenuItems[3].Checked = false;
         }
         AddOrRemove(ref menusender, "", false, false, 0, true);
      }
      private void SkinsNet_Click(object sender, System.EventArgs e)
      {
         MenuItem menusender = (MenuItem)sender;
         if (!menusender.Checked) 
         {
            menusender.Parent.MenuItems[0].Checked = false;
            menusender.Parent.MenuItems[2].Checked = false;
            menusender.Parent.MenuItems[3].Checked = false;
         }
         AddOrRemove(ref menusender, "", false, true, 0, true);
      }
      private void SkinsPPGross_Click(object sender, System.EventArgs e)
      {
         MenuItem menusender = (MenuItem)sender;
         if (!menusender.Checked) 
         {
            menusender.Parent.MenuItems[0].Checked = false;
            menusender.Parent.MenuItems[1].Checked = false;
            menusender.Parent.MenuItems[3].Checked = false;
         }
         AddOrRemove(ref menusender, "", true, false, 0, true);
      }
      private void SkinsPPNet_Click(object sender, System.EventArgs e)
      {
         MenuItem menusender = (MenuItem)sender;
         if (!menusender.Checked) 
         {
            menusender.Parent.MenuItems[0].Checked = false;
            menusender.Parent.MenuItems[1].Checked = false;
            menusender.Parent.MenuItems[2].Checked = false;
         }

         AddOrRemove(ref menusender, "", true, true, 0, true);
      }
      private void Round_Select(object sender, EventArgs e)
      {
         m_currentRound = ((MenuItem)sender).Index + 1;
      }

      private void Clear_Select(object sender, EventArgs e)
      {
         this.Cursor = Cursors.WaitCursor;
         for ( int i=0; i<this.TourneyMenu.MenuItems.Count-1; i++ )
         {
            for ( int j=0; j<this.TourneyMenu.MenuItems[i].MenuItems.Count; j++ )
            {
               for ( int k=0; k<this.TourneyMenu.MenuItems[i].MenuItems[j].MenuItems.Count; k++ )
               {
                  if ( this.TourneyMenu.MenuItems[i].MenuItems[j].MenuItems[k].Checked )
                  {
                     //this.TourneyMenu.MenuItems[i].MenuItems[j].MenuItems[k].Checked = true;
                  
                     m_currentRound = this.TourneyMenu.MenuItems[i].Index + 1;
                     this.TourneyMenu.MenuItems[i].MenuItems[j].MenuItems[k].PerformClick();
                  }
               }
            }
         }
         this.Cursor = Cursors.Default;
      }

      private void sortName_CheckedChanged(object sender, System.EventArgs e)
      {
         if (sortName.Checked) 
         {
            if ( this.checkBoxDescending.Checked )
               m_currentSort = "Name DESC";
            else
               m_currentSort = "Name ASC";
            WriteHTML();
         }
      }

      private void sortFlight_CheckedChanged(object sender, System.EventArgs e)
      {
         if (sortFlight.Checked) 
         {
            if ( this.checkBoxDescending.Checked )
               m_currentSort = "Flight ASC, Total DESC";
            else
               m_currentSort = "Flight ASC, Total ASC";
            WriteHTML();
         }
      }

      private void sortTotal_CheckedChanged(object sender, System.EventArgs e)
      {
         if (sortTotal.Checked) 
         {
            if ( this.checkBoxDescending.Checked )
               m_currentSort = "Total DESC";
            else
               m_currentSort = "Total ASC";
            WriteHTML();
         }
      }

      private void sortHole_SelectedIndexChanged(object sender, System.EventArgs e)
      {
         sortName.Checked = false;
         
         if ( this.checkBoxDescending.Checked )
            m_currentSort = sortHole.SelectedItem.ToString() + " DESC";
         else
            m_currentSort = sortHole.SelectedItem.ToString() + " ASC";
         WriteHTML();
      }

      private void checkBoxDescending_CheckedChanged(object sender, System.EventArgs e)
      {
         if ( m_currentSort != null && m_currentSort != "" )
         {
            if ( checkBoxDescending.Checked ) 
            {
               m_currentSort = m_currentSort.Remove(m_currentSort.Length-3,3);
               m_currentSort = m_currentSort + "DESC";
            }
            else 
            {
               m_currentSort = m_currentSort.Remove(m_currentSort.Length-4,4);
               m_currentSort = m_currentSort + "ASC";
            }
            WriteHTML();
         }
      }

      private void TourneyMenu_Click(object sender, System.EventArgs e)
      {
      
      }

      private void TotalBestRounds()
      {
         this.m_numbestrounds = this.comboBoxShowRounds.SelectedIndex;
         AddTotalColumn(TEAMTABLE);
         WriteHTML();
      }
      private void comboBoxShowRounds_SelectedIndexChanged(object sender, System.EventArgs e)
      {
         TotalBestRounds();
      }

      private void checkBoxIncludeFinal_CheckedChanged(object sender, System.EventArgs e)
      {
         TotalBestRounds();
      }

      int scrollNum = 0;

      private void scrollPrev_Click(object sender, EventArgs e)
      {
         scrollNum -= 1;
         if (scrollNum == -1) scrollNum = DSResults.Tables[TEAMTABLE].Rows.Count - 1;
         DoScroll();
      }

      private void scrollCycle_Click(object sender, EventArgs e)
      {
         if (timerScroll.Enabled)
         {
            timerScroll.Stop();
            scrollCycle.Text = "Cycle";
         }
         else
         {
            timerScroll.Start();
            scrollCycle.Text = "Stop";
         }
      }

      private void scrollNext_Click(object sender, EventArgs e)
      {
         scrollNum += 1;
         if (scrollNum == DSResults.Tables[TEAMTABLE].Rows.Count) scrollNum = 0;
         DoScroll();
      }
      
      private void ScrollScores_Click(object sender, EventArgs e)
      {
         BestBallTotals("Best Ball Net Par Points", true, true, 1);
         if (m_currentRound == 2) BestBallTotals("Both Balls Net Par Points", true, true, 2);
         scrollNum = 0;
         scrollPrev.Visible = true;
         scrollNext.Visible = true;
         scrollCycle.Visible = true;
         DSResults.Tables[TEAMTABLE].DefaultView.Sort = "Total";
         DoScroll();
      }

      private string GetGolferInfo(Golfer golfer)
      {
         string html = golfer.FirstName + " " + golfer.LastName;
         for (int round = 1; round <= m_currentRound; round++)
         {
            golfer.RoundNumber = round;
            golfer.LoadGolfersScore();
            int totalscore = 0;
            foreach (int x in golfer.Scores)
            {
               totalscore += x;
            }
            GolfCourse course = m_tourney.Course(m_tourney.GetCourseID(m_currentRound), golfer.Tee);
            int handi = Formulas.Handicap(golfer.HcpIndex, course.Slope);
            int netscore = totalscore - handi;
            html += " <font style='font-size:13pt;'>(" + handi.ToString() + ")</font><font style='font-size:16pt;'><br>Score Round " + round.ToString() + ": " + totalscore.ToString() + " <br>Net: " + netscore + "</font><br>";
         }
         return html + "<br>";
      }

      private void DoScroll()
      {
         System.Text.StringBuilder htmlstring = new System.Text.StringBuilder();
         htmlstring.Append("<html><style>body {margin-left:10px; background-color:#ffffff; font-family:sans-serif; font-size:18pt; } td { font-family:verdana; font-size:8pt; } .skin { background-color:red; font-weight:bold;} .rowheader { background-color:#394fbc; color:white; font-weight:bold; font-size:10pt; } .row1 { background-color:87cefa; } .row2 { background-color:fff8dc; } .row3 { background-color:e0eee0; } .row4 { background-color:e6e6fa; } .row5 { background-color:deb887; } .row6 { background-color:f0e68c; } .row7 { background-color:ffa500; } .row8 { background-color:ff1493; } .row9 { background-color:d8bfd8; } .row10 { background-color:dccdc; } table { background-color:black; } </style><body>");
         htmlstring.Append("<img align='right' valign='top' src='cj.jpg'><small>" + m_tourney.Name + " - Round " + m_currentRound + " - " + m_tourney.Course(m_tourney.GetCourseID(m_currentRound), 0).Name + "</small><br><br>");
         int teamid = int.Parse(DSResults.Tables[TEAMTABLE].DefaultView[scrollNum]["TeamID"].ToString());
         Team team = m_tourney.GetTeam(teamid);
         htmlstring.Append(GetGolferInfo(team.Golfers()[0]));
         htmlstring.Append(GetGolferInfo(team.Golfers()[1]));
         htmlstring.Append("Best Ball Net Par Points: <font class='row2'>" + DSResults.Tables[TEAMTABLE].DefaultView[scrollNum]["Best Ball Net Par Points"] + "</font><br><br>");
         if (m_currentRound == 2)
         {
            htmlstring.Append("Both Balls Net Par Points: <font class='row2'>" + DSResults.Tables[TEAMTABLE].DefaultView[scrollNum]["Both Balls Net Par Points"] + "</font><br><br>");
            htmlstring.Append("Total: <font class='row2'>" + DSResults.Tables[TEAMTABLE].DefaultView[scrollNum]["Total"] + "</font><br><br>");
         }
         int place = DSResults.Tables[TEAMTABLE].Rows.Count - scrollNum;
         htmlstring.Append("Current Place overall: <font class='row2'>" + place + "</font><br><br></body></html>");

         FillBrowser(htmlstring.ToString());

      }

      private void timerScroll_Tick(object sender, EventArgs e)
      {
         scrollNum += 1;
         if (scrollNum == DSResults.Tables[TEAMTABLE].Rows.Count) scrollNum = 0;
         DoScroll();
      }
   }
}
