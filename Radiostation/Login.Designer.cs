namespace Radiostation
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.dj_button = new System.Windows.Forms.Button();
            this.manager_button = new System.Windows.Forms.Button();
            this.visitor_button = new System.Windows.Forms.Button();
            this.adBlockTextBox = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // dj_button
            // 
            this.dj_button.BackColor = System.Drawing.Color.Blue;
            this.dj_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dj_button.Location = new System.Drawing.Point(363, 176);
            this.dj_button.Name = "dj_button";
            this.dj_button.Size = new System.Drawing.Size(75, 23);
            this.dj_button.TabIndex = 0;
            this.dj_button.Text = "DJ";
            this.dj_button.UseVisualStyleBackColor = false;
            this.dj_button.Click += new System.EventHandler(this.dj_button_Click);
            // 
            // manager_button
            // 
            this.manager_button.BackColor = System.Drawing.Color.Green;
            this.manager_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.manager_button.Location = new System.Drawing.Point(363, 214);
            this.manager_button.Name = "manager_button";
            this.manager_button.Size = new System.Drawing.Size(75, 23);
            this.manager_button.TabIndex = 1;
            this.manager_button.Text = "Manager";
            this.manager_button.UseVisualStyleBackColor = false;
            this.manager_button.Click += new System.EventHandler(this.manager_button_Click);
            // 
            // visitor_button
            // 
            this.visitor_button.BackColor = System.Drawing.Color.Cyan;
            this.visitor_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.visitor_button.Location = new System.Drawing.Point(363, 283);
            this.visitor_button.Name = "visitor_button";
            this.visitor_button.Size = new System.Drawing.Size(75, 23);
            this.visitor_button.TabIndex = 2;
            this.visitor_button.Text = "Гость";
            this.visitor_button.UseVisualStyleBackColor = false;
            this.visitor_button.Click += new System.EventHandler(this.visitor_button_Click);
            // 
            // adBlockTextBox
            // 
            this.adBlockTextBox.Location = new System.Drawing.Point(274, 84);
            this.adBlockTextBox.Name = "adBlockTextBox";
            this.adBlockTextBox.Size = new System.Drawing.Size(246, 20);
            this.adBlockTextBox.TabIndex = 44;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(274, 141);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(246, 20);
            this.textBox1.TabIndex = 45;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Impact", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(365, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 29);
            this.label1.TabIndex = 46;
            this.label1.Text = "Логин";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Impact", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(358, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 29);
            this.label2.TabIndex = 47;
            this.label2.Text = "Пароль";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Enabled = false;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.ImageLocation = "";
            this.pictureBox1.Location = new System.Drawing.Point(25, 197);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(263, 241);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 48;
            this.pictureBox1.TabStop = false;
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SkyBlue;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.adBlockTextBox);
            this.Controls.Add(this.visitor_button);
            this.Controls.Add(this.manager_button);
            this.Controls.Add(this.dj_button);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Login";
            this.Text = "Вход";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button dj_button;
        private System.Windows.Forms.Button manager_button;
        private System.Windows.Forms.Button visitor_button;
        private System.Windows.Forms.TextBox adBlockTextBox;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.PictureBox pictureBox1;
    }
}