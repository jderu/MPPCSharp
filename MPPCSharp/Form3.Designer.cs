using System.ComponentModel;

namespace MPPCSharp {
	partial class Form3 {
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
			this.SeatNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ClientName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.clientNameTextBox = new System.Windows.Forms.TextBox();
			this.seatNumberTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.reserveButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize) (this.table)).BeginInit();
			this.SuspendLayout();
			// 
			// table
			// 
			this.table.AllowUserToAddRows = false;
			this.table.AllowUserToDeleteRows = false;
			this.table.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.table.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {this.SeatNumber, this.ClientName});
			this.table.Location = new System.Drawing.Point(12, 12);
			this.table.Name = "table";
			this.table.ReadOnly = true;
			this.table.Size = new System.Drawing.Size(446, 495);
			this.table.TabIndex = 0;
			// 
			// SeatNumber
			// 
			this.SeatNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
			this.SeatNumber.HeaderText = "Seat number";
			this.SeatNumber.Name = "SeatNumber";
			this.SeatNumber.ReadOnly = true;
			this.SeatNumber.Width = 99;
			// 
			// ClientName
			// 
			this.ClientName.HeaderText = "Name";
			this.ClientName.Name = "ClientName";
			this.ClientName.ReadOnly = true;
			// 
			// clientNameTextBox
			// 
			this.clientNameTextBox.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular,
				System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.clientNameTextBox.Location = new System.Drawing.Point(594, 55);
			this.clientNameTextBox.Name = "clientNameTextBox";
			this.clientNameTextBox.Size = new System.Drawing.Size(222, 29);
			this.clientNameTextBox.TabIndex = 1;
			// 
			// seatNumberTextBox
			// 
			this.seatNumberTextBox.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular,
				System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.seatNumberTextBox.Location = new System.Drawing.Point(594, 123);
			this.seatNumberTextBox.Name = "seatNumberTextBox";
			this.seatNumberTextBox.Size = new System.Drawing.Size(222, 29);
			this.seatNumberTextBox.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular,
				System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.label1.Location = new System.Drawing.Point(594, 27);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(220, 28);
			this.label1.TabIndex = 3;
			this.label1.Text = "Client Name";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular,
				System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.label2.Location = new System.Drawing.Point(596, 91);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(218, 29);
			this.label2.TabIndex = 4;
			this.label2.Text = "Seat Number";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// reserveButton
			// 
			this.reserveButton.Location = new System.Drawing.Point(649, 174);
			this.reserveButton.Name = "reserveButton";
			this.reserveButton.Size = new System.Drawing.Size(101, 46);
			this.reserveButton.TabIndex = 5;
			this.reserveButton.Text = "Reserve";
			this.reserveButton.UseVisualStyleBackColor = true;
			this.reserveButton.Click += new System.EventHandler(this.button1_Click);
			// 
			// Form3
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(933, 519);
			this.Controls.Add(this.reserveButton);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.seatNumberTextBox);
			this.Controls.Add(this.clientNameTextBox);
			this.Controls.Add(this.table);
			this.Name = "Form3";
			this.Text = "Form3";
			((System.ComponentModel.ISupportInitialize) (this.table)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		#endregion

		private System.Windows.Forms.DataGridViewTextBoxColumn SeatNumber;
		private System.Windows.Forms.DataGridView table;
		private System.Windows.Forms.DataGridViewTextBoxColumn ClientName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button reserveButton;
		private System.Windows.Forms.TextBox seatNumberTextBox;
		private System.Windows.Forms.TextBox clientNameTextBox;
	}
}