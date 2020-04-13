using System.ComponentModel;

namespace client {
	partial class Form2 {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) { components.Dispose(); }

			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.table = new System.Windows.Forms.DataGridView();
			this.destination = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.departureTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.freeSeats = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize) (this.table)).BeginInit();
			this.SuspendLayout();
			// 
			// table
			// 
			this.table.AllowUserToAddRows = false;
			this.table.AllowUserToDeleteRows = false;
			this.table.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.table.Columns.AddRange(
				new System.Windows.Forms.DataGridViewColumn[] {this.destination, this.departureTime, this.freeSeats});
			this.table.Location = new System.Drawing.Point(115, 36);
			this.table.MultiSelect = false;
			this.table.Name = "table";
			this.table.ReadOnly = true;
			this.table.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.table.Size = new System.Drawing.Size(713, 396);
			this.table.TabIndex = 0;
			this.table.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.table_CellClick);
			// 
			// destination
			// 
			this.destination.HeaderText = "DestinationName";
			this.destination.Name = "destination";
			this.destination.ReadOnly = true;
			this.destination.Width = 200;
			// 
			// departureTime
			// 
			this.departureTime.HeaderText = "Departure Time";
			this.departureTime.Name = "departureTime";
			this.departureTime.ReadOnly = true;
			this.departureTime.Width = 200;
			// 
			// freeSeats
			// 
			this.freeSeats.HeaderText = "Free Seats";
			this.freeSeats.Name = "freeSeats";
			this.freeSeats.ReadOnly = true;
			this.freeSeats.Width = 200;
			// 
			// Form2
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(933, 519);
			this.Controls.Add(this.table);
			this.Name = "Form2";
			this.Text = "Form2";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
			((System.ComponentModel.ISupportInitialize) (this.table)).EndInit();
			this.ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.DataGridView table;
		private System.Windows.Forms.DataGridViewTextBoxColumn freeSeats;
		private System.Windows.Forms.DataGridViewTextBoxColumn departureTime;
		private System.Windows.Forms.DataGridViewTextBoxColumn destination;
	}
}