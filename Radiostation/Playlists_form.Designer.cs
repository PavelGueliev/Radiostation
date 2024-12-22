namespace Radiostation
{
    partial class Playlists_form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Playlists_form));
            this.dataGridViewPlaylists = new System.Windows.Forms.DataGridView();
            this.dataGridViewComposition = new System.Windows.Forms.DataGridView();
            this.addTrackButton = new System.Windows.Forms.Button();
            this.removeTrackButton = new System.Windows.Forms.Button();
            this.authorsComboBox = new System.Windows.Forms.ComboBox();
            this.titlesComboBox = new System.Windows.Forms.ComboBox();
            this.removePlaylistButton = new System.Windows.Forms.Button();
            this.addPlaylistButton = new System.Windows.Forms.Button();
            this.playlistTextBox = new System.Windows.Forms.TextBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.exit_button = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPlaylists)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewComposition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewPlaylists
            // 
            this.dataGridViewPlaylists.AllowUserToAddRows = false;
            this.dataGridViewPlaylists.AllowUserToDeleteRows = false;
            this.dataGridViewPlaylists.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewPlaylists.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPlaylists.Location = new System.Drawing.Point(10, 176);
            this.dataGridViewPlaylists.MultiSelect = false;
            this.dataGridViewPlaylists.Name = "dataGridViewPlaylists";
            this.dataGridViewPlaylists.Size = new System.Drawing.Size(760, 205);
            this.dataGridViewPlaylists.TabIndex = 18;
            this.dataGridViewPlaylists.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewPlaylists_CellBeginEdit);
            this.dataGridViewPlaylists.RowValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewPlaylists_RowValidated);
            this.dataGridViewPlaylists.SelectionChanged += new System.EventHandler(this.dataGridViewPlaylists_SelectionChanged);
            // 
            // dataGridViewComposition
            // 
            this.dataGridViewComposition.AllowUserToAddRows = false;
            this.dataGridViewComposition.AllowUserToDeleteRows = false;
            this.dataGridViewComposition.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewComposition.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewComposition.Location = new System.Drawing.Point(8, 527);
            this.dataGridViewComposition.MultiSelect = false;
            this.dataGridViewComposition.Name = "dataGridViewComposition";
            this.dataGridViewComposition.Size = new System.Drawing.Size(760, 205);
            this.dataGridViewComposition.TabIndex = 19;
            // 
            // addTrackButton
            // 
            this.addTrackButton.BackColor = System.Drawing.Color.Lime;
            this.addTrackButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addTrackButton.Location = new System.Drawing.Point(513, 469);
            this.addTrackButton.Name = "addTrackButton";
            this.addTrackButton.Size = new System.Drawing.Size(75, 23);
            this.addTrackButton.TabIndex = 27;
            this.addTrackButton.Text = "Добавить";
            this.addTrackButton.UseVisualStyleBackColor = false;
            this.addTrackButton.Click += new System.EventHandler(this.addTrackButton_Click);
            // 
            // removeTrackButton
            // 
            this.removeTrackButton.BackColor = System.Drawing.Color.Red;
            this.removeTrackButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.removeTrackButton.Location = new System.Drawing.Point(610, 469);
            this.removeTrackButton.Name = "removeTrackButton";
            this.removeTrackButton.Size = new System.Drawing.Size(75, 23);
            this.removeTrackButton.TabIndex = 28;
            this.removeTrackButton.Text = "Удалить";
            this.removeTrackButton.UseVisualStyleBackColor = false;
            this.removeTrackButton.Click += new System.EventHandler(this.removeTrackButton_Click);
            // 
            // authorsComboBox
            // 
            this.authorsComboBox.FormattingEnabled = true;
            this.authorsComboBox.Location = new System.Drawing.Point(126, 448);
            this.authorsComboBox.Name = "authorsComboBox";
            this.authorsComboBox.Size = new System.Drawing.Size(121, 21);
            this.authorsComboBox.TabIndex = 29;
            this.authorsComboBox.SelectedIndexChanged += new System.EventHandler(this.authorsComboBox_SelectedIndexChanged);
            // 
            // titlesComboBox
            // 
            this.titlesComboBox.FormattingEnabled = true;
            this.titlesComboBox.Location = new System.Drawing.Point(127, 488);
            this.titlesComboBox.Name = "titlesComboBox";
            this.titlesComboBox.Size = new System.Drawing.Size(121, 21);
            this.titlesComboBox.TabIndex = 30;
            this.titlesComboBox.SelectedIndexChanged += new System.EventHandler(this.titlesComboBox_SelectedIndexChanged);
            this.titlesComboBox.SelectedValueChanged += new System.EventHandler(this.titlesComboBox_SelectedValueChanged);
            // 
            // removePlaylistButton
            // 
            this.removePlaylistButton.BackColor = System.Drawing.Color.Red;
            this.removePlaylistButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.removePlaylistButton.Location = new System.Drawing.Point(610, 127);
            this.removePlaylistButton.Name = "removePlaylistButton";
            this.removePlaylistButton.Size = new System.Drawing.Size(75, 23);
            this.removePlaylistButton.TabIndex = 32;
            this.removePlaylistButton.Text = "Удалить";
            this.removePlaylistButton.UseVisualStyleBackColor = false;
            this.removePlaylistButton.Click += new System.EventHandler(this.removePlaylistButton_Click);
            // 
            // addPlaylistButton
            // 
            this.addPlaylistButton.BackColor = System.Drawing.Color.Lime;
            this.addPlaylistButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addPlaylistButton.Location = new System.Drawing.Point(513, 127);
            this.addPlaylistButton.Name = "addPlaylistButton";
            this.addPlaylistButton.Size = new System.Drawing.Size(75, 23);
            this.addPlaylistButton.TabIndex = 31;
            this.addPlaylistButton.Text = "Добавить";
            this.addPlaylistButton.UseVisualStyleBackColor = false;
            this.addPlaylistButton.Click += new System.EventHandler(this.addPlaylistButton_Click);
            // 
            // playlistTextBox
            // 
            this.playlistTextBox.Location = new System.Drawing.Point(144, 91);
            this.playlistTextBox.Name = "playlistTextBox";
            this.playlistTextBox.Size = new System.Drawing.Size(246, 20);
            this.playlistTextBox.TabIndex = 33;
            // 
            // searchButton
            // 
            this.searchButton.BackColor = System.Drawing.Color.Yellow;
            this.searchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchButton.Location = new System.Drawing.Point(411, 127);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(75, 23);
            this.searchButton.TabIndex = 34;
            this.searchButton.Text = "Найти";
            this.searchButton.UseVisualStyleBackColor = false;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 13);
            this.label1.TabIndex = 35;
            this.label1.Text = "Название плейлиста";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 456);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 36;
            this.label2.Text = "Автор";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 491);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 37;
            this.label3.Text = "Название песни";
            // 
            // exit_button
            // 
            this.exit_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.exit_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exit_button.Location = new System.Drawing.Point(708, 738);
            this.exit_button.Name = "exit_button";
            this.exit_button.Size = new System.Drawing.Size(75, 23);
            this.exit_button.TabIndex = 47;
            this.exit_button.Text = "Выход";
            this.exit_button.UseVisualStyleBackColor = false;
            this.exit_button.Click += new System.EventHandler(this.exit_button_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Enabled = false;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.ImageLocation = "";
            this.pictureBox1.Location = new System.Drawing.Point(-209, 211);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(557, 544);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 72;
            this.pictureBox1.TabStop = false;
            // 
            // Playlists_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SkyBlue;
            this.ClientSize = new System.Drawing.Size(780, 767);
            this.Controls.Add(this.exit_button);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.playlistTextBox);
            this.Controls.Add(this.removePlaylistButton);
            this.Controls.Add(this.addPlaylistButton);
            this.Controls.Add(this.titlesComboBox);
            this.Controls.Add(this.authorsComboBox);
            this.Controls.Add(this.removeTrackButton);
            this.Controls.Add(this.addTrackButton);
            this.Controls.Add(this.dataGridViewComposition);
            this.Controls.Add(this.dataGridViewPlaylists);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Playlists_form";
            this.Text = "Плейлисты";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPlaylists)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewComposition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewPlaylists;
        private System.Windows.Forms.DataGridView dataGridViewComposition;
        private System.Windows.Forms.Button addTrackButton;
        private System.Windows.Forms.Button removeTrackButton;
        private System.Windows.Forms.ComboBox authorsComboBox;
        private System.Windows.Forms.ComboBox titlesComboBox;
        private System.Windows.Forms.Button removePlaylistButton;
        private System.Windows.Forms.Button addPlaylistButton;
        private System.Windows.Forms.TextBox playlistTextBox;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button exit_button;
        public System.Windows.Forms.PictureBox pictureBox1;
    }
}