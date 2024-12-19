using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessTable
{
	public partial class FrmUpgradeInput : Form
	{
		private Label lblMessage;
		private ComboBox cmbOptions;
		private Button btnOK;

		public string SelectedOption { get; private set; }

		public FrmUpgradeInput()
		{
			cmbOptions = new ComboBox
			{
				DropDownStyle = ComboBoxStyle.DropDownList,
				Location = new System.Drawing.Point(10, 60),
				Width = 200
			};
			cmbOptions.Items.AddRange(new string[] { "1-Bishop", "2-Knight", "3-Rook", "4-Queen" });

			btnOK = new Button
			{
				Text = "OK",
				Location = new System.Drawing.Point(10, 100),
				DialogResult = DialogResult.OK
			};

			btnOK.Click += (sender, e) =>
			{
				if (cmbOptions.SelectedIndex >= 0)
				{
					SelectedOption = cmbOptions.SelectedItem.ToString();
					this.Close();
				}
				else
				{
					MessageBox.Show("Please select an option.");
				}
			};

			// Add controls to the form
			Controls.Add(lblMessage);
			Controls.Add(cmbOptions);
			Controls.Add(btnOK);

			// Set form properties
			this.Text = "Upgrade Selection";
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.StartPosition = FormStartPosition.CenterParent;
			this.ClientSize = new System.Drawing.Size(230, 150);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.AcceptButton = btnOK;
		}
	}
}
