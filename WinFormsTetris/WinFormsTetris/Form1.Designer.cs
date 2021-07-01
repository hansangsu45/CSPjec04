
using System;

namespace WinFormsTetris
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.ScoreTxt = new System.Windows.Forms.Label();
            this.BestTxt = new System.Windows.Forms.Label();
            this.DateTxt = new System.Windows.Forms.Label();
            this.ReTxt = new System.Windows.Forms.Label();
            this.RewindTxt = new System.Windows.Forms.Label();
            this.RewCnt = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ScoreTxt
            // 
            this.ScoreTxt.AutoSize = true;
            this.ScoreTxt.Font = new System.Drawing.Font("문체부 돋음체", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.ScoreTxt.Location = new System.Drawing.Point(103, 83);
            this.ScoreTxt.Name = "ScoreTxt";
            this.ScoreTxt.Size = new System.Drawing.Size(47, 23);
            this.ScoreTxt.TabIndex = 0;
            this.ScoreTxt.Text = "Test";
            this.ScoreTxt.Click += new System.EventHandler(this.ScoreTxt_Click);
            // 
            // BestTxt
            // 
            this.BestTxt.AutoSize = true;
            this.BestTxt.Font = new System.Drawing.Font("문체부 돋음체", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.BestTxt.Location = new System.Drawing.Point(103, 48);
            this.BestTxt.Name = "BestTxt";
            this.BestTxt.Size = new System.Drawing.Size(47, 23);
            this.BestTxt.TabIndex = 0;
            this.BestTxt.Text = "Test";
            this.BestTxt.Click += new System.EventHandler(this.ScoreTxt_Click);
            // 
            // DateTxt
            // 
            this.DateTxt.AutoSize = true;
            this.DateTxt.Font = new System.Drawing.Font("문체부 돋음체", 10.2F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.DateTxt.Location = new System.Drawing.Point(208, 83);
            this.DateTxt.Name = "DateTxt";
            this.DateTxt.Size = new System.Drawing.Size(8, 34);
            this.DateTxt.TabIndex = 0;
            this.DateTxt.Text = "\r\n\r\n";
            this.DateTxt.Click += new System.EventHandler(this.ScoreTxt_Click);
            // 
            // ReTxt
            // 
            this.ReTxt.AutoSize = true;
            this.ReTxt.Font = new System.Drawing.Font("문체부 돋음체", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.ReTxt.ForeColor = System.Drawing.Color.Coral;
            this.ReTxt.Location = new System.Drawing.Point(421, 187);
            this.ReTxt.Name = "ReTxt";
            this.ReTxt.Size = new System.Drawing.Size(129, 23);
            this.ReTxt.TabIndex = 0;
            this.ReTxt.Text = "다시하기(R)";
            this.ReTxt.Click += new System.EventHandler(this.ScoreTxt_Click);
            // 
            // RewindTxt
            // 
            this.RewindTxt.AutoSize = true;
            this.RewindTxt.Font = new System.Drawing.Font("문체부 돋음체", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.RewindTxt.ForeColor = System.Drawing.Color.DarkCyan;
            this.RewindTxt.Location = new System.Drawing.Point(336, 237);
            this.RewindTxt.Name = "RewindTxt";
            this.RewindTxt.Size = new System.Drawing.Size(130, 23);
            this.RewindTxt.TabIndex = 0;
            this.RewindTxt.Text = "되돌리기(G)";
            this.RewindTxt.Click += new System.EventHandler(this.ScoreTxt_Click);
            // 
            // RewCnt
            // 
            this.RewCnt.AutoSize = true;
            this.RewCnt.Font = new System.Drawing.Font("문체부 돋음체", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.RewCnt.ForeColor = System.Drawing.Color.DarkCyan;
            this.RewCnt.Location = new System.Drawing.Point(335, 214);
            this.RewCnt.Name = "RewCnt";
            this.RewCnt.Size = new System.Drawing.Size(84, 15);
            this.RewCnt.TabIndex = 0;
            this.RewCnt.Text = "되돌리기(G)";
            this.RewCnt.Click += new System.EventHandler(this.ScoreTxt_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.RewCnt);
            this.Controls.Add(this.RewindTxt);
            this.Controls.Add(this.ReTxt);
            this.Controls.Add(this.DateTxt);
            this.Controls.Add(this.BestTxt);
            this.Controls.Add(this.ScoreTxt);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void ScoreTxt_Click(object sender, EventArgs e)
        {
           
        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label ScoreTxt;
        private System.Windows.Forms.Label BestTxt;
        private System.Windows.Forms.Label DateTxt;
        private System.Windows.Forms.Label ReTxt;
        private System.Windows.Forms.Label RewindTxt;
        private System.Windows.Forms.Label RewCnt;
    }
}

