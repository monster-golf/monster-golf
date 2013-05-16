using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace MonsterGolf
{
    /// <summary>
    /// Summary description for Course.
    /// </summary>
    public class Course : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbAvailCourses;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;

        private System.Windows.Forms.Label[] Tee;
        private System.Windows.Forms.TextBox[] Slope;
        private System.Windows.Forms.TextBox[] Rating;
        private System.Windows.Forms.TextBox[,] Par;
        private System.Windows.Forms.Label[] HoleLabel;
        private System.Windows.Forms.TextBox[,] Hcp;
        private System.Windows.Forms.Label[] ParLabel;
        private System.Windows.Forms.Label[] HcpLabel;
        private System.Windows.Forms.Label ParHcpLabel;
        private System.Windows.Forms.Label TeeLabel;
        private System.Windows.Forms.Label[] SlopeLabel;
        private System.Windows.Forms.Label[] RatingLabel;
        private System.Windows.Forms.Label SlopeRatingLabel;
        private System.Windows.Forms.Label OutLabel;
        private System.Windows.Forms.Label InLabel;
        private System.Windows.Forms.Label[] Out;
        private System.Windows.Forms.Label[] In;
        private System.Windows.Forms.Label TotalLabel;
        private System.Windows.Forms.Label[] Total;
        private System.Windows.Forms.Button button2;

        public EditTournament ParentTournament;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label4;

        private int editGolfCourseID = -1;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public Course()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            DrawCard();
            AvailableCourses();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void DrawCard()
        {
            FormHelper formHelp = new FormHelper();

            this.SuspendLayout();
            this.Tee = new System.Windows.Forms.Label[6];
            this.TeeLabel = new System.Windows.Forms.Label();
            this.Slope = new System.Windows.Forms.TextBox[6];
            this.SlopeLabel = new System.Windows.Forms.Label[6];
            this.Rating = new System.Windows.Forms.TextBox[6];
            this.RatingLabel = new System.Windows.Forms.Label[6];
            this.Par = new System.Windows.Forms.TextBox[6, 18];
            this.HoleLabel = new System.Windows.Forms.Label[18];
            this.Hcp = new System.Windows.Forms.TextBox[6, 18];
            this.HcpLabel = new System.Windows.Forms.Label[6];
            this.ParLabel = new System.Windows.Forms.Label[6];
            this.ParHcpLabel = new System.Windows.Forms.Label();
            this.SlopeRatingLabel = new System.Windows.Forms.Label();
            this.Out = new System.Windows.Forms.Label[6];
            this.In = new System.Windows.Forms.Label[6];
            this.Total = new System.Windows.Forms.Label[6];
            this.OutLabel = new System.Windows.Forms.Label();
            this.InLabel = new System.Windows.Forms.Label();
            this.TotalLabel = new System.Windows.Forms.Label();

            Color color = Color.Beige;

            int toppoint = 74;

            int xpos = 0;
            int holenum;
            int txtwidth = 25;
            for (int x = 0; x < 6; x++)
            {
                if (x == 0)
                {
                    toppoint = 74;
                    xpos = 5;
                    formHelp.CreateLabel(ref this.TeeLabel, 80, 20, xpos, toppoint, ContentAlignment.MiddleLeft, color, "Tee Number");
                    this.Controls.Add(this.TeeLabel);
                    xpos += 80;
                    formHelp.CreateLabel(ref this.SlopeRatingLabel, 100, 20, xpos, toppoint, ContentAlignment.MiddleLeft, color, "");
                    this.Controls.Add(this.SlopeRatingLabel);
                    xpos += 100;
                    formHelp.CreateLabel(ref this.ParHcpLabel, 50, 20, xpos, toppoint, ContentAlignment.MiddleRight, color, "Hole");
                    this.Controls.Add(this.ParHcpLabel);
                }

                toppoint = 94 + (x * 40);

                xpos = 5;
                formHelp.CreateLabel(ref this.Tee[x], 80, 40, xpos, toppoint, ContentAlignment.MiddleLeft, color, "Tee " + (x + 1));
                this.Controls.Add(this.Tee[x]);
                xpos += 80;
                formHelp.CreateLabel(ref this.SlopeLabel[x], 50, 20, xpos, toppoint, ContentAlignment.MiddleRight, color, "Slope");
                this.Controls.Add(this.SlopeLabel[x]);
                formHelp.CreateLabel(ref this.RatingLabel[x], 50, 20, xpos, toppoint + 20, ContentAlignment.MiddleRight, color, "Rating");
                this.Controls.Add(this.RatingLabel[x]);
                xpos += 50;
                formHelp.CreateTextBox(ref this.Slope[x], "Slope[" + x + "]", x * 38 + 3, 50, 20, 3, xpos, toppoint, Color.White, HorizontalAlignment.Center, "");
                this.Controls.Add(this.Slope[x]);
                formHelp.CreateTextBox(ref this.Rating[x], "Rating[" + x + "]", x * 38 + 4, 50, 20, 4, xpos, toppoint + 20, Color.White, HorizontalAlignment.Center, "");
                this.Controls.Add(this.Rating[x]);
                xpos += 50;
                formHelp.CreateLabel(ref this.ParLabel[x], 50, 20, xpos, toppoint, ContentAlignment.MiddleRight, color, "Par");
                this.Controls.Add(this.ParLabel[x]);
                formHelp.CreateLabel(ref this.HcpLabel[x], 50, 20, xpos, toppoint + 20, ContentAlignment.MiddleRight, color, "Hcp");
                this.Controls.Add(this.HcpLabel[x]);

                for (int y = 0; y < 18; y++)
                {
                    xpos = 235 + (y * txtwidth);
                    holenum = y + 1;

                    if (y == 9)
                    {
                        if (x == 0)
                        {
                            formHelp.CreateLabel(ref this.OutLabel, 30, 20, xpos, 74, ContentAlignment.MiddleCenter, color, "Out");
                            this.Controls.Add(this.OutLabel);
                        }
                        formHelp.CreateLabel(ref this.Out[x], 30, 40, xpos, toppoint, ContentAlignment.TopCenter, color, "");
                        this.Controls.Add(this.Out[x]);
                    }
                    if (y > 8)
                    {
                        // add 30 for the out label
                        xpos += 30;
                    }

                    if (x == 0)
                    {
                        formHelp.CreateLabel(ref this.HoleLabel[y], txtwidth, 20, xpos, 74, ContentAlignment.MiddleCenter, color, holenum.ToString());
                        this.Controls.Add(this.HoleLabel[y]);
                    }

                    formHelp.CreateTextBox(ref this.Par[x, y], "Par[" + x + "]" + y, x * 38 + y + 5, txtwidth, 20, 2, xpos, toppoint, Color.White, HorizontalAlignment.Center, "");
                    this.Par[x, y].TextChanged += new EventHandler(Par_TextChanged);
                    this.Controls.Add(this.Par[x, y]);
                    formHelp.CreateTextBox(ref this.Hcp[x, y], "Hcp" + x + "[" + y + "]", x * 38 + y + 3 + 20, txtwidth, 20, 2, xpos, toppoint + 20, Color.White, HorizontalAlignment.Center, "");
                    this.Hcp[x, y].TextChanged += new EventHandler(Hcp_TextChanged);
                    this.Controls.Add(this.Hcp[x, y]);

                    if (y == 17)
                    {
                        xpos += txtwidth;
                        if (x == 0)
                        {
                            formHelp.CreateLabel(ref this.InLabel, 30, 20, xpos, 74, ContentAlignment.MiddleCenter, color, "In");
                            this.Controls.Add(this.InLabel);
                        }
                        formHelp.CreateLabel(ref this.In[x], 30, 40, xpos, toppoint, ContentAlignment.TopCenter, color, "");
                        this.Controls.Add(this.In[x]);
                        xpos += 30;
                        if (x == 0)
                        {
                            formHelp.CreateLabel(ref this.TotalLabel, 40, 20, xpos, 74, ContentAlignment.MiddleCenter, color, "Total");
                            this.Controls.Add(this.TotalLabel);
                        }
                        formHelp.CreateLabel(ref this.Total[x], 40, 40, xpos, toppoint, ContentAlignment.TopCenter, color, "");
                        this.Controls.Add(this.Total[x]);

                    }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Course));
            this.label1 = new System.Windows.Forms.Label();
            this.cbAvailCourses = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "Available Courses";
            // 
            // cbAvailCourses
            // 
            this.cbAvailCourses.Location = new System.Drawing.Point(112, 8);
            this.cbAvailCourses.Name = "cbAvailCourses";
            this.cbAvailCourses.Size = new System.Drawing.Size(232, 21);
            this.cbAvailCourses.TabIndex = 233;
            this.cbAvailCourses.TabStop = false;
            this.cbAvailCourses.SelectedIndexChanged += new System.EventHandler(this.cbAvailCourses_SelectedIndexChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(56, 40);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(296, 20);
            this.textBox1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 16);
            this.label2.TabIndex = 9;
            this.label2.Text = "Course";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(8, 344);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 24);
            this.button1.TabIndex = 231;
            this.button1.Text = "Save";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(96, 344);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 232;
            this.button2.Text = "Done";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(376, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 16);
            this.label3.TabIndex = 12;
            this.label3.Text = "Round(s)";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(432, 40);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(88, 20);
            this.textBox2.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(520, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 16);
            this.label4.TabIndex = 14;
            this.label4.Text = "(comma separated)";
            // 
            // Course
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(820, 394);
            this.Controls.Add(this.cbAvailCourses);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Course";
            this.Text = "Course";
            this.Load += new System.EventHandler(this.Course_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private void Course_Load(object sender, System.EventArgs e)
        {
        }

        private void Hcp_TextChanged(object sender, EventArgs e)
        {
            TextBox thistext = (TextBox)sender;
            try
            {
                int curhcp = int.Parse(thistext.Text);

                if (curhcp != 1)
                {
                    this.SelectNextControl(thistext, true, true, true, true);
                }
            }
            catch
            {
                // do nothing for invalid score;
            }
        }

        private void Par_TextChanged(object sender, EventArgs e)
        {
            TextBox thistext = (TextBox)sender;
            try
            {
                int currScore = int.Parse(thistext.Text);

                if (currScore != 1)
                {
                    string name = thistext.Name;
                    name = name.Replace("Par[", "");
                    int row = int.Parse(name.Substring(0, 1));
                    int total = 0;
                    int front = 0;
                    int back = 0;
                    for (int i = 0; i < 18; i++)
                    {
                        if (Par[row, i].Text != "")
                        {
                            try
                            {
                                if (i < 9)
                                    front += int.Parse(Par[row, i].Text);
                                else
                                    back += int.Parse(Par[row, i].Text);

                                total += int.Parse(Par[row, i].Text);
                            }
                            catch
                            {
                            }
                        }
                    }
                    Out[row].Text = front.ToString();
                    In[row].Text = back.ToString();
                    Total[row].Text = total.ToString();
                    this.SelectNextControl(thistext, true, true, true, true);
                }
            }
            catch
            {
                // do nothing for invalid score;
            }
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private bool CheckParHcpRowForNumber(int row, System.Windows.Forms.TextBox[,] controls)
        {
            bool error = false;
            for (int i = 0; i < 18; i++)
            {
                if (controls[row, i].Text.Trim() == "")
                    error = true;
                else
                {
                    try
                    {
                        System.Convert.ToInt32(controls[row, i].Text);
                    }
                    catch
                    {
                        error = true;
                    }
                }

                if (error)
                    break;
            }
            return error;
        }

        private string CheckPars(int row)
        {
            string error = "";
            if (CheckParHcpRowForNumber(row, Par))
                error = "Enter all pars (must be numeric) for Tee" + (row + 1) + ". ";

            return error;
        }

        private string CheckHcps(int row)
        {
            string error = "";
            if (CheckParHcpRowForNumber(row, Hcp))
                error = "Enter all handicaps (must be numeric) for Tee" + (row + 1) + ". ";

            if (error == "")
            {
                string allhcps = "";
                for (int i = 0; i < 18; i++)
                {
                    if (System.Convert.ToInt32(Hcp[row, i].Text) > 18 ||
                       System.Convert.ToInt32(Hcp[row, i].Text) < 1)
                    {
                        error = "Handicaps must be between 1 and 18 for Tee " + (row + 1) + ". ";
                    }
                    else
                    {
                        if (allhcps.IndexOf("," + Hcp[row, i].Text + ",") > -1)
                        {
                            error = "Handicaps must unique per hole for Tee " + (row + 1) + ". ";
                        }
                        else
                        {
                            allhcps += "," + Hcp[row, i].Text + ",";
                        }
                    }
                    if (error != "")
                        break;
                }
            }

            return error;
        }

        private bool UseRow(int row)
        {
            if (Slope[row].Text.Trim() != "")
                return true;
            if (Rating[row].Text.Trim() != "")
                return true;

            bool use = false;
            for (int i = 0; i < 18; i++)
            {
                if (Par[row, i].Text.Trim() != "")
                    use = true;
                if (Hcp[row, i].Text.Trim() != "")
                    use = true;

                if (use)
                    break;
            }

            return use;
        }

        private string CheckSlopeRating(int row)
        {
            string error = "";
            if (Slope[row].Text.Trim() == "" || Rating[row].Text.Trim() == "")
                error = "Must enter slope and rating for Tee " + (row + 1) + ". ";
            else
            {
                try
                {
                    System.Convert.ToInt32(Slope[row].Text);
                    System.Convert.ToDouble(Rating[row].Text);
                }
                catch
                {
                    error = "Slope and rating must be numeric for Tee " + (row + 1) + ". ";
                }
            }
            return error;
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            string error = "";

            if (textBox1.Text.Trim() == "")
                error = "You must enter a course name. ";

            if (textBox2.Text.Trim() == "")
                error += "You must enter the rounds. ";
            else
            {
                try
                {
                    string testrounds = textBox2.Text.Replace(",", "");
                    System.Convert.ToInt32(testrounds);
                }
                catch
                {
                    error += "All rounds must be numeric. ";
                }
            }

            int numrows = 0;
            for (int i = 0; i < 6; i++)
            {
                if (UseRow(i))
                {
                    error += CheckPars(i);
                    error += CheckHcps(i);
                    error += CheckSlopeRating(i);
                    numrows++;
                }
            }
            if (numrows < 1)
            {
                error += "You must use one tee. ";
            }

            if (error != "")
            {
                MessageBox.Show(error);
            }
            else
            {
                GolfCourse gc = new GolfCourse();
                gc.Name = textBox1.Text;
                gc.ID = this.editGolfCourseID;
                double[] teerating = new double[numrows];
                int[] teeslope = new int[numrows];
                GolfHole[] gh = new GolfHole[numrows * 18];
                string[] arounds = textBox2.Text.Split(',');

                int[] rounds = new int[arounds.Length];
                for (int i = 0; i < arounds.Length; i++)
                {
                    rounds[i] = int.Parse(arounds[i]);
                }
                gc.Rounds = rounds;

                int x = 0;
                int holecount = 0;
                for (int i = 0; i < 6; i++)
                {
                    if (UseRow(i))
                    {
                        teerating[x] = double.Parse(Rating[i].Text);
                        teeslope[x] = int.Parse(Slope[i].Text);
                        for (int j = 0; j < 18; j++)
                        {
                            gh[holecount] = new GolfHole(j + 1, x, int.Parse(Par[i, j].Text), int.Parse(Hcp[i, j].Text));
                            holecount++;
                        }
                        x++;
                    }
                }
                gc.NumberOfTees = x;
                gc.TeeRating = teerating;
                gc.TeeSlope = teeslope;
                gc.Holes = gh;
                ParentTournament.m_tournament.AddCourse(gc);
                ParentTournament.ShowCourses();
            }
        }

        public void FillCourse(GolfCourse thecg, bool copy)
        {
            if (ParentTournament != null && thecg != null)
            {
                if (copy)
                {
                    // clear all first
                    this.textBox1.Text = "";
                    this.textBox2.Text = "";
                    for (int j = 0; j < 6; j++)
                    {
                       try
                        {
                            this.Slope[j].Text = "";
                            this.Rating[j].Text = "";
                            this.Total[j].Text = "";
                            for (int k = 0; k < 18; k++)
                            {
                                this.Par[j, k].Text = "";
                                this.Hcp[j, k].Text = "";
                            }
                        }
                        catch { }
                    }
                }
                int current = 0;
                if (!copy) 
                {
                    this.cbAvailCourses.Enabled = false;
                    this.editGolfCourseID = thecg.ID;
                }
                this.textBox1.Text = thecg.Name;
                this.textBox2.Text = "";
                int[] rnd = thecg.Rounds;
                for (int i = 0; i < rnd.Length; i++)
                {
                    if (this.textBox2.Text != "")
                        this.textBox2.Text += ",";
                    this.textBox2.Text += rnd[i].ToString();
                }

                int findNum = (copy) ? 1 : this.ParentTournament.m_tournament.NumberOfCourses;
                for (int i = 0; i < findNum; i++)
                {
                    int teeNum = (copy) ? thecg.NumberOfTees : this.ParentTournament.m_tournament.NumberOfTees(i);
                    for (int j = 0; j < teeNum; j++)
                    {
                        GolfCourse cg = null;
                        try
                        {
                            cg = (copy) ? thecg : this.ParentTournament.m_tournament.Course(i, j);
                        }
                        catch { }

                        if (cg != null)
                        {
                            if (cg.ID == thecg.ID)
                            {
                                try
                                {
                                    this.Slope[current].Text = cg.Slope.ToString();
                                    this.Rating[current].Text = cg.Rating.ToString();
                                    this.Total[current].Text = cg.Par.ToString();
                                    for (int k = 0; k < 18; k++)
                                    {
                                        this.Par[current, k].Text = cg.Hole(k + 1).Par.ToString();
                                        this.Hcp[current, k].Text = cg.Hole(k + 1).Handicap.ToString();
                                    }
                                }
                                catch { }
                                current++;
                            }
                        }
                    }
                }
            }

        }

        private void AvailableCourses()
        {
            cbAvailCourses.Items.Clear();
            DataSet ds = DB.GetDataSet("select max(courseid) as courseid, course from courses group by course");
            ds.Tables[0].DefaultView.Sort = "course asc";
            for (int i = 0; i < ds.Tables[0].DefaultView.Count; i++)
            {
                cbAvailCourses.Items.Add(new ListItem(ds.Tables[0].DefaultView[i]["courseid"].ToString(), ds.Tables[0].DefaultView[i]["course"].ToString()));
            }
        }

        private void cbAvailCourses_SelectedIndexChanged(object sender, EventArgs e)
        {
            string courseId = ((ListItem)cbAvailCourses.SelectedItem).ID;
            if (courseId.Length > 0)
            {
                System.Data.DataSet rounds = DB.GetDataSet("select * from courses where courseid = " + courseId);
                //System.Data.DataSet tees = DB.GetDataSet("select * from teedetails where courseid = " + courseId);
                if (rounds.Tables[0].Rows.Count > 0)
                {
                    GolfCourse cg = new GolfCourse(rounds.Tables[0].Rows[0], 0, 1);
                    this.FillCourse(cg, true);
                }
            }
        }
    }
}
