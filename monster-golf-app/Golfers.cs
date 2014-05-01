using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace MonsterGolf
{
	/// <summary>
	/// Summary description for Golfers.
	/// </summary>
	public class Golfers : System.Windows.Forms.Form
	{
      //private Tournament m_tournament;
      private System.Windows.Forms.Label label5;
      private DataGridView dataGridViewGolfers;
      private Button buttonAdd;
      private TextBox textBoxFirst;
      private TextBox textBoxLast;
      private Label labelFirst;
      private Label labelLast;

		public Golfers()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
         ShowGolfers();
      }

      private void ShowGolfers()
      {
         DataSet ds = DB.GetDataSet("select * from users");
         ds.Tables[0].Columns["UserID"].ColumnMapping = MappingType.Hidden;
         //ds.Tables[0].Columns["WebID"].ColumnMapping = MappingType.Hidden;
         //ds.Tables[0].Columns["HcpIndex"].ColumnMapping = MappingType.Hidden;
         //ds.Tables[0].Columns["Image"].ColumnMapping = MappingType.Hidden;
         this.dataGridViewGolfers.DataSource = ds.Tables[0];
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Golfers));
         this.label5 = new System.Windows.Forms.Label();
         this.dataGridViewGolfers = new System.Windows.Forms.DataGridView();
         this.buttonAdd = new System.Windows.Forms.Button();
         this.textBoxFirst = new System.Windows.Forms.TextBox();
         this.textBoxLast = new System.Windows.Forms.TextBox();
         this.labelFirst = new System.Windows.Forms.Label();
         this.labelLast = new System.Windows.Forms.Label();
         ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGolfers)).BeginInit();
         this.SuspendLayout();
         // 
         // label5
         // 
         this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label5.Location = new System.Drawing.Point(8, 8);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(560, 16);
         this.label5.TabIndex = 0;
         this.label5.Text = "Add/Edit Golfers";
         // 
         // dataGridViewGolfers
         // 
         this.dataGridViewGolfers.AllowUserToAddRows = false;
         this.dataGridViewGolfers.AllowUserToDeleteRows = false;
         this.dataGridViewGolfers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                     | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.dataGridViewGolfers.BackgroundColor = System.Drawing.SystemColors.Control;
         this.dataGridViewGolfers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
         this.dataGridViewGolfers.Location = new System.Drawing.Point(8, 34);
         this.dataGridViewGolfers.Name = "dataGridViewGolfers";
         this.dataGridViewGolfers.Size = new System.Drawing.Size(559, 471);
         this.dataGridViewGolfers.TabIndex = 1;
         this.dataGridViewGolfers.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewGolfers_CellValueChanged);
         // 
         // buttonAdd
         // 
         this.buttonAdd.Location = new System.Drawing.Point(493, 5);
         this.buttonAdd.Name = "buttonAdd";
         this.buttonAdd.Size = new System.Drawing.Size(75, 23);
         this.buttonAdd.TabIndex = 2;
         this.buttonAdd.Text = "Add New";
         this.buttonAdd.UseVisualStyleBackColor = true;
         this.buttonAdd.Click += new System.EventHandler(this.buttonSave_Click);
         // 
         // textBoxFirst
         // 
         this.textBoxFirst.Location = new System.Drawing.Point(250, 8);
         this.textBoxFirst.Name = "textBoxFirst";
         this.textBoxFirst.Size = new System.Drawing.Size(100, 20);
         this.textBoxFirst.TabIndex = 3;
         // 
         // textBoxLast
         // 
         this.textBoxLast.Location = new System.Drawing.Point(387, 8);
         this.textBoxLast.Name = "textBoxLast";
         this.textBoxLast.Size = new System.Drawing.Size(100, 20);
         this.textBoxLast.TabIndex = 4;
         // 
         // labelFirst
         // 
         this.labelFirst.AutoSize = true;
         this.labelFirst.Location = new System.Drawing.Point(218, 11);
         this.labelFirst.Name = "labelFirst";
         this.labelFirst.Size = new System.Drawing.Size(26, 13);
         this.labelFirst.TabIndex = 5;
         this.labelFirst.Text = "First";
         // 
         // labelLast
         // 
         this.labelLast.AutoSize = true;
         this.labelLast.Location = new System.Drawing.Point(354, 11);
         this.labelLast.Name = "labelLast";
         this.labelLast.Size = new System.Drawing.Size(27, 13);
         this.labelLast.TabIndex = 6;
         this.labelLast.Text = "Last";
         // 
         // Golfers
         // 
         this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
         this.ClientSize = new System.Drawing.Size(576, 510);
         this.Controls.Add(this.labelLast);
         this.Controls.Add(this.labelFirst);
         this.Controls.Add(this.textBoxLast);
         this.Controls.Add(this.textBoxFirst);
         this.Controls.Add(this.buttonAdd);
         this.Controls.Add(this.dataGridViewGolfers);
         this.Controls.Add(this.label5);
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.Name = "Golfers";
         this.Text = "Golfers";
         this.Load += new System.EventHandler(this.Golfers_Load);
         ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGolfers)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }
		#endregion

      private void buttonSave_Click(object sender, System.EventArgs e)
      {
         
         if (textBoxFirst.Text.Trim().Length == 0 ||
            textBoxLast.Text.Trim().Length == 0)
         {
            MessageBox.Show("You must enter a first and last name.");
            return;
         }

         this.Cursor = Cursors.WaitCursor;
         // new user insert them
         string sql = "insert into users (FirstName, LastName) values (";
         sql += textBoxFirst.Text.Trim().Replace("'", "''");
         sql += ",";
         sql += textBoxLast.Text.Trim().Replace("'", "''");
         sql += ")";
         DB.Execute(sql);

         this.ShowGolfers();
         this.Cursor = Cursors.Default;
      }

      private void Golfers_Load(object sender, EventArgs e)
      {
         // TODO: This line of code loads data into the 'monsterScoringDataSet.Users' table. You can move, or remove it, as needed.

      }

      private void dataGridViewGolfers_CellValueChanged(object sender, DataGridViewCellEventArgs e)
      {
         if (e.RowIndex == -1)
            return;

         this.Cursor = Cursors.WaitCursor;
         DataGridView dgv = (DataGridView)sender;
         DataTable dt = (DataTable)dgv.DataSource;


         if (dt.DefaultView[e.RowIndex]["UserID"] != DBNull.Value)
         {
            string userid = dt.DefaultView[e.RowIndex]["UserID"].ToString();

            bool first = true;
            string sql = "update Users set ";
            for (int j = 0; j < dt.Columns.Count; j++)
            {
               if (dt.Columns[j].ColumnMapping != MappingType.Hidden)
               {
                  if (!first)
                     sql += ", ";
                  sql += "[" + dt.Columns[j].ColumnName + "]";
                  sql += "=";

                  if (dt.Columns[j].DataType == typeof(string))
                  {
                     sql += "'";
                     sql += dgv[dt.Columns[j].ColumnName, e.RowIndex].Value.ToString().Replace("'", "''"); //.Replace(".", "\\.");
                     sql += "'";
                  }
                  else
                  {
                     sql += dgv[dt.Columns[j].ColumnName, e.RowIndex].Value.ToString();
                  }

                  first = false;
               }
            }
            sql += " where UserID = " + userid;

            DB.Execute(sql);
         }
         else
         {
            // new user insert them
            bool first = true;
            string sql = "insert into users (";
            for (int j = 0; j < dt.Columns.Count; j++)
            {
               if (dt.Columns[j].ColumnMapping != MappingType.Hidden)
               {
                  if (!first)
                     sql += ", ";
                  sql += "[" + dt.Columns[j].ColumnName + "]";
                  first = false;
               }
            }
            sql += ") values (";
            first = true;
            for (int j = 0; j < dt.Columns.Count; j++)
            {
               if (dt.Columns[j].ColumnMapping != MappingType.Hidden)
               {
                  if (!first)
                     sql += ", ";
                  if (dt.Columns[j].DataType == typeof(string))
                  {
                     sql += "'";
                     sql += dgv[dt.Columns[j].ColumnName, e.RowIndex].Value.ToString().Replace("'", "''"); //.Replace(".", "\\.");
                     sql += "'";
                  }
                  else
                  {
                     sql += dgv[dt.Columns[j].ColumnName, e.RowIndex].Value.ToString();
                  }
                  first = false;
               }
            }
            sql += ")";

            DB.Execute(sql);

         }

         this.Cursor = Cursors.Default;
      }
	}
}
