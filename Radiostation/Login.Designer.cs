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
            this.dj_button = new System.Windows.Forms.Button();
            this.manager_button = new System.Windows.Forms.Button();
            this.visitor_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // dj_button
            // 
            this.dj_button.Location = new System.Drawing.Point(363, 176);
            this.dj_button.Name = "dj_button";
            this.dj_button.Size = new System.Drawing.Size(75, 23);
            this.dj_button.TabIndex = 0;
            this.dj_button.Text = "DJ";
            this.dj_button.UseVisualStyleBackColor = true;
            this.dj_button.Click += new System.EventHandler(this.dj_button_Click);
            // 
            // manager_button
            // 
            this.manager_button.Location = new System.Drawing.Point(363, 214);
            this.manager_button.Name = "manager_button";
            this.manager_button.Size = new System.Drawing.Size(75, 23);
            this.manager_button.TabIndex = 1;
            this.manager_button.Text = "Manager";
            this.manager_button.UseVisualStyleBackColor = true;
            this.manager_button.Click += new System.EventHandler(this.manager_button_Click);
            // 
            // visitor_button
            // 
            this.visitor_button.Location = new System.Drawing.Point(363, 283);
            this.visitor_button.Name = "visitor_button";
            this.visitor_button.Size = new System.Drawing.Size(75, 23);
            this.visitor_button.TabIndex = 2;
            this.visitor_button.Text = "Гость";
            this.visitor_button.UseVisualStyleBackColor = true;
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.visitor_button);
            this.Controls.Add(this.manager_button);
            this.Controls.Add(this.dj_button);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Login";
            this.Text = "Login";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button dj_button;
        private System.Windows.Forms.Button manager_button;
        private System.Windows.Forms.Button visitor_button;
    }
}