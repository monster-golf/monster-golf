using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace MonsterGolf
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Tournaments : System.Windows.Forms.Form
	{
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.ListBox listBox1;
      private System.Windows.Forms.Button button1;
      private System.Windows.Forms.Button button4;
      private System.Windows.Forms.Button AddScores;
      private System.Windows.Forms.Button button3;
      private System.Windows.Forms.Button button2;
      private System.Windows.Forms.GroupBox groupBox1;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Tournaments()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
         LoadTournaments();
		}

      public void LoadTournaments()
      {
         DataSet dst = DB.GetDataSet("SELECT Tournament.* FROM Tournament;");
         listBox1.Items.Clear();
         ListItem tourney = null;
         if (dst != null)
         {
            for (int i = 0; i < dst.Tables[0].Rows.Count; i++)
            {
               tourney = new ListItem(dst.Tables[0].Rows[i]["TournamentID"].ToString(),
                                      dst.Tables[0].Rows[i]["Slogan"].ToString());
               listBox1.Items.Add(tourney);
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
				if (components != null) 
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Tournaments));
         this.label1 = new System.Windows.Forms.Label();
         this.listBox1 = new System.Windows.Forms.ListBox();
         this.button1 = new System.Windows.Forms.Button();
         this.button4 = new System.Windows.Forms.Button();
         this.AddScores = new System.Windows.Forms.Button();
         this.button3 = new System.Windows.Forms.Button();
         this.button2 = new System.Windows.Forms.Button();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.groupBox1.SuspendLayout();
         this.SuspendLayout();
         // 
         // label1
         // 
         this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label1.Location = new System.Drawing.Point(8, 0);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(272, 16);
         this.label1.TabIndex = 2;
         this.label1.Text = "Available Tournaments";
         // 
         // listBox1
         // 
         this.listBox1.Location = new System.Drawing.Point(16, 48);
         this.listBox1.Name = "listBox1";
         this.listBox1.Size = new System.Drawing.Size(544, 329);
         this.listBox1.Sorted = true;
         this.listBox1.TabIndex = 3;
         this.listBox1.DoubleClick += new System.EventHandler(this.EditTournament_Click);
         // 
         // button1
         // 
         this.button1.Location = new System.Drawing.Point(16, 384);
         this.button1.Name = "button1";
         this.button1.Size = new System.Drawing.Size(90, 24);
         this.button1.TabIndex = 4;
         this.button1.Text = "Edit";
         this.button1.Click += new System.EventHandler(this.button1_Click);
         // 
         // button4
         // 
         this.button4.Location = new System.Drawing.Point(296, 360);
         this.button4.Name = "button4";
         this.button4.Size = new System.Drawing.Size(90, 24);
         this.button4.TabIndex = 8;
         this.button4.Text = "View Results";
         this.button4.Click += new System.EventHandler(this.button4_Click);
         // 
         // AddScores
         // 
         this.AddScores.Location = new System.Drawing.Point(200, 360);
         this.AddScores.Name = "AddScores";
         this.AddScores.Size = new System.Drawing.Size(90, 24);
         this.AddScores.TabIndex = 7;
         this.AddScores.Text = "Add Scores";
         this.AddScores.Click += new System.EventHandler(this.AddScores_Click);
         // 
         // button3
         // 
         this.button3.Location = new System.Drawing.Point(448, 360);
         this.button3.Name = "button3";
         this.button3.Size = new System.Drawing.Size(80, 24);
         this.button3.TabIndex = 6;
         this.button3.Text = "Close";
         this.button3.Click += new System.EventHandler(this.button3_Click);
         // 
         // button2
         // 
         this.button2.Location = new System.Drawing.Point(104, 360);
         this.button2.Name = "button2";
         this.button2.Size = new System.Drawing.Size(90, 24);
         this.button2.TabIndex = 5;
         this.button2.Text = "Add New";
         this.button2.Click += new System.EventHandler(this.button2_Click);
         // 
         // groupBox1
         // 
         this.groupBox1.Controls.Add(this.button4);
         this.groupBox1.Controls.Add(this.AddScores);
         this.groupBox1.Controls.Add(this.button3);
         this.groupBox1.Controls.Add(this.button2);
         this.groupBox1.Location = new System.Drawing.Point(8, 24);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(560, 392);
         this.groupBox1.TabIndex = 7;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "Select Tournament";
         // 
         // Tournaments
         // 
         this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
         this.ClientSize = new System.Drawing.Size(576, 422);
         this.Controls.Add(this.button1);
         this.Controls.Add(this.listBox1);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.groupBox1);
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.Name = "Tournaments";
         this.Text = "Monster Golf Tournament Scoring";
         this.Load += new System.EventHandler(this.Tournaments_Load);
         this.groupBox1.ResumeLayout(false);
         this.ResumeLayout(false);

      }
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
      //[STAThread]
      //static void Main() 
      //{
      //   Application.Run(new TournamentMDI());
      //}

      private void button3_Click(object sender, System.EventArgs e)
      {
         this.Close();
      }

      private void AddScores_Click(object sender, System.EventArgs e)
      {
         if (listBox1.SelectedIndex < 0) 
         {
            MessageBox.Show("Please select a tournament");
         }
         else 
         {
            enterScores();
         }
      }

      private void CreateEditTourney(EditTournament editTourney)
      {
         editTourney.ParentTournaments = this;
         editTourney.MdiParent = this.MdiParent;
         editTourney.Show();
         editTourney.Focus();
      }

      public void NewTourney()
      {
         EditTournament editTourney = new EditTournament();
         CreateEditTourney(editTourney);
      }
      private void button2_Click(object sender, System.EventArgs e)
      {
         NewTourney();
      }

      private void EditTournament_Click(object sender, System.EventArgs e)
      {
         if (listBox1.SelectedIndex < 0) 
         {
            MessageBox.Show("Please select a tournament");
         }
         else 
         {
            editTourney();
         }
      }

      private void button1_Click(object sender, System.EventArgs e)
      {
         EditTournament_Click(null,null);
      }

      private void button4_Click(object sender, System.EventArgs e)
      {
         if (listBox1.SelectedIndex < 0) 
         {
            MessageBox.Show("Please select a tournament");
         }
         else 
         {
            viewResults();
         }
      }

      private void editTourney()
      {
         ListItem selected = (ListItem)listBox1.SelectedItem;
         EditTourney(selected.ID);
      }
      public void EditTourney(string tourneyID)
      {
         EditTournament et = new EditTournament();
         et.TournamentID = int.Parse(tourneyID);
         CreateEditTourney(et);
      }

      private void viewResults()
      {
         ListItem selected = (ListItem)listBox1.SelectedItem;
         ViewResults(selected.ID);
      }
      public void ViewResults(string tourneyID)
      {
         this.Cursor = Cursors.WaitCursor;
         Results res = new Results(int.Parse(tourneyID));
         res.MdiParent = this.MdiParent;
         res.Show();
         res.Focus();
         this.Cursor = Cursors.Default;
      }

      private void enterScores()
      {
         ListItem selected = (ListItem)listBox1.SelectedItem;
         EnterScores(selected.ID);
      }
      public void EnterScores(string tourneyID)
      {
         Scoring s = new Scoring(int.Parse(tourneyID));
         s.MdiParent = this.MdiParent;
         s.Show();
         s.Focus();
      }

      private void Tournaments_Load(object sender, EventArgs e)
      {

      }
	}
}
