using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MonsterGolf
{
	/// <summary>
	/// Summary description for EditFlight.
	/// </summary>
	public class EditFlight : System.Windows.Forms.Form
	{
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TextBox textBox1;
      private System.Windows.Forms.Button button1;
      private System.Windows.Forms.Button button2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

      public EditTournament ParentEditTournament;
      public Tournament m_tournament;
      public string FlightName;

		public EditFlight()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			FlightName = "";
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditFlight));
         this.label1 = new System.Windows.Forms.Label();
         this.textBox1 = new System.Windows.Forms.TextBox();
         this.button1 = new System.Windows.Forms.Button();
         this.button2 = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // label1
         // 
         this.label1.Location = new System.Drawing.Point(8, 16);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(72, 16);
         this.label1.TabIndex = 0;
         this.label1.Text = "Flight Name:";
         // 
         // textBox1
         // 
         this.textBox1.Location = new System.Drawing.Point(80, 16);
         this.textBox1.Name = "textBox1";
         this.textBox1.Size = new System.Drawing.Size(288, 20);
         this.textBox1.TabIndex = 1;
         this.textBox1.Text = "textBox1";
         // 
         // button1
         // 
         this.button1.Location = new System.Drawing.Point(8, 48);
         this.button1.Name = "button1";
         this.button1.Size = new System.Drawing.Size(80, 24);
         this.button1.TabIndex = 2;
         this.button1.Text = "Save";
         this.button1.Click += new System.EventHandler(this.button1_Click);
         // 
         // button2
         // 
         this.button2.Location = new System.Drawing.Point(96, 48);
         this.button2.Name = "button2";
         this.button2.Size = new System.Drawing.Size(80, 24);
         this.button2.TabIndex = 3;
         this.button2.Text = "Close";
         this.button2.Click += new System.EventHandler(this.button2_Click);
         // 
         // EditFlight
         // 
         this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
         this.ClientSize = new System.Drawing.Size(384, 86);
         this.Controls.Add(this.button2);
         this.Controls.Add(this.button1);
         this.Controls.Add(this.textBox1);
         this.Controls.Add(this.label1);
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.Name = "EditFlight";
         this.Text = "EditFlight";
         this.Load += new System.EventHandler(this.EditFlight_Load);
         this.ResumeLayout(false);
         this.PerformLayout();

      }
		#endregion

      private void button1_Click(object sender, System.EventArgs e)
      {
         // SCREEN NO LONGER USED
         //bool bnew = false;
         //if ( FlightName == "" )
         //   bnew = true;
         //if ( textBox1.Text.Trim() == "" )
         //   return;

         //m_tournament.AddFlight(textBox1.Text);

         //if ( bnew )
         //{
         //   textBox1.Text = "";
         //   ParentEditTournament.ShowFlights();
         //}
         //else
         //{
         //   FlightName = textBox1.Text;
         //}
      }

      private void EditFlight_Load(object sender, System.EventArgs e)
      {
         textBox1.Text = FlightName;
      }

      private void button2_Click(object sender, System.EventArgs e)
      {
         this.Close();
      }
	}
}
