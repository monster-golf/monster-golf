using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace MonsterGolf
{
   /// <summary>
   /// Summary description for Results.
   /// </summary>
   public class Results : System.Windows.Forms.Form
   {
      private ListDictionary colorDictionary;
      private List<Color> _flightColors;
      private Tournament m_tourney;
      private int m_currentRound = -1;
      private string m_currentSort;
      private bool m_hasflights = true;
      private System.Data.DataSet DSResults;
      private DataTable DTResults;
      private int _throwoutrounds = 0;
      private int m_golfersperteam = 1;
      Team[] teamlist = null;
      private System.Windows.Forms.CheckBox checkBoxStrokeControl;
      private System.Windows.Forms.CheckBox checkBoxScrollScores;
      private Label label1;
      private ComboBox comboBoxScoringOption;
      private Label label2;
      private ComboBox comboBoxRounds;
      private CheckBox checkBoxTotalsOnly;
      private Label labelView;
      private CheckBox checkBoxHighlight;
      private ScrollScores scrollScores1;
      private DataGridView dataGridViewResults;
      private LinkLabel linkLabel1;
      private Label labelThrowOut;
      private TextBox textBoxThrowOut;
      private CheckBox checkBoxIncludeFinal;
      private CheckBox checkBoxIncludeIndvd;
      private CheckBox checkBoxColorFlights;

      public Results(int TournamentID)
      {
         //
         // Required for Windows Form Designer support
         //
         InitializeComponent();
         m_tourney = new Tournament(TournamentID);
         teamlist = m_tourney.Teams();
         scrollScores1.Tournament = m_tourney;

         this.Text = m_tourney.Name + " - Results";
         
         // set flights or not
         string[] flights = m_tourney.Flights();
         m_hasflights = (flights != null && flights.Length > 0);
         if (m_hasflights)
         {
            if (flights.Length == 1 && flights[0] == "(Unknown flight)")
               m_hasflights = false;
         }
         else
         {
            this.checkBoxColorFlights.Enabled = false;
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

         if (System.Configuration.ConfigurationManager.AppSettings["showfilterbest"] != "true")
         {
            labelThrowOut.Visible = false;
            textBoxThrowOut.Visible = false;
            checkBoxIncludeFinal.Visible = false;
         }
      }

      public Tournament Tournament
      {
         get { return m_tourney; }
      }

      private void AddScoringOptions()
      {
         // golfers per team
         DataSet ds = DB.GetDataSet("select * from TeamPlayers where TournamentID=" +
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
         ds = DB.GetDataSet("select * from ScoringFormats order by FormatName");
         ListItem li = new ListItem("-1", "-- select --");
         comboBoxScoringOption.Items.Add(li);

         for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
         {
            int minplayers = int.Parse(ds.Tables[0].Rows[i]["MinPlayers"].ToString());
            int maxplayers = int.Parse(ds.Tables[0].Rows[i]["MaxPlayers"].ToString());
            if (m_golfersperteam >= minplayers)
            {
               li = new ListItem(
                   ds.Tables[0].Rows[i]["FormatID"].ToString(),
                   ds.Tables[0].Rows[i]["FormatName"].ToString());
               comboBoxScoringOption.Items.Add(li);
            }
         }
         comboBoxScoringOption.Text = "-- select --";
      }

      public void AddTournamentToMenu()
      {
         // add the options
         AddScoringOptions();

         int courseid = 0;

         // add the rounds
         ListItem li = new ListItem("-1", "All");
         comboBoxRounds.Items.Add(li);

         for (int roundnum = 1; roundnum <= m_tourney.NumberOfRounds; roundnum++)
         {
            courseid = m_tourney.GetCourseID(roundnum);
            li = new ListItem(roundnum.ToString(),
                "Round " + roundnum.ToString() + " - " + m_tourney.Course(courseid, 0).Name);
            comboBoxRounds.Items.Add(li);
         }
         comboBoxRounds.Text = "All";
      }

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      protected override void Dispose(bool disposing)
      {
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code
      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
          System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
          System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
          System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
          System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Results));
          this.DSResults = new System.Data.DataSet();
          this.checkBoxStrokeControl = new System.Windows.Forms.CheckBox();
          this.label1 = new System.Windows.Forms.Label();
          this.comboBoxScoringOption = new System.Windows.Forms.ComboBox();
          this.label2 = new System.Windows.Forms.Label();
          this.comboBoxRounds = new System.Windows.Forms.ComboBox();
          this.checkBoxTotalsOnly = new System.Windows.Forms.CheckBox();
          this.labelView = new System.Windows.Forms.Label();
          this.checkBoxHighlight = new System.Windows.Forms.CheckBox();
          this.checkBoxScrollScores = new System.Windows.Forms.CheckBox();
          this.dataGridViewResults = new System.Windows.Forms.DataGridView();
          this.linkLabel1 = new System.Windows.Forms.LinkLabel();
          this.checkBoxColorFlights = new System.Windows.Forms.CheckBox();
          this.labelThrowOut = new System.Windows.Forms.Label();
          this.textBoxThrowOut = new System.Windows.Forms.TextBox();
          this.checkBoxIncludeFinal = new System.Windows.Forms.CheckBox();
          this.checkBoxIncludeIndvd = new System.Windows.Forms.CheckBox();
          this.scrollScores1 = new MonsterGolf.ScrollScores();
          ((System.ComponentModel.ISupportInitialize)(this.DSResults)).BeginInit();
          ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).BeginInit();
          this.SuspendLayout();
          // 
          // DSResults
          // 
          this.DSResults.DataSetName = "NewDataSet";
          this.DSResults.Locale = new System.Globalization.CultureInfo("en-US");
          // 
          // checkBoxStrokeControl
          // 
          this.checkBoxStrokeControl.Location = new System.Drawing.Point(718, 31);
          this.checkBoxStrokeControl.Name = "checkBoxStrokeControl";
          this.checkBoxStrokeControl.Size = new System.Drawing.Size(128, 24);
          this.checkBoxStrokeControl.TabIndex = 17;
          this.checkBoxStrokeControl.Text = "Use Stroke Control?";
          this.checkBoxStrokeControl.Visible = false;
          // 
          // label1
          // 
          this.label1.AutoSize = true;
          this.label1.Location = new System.Drawing.Point(271, 34);
          this.label1.Name = "label1";
          this.label1.Size = new System.Drawing.Size(43, 13);
          this.label1.TabIndex = 21;
          this.label1.Text = "Scoring";
          // 
          // comboBoxScoringOption
          // 
          this.comboBoxScoringOption.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
          this.comboBoxScoringOption.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
          this.comboBoxScoringOption.FormattingEnabled = true;
          this.comboBoxScoringOption.Location = new System.Drawing.Point(316, 31);
          this.comboBoxScoringOption.Name = "comboBoxScoringOption";
          this.comboBoxScoringOption.Size = new System.Drawing.Size(396, 21);
          this.comboBoxScoringOption.TabIndex = 7;
          this.comboBoxScoringOption.SelectedIndexChanged += new System.EventHandler(this.ScoringOption_Click);
          // 
          // label2
          // 
          this.label2.AutoSize = true;
          this.label2.Location = new System.Drawing.Point(9, 34);
          this.label2.Name = "label2";
          this.label2.Size = new System.Drawing.Size(39, 13);
          this.label2.TabIndex = 23;
          this.label2.Text = "Round";
          // 
          // comboBoxRounds
          // 
          this.comboBoxRounds.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
          this.comboBoxRounds.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
          this.comboBoxRounds.FormattingEnabled = true;
          this.comboBoxRounds.Location = new System.Drawing.Point(53, 31);
          this.comboBoxRounds.Name = "comboBoxRounds";
          this.comboBoxRounds.Size = new System.Drawing.Size(208, 21);
          this.comboBoxRounds.TabIndex = 6;
          this.comboBoxRounds.SelectedIndexChanged += new System.EventHandler(this.Round_Select);
          // 
          // checkBoxTotalsOnly
          // 
          this.checkBoxTotalsOnly.AutoSize = true;
          this.checkBoxTotalsOnly.Checked = true;
          this.checkBoxTotalsOnly.CheckState = System.Windows.Forms.CheckState.Checked;
          this.checkBoxTotalsOnly.Location = new System.Drawing.Point(39, 8);
          this.checkBoxTotalsOnly.Name = "checkBoxTotalsOnly";
          this.checkBoxTotalsOnly.Size = new System.Drawing.Size(79, 17);
          this.checkBoxTotalsOnly.TabIndex = 1;
          this.checkBoxTotalsOnly.Text = "Totals Only";
          this.checkBoxTotalsOnly.UseVisualStyleBackColor = true;
          this.checkBoxTotalsOnly.CheckedChanged += new System.EventHandler(this.checkBoxTotalsOnly_CheckedChanged);
          // 
          // labelView
          // 
          this.labelView.AutoSize = true;
          this.labelView.Location = new System.Drawing.Point(6, 9);
          this.labelView.Name = "labelView";
          this.labelView.Size = new System.Drawing.Size(30, 13);
          this.labelView.TabIndex = 26;
          this.labelView.Text = "View";
          // 
          // checkBoxHighlight
          // 
          this.checkBoxHighlight.AutoSize = true;
          this.checkBoxHighlight.Checked = true;
          this.checkBoxHighlight.CheckState = System.Windows.Forms.CheckState.Checked;
          this.checkBoxHighlight.Location = new System.Drawing.Point(233, 8);
          this.checkBoxHighlight.Name = "checkBoxHighlight";
          this.checkBoxHighlight.Size = new System.Drawing.Size(91, 17);
          this.checkBoxHighlight.TabIndex = 3;
          this.checkBoxHighlight.Text = "Highlight Best";
          this.checkBoxHighlight.UseVisualStyleBackColor = true;
          this.checkBoxHighlight.CheckedChanged += new System.EventHandler(this.checkBoxHighlight_CheckedChanged);
          // 
          // checkBoxScrollScores
          // 
          this.checkBoxScrollScores.AutoSize = true;
          this.checkBoxScrollScores.Location = new System.Drawing.Point(405, 8);
          this.checkBoxScrollScores.Name = "checkBoxScrollScores";
          this.checkBoxScrollScores.Size = new System.Drawing.Size(88, 17);
          this.checkBoxScrollScores.TabIndex = 5;
          this.checkBoxScrollScores.Text = "Scroll Scores";
          this.checkBoxScrollScores.UseVisualStyleBackColor = true;
          this.checkBoxScrollScores.CheckedChanged += new System.EventHandler(this.scrollCycle_Click);
          // 
          // dataGridViewResults
          // 
          this.dataGridViewResults.AllowUserToAddRows = false;
          this.dataGridViewResults.AllowUserToDeleteRows = false;
          this.dataGridViewResults.AllowUserToResizeRows = false;
          this.dataGridViewResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
          this.dataGridViewResults.BackgroundColor = System.Drawing.Color.White;
          this.dataGridViewResults.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
          dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
          dataGridViewCellStyle1.BackColor = System.Drawing.Color.Green;
          dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Yellow;
          dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Green;
          dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Yellow;
          dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
          this.dataGridViewResults.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
          this.dataGridViewResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
          dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
          dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
          dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
          dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Window;
          dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.DeepSkyBlue;
          dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
          this.dataGridViewResults.DefaultCellStyle = dataGridViewCellStyle2;
          this.dataGridViewResults.Location = new System.Drawing.Point(6, 55);
          this.dataGridViewResults.Name = "dataGridViewResults";
          this.dataGridViewResults.ReadOnly = true;
          this.dataGridViewResults.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
          this.dataGridViewResults.RowHeadersVisible = false;
          dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
          this.dataGridViewResults.RowsDefaultCellStyle = dataGridViewCellStyle3;
          this.dataGridViewResults.RowTemplate.Height = 24;
          this.dataGridViewResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
          this.dataGridViewResults.Size = new System.Drawing.Size(840, 447);
          this.dataGridViewResults.TabIndex = 31;
          this.dataGridViewResults.Visible = false;
          this.dataGridViewResults.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.OnCellMouseClick);
          this.dataGridViewResults.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseLeave);
          this.dataGridViewResults.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.OnSort);
          this.dataGridViewResults.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dataGridViewResults_MouseClick);
          this.dataGridViewResults.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseEnter);
          // 
          // linkLabel1
          // 
          this.linkLabel1.AutoSize = true;
          this.linkLabel1.Location = new System.Drawing.Point(495, 9);
          this.linkLabel1.Name = "linkLabel1";
          this.linkLabel1.Size = new System.Drawing.Size(84, 13);
          this.linkLabel1.TabIndex = 32;
          this.linkLabel1.TabStop = true;
          this.linkLabel1.Text = "Sort Flight, Total";
          this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.SortByFlightAndTotal);
          // 
          // checkBoxColorFlights
          // 
          this.checkBoxColorFlights.AutoSize = true;
          this.checkBoxColorFlights.Checked = true;
          this.checkBoxColorFlights.CheckState = System.Windows.Forms.CheckState.Checked;
          this.checkBoxColorFlights.Location = new System.Drawing.Point(323, 9);
          this.checkBoxColorFlights.Name = "checkBoxColorFlights";
          this.checkBoxColorFlights.Size = new System.Drawing.Size(83, 17);
          this.checkBoxColorFlights.TabIndex = 4;
          this.checkBoxColorFlights.Text = "Color Flights";
          this.checkBoxColorFlights.UseVisualStyleBackColor = true;
          this.checkBoxColorFlights.CheckedChanged += new System.EventHandler(this.checkBoxColorFlights_CheckedChanged);
          // 
          // labelThrowOut
          // 
          this.labelThrowOut.AutoSize = true;
          this.labelThrowOut.Location = new System.Drawing.Point(597, 10);
          this.labelThrowOut.Name = "labelThrowOut";
          this.labelThrowOut.Size = new System.Drawing.Size(86, 13);
          this.labelThrowOut.TabIndex = 34;
          this.labelThrowOut.Text = "Throw out worst:";
          // 
          // textBoxThrowOut
          // 
          this.textBoxThrowOut.Location = new System.Drawing.Point(685, 5);
          this.textBoxThrowOut.Name = "textBoxThrowOut";
          this.textBoxThrowOut.Size = new System.Drawing.Size(27, 20);
          this.textBoxThrowOut.TabIndex = 35;
          this.textBoxThrowOut.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
          // 
          // checkBoxIncludeFinal
          // 
          this.checkBoxIncludeFinal.AutoSize = true;
          this.checkBoxIncludeFinal.Checked = true;
          this.checkBoxIncludeFinal.CheckState = System.Windows.Forms.CheckState.Checked;
          this.checkBoxIncludeFinal.Location = new System.Drawing.Point(718, 9);
          this.checkBoxIncludeFinal.Name = "checkBoxIncludeFinal";
          this.checkBoxIncludeFinal.Size = new System.Drawing.Size(86, 17);
          this.checkBoxIncludeFinal.TabIndex = 36;
          this.checkBoxIncludeFinal.Text = "Include Final";
          this.checkBoxIncludeFinal.UseVisualStyleBackColor = true;
          this.checkBoxIncludeFinal.CheckedChanged += new System.EventHandler(this.checkBoxIncludeFinal_CheckedChanged);
          // 
          // checkBoxIncludeIndvd
          // 
          this.checkBoxIncludeIndvd.AutoSize = true;
          this.checkBoxIncludeIndvd.Checked = true;
          this.checkBoxIncludeIndvd.CheckState = System.Windows.Forms.CheckState.Checked;
          this.checkBoxIncludeIndvd.Location = new System.Drawing.Point(119, 8);
          this.checkBoxIncludeIndvd.Name = "checkBoxIncludeIndvd";
          this.checkBoxIncludeIndvd.Size = new System.Drawing.Size(114, 17);
          this.checkBoxIncludeIndvd.TabIndex = 2;
          this.checkBoxIncludeIndvd.Text = "Include Individuals";
          this.checkBoxIncludeIndvd.UseVisualStyleBackColor = true;
          this.checkBoxIncludeIndvd.CheckedChanged += new System.EventHandler(this.checkBoxIncludeIndvd_CheckChanged);
          // 
          // scrollScores1
          // 
          this.scrollScores1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                      | System.Windows.Forms.AnchorStyles.Right)));
          this.scrollScores1.BackColor = System.Drawing.Color.LightGray;
          this.scrollScores1.Location = new System.Drawing.Point(6, 509);
          this.scrollScores1.Margin = new System.Windows.Forms.Padding(4);
          this.scrollScores1.Name = "scrollScores1";
          this.scrollScores1.ScrollPlayers = false;
          this.scrollScores1.Size = new System.Drawing.Size(840, 32);
          this.scrollScores1.TabIndex = 30;
          this.scrollScores1.PlayerViewClosed += new MonsterGolf.onPlayerViewClose(this.scrollScores1_PlayerViewClosed);
          this.scrollScores1.PlayerZoom += new MonsterGolf.onZoom(this.scrollScores1_PlayerZoom);
          // 
          // Results
          // 
          this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
          this.AutoSize = true;
          this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
          this.BackColor = System.Drawing.Color.LightGray;
          this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
          this.ClientSize = new System.Drawing.Size(849, 542);
          this.Controls.Add(this.checkBoxScrollScores);
          this.Controls.Add(this.checkBoxColorFlights);
          this.Controls.Add(this.linkLabel1);
          this.Controls.Add(this.checkBoxHighlight);
          this.Controls.Add(this.checkBoxIncludeIndvd);
          this.Controls.Add(this.checkBoxIncludeFinal);
          this.Controls.Add(this.textBoxThrowOut);
          this.Controls.Add(this.labelThrowOut);
          this.Controls.Add(this.dataGridViewResults);
          this.Controls.Add(this.scrollScores1);
          this.Controls.Add(this.labelView);
          this.Controls.Add(this.checkBoxTotalsOnly);
          this.Controls.Add(this.comboBoxScoringOption);
          this.Controls.Add(this.label1);
          this.Controls.Add(this.checkBoxStrokeControl);
          this.Controls.Add(this.comboBoxRounds);
          this.Controls.Add(this.label2);
          this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
          this.Name = "Results";
          this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
          this.Text = "Results";
          this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
          this.Resize += new System.EventHandler(this.OnResize);
          ((System.ComponentModel.ISupportInitialize)(this.DSResults)).EndInit();
          ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).EndInit();
          this.ResumeLayout(false);
          this.PerformLayout();

      }
      #endregion

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
      Dictionary<string, int> bestScore = new Dictionary<string,int>();
      Dictionary<string, int> bestNet = new Dictionary<string,int>();
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

          if (checkBoxHighlight.Checked)
          {
              ListItem li = (ListItem)comboBoxScoringOption.SelectedItem;
              Formulas.ScoringType st = Formulas.GetScoringType(li.ID);
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
          ListItem li = (ListItem)comboBoxScoringOption.SelectedItem;
          if (li.ID != "-1")
          {
              Formulas.ScoringType st = Formulas.GetScoringType(li.ID);
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
            this.dataGridViewResults.Visible = false;
            return;
         }

         this.dataGridViewResults.Visible = true;

         if (m_currentSort == null) SetDefaultSort();

         // filter the default view
         if (comboBoxRounds.SelectedIndex > 0)
         {
            DTResults.DefaultView.RowFilter = "Round = '" + ((ListItem)comboBoxRounds.SelectedItem).ID + "'";
         }
         else
         {
            DTResults.DefaultView.RowFilter = "";
         }
         // sort the default view
         DTResults.DefaultView.Sort = m_currentSort;

         // set up column mappings
         for (int x = 0; x < DTResults.Columns.Count; x++)
         {
            if (checkBoxTotalsOnly.Checked)
            {
               string colName = DTResults.Columns[x].ColumnName;
               if (colName != "Name" && colName != "Flight" && colName != "Round" && colName != "Total" && colName != "Overall")
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
         if ( !m_hasflights )
            DTResults.Columns["Flight"].ColumnMapping = System.Data.MappingType.Hidden;

         this.dataGridViewResults.DataSource = null;

         if (checkBoxTotalsOnly.Checked)
         {
            DataTable dt = new DataTable();
            dt.Columns.Add("TeamID");
            dt.Columns["TeamID"].ColumnMapping = System.Data.MappingType.Hidden;
            dt.Columns.Add("Flight");
            dt.Columns.Add("Name");
            if (!m_hasflights)
               dt.Columns["Flight"].ColumnMapping = System.Data.MappingType.Hidden;
            for (int i = 0; i < m_tourney.NumberOfRounds; i++)
            {
                dt.Columns.Add("Round " + (i + 1).ToString(), typeof(int));
                if (comboBoxRounds.SelectedIndex > 0)
                {
                    if (comboBoxRounds.SelectedIndex != (i + 1))
                    {
                        dt.Columns["Round " + (i + 1).ToString()].ColumnMapping = System.Data.MappingType.Hidden;
                    }
                }
            }
            dt.Columns.Add("Overall", typeof(int));

            if (comboBoxRounds.SelectedIndex > 0)
            {
                dt.Columns["Overall"].ColumnMapping = System.Data.MappingType.Hidden;
            }
            if (checkBoxIncludeIndvd.Checked)
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

                        if (comboBoxRounds.SelectedIndex > 0)
                        {
                            if (comboBoxRounds.SelectedIndex != (i + 1))
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
                  if ( row != null )
                     dt.Rows.Add(row);
                  row = dt.NewRow();
                  curTeam = DTResults.DefaultView[i]["TeamID"].ToString();
                  row["TeamID"] = curTeam;
               }
               row["Name"] = DTResults.DefaultView[i]["Name"].ToString();
               row["Flight"] = DTResults.DefaultView[i]["Flight"].ToString();
               row["Round " + DTResults.DefaultView[i]["Round"].ToString()] = int.Parse(DTResults.DefaultView[i]["Total"].ToString());
               row["Overall"] = int.Parse(DTResults.DefaultView[i]["Overall"].ToString());
               if (checkBoxIncludeIndvd.Checked)
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
            if ( row != null )
               dt.Rows.Add(row);
            this.dataGridViewResults.DataSource = dt.DefaultView;

            if (checkBoxIncludeIndvd.Checked)
            {
                for (int i = 0; i < m_tourney.NumberOfRounds; i++)
                {
                    for (int g = 1; g <= m_golfersperteam; g++)
                    {
                        if (dataGridViewResults.Columns["P" + g.ToString() + " R" + (i + 1).ToString()] != null)
                            dataGridViewResults.Columns["P" + g.ToString() + " R" + (i + 1).ToString()].HeaderText = "Player";
                        if (dataGridViewResults.Columns["P" + g.ToString() + " R" + (i + 1).ToString() + " Score"] != null)
                        {
                            dataGridViewResults.Columns["P" + g.ToString() + " R" + (i + 1).ToString() + " Score"].HeaderText = "R" + (i + 1).ToString();
                            dataGridViewResults.Columns["P" + g.ToString() + " R" + (i + 1).ToString() + " Score"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                            dataGridViewResults.Columns["P" + g.ToString() + " R" + (i + 1).ToString() + " Score"].Width = 30;
                            dataGridViewResults.Columns["P" + g.ToString() + " R" + (i + 1).ToString() + " Score"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            dataGridViewResults.Columns["P" + g.ToString() + " R" + (i + 1).ToString() + " Score"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        }
                        if (dataGridViewResults.Columns["P" + g.ToString() + " R" + (i + 1).ToString() + " HCP"] != null)
                        {
                            dataGridViewResults.Columns["P" + g.ToString() + " R" + (i + 1).ToString() + " HCP"].HeaderText = "Hcp";
                            dataGridViewResults.Columns["P" + g.ToString() + " R" + (i + 1).ToString() + " HCP"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                            dataGridViewResults.Columns["P" + g.ToString() + " R" + (i + 1).ToString() + " HCP"].Width = 32;
                            dataGridViewResults.Columns["P" + g.ToString() + " R" + (i + 1).ToString() + " HCP"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            dataGridViewResults.Columns["P" + g.ToString() + " R" + (i + 1).ToString() + " HCP"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        }
                        if (dataGridViewResults.Columns["P" + g.ToString() + " R" + (i + 1).ToString() + " Net"] != null)
                        {
                            dataGridViewResults.Columns["P" + g.ToString() + " R" + (i + 1).ToString() + " Net"].HeaderText = "Net";
                            dataGridViewResults.Columns["P" + g.ToString() + " R" + (i + 1).ToString() + " Net"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                            dataGridViewResults.Columns["P" + g.ToString() + " R" + (i + 1).ToString() + " Net"].Width = 30;
                            dataGridViewResults.Columns["P" + g.ToString() + " R" + (i + 1).ToString() + " Net"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            dataGridViewResults.Columns["P" + g.ToString() + " R" + (i + 1).ToString() + " Net"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        }
                        if (dataGridViewResults.Columns["P" + g.ToString() + " R" + (i + 1).ToString() + " SC"] != null)
                        {
                            dataGridViewResults.Columns["P" + g.ToString() + " R" + (i + 1).ToString() + " SC"].HeaderText = "SC";
                            dataGridViewResults.Columns["P" + g.ToString() + " R" + (i + 1).ToString() + " SC"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                            dataGridViewResults.Columns["P" + g.ToString() + " R" + (i + 1).ToString() + " SC"].Width = 30;
                            dataGridViewResults.Columns["P" + g.ToString() + " R" + (i + 1).ToString() + " SC"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            dataGridViewResults.Columns["P" + g.ToString() + " R" + (i + 1).ToString() + " SC"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        }
                    }
                }
            }
         }
         else
         {
            this.dataGridViewResults.DataSource = DTResults.DefaultView;

            for (int g = 0; g < m_golfersperteam; g++)
            {
                if (dataGridViewResults.Columns["Player" + g.ToString()] != null)
                {
                    dataGridViewResults.Columns["Player" + g.ToString()].HeaderText = "Player";
                    dataGridViewResults.Columns["Player" + g.ToString()].Visible = checkBoxIncludeIndvd.Checked;
                }
                if (dataGridViewResults.Columns["PlayerGross" + g.ToString()] != null)
                {
                    dataGridViewResults.Columns["PlayerGross" + g.ToString()].HeaderText = "Gross";
                    dataGridViewResults.Columns["PlayerGross" + g.ToString()].Visible = checkBoxIncludeIndvd.Checked;
                    dataGridViewResults.Columns["PlayerGross" + g.ToString()].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dataGridViewResults.Columns["PlayerGross" + g.ToString()].Width = 40;
                    dataGridViewResults.Columns["PlayerGross" + g.ToString()].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridViewResults.Columns["PlayerGross" + g.ToString()].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                if (dataGridViewResults.Columns["PlayerHCP" + g.ToString()] != null)
                {
                    dataGridViewResults.Columns["PlayerHCP" + g.ToString()].HeaderText = "Hcp";
                    dataGridViewResults.Columns["PlayerHCP" + g.ToString()].Visible = checkBoxIncludeIndvd.Checked;
                    dataGridViewResults.Columns["PlayerHCP" + g.ToString()].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dataGridViewResults.Columns["PlayerHCP" + g.ToString()].Width = 32;
                    dataGridViewResults.Columns["PlayerHCP" + g.ToString()].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridViewResults.Columns["PlayerHCP" + g.ToString()].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                if (dataGridViewResults.Columns["PlayerNet" + g.ToString()] != null)
                {
                    dataGridViewResults.Columns["PlayerNet" + g.ToString()].HeaderText = "Net";
                    dataGridViewResults.Columns["PlayerNet" + g.ToString()].Visible = checkBoxIncludeIndvd.Checked;
                    dataGridViewResults.Columns["PlayerNet" + g.ToString()].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dataGridViewResults.Columns["PlayerNet" + g.ToString()].Width = 30;
                    dataGridViewResults.Columns["PlayerNet" + g.ToString()].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridViewResults.Columns["PlayerNet" + g.ToString()].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                if (dataGridViewResults.Columns["PlayerSC" + g.ToString()] != null)
                {
                    dataGridViewResults.Columns["PlayerSC" + g.ToString()].HeaderText = "SC";
                    dataGridViewResults.Columns["PlayerSC" + g.ToString()].Visible = checkBoxIncludeIndvd.Checked;
                    dataGridViewResults.Columns["PlayerSC" + g.ToString()].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dataGridViewResults.Columns["PlayerSC" + g.ToString()].Width = 30;
                    dataGridViewResults.Columns["PlayerSC" + g.ToString()].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridViewResults.Columns["PlayerSC" + g.ToString()].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
         }

         // default the score widths
         if (!this.checkBoxTotalsOnly.Checked)
         {
             for (int i = 1; i < 19; i++)
             {
                 this.dataGridViewResults.Columns[i.ToString()].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                 this.dataGridViewResults.Columns[i.ToString()].Width = 28;
                 this.dataGridViewResults.Columns[i.ToString()].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                 this.dataGridViewResults.Columns[i.ToString()].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
             }

             this.dataGridViewResults.Columns["Out"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
             this.dataGridViewResults.Columns["In"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
             this.dataGridViewResults.Columns["Total"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
             this.dataGridViewResults.Columns["Out"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
             this.dataGridViewResults.Columns["In"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
             this.dataGridViewResults.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
             this.dataGridViewResults.Columns["Out"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
             this.dataGridViewResults.Columns["In"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
             this.dataGridViewResults.Columns["Total"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
             this.dataGridViewResults.Columns["Out"].Width = 30;
             this.dataGridViewResults.Columns["In"].Width = 30;
             this.dataGridViewResults.Columns["Total"].Width = 40;
         }
         else
         {
             for (int i = 1; i <= m_tourney.NumberOfRounds; i++)
             {
                 if (this.dataGridViewResults.Columns["Round " + i.ToString()] != null)
                 {
                     this.dataGridViewResults.Columns["Round " + i.ToString()].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                     this.dataGridViewResults.Columns["Round " + i.ToString()].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                     this.dataGridViewResults.Columns["Round " + i.ToString()].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                     this.dataGridViewResults.Columns["Round " + i.ToString()].Width = 55;
                 }
             }
         }
         if (this.dataGridViewResults.Columns["Flight"] != null)
         {
             this.dataGridViewResults.Columns["Flight"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
             this.dataGridViewResults.Columns["Flight"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
             this.dataGridViewResults.Columns["Flight"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
             this.dataGridViewResults.Columns["Flight"].Width = 40;
         }
         if (this.dataGridViewResults.Columns["Overall"] != null)
         {
             this.dataGridViewResults.Columns["Overall"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
             this.dataGridViewResults.Columns["Overall"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
             this.dataGridViewResults.Columns["Overall"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
             this.dataGridViewResults.Columns["Overall"].Width = 50;
         }

         // set the highlight and flight colors
         SetGridColors();

         // unselect the selectd row
         this.dataGridViewResults.ClearSelection();

         // set the bottom scrolling
         ListItem li = (ListItem)comboBoxScoringOption.SelectedItem;
         this.scrollScores1.ScoringType = Formulas.GetScoringType(li.ID);
         this.scrollScores1.Results = DTResults;
         scrollScores1_PlayerZoom(null);
      }

      private void SetGridColors()
      {
         if (DTResults == null)
            return;

         System.Collections.Generic.Dictionary<string, int> bestscores = Highlights(DTResults.DefaultView);

         if (bestscores.Count > 0 || m_hasflights)
         {
            DataView view = ((DataView)this.dataGridViewResults.DataSource);
            for (int x = 0; x < view.Count; x++)
            {

               for (int i = 0; i < dataGridViewResults.ColumnCount; i++)
               {
                  if (m_hasflights && this.checkBoxColorFlights.Checked)
                  {
                     dataGridViewResults.Rows[x].Cells[i].Style.BackColor =
                        _flightColors[int.Parse(dataGridViewResults.Rows[x].Cells["Flight"].Value.ToString())];
                  }

                  if (bestscores.Count > 0 && bestscores.ContainsKey(dataGridViewResults.Columns[i].Name))
                  {
                      int score = 0;
                      if (int.TryParse(dataGridViewResults.Rows[x].Cells[i].Value.ToString(), out score))
                      {
                          if (bestscores[dataGridViewResults.Columns[i].Name] == score)
                          {
                              dataGridViewResults.Rows[x].Cells[i].Style.BackColor = Color.Red;
                              dataGridViewResults.Rows[x].Cells[i].Style.ForeColor = Color.White;
                          }
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

         DataView currentGridResults = (DataView)this.dataGridViewResults.DataSource;

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
               if (checkBoxTotalsOnly.Checked && colName == "Round")
               {
                  string roundTitles = "";
                  foreach (System.Data.DataRowView dr in currentGridResults)
                  {
                     if (!roundTitles.Contains(">Round " + dr[colName].ToString() + "<"))
                        roundTitles += "<td class=rowheader align=center><b>Round " + dr[colName].ToString() + "</b></td>";
                  }
                  htmlstring.Append(roundTitles);
               }
               else if (checkBoxTotalsOnly.Checked && colName == "Total")
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

                  if (checkBoxHighlight.Checked)
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

                  if (checkBoxTotalsOnly.Checked && currentGridResults.Table.Columns[x].ColumnName == "Round")
                  {
                     // don't show
                  }
                  else if (checkBoxTotalsOnly.Checked && currentGridResults.Table.Columns[x].ColumnName == "Total")
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
                        divscores.Append(this.AddImagesPopupScores(dr["TeamID"].ToString(), rowstring));
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
         //if (this.checkBoxScrollScores.Checked)
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

         if (checkBoxTotalsOnly.Checked)
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
         string filname = Application.StartupPath + "\\Results-" + this.m_tourney.Name.Replace(" ","") + DateTime.Now.ToString().Replace(" ", "").Replace("/","").Replace(":","") + ".html";
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

      private void ScoringOption_Click(object sender, System.EventArgs e)
      {
         ListItem li = (ListItem)((ComboBox)sender).SelectedItem;
         if (li.ID != "-1")
         {
             DTResults = Formulas.CalculateScores(this.m_tourney, teamlist, li.ID, _throwoutrounds, checkBoxIncludeFinal.Checked);
         }
         else
         {
            DTResults = null;
         }
         SetDefaultSort();
         OutputDataGrid();
      }

      private void Round_Select(object sender, EventArgs e)
      {
         m_currentRound = int.Parse(((ListItem)((ComboBox)sender).SelectedItem).ID);
         OutputDataGrid();
      }

      #region SPECIAL_SCROLLING

      private void scrollCycle_Click(object sender, EventArgs e)
      {
         if (this.checkBoxScrollScores.Checked)
         {
            // see if we should resized
            bool resize = !this.scrollScores1.PlayersInView;
            if (resize)
            {
               // show the first team so the first scroll moves the buttons properly
                if (dataGridViewResults.DataSource != null)
                {
                    this.scrollScores1.ShowTeam(((DataView)dataGridViewResults.DataSource)[0]["TeamID"].ToString());
                }
            }
            ResizeScrollScores();
            if (resize) this.OnResize(null, null);

            // start the scrolling
            scrollScores1.StartScroll();
         }
         else
         {
            scrollScores1.StopScroll();
            ResizeScrollScores();
            this.OnResize(null, null);
         }
      }
      #endregion

      private void checkBoxTotalsOnly_CheckedChanged(object sender, EventArgs e)
      {
         OutputDataGrid();
      }

      private void checkBoxHighlight_CheckedChanged(object sender, EventArgs e)
      {
         OutputDataGrid();
      }

      private void OnResize(object sender, EventArgs e)
      {
          int newGridHeight = this.scrollScores1.Top - this.comboBoxRounds.Bottom - 4;
         this.dataGridViewResults.Height = newGridHeight;
         this.dataGridViewResults.Width = this.Width - (this.dataGridViewResults.Left * 2) - 6;
         this.SetGridColors();
      }

      private void ResizeScrollScores()
      {
         this.scrollScores1.Height = this.scrollScores1.NecessaryHeight;
         this.scrollScores1.Top = this.Bottom - this.scrollScores1.Height - 4;
      }

      private void OnCellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
      {
         if (e.RowIndex > -1)
         {
            bool resize = !this.scrollScores1.PlayersInView;
            // show the team once to get the proper height
            this.scrollScores1.ShowTeam(((DataView)dataGridViewResults.DataSource)[e.RowIndex]["TeamID"].ToString());
            // set up the resized
            ResizeScrollScores();
            this.dataGridViewResults.CurrentCell = dataGridViewResults[e.ColumnIndex, e.RowIndex];
            //if (resize)
            this.OnResize(null, null);

            // show the team a second time after the resize for proper button positioning
            this.scrollScores1.ShowTeam(((DataView)dataGridViewResults.DataSource)[e.RowIndex]["TeamID"].ToString());
         }

         this.SetGridColors();
      }

      private void OnCellMouseEnter(object sender, DataGridViewCellEventArgs e)
      {
         if (e.RowIndex > -1 && e.ColumnIndex == 0)
         {
            this.Cursor = Cursors.Hand;
         }
      }

      private void OnCellMouseLeave(object sender, DataGridViewCellEventArgs e)
      {
         if (this.Cursor == Cursors.Hand)
            this.Cursor = Cursors.Default;
      }

      private void OnSort(object sender, DataGridViewCellMouseEventArgs e)
      {
          // this doesn't work becuase it is using built in sorting for the datagrid
          // which will not always match the datasource columns.
          //m_currentSort = dataGridViewResults.Columns[e.ColumnIndex].Name;
          //if (dataGridViewResults.SortOrder == SortOrder.Ascending) m_currentSort += " ASC";
          //else m_currentSort += " DESC";
         this.SetGridColors();
         this.dataGridViewResults.ClearSelection();
      }

      private void SortByFlightAndTotal(object sender, LinkLabelLinkClickedEventArgs e)
      {
          SetDefaultSort();
          this.OutputDataGrid();
      }

      private void checkBoxColorFlights_CheckedChanged(object sender, EventArgs e)
      {
         if (!((CheckBox)sender).Checked)
            this.OutputDataGrid(); // refresh the entire grid
         else
            this.SetGridColors(); // just set the colors
      }

      private void scrollScores1_PlayerViewClosed(EventArgs e)
      {
         if (this.checkBoxScrollScores.Checked)
         {
            // stop the scrolling
            this.checkBoxScrollScores.Checked = false;
         }
         else
         {
            // resize the grid
            ResizeScrollScores();
            this.OnResize(null, null);
         }
      }

      private void scrollScores1_PlayerZoom(EventArgs e)
      {
         ResizeScrollScores();
         this.OnResize(null, null);
      }

      private void textBox1_TextChanged(object sender, EventArgs e)
      {
         if (int.TryParse(textBoxThrowOut.Text, out this._throwoutrounds))
         {
            int maxThrowOuts = m_tourney.NumberOfRounds;
            if (checkBoxIncludeFinal.Checked)
               maxThrowOuts--;
            if (this._throwoutrounds <= maxThrowOuts)
            {
               ListItem li = (ListItem)this.comboBoxScoringOption.SelectedItem;
               DTResults = Formulas.CalculateScores(this.m_tourney, teamlist, li.ID, _throwoutrounds, checkBoxIncludeFinal.Checked);
               OutputDataGrid();
            }
            else
            {
               this._throwoutrounds = 0;
            }
         }
      }

      private void checkBoxIncludeFinal_CheckedChanged(object sender, EventArgs e)
      {
         textBox1_TextChanged(null, null);
      }

      private void checkBoxIncludeIndvd_CheckChanged(object sender, EventArgs e)
      {
          OutputDataGrid();
      }

      private void dataGridViewResults_MouseClick(object sender, MouseEventArgs e)
      {
          this.dataGridViewResults.ClearSelection();
      }
   }
}
