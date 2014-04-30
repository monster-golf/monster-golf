using System;
using System.Drawing;
using System.Windows.Forms;

namespace MonsterGolf
{
	/// <summary>
	/// Summary description for FormHelper.
	/// </summary>
	public class FormHelper
	{
		public FormHelper()
		{
			//
			// TODO: Add constructor logic here
			//
		}

      public void CreateLabel(ref Label currLabel, int width, int height, int x, int y, ContentAlignment align, Color color, string text) 
      {
         CreateLabel(ref currLabel, width, height, x, y, align, color, text, "");
      }

      public void CreateLabel(ref Label currLabel, int width, int height, int x, int y, ContentAlignment align, Color color, string text, string name) 
      {
         currLabel = new Label();
         currLabel.BorderStyle = BorderStyle.FixedSingle;
         currLabel.Size = new Size(width, height);
         currLabel.Location = new Point(x, y);
         currLabel.TextAlign = align;
         currLabel.BackColor = color;
         currLabel.Text = text;
         if ( name != "" )
            currLabel.Name = name;
      }
      public void CreateNumberBox(ref TextBox currTextBox, string name, int tabindex, int x, int y, int height, Color  color, HorizontalAlignment align, string text) 
      {
         CreateTextBox(ref currTextBox, name, tabindex, 20, height, 2, x, y, color, align, text); 
      }
      public void CreateTextBox(ref TextBox currTextBox, string name, int tabindex, int width, int height, int maxlength, int x, int y, Color  color, HorizontalAlignment align, string text) 
      {
         currTextBox = new System.Windows.Forms.TextBox();
         currTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         currTextBox.Location = new System.Drawing.Point(x, y);
         currTextBox.MaxLength = maxlength;
         currTextBox.Name = name;
         //currTextBox.Size = new System.Drawing.Size(width, height);
         currTextBox.Width = width;
         currTextBox.Height = height;
         if (tabindex < 0)
            currTextBox.TabStop = false;
         else
            currTextBox.TabIndex = tabindex;
         currTextBox.Text = text;
         currTextBox.TextAlign = align;
         currTextBox.BackColor = color;
      }
   }
}
