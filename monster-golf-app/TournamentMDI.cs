using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace MonsterGolf
{
	/// <summary>
	/// Summary description for Main.
	/// </summary>
	public class TournamentMDI : System.Windows.Forms.Form
	{
      private System.Windows.Forms.MainMenu mainMenu1;
      private System.Windows.Forms.MenuItem menuItem1;
      private System.Windows.Forms.MenuItem menuItem2;
      private System.Windows.Forms.MenuItem menuExit;
      private MenuItem menuItem3;
      private IContainer components;
      private MenuStrip menuStrip1;
      private Tournaments _tournaments;
      private int _curTopMenuItem = 0;
      private int _maxTourneysInView = 50;
      private MenuItem menuItem4;
      private MenuItem menuItem5;
      private string _openTourneyMenu = "";

		public TournamentMDI()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
         //menuItem2_Click(null, null);

         EstablishTournaments();
         LoadTournaments();
		}

      public void LoadTournaments()
      {
         if (this.menuStrip1.Items.Count > 0)
            this.menuStrip1.Items.Clear();

         //menuStrip1.LayoutStyle = ToolStripLayoutStyle.Table;

         if (_curTopMenuItem > 0)
         {
            // allow the user to scroll back
            ToolStripMenuItem up = new ToolStripMenuItem("-- previous --");
            up.ForeColor = Color.Blue;
            up.Click += new EventHandler(NextPrev_Click);
            this.menuStrip1.Items.Add(up);
         }

         // add the tournaments
         DataSet dst = DB.GetDataSet("SELECT Tournament.* FROM Tournament Order By Slogan"); 
         if (dst != null)
         {
            int i = 0;
            int stopPos = _curTopMenuItem + _maxTourneysInView;
            if (stopPos > dst.Tables[0].Rows.Count)
               stopPos = dst.Tables[0].Rows.Count;
            for (i = _curTopMenuItem; i < stopPos; i++)
            {
               ToolStripMenuItem tsmi = new ToolStripMenuItem(dst.Tables[0].Rows[i]["Slogan"].ToString());
               tsmi.Name = "TID" + dst.Tables[0].Rows[i]["TournamentID"].ToString();
               tsmi.Click += new EventHandler(TourneyMenu_Click);
               //tsmi.Alignment = ToolStripItemAlignment.Left;
               tsmi.TextAlign = ContentAlignment.MiddleLeft;
               this.menuStrip1.Items.Add(tsmi);
            }
         
            // allow the user to scroll down
            if (i < dst.Tables[0].Rows.Count)
            {
               ToolStripMenuItem down = new ToolStripMenuItem("-- next --");
               down.ForeColor = Color.Blue;
               down.Click += new EventHandler(NextPrev_Click);
               this.menuStrip1.Items.Add(down);
            }
         }
         // add the add new menu
         ToolStripSeparator tss = new ToolStripSeparator();
         tss.ForeColor = Color.Gray;
         this.menuStrip1.Items.Add(tss);

         ToolStripMenuItem newtsmi = new ToolStripMenuItem("-- add tournament --");
         newtsmi.ForeColor = Color.Blue;
         newtsmi.Click += new EventHandler(NewTourney_Click);
         this.menuStrip1.Items.Add(newtsmi);

         // add the submenu if there was one open
         if (_openTourneyMenu.Length > 0)
         {
            int opentsmi = menuStrip1.Items.IndexOfKey(_openTourneyMenu);
            _openTourneyMenu = "";
            if (opentsmi > -1)
               TourneyMenu_Click(menuStrip1.Items[opentsmi], null);
         }
      }

      void TourneyMenu_Click(object sender, EventArgs e)
      {
         ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
         if (tsmi.Name == _openTourneyMenu)
            return;

         string[] submenus = { "Modify", "Modify Field", "Enter Scores", "View Results" };

         // close the previous
         if (_openTourneyMenu.Length > 0)
         {
            int remAt = menuStrip1.Items.IndexOfKey(_openTourneyMenu);
            if (remAt > -1)
            {
               // set font and color back to default based on incoming control
               menuStrip1.Items[remAt].BackColor = tsmi.BackColor;
               menuStrip1.Items[remAt].Font = tsmi.Font;

               // remove the submenus
               remAt++;
               for (int j = 0; j < submenus.Length; j++)
               {
                  menuStrip1.Items.RemoveAt(remAt);
               }
            }
         }

         // open the new
         _openTourneyMenu = tsmi.Name;
         int addAt = menuStrip1.Items.IndexOfKey(_openTourneyMenu);
         // highlight the selected menu
         menuStrip1.Items[addAt].BackColor = Color.LightSteelBlue;
         Font f = new Font(tsmi.Font, FontStyle.Bold);
         menuStrip1.Items[addAt].Font = f;
         for (int j = 0; j < submenus.Length; j++)
         {
            addAt++;
            ToolStripMenuItem childtsmi = new ToolStripMenuItem("  " + submenus[j]);
            childtsmi.Name = _openTourneyMenu + "_" + submenus[j];
            childtsmi.Click += new EventHandler(TourneyAction_Click);
            childtsmi.BackColor = Color.LightYellow;
            //childtsmi.Alignment = ToolStripItemAlignment.Left;
            childtsmi.TextAlign = ContentAlignment.MiddleLeft;
            menuStrip1.Items.Insert(addAt, childtsmi);
         }
      }

      void NextPrev_Click(object sender, EventArgs e)
      {
         string action = ((ToolStripMenuItem)sender).Text;
         switch (action)
         {
            case "-- next --":
               _curTopMenuItem++;
               this.LoadTournaments();
               break;
            case "-- previous --":
               _curTopMenuItem--;
               this.LoadTournaments();
               break;
         }
      }

      private void EditTourneyWindow(int tourneyID)
      {
         EditTournament et = null;

         for (int i = 0; i < this.MdiChildren.Length; i++)
         {
            if (this.MdiChildren[i].GetType() == typeof(EditTournament))
            {
               if (((EditTournament)this.MdiChildren[i]).TournamentID == tourneyID)
               {
                  et = (EditTournament)this.MdiChildren[i];
                  break;
               }
            }
         }
         if (et == null)
         {
            et = new EditTournament();
            et.MdiParent = this;
            et.TournamentID = tourneyID;
         }
         et.Show();
         et.Focus();
      }

      void NewTourney_Click(object sender, EventArgs e)
      {
         EditTourneyWindow(-1);
      }

      void FieldWindow(int tourneyID)
      {
         Field fw = null;

         for (int i = 0; i < this.MdiChildren.Length; i++)
         {
            if (this.MdiChildren[i].GetType() == typeof(Field))
            {
               if (((Field)this.MdiChildren[i]).Tournament.TournamentID == tourneyID)
               {
                  fw = (Field)this.MdiChildren[i];
                  break;
               }
            }
         }
         if (fw == null)
         {
            fw = new Field(tourneyID);
            fw.MdiParent = this;
         }
         fw.Show();
         fw.Focus();
      }

      void ScoringWindow(int tourneyID)
      {
         Scoring s = null;
         for (int i = 0; i < this.MdiChildren.Length; i++)
         {
            if (this.MdiChildren[i].GetType() == typeof(Scoring))
            {
               if (((Scoring)this.MdiChildren[i]).Tournament.TournamentID == tourneyID)
               {
                  s = (Scoring)this.MdiChildren[i];
                  break;
               }
            }
         }
         if (s == null)
         {
            s = new Scoring(tourneyID);
            s.MdiParent = this;
         }
         if (!s.IsDisposed)
         {
            s.Show();
            s.Focus();
         }
      }

      void ResultsWindow(int tourneyID)
      {
         Results res = null;
         for (int i = 0; i < this.MdiChildren.Length; i++)
         {
            if (this.MdiChildren[i].GetType() == typeof(Results))
            {
               if (((Results)this.MdiChildren[i]).Tournament.TournamentID == tourneyID)
               {
                  res = (Results)this.MdiChildren[i];
                  break;
               }
            }
         }
         if (res == null)
         {
            res = new Results(tourneyID);
            res.MdiParent = this;
         }
         res.Show();
         res.Focus();
      }

      void TourneyAction_Click(object sender, EventArgs e)
      {
         //menuItem2_Click(null, null);
         this.Cursor = Cursors.WaitCursor;
         EstablishTournaments();
         string[] tidandaction = ((ToolStripMenuItem)sender).Name.Split('_');
         string action = tidandaction[1];
         string tourneyid = tidandaction[0];
         tourneyid = tourneyid.Replace("TID", "");
         switch (action)
         {
            case "Modify":
               EditTourneyWindow(int.Parse(tourneyid));
               break;
            case "Modify Field":
               FieldWindow(int.Parse(tourneyid));
               break;
            case "Enter Scores":
               ScoringWindow(int.Parse(tourneyid));
               break;
            case "View Results":
               ResultsWindow(int.Parse(tourneyid));
               break;
         }
         this.Cursor = Cursors.Default;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TournamentMDI));
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuExit = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem4});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem2,
            this.menuItem3,
            this.menuExit});
            this.menuItem1.Text = "File";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 0;
            this.menuItem2.MdiList = true;
            this.menuItem2.Text = "Tournaments";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            this.menuItem3.Text = "Publish HTML";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // menuExit
            // 
            this.menuExit.Index = 2;
            this.menuExit.Text = "Exit";
            this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 1;
            this.menuItem4.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem5});
            this.menuItem4.Text = "Edit";
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 0;
            this.menuItem5.Text = "Edit Golfers";
            this.menuItem5.Click += new System.EventHandler(this.menuItem5_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.Left;
            this.menuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(30, 713);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // TournamentMDI
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(1016, 713);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Menu = this.mainMenu1;
            this.Name = "TournamentMDI";
            this.Text = "Monster Golf Tournament Scoring";
            this.TransparencyKey = System.Drawing.Color.White;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);
            this.PerformLayout();

      }
		#endregion

      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      [STAThread]
      static void Main()
      {
         Application.Run(new TournamentMDI());
      }

      private void menuItem2_Click(object sender, System.EventArgs e)
      {
         EstablishTournaments();
         _tournaments.Show();
         //Tournaments t = new Tournaments();
         //t.MdiParent = this;
         //t.Show();
      }

      private void EstablishTournaments()
      {
         if (_tournaments == null)
            _tournaments = new Tournaments();
         if ( _tournaments.MdiParent == null )
            _tournaments.MdiParent = this;
      }

      private void menuExit_Click(object sender, System.EventArgs e)
      {
         this.Close();      
      }

      private void menuItem3_Click(object sender, EventArgs e)
      {
         if (this.ActiveMdiChild.GetType() == typeof(Results))
            ((Results)this.ActiveMdiChild).WriteHTML();
         else
            MessageBox.Show("You must have an results window active.");
      }

      private void menuItem5_Click(object sender, EventArgs e)
      {
         for (int i = 0; i < this.MdiChildren.Length; i++)
         {
            if (this.MdiChildren[i].GetType() == typeof(Golfers))
            {
               this.MdiChildren[i].Show();
               this.MdiChildren[i].BringToFront();
               return;
            }
         }
         Golfers g = new Golfers();
         g.MdiParent = this;
         g.Show();
      }
   }
}
