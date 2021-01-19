
namespace CS.ArchiveMaker.WinForm
{
    partial class Form_PrintSetting
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_save = new System.Windows.Forms.Button();
            this.btn_close = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.rbPortrait = new System.Windows.Forms.RadioButton();
            this.rbLanscape = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.tbMarginRight = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tbMarginLeft = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tbMarginBottom = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbMarginTop = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbPrinter = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbPaperHeight = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbPaperWidth = new System.Windows.Forms.TextBox();
            this.cmbPaper = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_save
            // 
            this.btn_save.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_save.Location = new System.Drawing.Point(288, 243);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(75, 23);
            this.btn_save.TabIndex = 2;
            this.btn_save.Text = "保存";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btn_close
            // 
            this.btn_close.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_close.Location = new System.Drawing.Point(389, 243);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(75, 23);
            this.btn_close.TabIndex = 3;
            this.btn_close.Text = "关闭";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label17.Location = new System.Drawing.Point(144, 141);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(21, 14);
            this.label17.TabIndex = 114;
            this.label17.Text = "mm";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label16.Location = new System.Drawing.Point(246, 141);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(21, 14);
            this.label16.TabIndex = 113;
            this.label16.Text = "mm";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.Location = new System.Drawing.Point(477, 141);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(21, 14);
            this.label15.TabIndex = 112;
            this.label15.Text = "mm";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.Location = new System.Drawing.Point(477, 97);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(21, 14);
            this.label14.TabIndex = 111;
            this.label14.Text = "mm";
            // 
            // rbPortrait
            // 
            this.rbPortrait.AutoSize = true;
            this.rbPortrait.Checked = true;
            this.rbPortrait.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbPortrait.Location = new System.Drawing.Point(147, 56);
            this.rbPortrait.Name = "rbPortrait";
            this.rbPortrait.Size = new System.Drawing.Size(53, 18);
            this.rbPortrait.TabIndex = 110;
            this.rbPortrait.TabStop = true;
            this.rbPortrait.Text = "纵向";
            this.rbPortrait.UseVisualStyleBackColor = true;
            // 
            // rbLanscape
            // 
            this.rbLanscape.AutoSize = true;
            this.rbLanscape.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbLanscape.Location = new System.Drawing.Point(81, 56);
            this.rbLanscape.Name = "rbLanscape";
            this.rbLanscape.Size = new System.Drawing.Size(53, 18);
            this.rbLanscape.TabIndex = 109;
            this.rbLanscape.TabStop = true;
            this.rbLanscape.Text = "横向";
            this.rbLanscape.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(12, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 108;
            this.label1.Text = "方  向：";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(387, 141);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(21, 14);
            this.label12.TabIndex = 107;
            this.label12.Text = "右";
            // 
            // tbMarginRight
            // 
            this.tbMarginRight.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbMarginRight.Location = new System.Drawing.Point(421, 138);
            this.tbMarginRight.Name = "tbMarginRight";
            this.tbMarginRight.Size = new System.Drawing.Size(50, 23);
            this.tbMarginRight.TabIndex = 106;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(273, 141);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(21, 14);
            this.label11.TabIndex = 105;
            this.label11.Text = "左";
            // 
            // tbMarginLeft
            // 
            this.tbMarginLeft.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbMarginLeft.Location = new System.Drawing.Point(304, 138);
            this.tbMarginLeft.Name = "tbMarginLeft";
            this.tbMarginLeft.Size = new System.Drawing.Size(50, 23);
            this.tbMarginLeft.TabIndex = 104;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(167, 141);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(21, 14);
            this.label10.TabIndex = 103;
            this.label10.Text = "下";
            // 
            // tbMarginBottom
            // 
            this.tbMarginBottom.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbMarginBottom.Location = new System.Drawing.Point(190, 138);
            this.tbMarginBottom.Name = "tbMarginBottom";
            this.tbMarginBottom.Size = new System.Drawing.Size(50, 23);
            this.tbMarginBottom.TabIndex = 102;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(65, 141);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(21, 14);
            this.label9.TabIndex = 101;
            this.label9.Text = "上";
            // 
            // tbMarginTop
            // 
            this.tbMarginTop.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbMarginTop.Location = new System.Drawing.Point(88, 138);
            this.tbMarginTop.Name = "tbMarginTop";
            this.tbMarginTop.Size = new System.Drawing.Size(50, 23);
            this.tbMarginTop.TabIndex = 100;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(7, 141);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 14);
            this.label8.TabIndex = 99;
            this.label8.Text = "页边距：";
            // 
            // cmbPrinter
            // 
            this.cmbPrinter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbPrinter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPrinter.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbPrinter.FormattingEnabled = true;
            this.cmbPrinter.Location = new System.Drawing.Point(67, 12);
            this.cmbPrinter.Name = "cmbPrinter";
            this.cmbPrinter.Size = new System.Drawing.Size(427, 22);
            this.cmbPrinter.TabIndex = 98;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(8, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 14);
            this.label5.TabIndex = 97;
            this.label5.Text = "打印机：";
            // 
            // tbPaperHeight
            // 
            this.tbPaperHeight.Enabled = false;
            this.tbPaperHeight.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbPaperHeight.Location = new System.Drawing.Point(421, 94);
            this.tbPaperHeight.Name = "tbPaperHeight";
            this.tbPaperHeight.Size = new System.Drawing.Size(50, 23);
            this.tbPaperHeight.TabIndex = 96;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(386, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 14);
            this.label4.TabIndex = 95;
            this.label4.Text = "高：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(273, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 14);
            this.label3.TabIndex = 94;
            this.label3.Text = "宽";
            // 
            // tbPaperWidth
            // 
            this.tbPaperWidth.Enabled = false;
            this.tbPaperWidth.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbPaperWidth.Location = new System.Drawing.Point(304, 94);
            this.tbPaperWidth.Name = "tbPaperWidth";
            this.tbPaperWidth.Size = new System.Drawing.Size(50, 23);
            this.tbPaperWidth.TabIndex = 93;
            // 
            // cmbPaper
            // 
            this.cmbPaper.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPaper.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbPaper.FormattingEnabled = true;
            this.cmbPaper.Location = new System.Drawing.Point(67, 94);
            this.cmbPaper.Name = "cmbPaper";
            this.cmbPaper.Size = new System.Drawing.Size(196, 22);
            this.cmbPaper.TabIndex = 92;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(8, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 14);
            this.label2.TabIndex = 91;
            this.label2.Text = "纸  张：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(360, 141);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(21, 14);
            this.label6.TabIndex = 115;
            this.label6.Text = "mm";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(359, 97);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(21, 14);
            this.label7.TabIndex = 116;
            this.label7.Text = "mm";
            // 
            // Form_PrintSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(506, 288);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.rbPortrait);
            this.Controls.Add(this.rbLanscape);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.tbMarginRight);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.tbMarginLeft);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.tbMarginBottom);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tbMarginTop);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cmbPrinter);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbPaperHeight);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbPaperWidth);
            this.Controls.Add(this.cmbPaper);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.btn_save);
            this.Name = "Form_PrintSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "打印设置";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.RadioButton rbPortrait;
        private System.Windows.Forms.RadioButton rbLanscape;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tbMarginRight;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbMarginLeft;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbMarginBottom;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbMarginTop;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbPrinter;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbPaperHeight;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbPaperWidth;
        private System.Windows.Forms.ComboBox cmbPaper;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
    }
}

