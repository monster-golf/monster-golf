using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace MonsterGolf
{
	/// <summary>
	/// Summary description for EditTournament.
	/// </summary>
	public class EditTournament : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
      private System.ComponentModel.Container components = null;
      private System.Windows.Forms.Button button2;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.TextBox textBox1;
      private System.Windows.Forms.Button button4;
      private System.Windows.Forms.Button button6;

      public Tournament m_tournament;
      public int TournamentID;
      private Field golfers;
      private bool loaded;
      //private EditFlight m_ef;
      private System.Windows.Forms.Button buttonResults;
      private Button button8;
      private Button button3;
      private ListBox listBox1;
      private Button button1;
      private Label label3;
      private LinkLabel linkUpdateWeb;
      public Tournaments ParentTournaments;

		public EditTournament()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
         TournamentID = -1;
         loaded = false;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditTournament));
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.buttonResults = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.linkUpdateWeb = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(392, 267);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(80, 24);
            this.button2.TabIndex = 1;
            this.button2.Text = "Close";
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Tournament Name:";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(8, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(320, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Enter Tournament Information";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(104, 24);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(282, 20);
            this.textBox1.TabIndex = 6;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(241, 268);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(80, 24);
            this.button4.TabIndex = 14;
            this.button4.Text = "Field";
            this.button4.Visible = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(284, 268);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(80, 23);
            this.button6.TabIndex = 16;
            this.button6.Text = "Add Scores";
            this.button6.Visible = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // buttonResults
            // 
            this.buttonResults.Location = new System.Drawing.Point(347, 267);
            this.buttonResults.Name = "buttonResults";
            this.buttonResults.Size = new System.Drawing.Size(80, 23);
            this.buttonResults.TabIndex = 22;
            this.buttonResults.Text = "View Results";
            this.buttonResults.Visible = false;
            this.buttonResults.Click += new System.EventHandler(this.buttonResults_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(97, 267);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(80, 24);
            this.button8.TabIndex = 20;
            this.button8.Text = "Remove";
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(11, 266);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(80, 24);
            this.button3.TabIndex = 13;
            this.button3.Text = "Add";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // listBox1
            // 
            this.listBox1.Location = new System.Drawing.Point(11, 75);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(461, 160);
            this.listBox1.TabIndex = 8;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            this.listBox1.DoubleClick += new System.EventHandler(this.EditCourse);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(392, 24);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 24);
            this.button1.TabIndex = 0;
            this.button1.Text = "Save";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "Courses:";
            // 
            // linkUpdateWeb
            // 
            this.linkUpdateWeb.AutoSize = true;
            this.linkUpdateWeb.Location = new System.Drawing.Point(362, 56);
            this.linkUpdateWeb.Name = "linkUpdateWeb";
            this.linkUpdateWeb.Size = new System.Drawing.Size(110, 13);
            this.linkUpdateWeb.TabIndex = 23;
            this.linkUpdateWeb.TabStop = true;
            this.linkUpdateWeb.Text = "Update Web Tourney";
            this.linkUpdateWeb.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkUpdateWeb_LinkClicked);
            // 
            // EditTournament
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(712, 436);
            this.Controls.Add(this.linkUpdateWeb);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.buttonResults);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EditTournament";
            this.Text = "EditTournament";
            this.Load += new System.EventHandler(this.EditTournament_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

      }
		#endregion

      private void EditTournament_Load(object sender, System.EventArgs e)
      {
         if ( TournamentID != -1 )
         {
            try
            {
               if ( !loaded ) 
               {
                  m_tournament = new Tournament(TournamentID);
                  textBox1.Text = m_tournament.Name;
                  loaded = true;
                  ShowCourses();
                  ShowFlights();
               }
            }
            catch (Exception ex)
            {
               MessageBox.Show("Unable to open tournament, error: " + ex.Message);
               TournamentID = -1;
            }
         }
         EnableItems();
      }

      private void EnableItems()
      {
         label3.Enabled = (TournamentID != -1);
         //label5.Enabled = (TournamentID != -1);
         listBox1.Enabled = (TournamentID != -1);
         //listBox2.Enabled = (TournamentID != -1);
         button3.Enabled = (TournamentID != -1);
         button8.Enabled = (TournamentID != -1);
         //button4.Enabled = (TournamentID != -1);
         //button6.Enabled = (TournamentID != -1);
         //button2.Enabled = (TournamentID != -1);
         //buttonResults.Enabled = (TournamentID != -1);
      }

      public void ShowCourses()
      {
         listBox1.Items.Clear();
         for (int i=0; i<m_tournament.NumberOfCourses; i++) 
         {
            listBox1.Items.Add(m_tournament.Course(i,0));
         }
      }
      public void ShowFlights()
      {
         //listBox2.Items.Clear();
         //string[] flights =  m_tournament.Flights();
         //if ( flights != null )
         //{
         //   for (int i=0; i<flights.Length; i++) 
         //   {
         //      listBox2.Items.Add(flights[i]);
         //   }
         //}
      }

      private void button2_Click(object sender, System.EventArgs e)
      {
         this.Close();
      }

      private int GetNextTournamentID()
      {
         int tourid = 1;
         string sql = "select max(TournamentID)+1 as nextid from Tournament";
         DataSet ds = DB.GetDataSet(sql);
         if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["nextid"] != DBNull.Value)
            tourid = int.Parse(ds.Tables[0].Rows[0]["nextid"].ToString());
         return tourid;
      }
      private void button1_Click(object sender, System.EventArgs e)
      {
         bool badd = false;
         try
         {
            if (TournamentID == -1)
            {
               if ( !loaded )
               {
                  m_tournament = new Tournament();
                  loaded = true;
               }
               
               badd = true;
               TournamentID = GetNextTournamentID();

               m_tournament.TournamentID = TournamentID;
               m_tournament.Name = textBox1.Text;
            }
            else
            {  
               m_tournament.Name = textBox1.Text;
            }
            m_tournament.Save();
            if ( ParentTournaments != null )
               ParentTournaments.LoadTournaments();
            ((TournamentMDI)this.MdiParent).LoadTournaments();
         }
         catch  
         {
            if ( badd )
               TournamentID = -1;
         }
         EnableItems();
      }

      private void button4_Click(object sender, System.EventArgs e)
      {
         if ( golfers == null || golfers.IsDisposed )
            golfers = new Field(TournamentID);
         golfers.MdiParent = this.MdiParent;
         golfers.Show();
         golfers.Focus();
      }

      private void button6_Click(object sender, System.EventArgs e)
      {
         if ( TournamentID == -1 )
         {
            MessageBox.Show("Please create save the tournament first.");
         }
         else
         {
            Scoring s = new Scoring(TournamentID);
            if (!s.IsDisposed)
            {
               s.MdiParent = this.MdiParent;
               s.Show();
               s.Focus();
            }
         }
      }

      private void button3_Click(object sender, System.EventArgs e)
      {
         if ( TournamentID == -1 )
         {
            MessageBox.Show("Please save the new tournament first;");
            return;
         }

         Course c = new Course();
         c.ParentTournament = this;
         c.MdiParent = this.MdiParent;
         c.Show();
      }

      private void button8_Click(object sender, System.EventArgs e)
      {
         if ( listBox1.SelectedIndex > -1 )
         {
            GolfCourse cg = (GolfCourse)listBox1.SelectedItem;
            m_tournament.RemoveCourse(cg.ID);
            ShowCourses();
         }
      }

      private void EditCourse(object sender, System.EventArgs e)
      {
         if ( listBox1.SelectedIndex > -1 )
         {
            Course c = new Course();
            c.ParentTournament = this;
            GolfCourse cg = (GolfCourse)listBox1.SelectedItem;
            c.FillCourse(cg, false);
            c.MdiParent = this.MdiParent;
            c.Show();
            c.Focus();
         }
      }

      private void buttonResults_Click(object sender, System.EventArgs e)
      {
         this.Cursor = Cursors.WaitCursor;
         Results res = new Results(this.m_tournament.TournamentID);
         res.MdiParent = this.MdiParent;
         res.Show();
         res.Focus();
         this.Cursor = Cursors.Default;
      }

      private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
      {

      }

      private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
      {

      }
      private void AddTeeDetails(DataSet teeData, int gcID, int tee)
      {
          if (!DB.IsEmpty(teeData))
          {
              string parcols = "";
              string hcpcols = "";
              string parvals = "";
              string hcpvals = "";
              for (int h = 1; h <= 18; h++)
              {
                  parcols += ",Par" + h;
                  hcpcols += ",Handicap" + h;
                  parvals += "," + DB.Str(teeData.Tables[0].Rows[0], "Par" + h);
                  hcpvals += "," + DB.Str(teeData.Tables[0].Rows[0], "Handicap" + h);
              }
              string sql = "SET IDENTITY_INSERT mg_TourneyCourseDetails ON; INSERT INTO mg_TourneyCourseDetails (ID, CourseID, TeeNumber, Slope, Rating" + parcols + hcpcols;
              sql += ") VALUES (";
              sql += DB.Str(teeData.Tables[0].Rows[0], "ID") + "," + gcID + "," + tee + "," + DB.Str(teeData.Tables[0].Rows[0], "Slope") + "," + DB.Str(teeData.Tables[0].Rows[0], "Rating");
              sql += parvals + hcpvals + "); SET IDENTITY_INSERT mg_TourneyCourseDetails OFF";
              WEBDB.Execute(sql);
          }
      }
      private void AddTeeDetails(GolfCourse gc)
      {
          for (int tee = 0; tee < gc.NumberOfTees; tee++)
          {
              DataSet teeData = gc.TeeData(tee);
              AddTeeDetails(teeData, gc.ID, tee);
          }
      }

      private void linkUpdateWeb_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
          string name = m_tournament.Name.Replace("'", "''");
          string sql = "SELECT tournamentId from MG_Tourney where tournamentId = " + m_tournament.TournamentID;
          //DataSet ds = WEBDB.GetDataSet(sql);
          //if (DB.IsEmpty(ds))
          //{
          //    sql = "INSERT INTO MG_Tourney (TournamentID, Slogan, NumRounds) VALUES (";
          //    sql += m_tournament.TournamentID + ",'" + name + "', " + m_tournament.NumberOfRounds + ");";
          //}
          //else
          //{
          //    sql = "update mg_Tourney set Slogan = '" + name + "', NumRounds = " + m_tournament.NumberOfRounds + " where TournamentID = " + m_tournament.TournamentID;
          //}
          //WEBDB.Execute(sql);

          sql = "select c.CourseID, c.[Round], c.Course, cd.ID from mg_TourneyCourses c left join mg_TourneyCourseDetails cd on cd.CourseID = c.CourseID where tournamentId = " + m_tournament.TournamentID;
          DataSet ds = WEBDB.GetDataSet(sql);
          for (int i = 0; i < m_tournament.NumberOfCourses; i++)
          {
              GolfCourse gc = m_tournament.Course(i, 0);
              name = gc.Name.Replace("'", "''");
              string rounds = "";
              foreach (int round in gc.Rounds)
              {
                  if (rounds != "") rounds += ",";
                  rounds += round;
              }
              if (DB.IsEmpty(ds) || ds.Tables[0].Select("CourseID = " + gc.ID).Length == 0)
              {
                  sql = "INSERT INTO mg_TourneyCourses (CourseID, TournamentID, [Round], Course) VALUES (";
                  sql += gc.ID + "," + m_tournament.TournamentID + ",'" + rounds + "','" + name + "')";
                  WEBDB.Execute(sql);
                  AddTeeDetails(gc);
              }
              else
              {
                  sql = "UPDATE mg_TourneyCourses set [Round] = '" + rounds + "', Course = '" + name + "' where CourseID = " + gc.ID;
                  WEBDB.Execute(sql);
                  if (ds.Tables[0].Select("CourseID = " + gc.ID + " AND ID IS NULL").Length > 0) AddTeeDetails(gc);
                  else
                  {
                      for (int tee = 0; tee < gc.NumberOfTees; tee++)
                      {
                          DataSet teeData = gc.TeeData(tee);
                          if (!DB.IsEmpty(teeData))
                          {
                              string teeID = DB.Str(teeData.Tables[0].Rows[0], "ID");

                              sql = "select * from mg_TourneyCourseDetails where ID = " + teeID;
                              DataSet finddetails = WEBDB.GetDataSet(sql);
                              if (DB.IsEmpty(finddetails)) AddTeeDetails(teeData, gc.ID, tee);
                              else
                              {
                                  string parcols = "";
                                  string hcpcols = "";
                                  for (int h = 1; h <= 18; h++)
                                  {
                                      parcols += ",Par" + h + " = " + DB.Str(teeData.Tables[0].Rows[0], "Par" + h);
                                      hcpcols += ",Handicap" + h + " = " + DB.Str(teeData.Tables[0].Rows[0], "Handicap" + h);
                                  }
                                  sql = "UPDATE mg_TourneyCourseDetails set CourseID = " + gc.ID + ", TeeNumber = " + tee + ", Slope = " + DB.Str(teeData.Tables[0].Rows[0], "Slope") + ", Rating = " + DB.Str(teeData.Tables[0].Rows[0], "Rating");
                                  sql += parcols + hcpcols;
                                  sql += " where ID = " + DB.Str(teeData.Tables[0].Rows[0], "ID");
                                  WEBDB.Execute(sql);
                              }
                          }
                      }
                  }
              }
          }
      }
	}
}
