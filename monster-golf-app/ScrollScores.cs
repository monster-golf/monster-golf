using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace MonsterGolf
{
   public delegate void onPlayerViewClose(EventArgs e);
   public delegate void onZoom(EventArgs e);

   public partial class ScrollScores : UserControl
   {
      private const int _bottomSpace = 2;
      //private int[] _pictureHeight = { 180, 220, 280 };
      private Formulas.ScoringType _scoringType = Formulas.ScoringType.Gross;
      private DataTable DTResults;
      private string _currentBottom = "";
      private string _currentLeader = "";
      private bool _scrollPlayers = false;
      private int _currentScroll = 0;
      private bool _playersInView = false;
      public int WhatImageSize = 180;
      private DataTable _DTTeamScores;
      private string _currentTeamID = "";
      public Tournament Tournament;

      public event onPlayerViewClose PlayerViewClosed;
      public event onZoom PlayerZoom;

      public enum ImageSize
      {
         Small=0,
         Medium=1,
         Large=2
      }

      public ScrollScores()
      {
         InitializeComponent();

         _DTTeamScores = new DataTable();
         _DTTeamScores.Columns.Add("Hole");
         for ( int j=1; j<19; j++)
         {
            _DTTeamScores.Columns.Add(j.ToString());
            if ( j==9 )
               _DTTeamScores.Columns.Add("Out");
            if (j==18)
            {
               _DTTeamScores.Columns.Add("In");
               _DTTeamScores.Columns.Add("Total");
            }
         }
      }

      #region Properties
      public bool PlayersInView
      {
         get { return _playersInView; }
      }

      public Formulas.ScoringType ScoringType
      {
         set 
         { 
            _scoringType = value;
         }
      }

      public bool ScrollPlayers
      {
         set { 
            _scrollPlayers = value;
            ShowTeam("");
            this.timerRunningScroll.Enabled = true;
         }
         get { return _scrollPlayers; }
      }

      public int NecessaryHeight
      {
         get 
         {
            int height = labelLeader.Height + labelScroll.Height + _bottomSpace;
            if (_playersInView)
            {
               height += this.WhatImageSize;
               height += this.dataGridViewTeamScores.Height;
               height += this.buttonClosePlayers.Height;
               height += _bottomSpace;
            }
            return height;
         }
      }

      public DataTable Results
      {
         set { DTResults = value.Copy(); _currentBottom = ""; _currentLeader = ""; }
      }

      private void RemoveTeamView()
      {
         for (int i = this.Controls.Count-1; i >= 0; i--)
         {
            if (this.Controls[i].Name.StartsWith("image"))
               this.Controls.RemoveAt(i);
         }
         this.dataGridViewTeamScores.Visible = false;
         this.buttonClosePlayers.Visible = false;
         this.buttonZoomOut.Visible = false;
         this.buttonZoomIn.Visible = false;
         this.buttonPrev.Visible = false;
         this.buttonNext.Visible = false;
         this.buttonPause.Visible = false;
         this.labelName.Visible = false;
         _playersInView = false;
      }
      #endregion

      public void ShowTeam(string id)
      {
         if (DTResults == null)
            return;

         // remove previous
         RemoveTeamView();

         _currentTeamID = id;
         if (id == "")
         {
            _currentTeamID = DTResults.Rows[0]["TeamID"].ToString();
         }

         string curFilter = DTResults.DefaultView.RowFilter = "";
         DTResults.DefaultView.RowFilter = "TeamID=" + _currentTeamID;

         if ( DTResults.DefaultView.Count > 0)
         {
            // show the picture
            int totalWidth = 0;
            int totalHeight = 0;
            string[] images = DTResults.DefaultView[0]["Image"].ToString().Split(';');
            for (int j = 0; j < images.Length; j++)
            {
               if (images[j].Length > 0)
               {
                  try
                  {
                     Image img = Image.FromFile("Images/" + images[j]);
                     PictureBox pb = new PictureBox();
                     double pctGrowShrink = (double)this.WhatImageSize / (double)img.Height;
                     int width = (int)((double)img.Width * pctGrowShrink);
                     pb.Size = new Size(width, this.WhatImageSize);
                     pb.Location = new Point((totalWidth), 0);
                     totalWidth += width;
                     // set the height if we have a picture
                     totalHeight = this.WhatImageSize;
                     pb.Visible = true;
                     pb.Name = "image" + j.ToString();
                     pb.Image = img.GetThumbnailImage(width, (int)this.WhatImageSize,
                        null, System.IntPtr.Zero);
                     this.Controls.Add(pb);
                  }
                  catch
                  {
                     // bad image move on
                  }
               }
            }

            // set the team/player name
            labelName.Text = DTResults.DefaultView[0]["Name"].ToString();

            // show the scores
            int curCourse = -1;
            int curRow = 0;
            _DTTeamScores.Rows.Clear();
            DataRow dr;
            for (int j=0; j < DTResults.DefaultView.Count; j++)
            {
               string round = DTResults.DefaultView[j]["Round"].ToString();
               if (Tournament != null)
               {
                  // show the pars
                  int courseID = Tournament.GetCourseID(int.Parse(round));
                  if ( courseID != curCourse )
                  {
                     curCourse = courseID;
                     dr = _DTTeamScores.NewRow();
                     _DTTeamScores.Rows.Add(dr);
                     curRow = _DTTeamScores.Rows.Count - 1;
                     GolfCourse c = Tournament.Course(courseID, 0);
                     _DTTeamScores.Rows[curRow]["Hole"] = c.Name + " Par";
                     int outTotal = 0;
                     int inTotal = 0;
                     for (int i = 0; i < c.Holes.Length; i++)
                     {
                        _DTTeamScores.Rows[curRow][(i + 1).ToString()] = c.Holes[i].Par;
                        if (i < 9)
                           outTotal += c.Holes[i].Par;
                        if (i > 8)
                           inTotal += c.Holes[i].Par;
                     }
                     _DTTeamScores.Rows[curRow]["Out"] = outTotal.ToString();
                     _DTTeamScores.Rows[curRow]["In"] = inTotal.ToString();
                     _DTTeamScores.Rows[curRow]["Total"] = (outTotal + inTotal).ToString(); 
                  }
               }
               // show the scores
               dr = _DTTeamScores.NewRow();
               _DTTeamScores.Rows.Add(dr);
               curRow = _DTTeamScores.Rows.Count - 1;
               _DTTeamScores.Rows[curRow]["Hole"] = "Round " + round;
               for (int i = 1; i < 19; i++)
               {
                  _DTTeamScores.Rows[curRow][i.ToString()] = DTResults.DefaultView[j][i.ToString()].ToString();
                  if (i == 9)
                  {
                     _DTTeamScores.Rows[curRow]["Out"] = DTResults.DefaultView[j]["Out"].ToString();
                  }
                  if (i == 18)
                  {
                     _DTTeamScores.Rows[curRow]["In"] = DTResults.DefaultView[j]["In"].ToString();
                     _DTTeamScores.Rows[curRow]["Total"] = DTResults.DefaultView[j]["Total"].ToString();
                  }
               }
            }
            // add just the Overall to the end
            dr = _DTTeamScores.NewRow();
            _DTTeamScores.Rows.Add(dr);
            curRow = _DTTeamScores.Rows.Count - 1;
            _DTTeamScores.Rows[curRow]["Total"] = DTResults.DefaultView[0]["Overall"].ToString();
            // add the data to the grid
            this.dataGridViewTeamScores.DataSource = _DTTeamScores;
            // size, color and show the grid
            this.buttonClosePlayers.Visible = true;
            this.buttonZoomOut.Visible = true;
            this.buttonZoomIn.Visible = true;
            this.labelName.Visible = true;
            this.buttonPause.Visible = true;
            this.buttonNext.Visible = true;
            this.buttonPrev.Visible = true;
            this.buttonClosePlayers.Top = totalHeight;
            this.buttonZoomOut.Top = totalHeight;
            this.buttonZoomIn.Top = totalHeight;
            this.buttonNext.Top = totalHeight;
            this.buttonPrev.Top = totalHeight;
            this.buttonPause.Top = totalHeight;
            if (!this.timerScrollScores.Enabled)
               this.buttonPause.Enabled = false;
            else
               this.buttonPause.Enabled = true;
            this.labelName.Top = totalHeight;
            this.dataGridViewTeamScores.Visible = true;
            this.dataGridViewTeamScores.Top = totalHeight + buttonClosePlayers.Height;
            //this.dataGridViewTeamScores.Width = this.Width - totalWidth;
            this.dataGridViewTeamScores.Height = this.dataGridViewTeamScores.ColumnHeadersHeight
               + (this.dataGridViewTeamScores.Rows.Count * this.dataGridViewTeamScores.Rows[0].Height
               + 20 ); // add 20 for scrollbar

            for (int i = 1; i < 19; i++)
            {
               this.dataGridViewTeamScores.Columns[i.ToString()].Width = 28;
            }
            this.dataGridViewTeamScores.Columns["Out"].Width = 30;
            this.dataGridViewTeamScores.Columns["In"].Width = 30;
            this.dataGridViewTeamScores.Columns["Total"].Width = 40;
            for (int i = 0; i < this.dataGridViewTeamScores.Rows.Count; i++)
            {
               if (this.dataGridViewTeamScores.Rows[i].Cells[0].Value.ToString().EndsWith("Par"))
               {
                  this.dataGridViewTeamScores.Rows[i].Cells["Hole"].Style.BackColor = Color.LightGreen;
                  for ( int j = 1; j<19; j++ )
                     this.dataGridViewTeamScores.Rows[i].Cells[j.ToString()].Style.BackColor = Color.LightGreen;
               }
               this.dataGridViewTeamScores.Rows[i].Cells["Out"].Style.BackColor = Color.LightGreen;
               this.dataGridViewTeamScores.Rows[i].Cells["In"].Style.BackColor = Color.LightGreen;
               this.dataGridViewTeamScores.Rows[i].Cells["Total"].Style.BackColor = Color.LightGreen;
            }
            _playersInView = true;
         }

         DTResults.DefaultView.RowFilter = curFilter;
      }

      private void OnResize(object sender, EventArgs e)
      {
         //int totalWidth = 0;
         //for (int i = 0; i < this.Controls.Count; i++)
         //   totalWidth += ((this.Controls[i].Name.StartsWith("image")) ? this.Controls[i].Width : 0);
         //this.dataGridViewTeamScores.Left = totalWidth;
         //this.dataGridViewTeamScores.Width = this.Width - totalWidth;
         //this.buttonClosePlayers.Left = totalWidth;
         //this.buttonZoomOut.Left = totalWidth + this.buttonClosePlayers.Width;
         //this.buttonZoomIn.Left = totalWidth + this.buttonClosePlayers.Width + this.buttonZoomOut.Width;
         //this.labelName.Left = totalWidth + this.buttonClosePlayers.Width + this.buttonZoomOut.Width + this.buttonZoomIn.Width + 4;
      }

      private void timerRunningScroll_Tick(object sender, EventArgs e)
      {
         if (_currentBottom.Length == 0 && DTResults != null)
         {
            // set up the bottom
            string prevSort = DTResults.DefaultView.Sort;
            bool showToPar = false;

            if (_scoringType == Formulas.ScoringType.GrossParPoints ||
                _scoringType == Formulas.ScoringType.NetParPoints)
               DTResults.DefaultView.Sort = "Overall DESC";
            else
            {
               showToPar = true;
               DTResults.DefaultView.Sort = "Overall ASC";
            }

            string curTeamID = "";
            string leading = "";
            string leadingToPar = "";
            int position = 0;
            int curTotal = 0;
            int lastTotal = 0;
            for (int i = 0; i < DTResults.DefaultView.Count; i++)
            {
               if (curTeamID != DTResults.DefaultView[i]["TeamID"].ToString())
               {
                  if (leading == "" ||
                      DTResults.DefaultView[i]["Overall"].ToString() == leading)
                  {
                     if (DTResults.DefaultView[i]["Overall"].ToString() != "0" &&
                         DTResults.DefaultView[i]["Overall"].ToString() != "")
                     {
                        leading = DTResults.DefaultView[i]["Overall"].ToString();
                        this._currentLeader += (this._currentLeader.Length > 0) ? "   " : "";
                        this._currentLeader += DTResults.DefaultView[i]["Name"].ToString();
                        if (showToPar)
                           leadingToPar = (DTResults.DefaultView[i]["Overall ToPar"].ToString() == "0") ? "par" : DTResults.DefaultView[i]["Overall ToPar"].ToString();
                     }
                  }
                  curTotal = int.Parse(DTResults.DefaultView[i]["Overall"].ToString());
                  if (lastTotal == 0 || lastTotal != curTotal)
                  {
                     position++;
                     lastTotal = curTotal;
                  }
                  this._currentBottom += position.ToString() + ". ";
                  this._currentBottom += DTResults.DefaultView[i]["Name"].ToString();
                  this._currentBottom += " " + DTResults.DefaultView[i]["Overall"].ToString();
                  if (showToPar)
                  {
                     this._currentBottom += " (" + ((DTResults.DefaultView[i]["Overall ToPar"].ToString() == "0") ? "par" : DTResults.DefaultView[i]["Overall ToPar"].ToString()) + ")";
                  }
                  this._currentBottom += "    ";
               }
               curTeamID = DTResults.DefaultView[i]["TeamID"].ToString();
            }
            DTResults.DefaultView.Sort = prevSort;

            this.labelLeader.Text = "Leader(s): " + this._currentLeader + "   " + leading;
            if (showToPar)
               this.labelLeader.Text += " (" + leadingToPar + ")";
         }

         // move the bottom
         this.labelScroll.Text = _currentBottom;
         if (_currentBottom.Length * 7 > this.labelLeader.Width)
         {
            this.labelScroll.Text = _currentBottom.Substring(0, ((int)this.labelLeader.Width / 7));
            _currentBottom = _currentBottom.Substring(1) + _currentBottom.Substring(0, 1);
         }
         else
         {
            this.labelScroll.Text = _currentBottom;
         }
      }

      public void StartScroll()
      {
         _currentScroll = -1;
         timerScrollScores_Tick(null, null);
         this.timerScrollScores.Start();
      }
      public void StopScroll()
      {
         this.timerScrollScores.Stop();
         this.RemoveTeamView();
      }
      public void TogglePauseScroll()
      {
         if ( this.timerScrollScores.Enabled )
            this.timerScrollScores.Stop();
         else
            this.timerScrollScores.Start();
      }

      private void ScrollNextPrev(bool previous)
      {
         if (DTResults == null)
            return;
         string teamid = _currentTeamID;
         int startpos = _currentScroll;
         while (teamid == _currentTeamID)
         {
            if (previous)
            {
               _currentScroll--;
               if (_currentScroll < 0)
                  _currentScroll = DTResults.Rows.Count - 1;
            }
            else
            {
               _currentScroll++;
               if (_currentScroll >= DTResults.Rows.Count)
                  _currentScroll = 0;
            }
            if (_currentScroll == startpos)
               break; // all the teams in the data set are the same, show the same team
            teamid = DTResults.Rows[_currentScroll]["TeamID"].ToString();
         }
         this.ShowTeam(teamid);
      }
      public void ScrollNext()
      {
         ScrollNextPrev(false);
      }
      public void ScrollPrev()
      {
         ScrollNextPrev(true);
      }

      private void timerScrollScores_Tick(object sender, EventArgs e)
      {
         ScrollNext();
      }

      private void buttonClosePlayers_Click(object sender, EventArgs e)
      {
         this.RemoveTeamView();
         try { PlayerViewClosed(new EventArgs()); }
         catch { }  
      }

      private void buttonZoomOut_Click(object sender, EventArgs e)
      {
         this.WhatImageSize += 60;

         ShowTeam(_currentTeamID);
         try { PlayerZoom(new EventArgs()); }
         catch { }
      }

      private void buttonZoomIn_Click(object sender, EventArgs e)
      {
         this.WhatImageSize -= 60;
         if (this.WhatImageSize < 180)
            this.WhatImageSize = 180;

         ShowTeam(_currentTeamID);
         try { PlayerZoom(new EventArgs()); }
         catch { }
      }

      private void buttonPause_Click(object sender, EventArgs e)
      {
         TogglePauseScroll();
         if (buttonPause.Text == "||")
            buttonPause.Text = ">";
         else
            buttonPause.Text = "||";
      }

      private void buttonPrev_Click(object sender, EventArgs e)
      {
         bool restartTimer = this.timerScrollScores.Enabled;
         if (restartTimer)
            this.timerScrollScores.Stop();
         ScrollPrev();
         if (restartTimer)
            this.timerScrollScores.Start();
      }

      private void buttonNext_Click(object sender, EventArgs e)
      {
         bool restartTimer = this.timerScrollScores.Enabled;
         if (restartTimer)
            this.timerScrollScores.Stop();
         ScrollNext();
         if (restartTimer)
            this.timerScrollScores.Start();
      }
   }
}
