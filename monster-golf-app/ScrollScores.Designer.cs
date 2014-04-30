namespace MonsterGolf
{
    partial class ScrollScores
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
           this.components = new System.ComponentModel.Container();
           System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
           System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
           this.labelLeader = new System.Windows.Forms.Label();
           this.labelScroll = new System.Windows.Forms.Label();
           this.timerRunningScroll = new System.Windows.Forms.Timer(this.components);
           this.timerScrollScores = new System.Windows.Forms.Timer(this.components);
           this.dataGridViewTeamScores = new System.Windows.Forms.DataGridView();
           this.buttonClosePlayers = new System.Windows.Forms.Button();
           this.linkLabelZoom = new System.Windows.Forms.LinkLabel();
           this.buttonZoomOut = new System.Windows.Forms.Button();
           this.buttonZoomIn = new System.Windows.Forms.Button();
           this.labelName = new System.Windows.Forms.Label();
           this.buttonPrev = new System.Windows.Forms.Button();
           this.buttonPause = new System.Windows.Forms.Button();
           this.buttonNext = new System.Windows.Forms.Button();
           ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTeamScores)).BeginInit();
           this.SuspendLayout();
           // 
           // labelLeader
           // 
           this.labelLeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                       | System.Windows.Forms.AnchorStyles.Right)));
           this.labelLeader.BackColor = System.Drawing.Color.Green;
           this.labelLeader.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           this.labelLeader.ForeColor = System.Drawing.Color.Yellow;
           this.labelLeader.Location = new System.Drawing.Point(0, 331);
           this.labelLeader.Name = "labelLeader";
           this.labelLeader.Size = new System.Drawing.Size(594, 14);
           this.labelLeader.TabIndex = 0;
           this.labelLeader.Text = "Leader(s):";
           // 
           // labelScroll
           // 
           this.labelScroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                       | System.Windows.Forms.AnchorStyles.Right)));
           this.labelScroll.BackColor = System.Drawing.Color.Yellow;
           this.labelScroll.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           this.labelScroll.ForeColor = System.Drawing.Color.Black;
           this.labelScroll.Location = new System.Drawing.Point(0, 346);
           this.labelScroll.Name = "labelScroll";
           this.labelScroll.Size = new System.Drawing.Size(594, 13);
           this.labelScroll.TabIndex = 1;
           this.labelScroll.Text = "Scores";
           // 
           // timerRunningScroll
           // 
           this.timerRunningScroll.Enabled = true;
           this.timerRunningScroll.Interval = 200;
           this.timerRunningScroll.Tick += new System.EventHandler(this.timerRunningScroll_Tick);
           // 
           // timerScrollScores
           // 
           this.timerScrollScores.Interval = 5000;
           this.timerScrollScores.Tick += new System.EventHandler(this.timerScrollScores_Tick);
           // 
           // dataGridViewTeamScores
           // 
           this.dataGridViewTeamScores.AllowUserToAddRows = false;
           this.dataGridViewTeamScores.AllowUserToDeleteRows = false;
           this.dataGridViewTeamScores.AllowUserToResizeColumns = false;
           this.dataGridViewTeamScores.AllowUserToResizeRows = false;
           this.dataGridViewTeamScores.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                       | System.Windows.Forms.AnchorStyles.Right)));
           this.dataGridViewTeamScores.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
           this.dataGridViewTeamScores.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
           this.dataGridViewTeamScores.BorderStyle = System.Windows.Forms.BorderStyle.None;
           this.dataGridViewTeamScores.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
           dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
           dataGridViewCellStyle1.BackColor = System.Drawing.Color.Green;
           dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Yellow;
           dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Green;
           dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Yellow;
           dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
           this.dataGridViewTeamScores.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
           this.dataGridViewTeamScores.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
           dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
           dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
           dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
           dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
           dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.WindowText;
           dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
           this.dataGridViewTeamScores.DefaultCellStyle = dataGridViewCellStyle2;
           this.dataGridViewTeamScores.Location = new System.Drawing.Point(3, 238);
           this.dataGridViewTeamScores.MultiSelect = false;
           this.dataGridViewTeamScores.Name = "dataGridViewTeamScores";
           this.dataGridViewTeamScores.ReadOnly = true;
           this.dataGridViewTeamScores.RowHeadersVisible = false;
           this.dataGridViewTeamScores.Size = new System.Drawing.Size(588, 90);
           this.dataGridViewTeamScores.TabIndex = 2;
           this.dataGridViewTeamScores.Visible = false;
           // 
           // buttonClosePlayers
           // 
           this.buttonClosePlayers.Anchor = System.Windows.Forms.AnchorStyles.Left;
           this.buttonClosePlayers.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           this.buttonClosePlayers.Location = new System.Drawing.Point(5, 216);
           this.buttonClosePlayers.Name = "buttonClosePlayers";
           this.buttonClosePlayers.Size = new System.Drawing.Size(23, 23);
           this.buttonClosePlayers.TabIndex = 3;
           this.buttonClosePlayers.Text = "X";
           this.buttonClosePlayers.UseVisualStyleBackColor = true;
           this.buttonClosePlayers.Visible = false;
           this.buttonClosePlayers.Click += new System.EventHandler(this.buttonClosePlayers_Click);
           // 
           // linkLabelZoom
           // 
           this.linkLabelZoom.AutoSize = true;
           this.linkLabelZoom.Location = new System.Drawing.Point(265, 4);
           this.linkLabelZoom.Name = "linkLabelZoom";
           this.linkLabelZoom.Size = new System.Drawing.Size(0, 13);
           this.linkLabelZoom.TabIndex = 4;
           // 
           // buttonZoomOut
           // 
           this.buttonZoomOut.Anchor = System.Windows.Forms.AnchorStyles.Left;
           this.buttonZoomOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           this.buttonZoomOut.Location = new System.Drawing.Point(27, 216);
           this.buttonZoomOut.Name = "buttonZoomOut";
           this.buttonZoomOut.Size = new System.Drawing.Size(24, 23);
           this.buttonZoomOut.TabIndex = 5;
           this.buttonZoomOut.Text = "+";
           this.buttonZoomOut.UseVisualStyleBackColor = true;
           this.buttonZoomOut.Visible = false;
           this.buttonZoomOut.Click += new System.EventHandler(this.buttonZoomOut_Click);
           // 
           // buttonZoomIn
           // 
           this.buttonZoomIn.Anchor = System.Windows.Forms.AnchorStyles.Left;
           this.buttonZoomIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           this.buttonZoomIn.Location = new System.Drawing.Point(50, 216);
           this.buttonZoomIn.Name = "buttonZoomIn";
           this.buttonZoomIn.Size = new System.Drawing.Size(23, 23);
           this.buttonZoomIn.TabIndex = 6;
           this.buttonZoomIn.Text = "-";
           this.buttonZoomIn.UseVisualStyleBackColor = true;
           this.buttonZoomIn.Visible = false;
           this.buttonZoomIn.Click += new System.EventHandler(this.buttonZoomIn_Click);
           // 
           // labelName
           // 
           this.labelName.Anchor = System.Windows.Forms.AnchorStyles.Left;
           this.labelName.AutoSize = true;
           this.labelName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           this.labelName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
           this.labelName.Location = new System.Drawing.Point(165, 220);
           this.labelName.Name = "labelName";
           this.labelName.Size = new System.Drawing.Size(49, 16);
           this.labelName.TabIndex = 7;
           this.labelName.Text = "Name";
           this.labelName.Visible = false;
           // 
           // buttonPrev
           // 
           this.buttonPrev.Anchor = System.Windows.Forms.AnchorStyles.Left;
           this.buttonPrev.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           this.buttonPrev.Location = new System.Drawing.Point(79, 216);
           this.buttonPrev.Name = "buttonPrev";
           this.buttonPrev.Size = new System.Drawing.Size(29, 23);
           this.buttonPrev.TabIndex = 8;
           this.buttonPrev.Text = "<<";
           this.buttonPrev.UseVisualStyleBackColor = true;
           this.buttonPrev.Click += new System.EventHandler(this.buttonPrev_Click);
           // 
           // buttonPause
           // 
           this.buttonPause.Anchor = System.Windows.Forms.AnchorStyles.Left;
           this.buttonPause.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           this.buttonPause.Location = new System.Drawing.Point(107, 216);
           this.buttonPause.Name = "buttonPause";
           this.buttonPause.Size = new System.Drawing.Size(23, 23);
           this.buttonPause.TabIndex = 9;
           this.buttonPause.Text = "||";
           this.buttonPause.UseVisualStyleBackColor = true;
           this.buttonPause.Click += new System.EventHandler(this.buttonPause_Click);
           // 
           // buttonNext
           // 
           this.buttonNext.Anchor = System.Windows.Forms.AnchorStyles.Left;
           this.buttonNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           this.buttonNext.Location = new System.Drawing.Point(130, 216);
           this.buttonNext.Name = "buttonNext";
           this.buttonNext.Size = new System.Drawing.Size(29, 23);
           this.buttonNext.TabIndex = 10;
           this.buttonNext.Text = ">>";
           this.buttonNext.UseVisualStyleBackColor = true;
           this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
           // 
           // ScrollScores
           // 
           this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
           this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
           this.BackColor = System.Drawing.Color.LightGray;
           this.Controls.Add(this.buttonNext);
           this.Controls.Add(this.buttonPause);
           this.Controls.Add(this.buttonPrev);
           this.Controls.Add(this.labelName);
           this.Controls.Add(this.buttonZoomIn);
           this.Controls.Add(this.buttonZoomOut);
           this.Controls.Add(this.buttonClosePlayers);
           this.Controls.Add(this.labelScroll);
           this.Controls.Add(this.labelLeader);
           this.Controls.Add(this.dataGridViewTeamScores);
           this.Controls.Add(this.linkLabelZoom);
           this.Name = "ScrollScores";
           this.Size = new System.Drawing.Size(594, 360);
           this.Resize += new System.EventHandler(this.OnResize);
           ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTeamScores)).EndInit();
           this.ResumeLayout(false);
           this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelLeader;
        private System.Windows.Forms.Label labelScroll;
        private System.Windows.Forms.Timer timerRunningScroll;
       private System.Windows.Forms.Timer timerScrollScores;
       private System.Windows.Forms.DataGridView dataGridViewTeamScores;
       private System.Windows.Forms.Button buttonClosePlayers;
       private System.Windows.Forms.LinkLabel linkLabelZoom;
       private System.Windows.Forms.Button buttonZoomOut;
       private System.Windows.Forms.Button buttonZoomIn;
       private System.Windows.Forms.Label labelName;
       private System.Windows.Forms.Button buttonPrev;
       private System.Windows.Forms.Button buttonPause;
       private System.Windows.Forms.Button buttonNext;
    }
}
