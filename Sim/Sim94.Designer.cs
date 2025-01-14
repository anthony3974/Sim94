
namespace Sim
{
    partial class Sim94
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Sim94));
            this.btnStart = new System.Windows.Forms.Button();
            this.lbltitlelogo = new System.Windows.Forms.Label();
            this.picTitle = new System.Windows.Forms.PictureBox();
            this.tmrKeyTimer = new System.Windows.Forms.Timer(this.components);
            this.lblC = new System.Windows.Forms.Label();
            this.txtOpt = new System.Windows.Forms.TextBox();
            this.lblOpt = new System.Windows.Forms.Label();
            this.lbl3DN = new System.Windows.Forms.Label();
            this.lbl3DL = new System.Windows.Forms.Label();
            this.btn3DS = new System.Windows.Forms.Button();
            this.picText = new System.Windows.Forms.PictureBox();
            this.txt3DT = new System.Windows.Forms.TextBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btn3DL = new System.Windows.Forms.Button();
            this.eventLog1 = new System.Diagnostics.EventLog();
            ((System.ComponentModel.ISupportInitialize)(this.picTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.DarkTurquoise;
            this.btnStart.FlatAppearance.BorderColor = System.Drawing.Color.DodgerBlue;
            this.btnStart.FlatAppearance.BorderSize = 7;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Font = new System.Drawing.Font("Rockwell", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(273, 335);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(187, 73);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // lbltitlelogo
            // 
            this.lbltitlelogo.BackColor = System.Drawing.Color.GreenYellow;
            this.lbltitlelogo.Font = new System.Drawing.Font("Engravers MT", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltitlelogo.ForeColor = System.Drawing.Color.Red;
            this.lbltitlelogo.Location = new System.Drawing.Point(528, 146);
            this.lbltitlelogo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbltitlelogo.Name = "lbltitlelogo";
            this.lbltitlelogo.Size = new System.Drawing.Size(96, 58);
            this.lbltitlelogo.TabIndex = 2;
            this.lbltitlelogo.Text = "94";
            this.lbltitlelogo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // picTitle
            // 
            this.picTitle.Location = new System.Drawing.Point(0, 0);
            this.picTitle.Margin = new System.Windows.Forms.Padding(4);
            this.picTitle.Name = "picTitle";
            this.picTitle.Size = new System.Drawing.Size(733, 603);
            this.picTitle.TabIndex = 3;
            this.picTitle.TabStop = false;
            this.picTitle.Tag = "Dont not delete";
            // 
            // tmrKeyTimer
            // 
            this.tmrKeyTimer.Interval = 35;
            this.tmrKeyTimer.Tick += new System.EventHandler(this.TmrKeyTimer_Tick);
            // 
            // lblC
            // 
            this.lblC.AutoSize = true;
            this.lblC.BackColor = System.Drawing.Color.LimeGreen;
            this.lblC.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblC.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblC.Location = new System.Drawing.Point(308, 516);
            this.lblC.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblC.Name = "lblC";
            this.lblC.Size = new System.Drawing.Size(112, 25);
            this.lblC.TabIndex = 4;
            this.lblC.Text = "c - Controls";
            // 
            // txtOpt
            // 
            this.txtOpt.BackColor = System.Drawing.Color.PaleGreen;
            this.txtOpt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOpt.Location = new System.Drawing.Point(273, 298);
            this.txtOpt.Margin = new System.Windows.Forms.Padding(4);
            this.txtOpt.MaxLength = 15;
            this.txtOpt.Name = "txtOpt";
            this.txtOpt.Size = new System.Drawing.Size(186, 22);
            this.txtOpt.TabIndex = 5;
            this.txtOpt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtOpt_KeyDown);
            // 
            // lblOpt
            // 
            this.lblOpt.AutoSize = true;
            this.lblOpt.BackColor = System.Drawing.Color.LimeGreen;
            this.lblOpt.Font = new System.Drawing.Font("Modern No. 20", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOpt.Location = new System.Drawing.Point(235, 236);
            this.lblOpt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOpt.Name = "lblOpt";
            this.lblOpt.Size = new System.Drawing.Size(260, 25);
            this.lblOpt.TabIndex = 6;
            this.lblOpt.Text = "Enter Name (Optional)";
            // 
            // lbl3DN
            // 
            this.lbl3DN.BackColor = System.Drawing.Color.Green;
            this.lbl3DN.Font = new System.Drawing.Font("Modern No. 20", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl3DN.Location = new System.Drawing.Point(247, 246);
            this.lbl3DN.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl3DN.Name = "lbl3DN";
            this.lbl3DN.Size = new System.Drawing.Size(280, 26);
            this.lbl3DN.TabIndex = 7;
            // 
            // lbl3DL
            // 
            this.lbl3DL.BackColor = System.Drawing.Color.Yellow;
            this.lbl3DL.Font = new System.Drawing.Font("Engravers MT", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl3DL.ForeColor = System.Drawing.Color.Red;
            this.lbl3DL.Location = new System.Drawing.Point(539, 156);
            this.lbl3DL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl3DL.Name = "lbl3DL";
            this.lbl3DL.Size = new System.Drawing.Size(96, 58);
            this.lbl3DL.TabIndex = 8;
            this.lbl3DL.Text = "94";
            this.lbl3DL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btn3DS
            // 
            this.btn3DS.BackColor = System.Drawing.Color.DarkTurquoise;
            this.btn3DS.Enabled = false;
            this.btn3DS.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btn3DS.FlatAppearance.BorderSize = 8;
            this.btn3DS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn3DS.Font = new System.Drawing.Font("Rockwell", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn3DS.Location = new System.Drawing.Point(284, 345);
            this.btn3DS.Margin = new System.Windows.Forms.Padding(4);
            this.btn3DS.Name = "btn3DS";
            this.btn3DS.Size = new System.Drawing.Size(187, 73);
            this.btn3DS.TabIndex = 9;
            this.btn3DS.Text = "Start";
            this.btn3DS.UseVisualStyleBackColor = false;
            // 
            // picText
            // 
            this.picText.Image = ((System.Drawing.Image)(resources.GetObject("picText.Image")));
            this.picText.Location = new System.Drawing.Point(97, 85);
            this.picText.Margin = new System.Windows.Forms.Padding(4);
            this.picText.Name = "picText";
            this.picText.Size = new System.Drawing.Size(404, 47);
            this.picText.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picText.TabIndex = 10;
            this.picText.TabStop = false;
            // 
            // txt3DT
            // 
            this.txt3DT.BackColor = System.Drawing.Color.MediumAquamarine;
            this.txt3DT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt3DT.Enabled = false;
            this.txt3DT.Location = new System.Drawing.Point(281, 304);
            this.txt3DT.Margin = new System.Windows.Forms.Padding(4);
            this.txt3DT.MaxLength = 20;
            this.txt3DT.Name = "txt3DT";
            this.txt3DT.Size = new System.Drawing.Size(186, 22);
            this.txt3DT.TabIndex = 11;
            // 
            // btnLoad
            // 
            this.btnLoad.BackColor = System.Drawing.Color.DarkTurquoise;
            this.btnLoad.FlatAppearance.BorderColor = System.Drawing.Color.DodgerBlue;
            this.btnLoad.FlatAppearance.BorderSize = 7;
            this.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoad.Font = new System.Drawing.Font("Rockwell", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoad.Location = new System.Drawing.Point(273, 422);
            this.btnLoad.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(187, 73);
            this.btnLoad.TabIndex = 12;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = false;
            this.btnLoad.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // btn3DL
            // 
            this.btn3DL.BackColor = System.Drawing.Color.DarkTurquoise;
            this.btn3DL.Enabled = false;
            this.btn3DL.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btn3DL.FlatAppearance.BorderSize = 8;
            this.btn3DL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn3DL.Font = new System.Drawing.Font("Rockwell", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn3DL.Location = new System.Drawing.Point(284, 432);
            this.btn3DL.Margin = new System.Windows.Forms.Padding(4);
            this.btn3DL.Name = "btn3DL";
            this.btn3DL.Size = new System.Drawing.Size(187, 73);
            this.btn3DL.TabIndex = 13;
            this.btn3DL.Text = "Start";
            this.btn3DL.UseVisualStyleBackColor = false;
            // 
            // eventLog1
            // 
            this.eventLog1.SynchronizingObject = this;
            // 
            // Sim94
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.ForestGreen;
            this.ClientSize = new System.Drawing.Size(733, 603);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.btn3DL);
            this.Controls.Add(this.txtOpt);
            this.Controls.Add(this.txt3DT);
            this.Controls.Add(this.picText);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btn3DS);
            this.Controls.Add(this.lbltitlelogo);
            this.Controls.Add(this.lbl3DL);
            this.Controls.Add(this.lblOpt);
            this.Controls.Add(this.lbl3DN);
            this.Controls.Add(this.lblC);
            this.Controls.Add(this.picTitle);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Sim94";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Farming Sim 94";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Sim94_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.picTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lbltitlelogo;
        private System.Windows.Forms.PictureBox picTitle;
        private System.Windows.Forms.Timer tmrKeyTimer;
        private System.Windows.Forms.Label lblC;
        private System.Windows.Forms.TextBox txtOpt;
        private System.Windows.Forms.Label lblOpt;
        private System.Windows.Forms.Label lbl3DN;
        private System.Windows.Forms.Label lbl3DL;
        private System.Windows.Forms.Button btn3DS;
        private System.Windows.Forms.PictureBox picText;
        private System.Windows.Forms.TextBox txt3DT;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btn3DL;
        private System.Diagnostics.EventLog eventLog1;
    }
}

