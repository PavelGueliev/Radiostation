namespace Radiostation
{
    partial class FormDJ
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.playlists_form_button = new System.Windows.Forms.Button();
            this.ads_block_form_button = new System.Windows.Forms.Button();
            this.Track_form_button = new System.Windows.Forms.Button();
            this.shedule_playlists_form_button = new System.Windows.Forms.Button();
            this.exit_button = new System.Windows.Forms.Button();
            this.shedule_ads_form_button = new System.Windows.Forms.Button();
            this.ads_form_button = new System.Windows.Forms.Button();
            this.genre_form_button = new System.Windows.Forms.Button();
            this.online_event_form_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // playlists_form_button
            // 
            this.playlists_form_button.Location = new System.Drawing.Point(223, 215);
            this.playlists_form_button.Name = "playlists_form_button";
            this.playlists_form_button.Size = new System.Drawing.Size(348, 41);
            this.playlists_form_button.TabIndex = 0;
            this.playlists_form_button.Text = "Плейлисты";
            this.playlists_form_button.UseVisualStyleBackColor = true;
            this.playlists_form_button.Click += new System.EventHandler(this.playlists_form_button_Click);
            // 
            // ads_block_form_button
            // 
            this.ads_block_form_button.Location = new System.Drawing.Point(223, 262);
            this.ads_block_form_button.Name = "ads_block_form_button";
            this.ads_block_form_button.Size = new System.Drawing.Size(348, 43);
            this.ads_block_form_button.TabIndex = 1;
            this.ads_block_form_button.Text = "Рекламные блоки";
            this.ads_block_form_button.UseVisualStyleBackColor = true;
            this.ads_block_form_button.Click += new System.EventHandler(this.ads_block_form_button_Click);
            // 
            // Track_form_button
            // 
            this.Track_form_button.Location = new System.Drawing.Point(223, 64);
            this.Track_form_button.Name = "Track_form_button";
            this.Track_form_button.Size = new System.Drawing.Size(348, 45);
            this.Track_form_button.TabIndex = 2;
            this.Track_form_button.Text = "Треки";
            this.Track_form_button.UseVisualStyleBackColor = true;
            this.Track_form_button.Click += new System.EventHandler(this.Track_form_button_Click);
            // 
            // shedule_playlists_form_button
            // 
            this.shedule_playlists_form_button.Location = new System.Drawing.Point(223, 311);
            this.shedule_playlists_form_button.Name = "shedule_playlists_form_button";
            this.shedule_playlists_form_button.Size = new System.Drawing.Size(348, 47);
            this.shedule_playlists_form_button.TabIndex = 3;
            this.shedule_playlists_form_button.Text = "Расписание плейлистов";
            this.shedule_playlists_form_button.UseVisualStyleBackColor = true;
            this.shedule_playlists_form_button.Click += new System.EventHandler(this.shedule_playlists_form_button_Click);
            // 
            // exit_button
            // 
            this.exit_button.Location = new System.Drawing.Point(646, 456);
            this.exit_button.Name = "exit_button";
            this.exit_button.Size = new System.Drawing.Size(104, 65);
            this.exit_button.TabIndex = 4;
            this.exit_button.Text = "Выход";
            this.exit_button.UseVisualStyleBackColor = true;
            this.exit_button.Click += new System.EventHandler(this.exit_button_Click);
            // 
            // shedule_ads_form_button
            // 
            this.shedule_ads_form_button.Location = new System.Drawing.Point(223, 364);
            this.shedule_ads_form_button.Name = "shedule_ads_form_button";
            this.shedule_ads_form_button.Size = new System.Drawing.Size(348, 50);
            this.shedule_ads_form_button.TabIndex = 5;
            this.shedule_ads_form_button.Text = "Расписание рекламы";
            this.shedule_ads_form_button.UseVisualStyleBackColor = true;
            this.shedule_ads_form_button.Click += new System.EventHandler(this.shedule_ads_form_button_Click);
            // 
            // ads_form_button
            // 
            this.ads_form_button.Location = new System.Drawing.Point(223, 166);
            this.ads_form_button.Name = "ads_form_button";
            this.ads_form_button.Size = new System.Drawing.Size(348, 43);
            this.ads_form_button.TabIndex = 6;
            this.ads_form_button.Text = "Реклама";
            this.ads_form_button.UseVisualStyleBackColor = true;
            this.ads_form_button.Click += new System.EventHandler(this.ads_form_button_Click);
            // 
            // genre_form_button
            // 
            this.genre_form_button.Location = new System.Drawing.Point(223, 115);
            this.genre_form_button.Name = "genre_form_button";
            this.genre_form_button.Size = new System.Drawing.Size(348, 45);
            this.genre_form_button.TabIndex = 7;
            this.genre_form_button.Text = "Жанр";
            this.genre_form_button.UseVisualStyleBackColor = true;
            this.genre_form_button.Click += new System.EventHandler(this.genre_form_button_Click);
            // 
            // online_event_form_button
            // 
            this.online_event_form_button.Location = new System.Drawing.Point(223, 420);
            this.online_event_form_button.Name = "online_event_form_button";
            this.online_event_form_button.Size = new System.Drawing.Size(348, 50);
            this.online_event_form_button.TabIndex = 8;
            this.online_event_form_button.Text = "Эфирные события";
            this.online_event_form_button.UseVisualStyleBackColor = true;
            this.online_event_form_button.Click += new System.EventHandler(this.online_event_form_button_Click);
            // 
            // FormDJ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.online_event_form_button);
            this.Controls.Add(this.genre_form_button);
            this.Controls.Add(this.ads_form_button);
            this.Controls.Add(this.shedule_ads_form_button);
            this.Controls.Add(this.exit_button);
            this.Controls.Add(this.shedule_playlists_form_button);
            this.Controls.Add(this.Track_form_button);
            this.Controls.Add(this.ads_block_form_button);
            this.Controls.Add(this.playlists_form_button);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormDJ";
            this.Text = "Главное меню";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button playlists_form_button;
        private System.Windows.Forms.Button ads_block_form_button;
        private System.Windows.Forms.Button Track_form_button;
        private System.Windows.Forms.Button shedule_playlists_form_button;
        private System.Windows.Forms.Button exit_button;
        private System.Windows.Forms.Button shedule_ads_form_button;
        private System.Windows.Forms.Button ads_form_button;
        private System.Windows.Forms.Button genre_form_button;
        private System.Windows.Forms.Button online_event_form_button;
    }
}

