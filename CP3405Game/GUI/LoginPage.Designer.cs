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
            this.txt_RoomNumber = new System.Windows.Forms.TextBox();
            this.btn_New = new System.Windows.Forms.Button();
            this.btn_join = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txt_RoomNumber
            // 
            this.txt_RoomNumber.Font = new System.Drawing.Font("黑体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_RoomNumber.Location = new System.Drawing.Point(103, 31);
            this.txt_RoomNumber.Name = "txt_RoomNumber";
            this.txt_RoomNumber.Size = new System.Drawing.Size(391, 36);
            this.txt_RoomNumber.TabIndex = 0;
            // 
            // btn_New
            // 
            this.btn_New.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_New.Location = new System.Drawing.Point(103, 73);
            this.btn_New.Name = "btn_New";
            this.btn_New.Size = new System.Drawing.Size(192, 62);
            this.btn_New.TabIndex = 1;
            this.btn_New.Text = "New Room";
            this.btn_New.UseVisualStyleBackColor = true;
            this.btn_New.Click += new System.EventHandler(this.btn_New_Click);
            // 
            // btn_join
            // 
            this.btn_join.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_join.Location = new System.Drawing.Point(302, 73);
            this.btn_join.Name = "btn_join";
            this.btn_join.Size = new System.Drawing.Size(192, 62);
            this.btn_join.TabIndex = 2;
            this.btn_join.Text = "Join in Room";
            this.btn_join.UseVisualStyleBackColor = true;
            this.btn_join.Click += new System.EventHandler(this.btn_join_Click);
            // 
            // LoginPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 155);
            this.Controls.Add(this.btn_join);
            this.Controls.Add(this.btn_New);
            this.Controls.Add(this.txt_RoomNumber);
            this.Name = "LoginPage";
            this.Text = "LoginPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_RoomNumber;
        private System.Windows.Forms.Button btn_New;
        private System.Windows.Forms.Button btn_join;
    }
}