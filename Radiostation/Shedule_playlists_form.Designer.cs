namespace Radiostation
{
    partial class Shedule_playlists_form
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
            this.delete_button = new System.Windows.Forms.Button();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.exit_button = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.search_button = new System.Windows.Forms.Button();
            this.add_button = new System.Windows.Forms.Button();
            this.dataGridViewShedulePlaylist = new System.Windows.Forms.DataGridView();
            this.dataGridViewComposition = new System.Windows.Forms.DataGridView();
            this.playlistComboBox = new System.Windows.Forms.ComboBox();
            this.presenterComboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewShedulePlaylist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewComposition)).BeginInit();
            this.SuspendLayout();
            // 
            // delete_button
            // 
            this.delete_button.Location = new System.Drawing.Point(216, 169);
            this.delete_button.Name = "delete_button";
            this.delete_button.Size = new System.Drawing.Size(75, 23);
            this.delete_button.TabIndex = 27;
            this.delete_button.Text = "Удалить";
            this.delete_button.UseVisualStyleBackColor = true;
            this.delete_button.Click += new System.EventHandler(this.delete_button_Click);
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Checked = false;
            this.dateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePicker.Location = new System.Drawing.Point(153, 118);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.ShowCheckBox = true;
            this.dateTimePicker.Size = new System.Drawing.Size(137, 20);
            this.dateTimePicker.TabIndex = 26;
            // 
            // exit_button
            // 
            this.exit_button.Location = new System.Drawing.Point(695, 703);
            this.exit_button.Name = "exit_button";
            this.exit_button.Size = new System.Drawing.Size(75, 23);
            this.exit_button.TabIndex = 25;
            this.exit_button.Text = "Выход";
            this.exit_button.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(126, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "Дата воспроизведения";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Плейлист";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "ФИО";
            // 
            // search_button
            // 
            this.search_button.Location = new System.Drawing.Point(24, 169);
            this.search_button.Name = "search_button";
            this.search_button.Size = new System.Drawing.Size(75, 23);
            this.search_button.TabIndex = 21;
            this.search_button.Text = "Найти";
            this.search_button.UseVisualStyleBackColor = true;
            this.search_button.Click += new System.EventHandler(this.search_button_Click);
            // 
            // add_button
            // 
            this.add_button.Location = new System.Drawing.Point(120, 169);
            this.add_button.Name = "add_button";
            this.add_button.Size = new System.Drawing.Size(75, 23);
            this.add_button.TabIndex = 20;
            this.add_button.Text = "Добавить";
            this.add_button.UseVisualStyleBackColor = true;
            this.add_button.Click += new System.EventHandler(this.add_button_Click);
            // 
            // dataGridViewShedulePlaylist
            // 
            this.dataGridViewShedulePlaylist.AllowUserToAddRows = false;
            this.dataGridViewShedulePlaylist.AllowUserToDeleteRows = false;
            this.dataGridViewShedulePlaylist.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewShedulePlaylist.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewShedulePlaylist.Location = new System.Drawing.Point(10, 281);
            this.dataGridViewShedulePlaylist.MultiSelect = false;
            this.dataGridViewShedulePlaylist.Name = "dataGridViewShedulePlaylist";
            this.dataGridViewShedulePlaylist.Size = new System.Drawing.Size(760, 205);
            this.dataGridViewShedulePlaylist.TabIndex = 19;
            this.dataGridViewShedulePlaylist.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewShedulePlaylist_CellBeginEdit);
            this.dataGridViewShedulePlaylist.RowValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewShedulePlaylist_RowValidated);
            this.dataGridViewShedulePlaylist.SelectionChanged += new System.EventHandler(this.dataGridViewShedulePlaylist_SelectionChanged);
            // 
            // dataGridViewComposition
            // 
            this.dataGridViewComposition.AllowUserToAddRows = false;
            this.dataGridViewComposition.AllowUserToDeleteRows = false;
            this.dataGridViewComposition.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewComposition.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewComposition.Location = new System.Drawing.Point(10, 492);
            this.dataGridViewComposition.MultiSelect = false;
            this.dataGridViewComposition.Name = "dataGridViewComposition";
            this.dataGridViewComposition.Size = new System.Drawing.Size(760, 205);
            this.dataGridViewComposition.TabIndex = 28;
            // 
            // playlistComboBox
            // 
            this.playlistComboBox.FormattingEnabled = true;
            this.playlistComboBox.Location = new System.Drawing.Point(153, 76);
            this.playlistComboBox.Name = "playlistComboBox";
            this.playlistComboBox.Size = new System.Drawing.Size(266, 21);
            this.playlistComboBox.TabIndex = 41;
            // 
            // presenterComboBox
            // 
            this.presenterComboBox.FormattingEnabled = true;
            this.presenterComboBox.Location = new System.Drawing.Point(153, 36);
            this.presenterComboBox.Name = "presenterComboBox";
            this.presenterComboBox.Size = new System.Drawing.Size(266, 21);
            this.presenterComboBox.TabIndex = 40;
            // 
            // Shedule_playlists_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 730);
            this.Controls.Add(this.playlistComboBox);
            this.Controls.Add(this.presenterComboBox);
            this.Controls.Add(this.dataGridViewComposition);
            this.Controls.Add(this.delete_button);
            this.Controls.Add(this.dateTimePicker);
            this.Controls.Add(this.exit_button);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.search_button);
            this.Controls.Add(this.add_button);
            this.Controls.Add(this.dataGridViewShedulePlaylist);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Shedule_playlists_form";
            this.Text = "Расписание плейлистов";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewShedulePlaylist)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewComposition)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button delete_button;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private System.Windows.Forms.Button exit_button;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button search_button;
        private System.Windows.Forms.Button add_button;
        private System.Windows.Forms.DataGridView dataGridViewShedulePlaylist;
        private System.Windows.Forms.DataGridView dataGridViewComposition;
        private System.Windows.Forms.ComboBox playlistComboBox;
        private System.Windows.Forms.ComboBox presenterComboBox;
    }
}