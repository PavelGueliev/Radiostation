namespace Radiostation
{
    partial class Shedule_ads_block_form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Shedule_ads_block_form));
            this.delete_button = new System.Windows.Forms.Button();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.exit_button = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.search_button = new System.Windows.Forms.Button();
            this.add_button = new System.Windows.Forms.Button();
            this.dataGridViewSheduleBlock = new System.Windows.Forms.DataGridView();
            this.dataGridViewComposition = new System.Windows.Forms.DataGridView();
            this.presenterComboBox = new System.Windows.Forms.ComboBox();
            this.adBlockComboBox = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSheduleBlock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewComposition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // delete_button
            // 
            this.delete_button.BackColor = System.Drawing.Color.Red;
            this.delete_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.delete_button.Location = new System.Drawing.Point(215, 169);
            this.delete_button.Name = "delete_button";
            this.delete_button.Size = new System.Drawing.Size(75, 23);
            this.delete_button.TabIndex = 27;
            this.delete_button.Text = "Удалить";
            this.delete_button.UseVisualStyleBackColor = false;
            this.delete_button.Click += new System.EventHandler(this.delete_button_Click);
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Checked = false;
            this.dateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePicker.Location = new System.Drawing.Point(153, 105);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.ShowCheckBox = true;
            this.dateTimePicker.Size = new System.Drawing.Size(137, 20);
            this.dateTimePicker.TabIndex = 26;
            // 
            // exit_button
            // 
            this.exit_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.exit_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exit_button.Location = new System.Drawing.Point(695, 715);
            this.exit_button.Name = "exit_button";
            this.exit_button.Size = new System.Drawing.Size(75, 23);
            this.exit_button.TabIndex = 25;
            this.exit_button.Text = "Выход";
            this.exit_button.UseVisualStyleBackColor = false;
            this.exit_button.Click += new System.EventHandler(this.exit_button_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(126, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "Дата воспроизведения";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Рекламный блок";
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
            this.search_button.BackColor = System.Drawing.Color.Yellow;
            this.search_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.search_button.Location = new System.Drawing.Point(24, 169);
            this.search_button.Name = "search_button";
            this.search_button.Size = new System.Drawing.Size(75, 23);
            this.search_button.TabIndex = 21;
            this.search_button.Text = "Найти";
            this.search_button.UseVisualStyleBackColor = false;
            this.search_button.Click += new System.EventHandler(this.search_button_Click);
            // 
            // add_button
            // 
            this.add_button.BackColor = System.Drawing.Color.Lime;
            this.add_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.add_button.Location = new System.Drawing.Point(120, 169);
            this.add_button.Name = "add_button";
            this.add_button.Size = new System.Drawing.Size(75, 23);
            this.add_button.TabIndex = 20;
            this.add_button.Text = "Добавить";
            this.add_button.UseVisualStyleBackColor = false;
            this.add_button.Click += new System.EventHandler(this.add_button_Click);
            // 
            // dataGridViewSheduleBlock
            // 
            this.dataGridViewSheduleBlock.AllowUserToAddRows = false;
            this.dataGridViewSheduleBlock.AllowUserToDeleteRows = false;
            this.dataGridViewSheduleBlock.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewSheduleBlock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSheduleBlock.Location = new System.Drawing.Point(10, 281);
            this.dataGridViewSheduleBlock.MultiSelect = false;
            this.dataGridViewSheduleBlock.Name = "dataGridViewSheduleBlock";
            this.dataGridViewSheduleBlock.Size = new System.Drawing.Size(760, 205);
            this.dataGridViewSheduleBlock.TabIndex = 19;
            this.dataGridViewSheduleBlock.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewSheduleBlock_CellBeginEdit);
            this.dataGridViewSheduleBlock.RowValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewSheduleBlock_RowValidated);
            this.dataGridViewSheduleBlock.SelectionChanged += new System.EventHandler(this.dataGridViewSheduleBlock_SelectionChanged);
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
            this.dataGridViewComposition.TabIndex = 37;
            // 
            // presenterComboBox
            // 
            this.presenterComboBox.FormattingEnabled = true;
            this.presenterComboBox.Location = new System.Drawing.Point(153, 35);
            this.presenterComboBox.Name = "presenterComboBox";
            this.presenterComboBox.Size = new System.Drawing.Size(266, 21);
            this.presenterComboBox.TabIndex = 38;
            // 
            // adBlockComboBox
            // 
            this.adBlockComboBox.FormattingEnabled = true;
            this.adBlockComboBox.Location = new System.Drawing.Point(153, 75);
            this.adBlockComboBox.Name = "adBlockComboBox";
            this.adBlockComboBox.Size = new System.Drawing.Size(266, 21);
            this.adBlockComboBox.TabIndex = 39;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Enabled = false;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.ImageLocation = "";
            this.pictureBox1.Location = new System.Drawing.Point(-111, 388);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(414, 367);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 74;
            this.pictureBox1.TabStop = false;
            // 
            // Shedule_ads_block_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SkyBlue;
            this.ClientSize = new System.Drawing.Size(780, 743);
            this.Controls.Add(this.adBlockComboBox);
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
            this.Controls.Add(this.dataGridViewSheduleBlock);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Shedule_ads_block_form";
            this.Text = "Расписание рекламных блоков";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSheduleBlock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewComposition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
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
        private System.Windows.Forms.DataGridView dataGridViewSheduleBlock;
        private System.Windows.Forms.DataGridView dataGridViewComposition;
        private System.Windows.Forms.ComboBox presenterComboBox;
        private System.Windows.Forms.ComboBox adBlockComboBox;
        public System.Windows.Forms.PictureBox pictureBox1;
    }
}