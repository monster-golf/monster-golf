using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Xml;

namespace MonsterGolf
{
	/// <summary>
	/// Summary description for CopyScores.
	/// </summary>
	public class CopyScores : System.Windows.Forms.Form
	{
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label labelTournamentName;
      private System.Windows.Forms.Button buttonCopy;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
      private System.Windows.Forms.ComboBox comboBoxTournaments;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Label labelFromRound;
      private System.Windows.Forms.TextBox textBoxToRound;
      public Scoring ParentScoreSheet;

		public CopyScores(Scoring parent)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			ParentScoreSheet = parent;
         this.labelTournamentName.Text = parent.Tournament.Name;
         this.labelFromRound.Text = parent.SelectedRound().ToString();

         System.Xml.XmlDocument tournaments = new System.Xml.XmlDocument();
         tournaments.Load("Tournaments\\TournamentList.xml");

         System.Xml.XmlNodeList tournament = tournaments.DocumentElement.GetElementsByTagName("TOURNAMENT");
         ListItem tourney = null;

         tourney = new ListItem("-1","-- Select --");
         comboBoxTournaments.Items.Add(tourney);
         for (int i=0; i<tournament.Count; i++) 
         {
            if ( tournament[i].Attributes.GetNamedItem("id").Value != ParentScoreSheet.Tournament.TournamentID.ToString() )
            {
               tourney = new ListItem(tournament[i].Attributes.GetNamedItem("id").Value,
                  tournament[i].Attributes.GetNamedItem("name").Value);
               comboBoxTournaments.Items.Add(tourney);
            }
         }
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CopyScores));
         this.label1 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.labelTournamentName = new System.Windows.Forms.Label();
         this.comboBoxTournaments = new System.Windows.Forms.ComboBox();
         this.buttonCopy = new System.Windows.Forms.Button();
         this.label3 = new System.Windows.Forms.Label();
         this.label4 = new System.Windows.Forms.Label();
         this.labelFromRound = new System.Windows.Forms.Label();
         this.textBoxToRound = new System.Windows.Forms.TextBox();
         this.SuspendLayout();
         // 
         // label1
         // 
         this.label1.Location = new System.Drawing.Point(8, 8);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(104, 16);
         this.label1.TabIndex = 0;
         this.label1.Text = "Copy scores from:";
         // 
         // label2
         // 
         this.label2.Location = new System.Drawing.Point(16, 64);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(88, 23);
         this.label2.TabIndex = 1;
         this.label2.Text = "Copy scores to:";
         // 
         // labelTournamentName
         // 
         this.labelTournamentName.Location = new System.Drawing.Point(112, 8);
         this.labelTournamentName.Name = "labelTournamentName";
         this.labelTournamentName.Size = new System.Drawing.Size(312, 23);
         this.labelTournamentName.TabIndex = 2;
         // 
         // comboBoxTournaments
         // 
         this.comboBoxTournaments.Location = new System.Drawing.Point(112, 64);
         this.comboBoxTournaments.Name = "comboBoxTournaments";
         this.comboBoxTournaments.Size = new System.Drawing.Size(312, 21);
         this.comboBoxTournaments.Sorted = true;
         this.comboBoxTournaments.TabIndex = 3;
         this.comboBoxTournaments.Text = "-- Select --";
         // 
         // buttonCopy
         // 
         this.buttonCopy.Location = new System.Drawing.Point(16, 128);
         this.buttonCopy.Name = "buttonCopy";
         this.buttonCopy.Size = new System.Drawing.Size(75, 23);
         this.buttonCopy.TabIndex = 4;
         this.buttonCopy.Text = "Copy";
         this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Click);
         // 
         // label3
         // 
         this.label3.Location = new System.Drawing.Point(40, 96);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(64, 16);
         this.label3.TabIndex = 5;
         this.label3.Text = "To Round:";
         // 
         // label4
         // 
         this.label4.Location = new System.Drawing.Point(32, 40);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(72, 16);
         this.label4.TabIndex = 6;
         this.label4.Text = "From Round:";
         // 
         // labelFromRound
         // 
         this.labelFromRound.Location = new System.Drawing.Point(112, 40);
         this.labelFromRound.Name = "labelFromRound";
         this.labelFromRound.Size = new System.Drawing.Size(304, 23);
         this.labelFromRound.TabIndex = 7;
         // 
         // textBoxToRound
         // 
         this.textBoxToRound.Location = new System.Drawing.Point(112, 96);
         this.textBoxToRound.Name = "textBoxToRound";
         this.textBoxToRound.Size = new System.Drawing.Size(100, 20);
         this.textBoxToRound.TabIndex = 8;
         // 
         // CopyScores
         // 
         this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
         this.ClientSize = new System.Drawing.Size(432, 166);
         this.Controls.Add(this.textBoxToRound);
         this.Controls.Add(this.labelFromRound);
         this.Controls.Add(this.label4);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.buttonCopy);
         this.Controls.Add(this.comboBoxTournaments);
         this.Controls.Add(this.labelTournamentName);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.label1);
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.Name = "CopyScores";
         this.Text = "Monster Golf Tournament Scoring";
         this.ResumeLayout(false);
         this.PerformLayout();

      }
		#endregion

      private void buttonCopy_Click(object sender, System.EventArgs e)
      {
         int toround = 0;
         try
         {
            toround = int.Parse(this.textBoxToRound.Text);
         }
         catch
         {
            toround = 0;
         }
         if ( toround == 0 )
         {
            MessageBox.Show("You must enter a valid To Round");
            return;
         }

         int numberofcopies = 0;
         if ( comboBoxTournaments.SelectedIndex > 0 ) 
         {
            ListItem selected = (ListItem)comboBoxTournaments.SelectedItem;
            Tournament copyTo = new Tournament(int.Parse(selected.ID));
            ListItem[] golfers = copyTo.Golfers();
            ListItem[] golfersfrom = ParentScoreSheet.Tournament.Golfers();
            for ( int i=0; i < golfers.Length; i++ )
            {
               Golfer golfer = new Golfer(int.Parse(golfers[i].ID));

               for ( int j=0; j < golfersfrom.Length; j++ )
               {
                  Golfer golferfrom = new Golfer(int.Parse(golfersfrom[j].ID));

                  if ( golferfrom.ID == golfer.ID )
                  {
                     string query = "select * from tourneyscores where tournamentid=" + ParentScoreSheet.Tournament.TournamentID.ToString() + " and roundnumber=" + this.labelFromRound.Text;
                     DataSet ds = DB.GetDataSet(query);
                     if ( ds.Tables[0].Rows.Count > 0 )
                     {
                        DataSet scores = DB.GetDataSet("select * from tourneyscores where userid = " + golfer.ID.ToString() +
                           " and tournamentid = " + copyTo.TournamentID.ToString() +
                           " and roundnumber = " + this.textBoxToRound.Text);
                        if (scores == null || scores.Tables[0].Rows.Count == 0)
                        {
                           DB.Execute("insert into tourneyscores (userid, handicap, tournamentid, roundnumber, teenumber) values (" +
                              golfer.ID.ToString() + "," + golfer.HcpIndex.ToString() + "," + copyTo.TournamentID.ToString() + "," +
                              this.textBoxToRound.Text + "," + golfer.Tee.ToString() + ")");
                        }

                        string sql = "update tourneyscores set ";
                        for (int x = 1; x < 19; x++)
                        {
                           sql += "hole" + x.ToString() + "=" + ds.Tables[0].Rows[0]["hole" + x.ToString()].ToString() + ",";
                        }
                        if (sql.EndsWith(",")) sql = sql.Substring(0, sql.Length - 1);
                        sql += " where userid = " + golfer.ID.ToString() + " and tournamentid = " + copyTo.TournamentID.ToString() +
                           " and roundnumber = " + this.textBoxToRound.Text;
                        DB.Execute(sql);
                     }

                     break;
                  }
               }
            }
            
            MessageBox.Show("Copy complete, " + numberofcopies.ToString() + " scores copied.");
         }
      }
	}
}
