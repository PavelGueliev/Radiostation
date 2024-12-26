namespace Radiostation
{
    partial class Shedule_Ads_And_Playlists
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Shedule_Ads_And_Playlists));
            this.exit_button = new System.Windows.Forms.Button();
            this.search_button = new System.Windows.Forms.Button();
            this.dataGridViewShedulePlaylistsAds = new System.Windows.Forms.DataGridView();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.presenterComboBox = new System.Windows.Forms.ComboBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewShedulePlaylistsAds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // exit_button
            // 
            this.exit_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.exit_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exit_button.Location = new System.Drawing.Point(698, 509);
            this.exit_button.Name = "exit_button";
            this.exit_button.Size = new System.Drawing.Size(75, 23);
            this.exit_button.TabIndex = 79;
            this.exit_button.Text = "Выход";
            this.exit_button.UseVisualStyleBackColor = false;
            this.exit_button.Click += new System.EventHandler(this.exit_button_Click);
            // 
            // search_button
            // 
            this.search_button.BackColor = System.Drawing.Color.Yellow;
            this.search_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.search_button.Location = new System.Drawing.Point(26, 172);
            this.search_button.Name = "search_button";
            this.search_button.Size = new System.Drawing.Size(75, 23);
            this.search_button.TabIndex = 77;
            this.search_button.Text = "Найти";
            this.search_button.UseVisualStyleBackColor = false;
            this.search_button.Click += new System.EventHandler(this.search_button_Click);
            // 
            // dataGridViewShedulePlaylistsAds
            // 
            this.dataGridViewShedulePlaylistsAds.AllowUserToAddRows = false;
            this.dataGridViewShedulePlaylistsAds.AllowUserToDeleteRows = false;
            this.dataGridViewShedulePlaylistsAds.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewShedulePlaylistsAds.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewShedulePlaylistsAds.Location = new System.Drawing.Point(12, 242);
            this.dataGridViewShedulePlaylistsAds.MultiSelect = false;
            this.dataGridViewShedulePlaylistsAds.Name = "dataGridViewShedulePlaylistsAds";
            this.dataGridViewShedulePlaylistsAds.Size = new System.Drawing.Size(760, 261);
            this.dataGridViewShedulePlaylistsAds.TabIndex = 75;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Enabled = false;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.ImageLocation = "";
            this.pictureBox1.Location = new System.Drawing.Point(-66, 184);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(414, 367);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 83;
            this.pictureBox1.TabStop = false;
            // 
            // presenterComboBox
            // 
            this.presenterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.presenterComboBox.FormattingEnabled = true;
            this.presenterComboBox.Location = new System.Drawing.Point(155, 48);
            this.presenterComboBox.Name = "presenterComboBox";
            this.presenterComboBox.Size = new System.Drawing.Size(266, 21);
            this.presenterComboBox.TabIndex = 88;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Checked = false;
            this.dateTimePicker1.Location = new System.Drawing.Point(155, 118);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.ShowCheckBox = true;
            this.dateTimePicker1.Size = new System.Drawing.Size(137, 20);
            this.dateTimePicker1.TabIndex = 87;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 118);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 13);
            this.label1.TabIndex = 86;
            this.label1.Text = "Дата воспроизведения";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 84;
            this.label4.Text = "ФИО";
            // 
            // Shedule_Ads_And_Playlists
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SkyBlue;
            this.ClientSize = new System.Drawing.Size(785, 539);
            this.Controls.Add(this.presenterComboBox);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.exit_button);
            this.Controls.Add(this.search_button);
            this.Controls.Add(this.dataGridViewShedulePlaylistsAds);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Shedule_Ads_And_Playlists";
            this.Text = "Просмотр расписания рекламных блоков и плейлистов";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewShedulePlaylistsAds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button exit_button;
        private System.Windows.Forms.Button search_button;
        private System.Windows.Forms.DataGridView dataGridViewShedulePlaylistsAds;
        public System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ComboBox presenterComboBox;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
    }
}