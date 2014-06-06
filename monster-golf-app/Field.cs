using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace MonsterGolf
{
	/// <summary>
	/// Summary description for Field.
	/// </summary>
	public class Field : System.Windows.Forms.Form
   {
      private IContainer components;
      private Tournament m_tournament = null;
      private System.Windows.Forms.CheckedListBox listGolfers;
      private System.Windows.Forms.Button btnAdd;
      private System.Windows.Forms.Label lblNew;
      private System.Windows.Forms.TextBox tbNewNameFirst;
      private Label labelLast;
      private TextBox tbNewNameLast;
      private bool m_init = false;
      private Label labelInTournament;
      Color m_nameC = System.Drawing.Color.LightBlue;
      private DataGridView dataGridTournament;
      private ContextMenuStrip contextMenuStripDGTeams;
      private ToolStripMenuItem createTeamToolStripMenuItem;
      private ToolStripMenuItem removeFromTeamToolStripMenuItem;
      private ToolTip toolTipTeamMembers;
      private LinkLabel linkUpdateUsrs;
      private LinkLabel linkDownload;
      private LinkLabel linkUpdateWebTourneyUsers;
      private LinkLabel linkUpdateWebTourney;
      private DataGridViewTextBoxColumn GolferLast;
      private DataGridViewTextBoxColumn GolferFirst;
      private DataGridViewTextBoxColumn Handicap;
      private DataGridViewTextBoxColumn GolferID;
      private DataGridViewTextBoxColumn TeamID;
      private DataGridViewTextBoxColumn Flight;
      private DataGridViewTextBoxColumn TeamHcp;
      private DataGridViewTextBoxColumn WebId;
      Color m_dataC = System.Drawing.Color.LightBlue;

      public Field(int TournamentID)
      {
          //
          // Required for Windows Form Designer support
          //
          m_init = true;
          m_tournament = new Tournament(TournamentID);
          InitializeComponent();
          this.Text = m_tournament.Name + " - Field";
          bool golfersIn = AddGolfersToForm();
          m_init = false;
          inTournament();
      }

      public Tournament Tournament
      {
         get { return m_tournament; }
      }

      private bool AddGolfersToForm() 
      {
         DataSet golfers = GolferList.GetAvailableGolfers();
         DataSet golfersin = m_tournament.GolfersDataSet();
         bool intourn = false;

         for ( int i=0; i<golfers.Tables[0].Rows.Count; i++ )
         {
            intourn = false;
            if (golfersin != null) 
            {
               for (int x = 0; x < golfersin.Tables[0].Rows.Count; x++)
               {
                  if (int.Parse(golfersin.Tables[0].Rows[x]["userid"].ToString()) == int.Parse(golfers.Tables[0].Rows[i]["userid"].ToString())) 
                  {
                     intourn = true;
                     break;
                  }
               }
            }
            Golfer golfer = new Golfer(golfers.Tables[0].Rows[i]);
            listGolfers.Items.Add(golfer);
            listGolfers.SetItemChecked(listGolfers.Items.Count-1, intourn);
         }

         return (golfersin != null && golfersin.Tables[0].Rows.Count > 0);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Field));
            this.listGolfers = new System.Windows.Forms.CheckedListBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.tbNewNameFirst = new System.Windows.Forms.TextBox();
            this.lblNew = new System.Windows.Forms.Label();
            this.labelLast = new System.Windows.Forms.Label();
            this.tbNewNameLast = new System.Windows.Forms.TextBox();
            this.labelInTournament = new System.Windows.Forms.Label();
            this.dataGridTournament = new System.Windows.Forms.DataGridView();
            this.GolferLast = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GolferFirst = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Handicap = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GolferID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TeamID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Flight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TeamHcp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WebId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStripDGTeams = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.createTeamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeFromTeamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTipTeamMembers = new System.Windows.Forms.ToolTip(this.components);
            this.linkUpdateUsrs = new System.Windows.Forms.LinkLabel();
            this.linkDownload = new System.Windows.Forms.LinkLabel();
            this.linkUpdateWebTourneyUsers = new System.Windows.Forms.LinkLabel();
            this.linkUpdateWebTourney = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridTournament)).BeginInit();
            this.contextMenuStripDGTeams.SuspendLayout();
            this.SuspendLayout();
            // 
            // listGolfers
            // 
            this.listGolfers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listGolfers.BackColor = System.Drawing.SystemColors.Control;
            this.listGolfers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listGolfers.HorizontalScrollbar = true;
            this.listGolfers.Location = new System.Drawing.Point(3, 47);
            this.listGolfers.Margin = new System.Windows.Forms.Padding(6, 3, 3, 6);
            this.listGolfers.MultiColumn = true;
            this.listGolfers.Name = "listGolfers";
            this.listGolfers.Size = new System.Drawing.Size(310, 330);
            this.listGolfers.TabIndex = 5;
            this.listGolfers.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listGolfers_ItemCheck);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(248, 2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(53, 21);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "Create";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // tbNewNameFirst
            // 
            this.tbNewNameFirst.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbNewNameFirst.Location = new System.Drawing.Point(66, 2);
            this.tbNewNameFirst.Name = "tbNewNameFirst";
            this.tbNewNameFirst.Size = new System.Drawing.Size(70, 20);
            this.tbNewNameFirst.TabIndex = 1;
            // 
            // lblNew
            // 
            this.lblNew.AutoSize = true;
            this.lblNew.Location = new System.Drawing.Point(3, 4);
            this.lblNew.Name = "lblNew";
            this.lblNew.Size = new System.Drawing.Size(60, 13);
            this.lblNew.TabIndex = 4;
            this.lblNew.Text = "Golfer First:";
            // 
            // labelLast
            // 
            this.labelLast.AutoSize = true;
            this.labelLast.Location = new System.Drawing.Point(138, 4);
            this.labelLast.Name = "labelLast";
            this.labelLast.Size = new System.Drawing.Size(30, 13);
            this.labelLast.TabIndex = 5;
            this.labelLast.Text = "Last:";
            // 
            // tbNewNameLast
            // 
            this.tbNewNameLast.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbNewNameLast.Location = new System.Drawing.Point(170, 2);
            this.tbNewNameLast.Name = "tbNewNameLast";
            this.tbNewNameLast.Size = new System.Drawing.Size(70, 20);
            this.tbNewNameLast.TabIndex = 2;
            // 
            // labelInTournament
            // 
            this.labelInTournament.Location = new System.Drawing.Point(329, 3);
            this.labelInTournament.Name = "labelInTournament";
            this.labelInTournament.Size = new System.Drawing.Size(291, 14);
            this.labelInTournament.TabIndex = 7;
            this.labelInTournament.Text = "In Tournament";
            // 
            // dataGridTournament
            // 
            this.dataGridTournament.AllowUserToAddRows = false;
            this.dataGridTournament.AllowUserToDeleteRows = false;
            this.dataGridTournament.AllowUserToOrderColumns = true;
            this.dataGridTournament.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridTournament.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridTournament.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridTournament.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridTournament.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.GolferLast,
            this.GolferFirst,
            this.Handicap,
            this.GolferID,
            this.TeamID,
            this.Flight,
            this.TeamHcp,
            this.WebId});
            this.dataGridTournament.ContextMenuStrip = this.contextMenuStripDGTeams;
            this.dataGridTournament.Location = new System.Drawing.Point(325, 47);
            this.dataGridTournament.Margin = new System.Windows.Forms.Padding(3, 3, 6, 6);
            this.dataGridTournament.Name = "dataGridTournament";
            this.dataGridTournament.Size = new System.Drawing.Size(310, 330);
            this.dataGridTournament.TabIndex = 8;
            this.dataGridTournament.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.EnterInTournamentCell);
            this.dataGridTournament.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataChanged);
            this.dataGridTournament.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridTournamentDBComplete);
            // 
            // GolferLast
            // 
            this.GolferLast.DataPropertyName = "LastName";
            this.GolferLast.HeaderText = "Last Name";
            this.GolferLast.Name = "GolferLast";
            this.GolferLast.ReadOnly = true;
            // 
            // GolferFirst
            // 
            this.GolferFirst.DataPropertyName = "FirstName";
            this.GolferFirst.HeaderText = "First Name";
            this.GolferFirst.Name = "GolferFirst";
            this.GolferFirst.ReadOnly = true;
            // 
            // Handicap
            // 
            this.Handicap.DataPropertyName = "Handicap";
            this.Handicap.HeaderText = "Hcp Index";
            this.Handicap.Name = "Handicap";
            this.Handicap.Width = 80;
            // 
            // GolferID
            // 
            this.GolferID.DataPropertyName = "UserID";
            this.GolferID.HeaderText = "GolferID";
            this.GolferID.Name = "GolferID";
            this.GolferID.ReadOnly = true;
            this.GolferID.Visible = false;
            // 
            // TeamID
            // 
            this.TeamID.DataPropertyName = "TeamID";
            this.TeamID.HeaderText = "Team";
            this.TeamID.Name = "TeamID";
            this.TeamID.ReadOnly = true;
            this.TeamID.Width = 50;
            // 
            // Flight
            // 
            this.Flight.DataPropertyName = "Flight";
            this.Flight.HeaderText = "Flight";
            this.Flight.Name = "Flight";
            this.Flight.Width = 50;
            // 
            // TeamHcp
            // 
            this.TeamHcp.DataPropertyName = "TeamHcp";
            this.TeamHcp.HeaderText = "Team Hcp";
            this.TeamHcp.Name = "TeamHcp";
            this.TeamHcp.ReadOnly = true;
            this.TeamHcp.Width = 80;
            // 
            // WebId
            // 
            this.WebId.DataPropertyName = "WebId";
            this.WebId.HeaderText = "Web Id";
            this.WebId.Name = "WebId";
            // 
            // contextMenuStripDGTeams
            // 
            this.contextMenuStripDGTeams.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createTeamToolStripMenuItem,
            this.removeFromTeamToolStripMenuItem});
            this.contextMenuStripDGTeams.Name = "contextMenuStripDGTeams";
            this.contextMenuStripDGTeams.Size = new System.Drawing.Size(182, 48);
            // 
            // createTeamToolStripMenuItem
            // 
            this.createTeamToolStripMenuItem.Name = "createTeamToolStripMenuItem";
            this.createTeamToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.createTeamToolStripMenuItem.Text = "Add To Team";
            this.createTeamToolStripMenuItem.Click += new System.EventHandler(this.addToTeamClick);
            // 
            // removeFromTeamToolStripMenuItem
            // 
            this.removeFromTeamToolStripMenuItem.Name = "removeFromTeamToolStripMenuItem";
            this.removeFromTeamToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.removeFromTeamToolStripMenuItem.Text = "Remove From Team";
            this.removeFromTeamToolStripMenuItem.Click += new System.EventHandler(this.removeFromTeamClick);
            // 
            // toolTipTeamMembers
            // 
            this.toolTipTeamMembers.AutoPopDelay = 0;
            this.toolTipTeamMembers.InitialDelay = 500;
            this.toolTipTeamMembers.ReshowDelay = 100;
            // 
            // linkUpdateUsrs
            // 
            this.linkUpdateUsrs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkUpdateUsrs.AutoSize = true;
            this.linkUpdateUsrs.Location = new System.Drawing.Point(519, 6);
            this.linkUpdateUsrs.Name = "linkUpdateUsrs";
            this.linkUpdateUsrs.Size = new System.Drawing.Size(116, 13);
            this.linkUpdateUsrs.TabIndex = 9;
            this.linkUpdateUsrs.TabStop = true;
            this.linkUpdateUsrs.Text = "Update HCP from Web";
            this.linkUpdateUsrs.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkUpdateUsrs_LinkClicked);
            // 
            // linkDownload
            // 
            this.linkDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkDownload.AutoSize = true;
            this.linkDownload.Location = new System.Drawing.Point(418, 6);
            this.linkDownload.Name = "linkDownload";
            this.linkDownload.Size = new System.Drawing.Size(91, 13);
            this.linkDownload.TabIndex = 10;
            this.linkDownload.TabStop = true;
            this.linkDownload.Text = "Download Scores";
            this.linkDownload.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkDownload_LinkClicked);
            // 
            // linkUpdateWebTourneyUsers
            // 
            this.linkUpdateWebTourneyUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkUpdateWebTourneyUsers.AutoSize = true;
            this.linkUpdateWebTourneyUsers.Location = new System.Drawing.Point(418, 19);
            this.linkUpdateWebTourneyUsers.Name = "linkUpdateWebTourneyUsers";
            this.linkUpdateWebTourneyUsers.Size = new System.Drawing.Size(98, 13);
            this.linkUpdateWebTourneyUsers.TabIndex = 11;
            this.linkUpdateWebTourneyUsers.TabStop = true;
            this.linkUpdateWebTourneyUsers.Text = "Update Web Users";
            this.linkUpdateWebTourneyUsers.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkUpdateWebTourneyUsers_LinkClicked);
            // 
            // linkUpdateWebTourney
            // 
            this.linkUpdateWebTourney.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkUpdateWebTourney.AutoSize = true;
            this.linkUpdateWebTourney.Location = new System.Drawing.Point(519, 19);
            this.linkUpdateWebTourney.Name = "linkUpdateWebTourney";
            this.linkUpdateWebTourney.Size = new System.Drawing.Size(110, 13);
            this.linkUpdateWebTourney.TabIndex = 12;
            this.linkUpdateWebTourney.TabStop = true;
            this.linkUpdateWebTourney.Text = "Update Web Tourney";
            this.linkUpdateWebTourney.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkUpdateWebTourney_LinkClicked);
            // 
            // Field
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(640, 382);
            this.Controls.Add(this.linkUpdateWebTourney);
            this.Controls.Add(this.linkUpdateWebTourneyUsers);
            this.Controls.Add(this.linkDownload);
            this.Controls.Add(this.linkUpdateUsrs);
            this.Controls.Add(this.dataGridTournament);
            this.Controls.Add(this.tbNewNameLast);
            this.Controls.Add(this.tbNewNameFirst);
            this.Controls.Add(this.labelInTournament);
            this.Controls.Add(this.labelLast);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lblNew);
            this.Controls.Add(this.listGolfers);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Field";
            this.Text = "Field";
            this.Resize += new System.EventHandler(this.FieldScreenSize);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridTournament)).EndInit();
            this.contextMenuStripDGTeams.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

      }
		#endregion

      private void listGolfers_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
      {
         if (!m_init) 
         {
            this.Cursor = Cursors.WaitCursor;

            Golfer golfer = (Golfer)listGolfers.Items[e.Index];

            if (e.NewValue == System.Windows.Forms.CheckState.Checked)
            {
                Team t = new Team();
                t.AddGolfer(t.NumberOfGolfers, golfer);
                m_tournament.AddTeam(t);
            }
            else
            {
                m_tournament.RemoveGolfer(golfer.ID);
                WEBDB.Execute("delete from mg_Tourneyscores where UserID = (select WebId from mg_tourneyUsers Where UserID = " + golfer.ID + ") and TourneyID = " + m_tournament.TournamentID);
            }
            inTournament();
            this.Cursor = Cursors.Default;
         }
      }

      private void inTournament()
      {
         int golfercount = (m_tournament.GolfersDataSet() == null) ? 0 : m_tournament.GolfersDataSet().Tables[0].Rows.Count;

         labelInTournament.Text = "In Tournament (" + golfercount.ToString() + " players)";
         if (golfercount > 0)
         {
            DataGridViewColumn sortC = dataGridTournament.SortedColumn;
            SortOrder sortO = dataGridTournament.SortOrder;

            DataSet golfer = m_tournament.GolfersDataSet().Copy();
            dataGridTournament.DataSource = golfer.Tables[0];
            dataGridTournament.Show();
            if (sortO != SortOrder.None)
            {
               // get the sort direction
               ListSortDirection sortD = (sortO == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending;
               // sort the column
               dataGridTournament.Sort(dataGridTournament.Columns[sortC.Name], sortD);
            }
         }
      }

      private void ColorGridRows()
      {
         string teamid = "-1";
         Color curC = Color.Khaki;
         for (int i = 0; i < dataGridTournament.Rows.Count; i++)
         {
            if (dataGridTournament.Rows[i].Cells["TeamID"].Value.ToString() != teamid)
            {
               teamid = dataGridTournament.Rows[i].Cells["TeamID"].Value.ToString();
               curC = (curC == Color.LightBlue) ? Color.Khaki : Color.LightBlue;
            }
            for ( int x = 0; x<dataGridTournament.Rows[i].Cells.Count; x++)
               dataGridTournament.Rows[i].Cells[x].Style.BackColor = curC;
         }
      }

      private void SetTeam(int golferID, int teamID, int teamIDTo)
      {
         Golfer golfer = new Golfer(golferID);

         if (golfer != null)
         {
            golfer.TournamentID = this.m_tournament.TournamentID;
            if (teamID != teamIDTo)
            {
               if (teamIDTo == -1)
               {
                  DataSet ds = this.m_tournament.GolfersDataSet();
                  // move this golfer to a new team if not by themselves already
                  ds.Tables[0].DefaultView.RowFilter = "teamid=" + teamID;
                  int numOnTeam = ds.Tables[0].DefaultView.Count;
                  ds.Tables[0].DefaultView.RowFilter = "";
                  if (numOnTeam > 1)
                  {
                     Team team = m_tournament.GetTeam(teamID);
                     team.RemoveGolfer(golferID);
                     if (team.NumberOfGolfers > 0)
                        m_tournament.ModifyTeam(team);

                     team = new Team();
                     team.AddGolfer(team.NumberOfGolfers, golfer);
                     m_tournament.AddTeam(team);
                  }
               }
               else
               {
                  Team team = m_tournament.GetTeam(teamID);
                  team.RemoveGolfer(golferID);
                  if (team.NumberOfGolfers > 0)
                     m_tournament.ModifyTeam(team);

                  team = m_tournament.GetTeam(teamIDTo);
                  team.AddGolfer(team.NumberOfGolfers, golfer);
                  m_tournament.ModifyTeam(team);
               }
            }
         }
         
      }

      private void btnAdd_Click(object sender, System.EventArgs e)
      {
         if (tbNewNameFirst.Text.Trim() != "" && tbNewNameLast.Text.Trim() != "")
         {
            DataSet ds = DB.GetDataSet("select * from users where lastname like '" + tbNewNameLast.Text.Trim() +
               "' and firstname like '" + tbNewNameFirst.Text.Trim() + "'");
            if (ds.Tables[0].Rows.Count > 0)
            {
               MessageBox.Show("The golfer you are adding already exists in the system.");
            }
            else
            {
               Golfer golfer = new Golfer();
               golfer.FirstName = tbNewNameFirst.Text.Trim();
               golfer.LastName = tbNewNameLast.Text.Trim();
               golfer.HcpIndex = 0.0;
               golfer.Save();
               listGolfers.Items.Add(golfer);
               listGolfers.SetItemChecked(listGolfers.Items.Count - 1, true);
               tbNewNameFirst.Text = "";
               tbNewNameLast.Text = "";
            }
         }
         else
         {
            MessageBox.Show("Please enter a fullname for the golfer.");
         }
         this.tbNewNameFirst.Focus();
      }

      private void InTourneyHeaderMouseEnter(object sender, EventArgs e)
      {
         this.Cursor = Cursors.Hand;
      }

      private void InTourneyHeaderMouseLeave(object sender, EventArgs e)
      {
         this.Cursor = Cursors.Default;
      }

      private void FieldScreenSize(object sender, EventArgs e)
      {
          //int h = this.Height;
          int w = this.Width;
          //int bottom = this.Bottom - this.listGolfers.Top - 4;
          this.listGolfers.Width = (w / 2) - 10;
          //this.listGolfers.Left = 2;
          //this.listGolfers.Height = bottom;
          this.dataGridTournament.Width = (w / 2) - 20;
          this.dataGridTournament.Left = (w / 2) + 2;
          //this.dataGridTournament.Height = bottom;
          this.labelInTournament.Left = dataGridTournament.Left;
      }

      private void dataGridTournamentDBComplete(object sender, DataGridViewBindingCompleteEventArgs e)
      {
         ColorGridRows();
      }

      private void DataChanged(object sender, DataGridViewCellEventArgs e)
      {
         if ( e.RowIndex == -1 )
            return;

         this.Cursor = Cursors.WaitCursor;
         DataGridView dgv = (DataGridView)sender;
         int scrPosY = dgv.FirstDisplayedScrollingRowIndex;
         int scrPosX = dgv.FirstDisplayedScrollingColumnIndex;
         bool reset = false;

         if (dgv[e.ColumnIndex, e.RowIndex].OwningColumn.Name == "Handicap")
         {
            double hcp = 0.0;
            try
            {
               hcp = double.Parse(dgv[e.ColumnIndex, e.RowIndex].Value.ToString());
            }
            catch
            {
               MessageBox.Show("Invalid Handicap");
               this.Cursor = Cursors.Default;
               return;
            }

            int golferid = int.Parse(dgv["GolferID", e.RowIndex].Value.ToString());
            Golfer golfer = new Golfer(golferid);

            if (golfer != null)
            {
               golfer.TournamentID = m_tournament.TournamentID;
               golfer.RoundNumber = 1;
               golfer.HcpIndex = hcp;
               golfer.Save();
               reset = true;
            }
         }
         else if (dgv[e.ColumnIndex, e.RowIndex].OwningColumn.Name == "Flight")
         {
            int golferid = int.Parse(dgv["GolferID", e.RowIndex].Value.ToString());
            Golfer golfer = new Golfer(golferid);
            if (golfer != null)
            {
               int teamid = int.Parse(dgv["TeamID", e.RowIndex].Value.ToString());
               Team team = m_tournament.GetTeam(teamid);
               team.Flight = dgv[e.ColumnIndex, e.RowIndex].Value.ToString().Trim();
               reset = true;
            }
         }
         else if (dgv[e.ColumnIndex, e.RowIndex].OwningColumn.Name == "TeeNumber")
         {
             int golferid = int.Parse(dgv["GolferID", e.RowIndex].Value.ToString());
             Golfer golfer = new Golfer(golferid);
             if (golfer != null)
             {
                 golfer.TournamentID = m_tournament.TournamentID;
                 golfer.RoundNumber = 1;
                 golfer.Tee = int.Parse(dgv[e.ColumnIndex, e.RowIndex].Value.ToString().Trim());
                 golfer.Save();
                 reset = true;
             }
         }
         if (reset)
         {
             this.m_tournament.ReloadGolfers();
             inTournament();

             if (scrPosY > 0 || scrPosX > 0)
             {
                 this.dataGridTournament.FirstDisplayedScrollingColumnIndex = scrPosX;
                 this.dataGridTournament.FirstDisplayedScrollingRowIndex = scrPosY;
             }
         }
         this.Cursor = Cursors.Default;
      }

      private void removeFromTeamClick(object sender, EventArgs e)
      {
         if ( MessageBox.Show("Are you sure you want to remove the selected golfers from their teams?", "Remove From Team", MessageBoxButtons.YesNo) ==
            DialogResult.Yes)
         {
            this.Cursor = Cursors.WaitCursor;
            bool reset = false;
            
            int curRow = -1;
            int prevRow = -1;
            int teamId = -1;
            for (int i = 0; i < this.dataGridTournament.SelectedCells.Count; i++)
            {
               curRow = this.dataGridTournament.SelectedCells[i].OwningRow.Index;
               if (curRow != prevRow)
               {
                   reset = true;
                   teamId = int.Parse(this.dataGridTournament.Rows[curRow].Cells["TeamID"].Value.ToString());
                   SetTeam(int.Parse(this.dataGridTournament.Rows[curRow].Cells["GolferID"].Value.ToString()),
                      teamId,
                      -1);
               }
               prevRow = curRow;
            }
            if (reset)
            {
                WEBDB.Execute("delete from mg_TourneyTeamPlayers where TeamID = " + teamId);
                this.inTournament();
            }

            this.Cursor = Cursors.Default;
         }
      }

      private void addToTeamClick(object sender, EventArgs e)
      {
         this.Cursor = Cursors.WaitCursor;
         int teamid = -1;
         bool reset = false;
         int curRow = -1;
         int prevRow = -1;
         for (int i = 0; i < this.dataGridTournament.SelectedCells.Count; i++)
         {
            curRow = this.dataGridTournament.SelectedCells[i].OwningRow.Index;

            if (prevRow == -1)
            {
               teamid = int.Parse(this.dataGridTournament.Rows[curRow].Cells["TeamID"].Value.ToString());
            }
            else if ( curRow != prevRow )
            {
               reset = true;
               this.SetTeam(
                  int.Parse(this.dataGridTournament.Rows[curRow].Cells["GolferID"].Value.ToString()),
                  int.Parse(this.dataGridTournament.Rows[curRow].Cells["TeamID"].Value.ToString()),
                  teamid);
            }

            prevRow = curRow;
         }
         if ( reset )
            this.inTournament();

         this.Cursor = Cursors.Default;
      }

      private void EnterInTournamentCell(object sender, DataGridViewCellEventArgs e)
      {
         if (e.RowIndex > -1)
         {
            if (e.ColumnIndex == 4 && dataGridTournament.Rows[e.RowIndex].Cells["TeamID"].ToolTipText == "")
            {
               DataSet ds = m_tournament.GolfersDataSet();
               ds.Tables[0].DefaultView.RowFilter = "TeamID = " + dataGridTournament.Rows[e.RowIndex].Cells["TeamID"].Value.ToString();
               string tip = "";
               for (int i = 0; i < ds.Tables[0].DefaultView.Count; i++)
               {
                  if (tip != "") tip += " - ";
                  tip += ds.Tables[0].DefaultView[i]["LastName"] + ", " + ds.Tables[0].DefaultView[i]["FirstName"];
               }
               if ( tip != "" )
                  dataGridTournament.Rows[e.RowIndex].Cells["TeamID"].ToolTipText = tip;
            }
         }
      }

      private void linkUpdateUsrs_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
          // Used to get the handicaps from the web.
          DataSet ds = WEBDB.GetDataSet("select ttp.ID, ttu.UserId, mu.Handicap, mu.UserId as WebId from mg_TourneyTeamPlayers ttp join mg_TourneyUsers ttu on ttu.UserId = ttp.UserId join mg_users mu on mu.UserId = ttu.WebId where ttp.TournamentId = " + Tournament.TournamentID);
          // used to get the handicap's stored at the TeamPlayer level
          //DataSet ds = DB.GetDataSet("Select Handicap,UserID from TeamPlayers where TournamentID = " + Tournament.TournamentID);
          foreach (DataRow dr in ds.Tables[0].Rows)
          {
              // used to update the Teamplayers handicap pre-tournament.
              DB.Execute(string.Format("update TeamPlayers set Handicap = {0} where TournamentID = {1} AND UserID = {2};\n", dr["Handicap"], Tournament.TournamentID, dr["UserId"]));
              DB.Execute(string.Format("update Users set HcpIndex = {0} where UserID = {1};\n", dr["Handicap"], dr["UserId"]));
              WEBDB.Execute(string.Format("update mg_TourneyUsers set HcpIndex = {0} where UserID = {1};\n", dr["Handicap"], dr["UserId"]));

              //-- Used to update the in flight Tournament hcp index
              //DB.Execute(string.Format("update TourneyScores set Handicap = {0} where UserID = {1} and TournamentId = {2};\n", dr["Handicap"], dr["UserId"], Tournament.TournamentID));
          }
          inTournament();
      }

      private void linkDownload_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
          DataSet ds = WEBDB.GetDataSet("select tu.UserId, RoundNum, Hole1, Hole2, Hole3, Hole4, Hole5, Hole6, Hole7, Hole8, Hole9, Hole10, Hole11, Hole12, Hole13, Hole14, Hole15, Hole16, Hole17, Hole18 from mg_TourneyScores ts join mg_tourneyusers tu on tu.WebId = ts.UserID WHERE ts.TourneyId = " + Tournament.TournamentID);
          foreach (DataRow dr in ds.Tables[0].Rows)
          {
              int golferid, round;
              if (int.TryParse(dr["UserId"].ToString(), out golferid) &&
                  int.TryParse(dr["RoundNum"].ToString(), out round))
              {
                  Golfer g = new Golfer(golferid);
                  g.TournamentID = Tournament.TournamentID;
                  g.RoundNumber = round;
                  g.LoadGolfersScore();

                  int score;
                  for (int x = 1; x <= 18; x++)
                  {
                      if (dr["Hole" + x.ToString()] != DBNull.Value)
                      {
                          if (int.TryParse(dr["Hole" + x.ToString()].ToString(), out score))
                          {
                              g.Scores[x - 1] = score;
                          }
                      }
                  }
                  g.SaveScores();
              }
          }
      }

      private string GetWebIdFromGolfer(string fname, string lname, bool insertifmissing)
      {
          DataSet ds = WEBDB.GetDataSet("SELECT UserId as WebId FROM mg_Users WHERE FirstName = '" + fname + "' and LastName = '" + lname + "'");
          string webId = "0";
          if (!DB.IsEmpty(ds)) webId = ds.Tables[0].Rows[0]["WebId"].ToString();
          else if (insertifmissing)
          {
              WEBDB.Execute("INSERT INTO MG_Users (UserName, Email, FirstName, LastName, UserTypeID, Handicap) VALUES ('" + fname + lname + "', 'monster@monstergolf.org', '" + fname + "', '" + lname + "',1,0)");
              return GetWebIdFromGolfer(fname, lname, false);
          }
          return webId;
      }
      private void linkUpdateWebTourneyUsers_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
          DataSet golfers = GolferList.GetAvailableGolfers();
          if (!DB.IsEmpty(golfers))
          {
              foreach (DataRow dr in golfers.Tables[0].Rows)
              {
                  string fname = DB.ExecStr(DB.Str(dr, "FirstName"));
                  string lname = DB.ExecStr(DB.Str(dr, "LastName"));
                  string handicap = DB.Str(dr, "HcpIndex"); 
                  DataSet ds = WEBDB.GetDataSet("select WebId from mg_TourneyUsers where UserID = " + dr["userId"]);
                  bool empty = DB.IsEmpty(ds);
                  string webId = "";
                  if (!empty) webId = DB.Str(ds.Tables[0].Rows[0], "WebId");
                  if (empty)
                  {
                      webId = GetWebIdFromGolfer(fname, lname, true);
                      string sql = "SET IDENTITY_INSERT mg_TourneyUsers ON INSERT INTO mg_TourneyUsers (UserID, FirstName, LastName, NickName, Image, Description, HCPIndex, WebID) VALUES (";
                      sql += dr["userId"] + ",'" + fname + "','" + lname + "',NULL,NULL,NULL," + handicap + "," + webId + ")";
                      WEBDB.Execute(sql);
                      if (webId != "0") DB.Execute("Update users Set WebId = " + webId + " WHERE UserId = " + DB.Str(dr, "UserId"));
                  }
                  else 
                  {
                      string currwebId = GetWebIdFromGolfer(fname, lname, false);
                      if (webId != currwebId && currwebId != "0")
                      {
                          WEBDB.Execute("update mg_TourneyUsers Set WebId = " + currwebId + ", HCPIndex = " + handicap + " WHERE UserId = " + DB.Str(dr, "UserId"));
                          DB.Execute("Update users Set WebId = " + currwebId + " WHERE UserId = " + DB.Str(dr, "UserId"));
                      }
                      else
                      {
                          WEBDB.Execute("update mg_TourneyUsers Set HCPIndex = " + handicap + " WHERE UserId = " + DB.Str(dr, "UserId"));
                      }
                  }
              }
          }
          inTournament();
      }

      private void linkUpdateWebTourney_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
          DataSet golfers = m_tournament.GolfersDataSet();
          if (!DB.IsEmpty(golfers))
          {
              foreach (DataRow dr in golfers.Tables[0].Rows)
              {
                  string teamId = DB.Str(dr, "TeamID");
                  DataSet tourneyTeams = WEBDB.GetDataSet("select * from mg_TourneyTeams WHERE TeamID = " + teamId);

                  string name = DB.ExecStr(DB.Str(dr, "TeamName"));
                  string flight = DB.ExecStr(DB.Str(dr, "Flight"));
                  string sql = "";
                  if (DB.IsEmpty(tourneyTeams))
                  {
                      sql = "SET IDENTITY_INSERT MG_TourneyTeams ON; INSERT INTO MG_TourneyTeams (TeamID, TeamName, RoundNumber, TournamentID, Flight) VALUES (";
                      sql += teamId + ",'" + name + "',0," + m_tournament.TournamentID + ",'" + flight + "'); SET IDENTITY_INSERT MG_TourneyTeams OFF";
                      WEBDB.Execute(sql);
                  }
                  else
                  {
                      sql = "UPDATE mg_TourneyTeams SET TeamName = '" + name + "', Flight = '" + flight + "' WHERE TeamID = " + teamId;
                      WEBDB.Execute(sql);
                  }

                  string userId = DB.Str(dr, "userid");
                  string handicap = DB.Str(dr, "Handicap");
                  string tee = DB.Str(dr, "TeeNumber");
                  DataSet tourneyPlayers = WEBDB.GetDataSet("select * from mg_TourneyTeamPlayers WHERE TeamID = " + teamId + " AND UserID = " + userId);
                  if (DB.IsEmpty(tourneyPlayers))
                  {
                      sql = "INSERT INTO mg_TourneyTeamPlayers (TeamID, UserID, TeeNumber, TournamentID, Handicap) VALUES (";
                      sql += teamId + "," + userId + "," + tee + "," + m_tournament.TournamentID + "," + handicap + ")";
                      WEBDB.Execute(sql);
                  }
                  else
                  {
                      sql = "UPDATE mg_TourneyTeamPlayers SET TeeNumber = " + tee + ", Handicap = " + handicap;
                      sql += " WHERE [ID] = " + DB.Str(tourneyPlayers.Tables[0].Rows[0], "ID");
                      WEBDB.Execute(sql);
                  }
              }
          }
      }
   }
}
