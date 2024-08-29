namespace QLBHNT
{
    partial class frmQLHopDong
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgv_HopDong = new System.Windows.Forms.DataGridView();
            this.txt_timkiem = new System.Windows.Forms.TextBox();
            this.cbo_tenBH = new System.Windows.Forms.ComboBox();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.btn_sua = new System.Windows.Forms.Button();
            this.btn_xoa = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtp_ngayBD = new System.Windows.Forms.DateTimePicker();
            this.dtp_ngayKT = new System.Windows.Forms.DateTimePicker();
            this.cbo_stBH = new System.Windows.Forms.ComboBox();
            this.btn_timkiem = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_HopDong)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_HopDong
            // 
            this.dgv_HopDong.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_HopDong.BackgroundColor = System.Drawing.SystemColors.ActiveCaption;
            this.dgv_HopDong.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_HopDong.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_HopDong.Location = new System.Drawing.Point(6, 12);
            this.dgv_HopDong.Name = "dgv_HopDong";
            this.dgv_HopDong.RowHeadersWidth = 51;
            this.dgv_HopDong.RowTemplate.Height = 24;
            this.dgv_HopDong.Size = new System.Drawing.Size(1169, 384);
            this.dgv_HopDong.TabIndex = 1;
            this.dgv_HopDong.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_HopDong_CellClick);
            this.dgv_HopDong.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // txt_timkiem
            // 
            this.txt_timkiem.Location = new System.Drawing.Point(135, 416);
            this.txt_timkiem.Multiline = true;
            this.txt_timkiem.Name = "txt_timkiem";
            this.txt_timkiem.Size = new System.Drawing.Size(273, 32);
            this.txt_timkiem.TabIndex = 6;
            this.txt_timkiem.TextChanged += new System.EventHandler(this.txttimkiem_TextChanged);
            // 
            // cbo_tenBH
            // 
            this.cbo_tenBH.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbo_tenBH.FormattingEnabled = true;
            this.cbo_tenBH.Location = new System.Drawing.Point(242, 473);
            this.cbo_tenBH.Name = "cbo_tenBH";
            this.cbo_tenBH.Size = new System.Drawing.Size(285, 33);
            this.cbo_tenBH.TabIndex = 111;
            // 
            // btn_refresh
            // 
            this.btn_refresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_refresh.Location = new System.Drawing.Point(809, 574);
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.Size = new System.Drawing.Size(165, 50);
            this.btn_refresh.TabIndex = 110;
            this.btn_refresh.Text = "Làm mới";
            this.btn_refresh.UseVisualStyleBackColor = true;
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            // 
            // btn_sua
            // 
            this.btn_sua.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btn_sua.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_sua.Location = new System.Drawing.Point(243, 574);
            this.btn_sua.Name = "btn_sua";
            this.btn_sua.Size = new System.Drawing.Size(165, 50);
            this.btn_sua.TabIndex = 109;
            this.btn_sua.Text = "Sửa Hợp Đồng";
            this.btn_sua.UseVisualStyleBackColor = false;
            this.btn_sua.Click += new System.EventHandler(this.btn_sua_Click);
            // 
            // btn_xoa
            // 
            this.btn_xoa.BackColor = System.Drawing.Color.IndianRed;
            this.btn_xoa.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_xoa.Location = new System.Drawing.Point(519, 574);
            this.btn_xoa.Name = "btn_xoa";
            this.btn_xoa.Size = new System.Drawing.Size(165, 50);
            this.btn_xoa.TabIndex = 108;
            this.btn_xoa.Text = "Xóa Hợp Đồng";
            this.btn_xoa.UseVisualStyleBackColor = false;
            this.btn_xoa.Click += new System.EventHandler(this.btn_xoa_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(608, 527);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(158, 25);
            this.label12.TabIndex = 106;
            this.label12.Text = "Số tiền bảo hiểm";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(608, 476);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(218, 25);
            this.label7.TabIndex = 103;
            this.label7.Text = "Ngày kết thúc hợp đồng";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(12, 527);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(215, 25);
            this.label6.TabIndex = 102;
            this.label6.Text = "Ngày bắt đầu hợp đồng";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 476);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 25);
            this.label2.TabIndex = 101;
            this.label2.Text = "Tên bảo hiểm";
            // 
            // dtp_ngayBD
            // 
            this.dtp_ngayBD.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtp_ngayBD.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtp_ngayBD.Location = new System.Drawing.Point(243, 527);
            this.dtp_ngayBD.Name = "dtp_ngayBD";
            this.dtp_ngayBD.Size = new System.Drawing.Size(285, 30);
            this.dtp_ngayBD.TabIndex = 159;
            // 
            // dtp_ngayKT
            // 
            this.dtp_ngayKT.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtp_ngayKT.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtp_ngayKT.Location = new System.Drawing.Point(865, 476);
            this.dtp_ngayKT.Name = "dtp_ngayKT";
            this.dtp_ngayKT.Size = new System.Drawing.Size(285, 30);
            this.dtp_ngayKT.TabIndex = 160;
            // 
            // cbo_stBH
            // 
            this.cbo_stBH.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbo_stBH.FormattingEnabled = true;
            this.cbo_stBH.Location = new System.Drawing.Point(865, 519);
            this.cbo_stBH.Name = "cbo_stBH";
            this.cbo_stBH.Size = new System.Drawing.Size(285, 33);
            this.cbo_stBH.TabIndex = 161;
            // 
            // btn_timkiem
            // 
            this.btn_timkiem.Location = new System.Drawing.Point(17, 416);
            this.btn_timkiem.Name = "btn_timkiem";
            this.btn_timkiem.Size = new System.Drawing.Size(101, 32);
            this.btn_timkiem.TabIndex = 163;
            this.btn_timkiem.Text = "Tìm kiếm";
            this.btn_timkiem.UseVisualStyleBackColor = true;
            this.btn_timkiem.Click += new System.EventHandler(this.btn_timkiem_Click);
            // 
            // frmQLHopDong
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1187, 636);
            this.Controls.Add(this.btn_timkiem);
            this.Controls.Add(this.cbo_stBH);
            this.Controls.Add(this.dtp_ngayKT);
            this.Controls.Add(this.dtp_ngayBD);
            this.Controls.Add(this.cbo_tenBH);
            this.Controls.Add(this.btn_refresh);
            this.Controls.Add(this.btn_sua);
            this.Controls.Add(this.btn_xoa);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_timkiem);
            this.Controls.Add(this.dgv_HopDong);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmQLHopDong";
            this.Text = "frmQLHopDong";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmQLHopDong_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_HopDong)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_HopDong;
        private System.Windows.Forms.TextBox txt_timkiem;
        private System.Windows.Forms.ComboBox cbo_tenBH;
        private System.Windows.Forms.Button btn_refresh;
        private System.Windows.Forms.Button btn_sua;
        private System.Windows.Forms.Button btn_xoa;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtp_ngayBD;
        private System.Windows.Forms.DateTimePicker dtp_ngayKT;
        private System.Windows.Forms.ComboBox cbo_stBH;
        private System.Windows.Forms.Button btn_timkiem;
    }
}