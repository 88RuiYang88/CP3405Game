namespace CP3405Game.GUI
{
    partial class LoginPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginPage));
            this.txt_RoomNumber = new System.Windows.Forms.TextBox();
            this.btn_New = new System.Windows.Forms.Button();
            this.btn_join = new System.Windows.Forms.Button();
            this.pic2 = new System.Windows.Forms.PictureBox();
            this.pic4 = new System.Windows.Forms.PictureBox();
            this.pic3 = new System.Windows.Forms.PictureBox();
            this.pic1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pic2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic1)).BeginInit();
            this.SuspendLayout();
            // 
            // txt_RoomNumber
            // 
            this.txt_RoomNumber.Font = new System.Drawing.Font("黑体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_RoomNumber.Location = new System.Drawing.Point(59, 12);
            this.txt_RoomNumber.Name = "txt_RoomNumber";
            this.txt_RoomNumber.Size = new System.Drawing.Size(391, 36);
            this.txt_RoomNumber.TabIndex = 0;
            // 
            // btn_New
            // 
            this.btn_New.BackColor = System.Drawing.Color.Transparent;
            this.btn_New.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_New.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_New.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_New.Location = new System.Drawing.Point(62, 510);
            this.btn_New.Name = "btn_New";
            this.btn_New.Size = new System.Drawing.Size(192, 62);
            this.btn_New.TabIndex = 1;
            this.btn_New.UseVisualStyleBackColor = false;
            this.btn_New.Click += new System.EventHandler(this.btn_New_Click);
            this.btn_New.MouseEnter += new System.EventHandler(this.btn_New_MouseEnter);
            this.btn_New.MouseLeave += new System.EventHandler(this.btn_New_MouseLeave);
            // 
            // btn_join
            // 
            this.btn_join.BackColor = System.Drawing.Color.Transparent;
            this.btn_join.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_join.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_join.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_join.Location = new System.Drawing.Point(258, 510);
            this.btn_join.Name = "btn_join";
            this.btn_join.Size = new System.Drawing.Size(192, 62);
            this.btn_join.TabIndex = 2;
            this.btn_join.UseVisualStyleBackColor = false;
            this.btn_join.Click += new System.EventHandler(this.btn_join_Click);
            this.btn_join.MouseEnter += new System.EventHandler(this.btn_join_MouseEnter);
            this.btn_join.MouseLeave += new System.EventHandler(this.btn_join_MouseLeave);
            // 
            // pic2
            // 
            this.pic2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pic2.Image = global::CP3405Game.Properties.Resources._2;
            this.pic2.Location = new System.Drawing.Point(58, 173);
            this.pic2.Name = "pic2";
            this.pic2.Size = new System.Drawing.Size(390, 100);
            this.pic2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic2.TabIndex = 6;
            this.pic2.TabStop = false;
            this.pic2.Click += new System.EventHandler(this.pic2_Click);
            this.pic2.Paint += new System.Windows.Forms.PaintEventHandler(this.pic2_Paint);
            // 
            // pic4
            // 
            this.pic4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pic4.Image = global::CP3405Game.Properties.Resources._4;
            this.pic4.Location = new System.Drawing.Point(58, 385);
            this.pic4.Name = "pic4";
            this.pic4.Size = new System.Drawing.Size(390, 100);
            this.pic4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic4.TabIndex = 5;
            this.pic4.TabStop = false;
            this.pic4.Click += new System.EventHandler(this.pic4_Click);
            this.pic4.Paint += new System.Windows.Forms.PaintEventHandler(this.pic4_Paint);
            // 
            // pic3
            // 
            this.pic3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pic3.Image = global::CP3405Game.Properties.Resources._3;
            this.pic3.Location = new System.Drawing.Point(58, 279);
            this.pic3.Name = "pic3";
            this.pic3.Size = new System.Drawing.Size(390, 100);
            this.pic3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic3.TabIndex = 4;
            this.pic3.TabStop = false;
            this.pic3.Click += new System.EventHandler(this.pic3_Click);
            this.pic3.Paint += new System.Windows.Forms.PaintEventHandler(this.pic3_Paint);
            // 
            // pic1
            // 
            this.pic1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pic1.Image = global::CP3405Game.Properties.Resources._1;
            this.pic1.Location = new System.Drawing.Point(60, 67);
            this.pic1.Name = "pic1";
            this.pic1.Size = new System.Drawing.Size(390, 100);
            this.pic1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic1.TabIndex = 3;
            this.pic1.TabStop = false;
            this.pic1.Click += new System.EventHandler(this.pic1_Click);
            this.pic1.Paint += new System.Windows.Forms.PaintEventHandler(this.pic1_Paint);
            // 
            // LoginPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(515, 608);
            this.Controls.Add(this.pic2);
            this.Controls.Add(this.pic4);
            this.Controls.Add(this.pic3);
            this.Controls.Add(this.pic1);
            this.Controls.Add(this.btn_join);
            this.Controls.Add(this.btn_New);
            this.Controls.Add(this.txt_RoomNumber);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LoginPage";
            this.Text = "LoginPage";
            this.Load += new System.EventHandler(this.LoginPage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pic2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_RoomNumber;
        private System.Windows.Forms.Button btn_New;
        private System.Windows.Forms.Button btn_join;
        private System.Windows.Forms.PictureBox pic1;
        private System.Windows.Forms.PictureBox pic3;
        private System.Windows.Forms.PictureBox pic4;
        private System.Windows.Forms.PictureBox pic2;
    }
}