namespace Radiostation
{
    partial class Ad_record_form
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
            this.exit_button = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.Second_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.Minut_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.Hours_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.textBoxTitle = new System.Windows.Forms.TextBox();
            this.delete_button = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.search_button = new System.Windows.Forms.Button();
            this.add_button = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.Second_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Minut_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Hours_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // exit_button
            // 
            this.exit_button.Location = new System.Drawing.Point(691, 503);
            this.exit_button.Name = "exit_button";
            this.exit_button.Size = new System.Drawing.Size(75, 23);
            this.exit_button.TabIndex = 55;
            this.exit_button.Text = "Выход";
            this.exit_button.UseVisualStyleBackColor = true;
            this.exit_button.Click += new System.EventHandler(this.exit_button_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(341, 123);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 13);
            this.label7.TabIndex = 54;
            this.label7.Text = "Секунд";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(252, 120);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 13);
            this.label6.TabIndex = 53;
            this.label6.Text = "Минут";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(169, 118);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 52;
            this.label5.Text = "Часов";
            // 
            // Second_numericUpDown
            // 
            this.Second_numericUpDown.Location = new System.Drawing.Point(296, 116);
            this.Second_numericUpDown.Name = "Second_numericUpDown";
            this.Second_numericUpDown.Size = new System.Drawing.Size(39, 20);
            this.Second_numericUpDown.TabIndex = 49;
            // 
            // Minut_numericUpDown
            // 
            this.Minut_numericUpDown.Location = new System.Drawing.Point(212, 113);
            this.Minut_numericUpDown.Name = "Minut_numericUpDown";
            this.Minut_numericUpDown.Size = new System.Drawing.Size(34, 20);
            this.Minut_numericUpDown.TabIndex = 48;
            // 
            // Hours_numericUpDown
            // 
            this.Hours_numericUpDown.Location = new System.Drawing.Point(118, 113);
            this.Hours_numericUpDown.Name = "Hours_numericUpDown";
            this.Hours_numericUpDown.Size = new System.Drawing.Size(45, 20);
            this.Hours_numericUpDown.TabIndex = 47;
            // 
            // textBoxTitle
            // 
            this.textBoxTitle.Location = new System.Drawing.Point(118, 73);
            this.textBoxTitle.Name = "textBoxTitle";
            this.textBoxTitle.Size = new System.Drawing.Size(121, 20);
            this.textBoxTitle.TabIndex = 46;
            // 
            // delete_button
            // 
            this.delete_button.Location = new System.Drawing.Point(212, 255);
            this.delete_button.Name = "delete_button";
            this.delete_button.Size = new System.Drawing.Size(75, 23);
            this.delete_button.TabIndex = 44;
            this.delete_button.Text = "Удалить";
            this.delete_button.UseVisualStyleBackColor = true;
            this.delete_button.Click += new System.EventHandler(this.delete_button_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 43;
            this.label3.Text = "Длительность";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 41;
            this.label1.Text = "Название";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // search_button
            // 
            this.search_button.Location = new System.Drawing.Point(20, 255);
            this.search_button.Name = "search_button";
            this.search_button.Size = new System.Drawing.Size(75, 23);
            this.search_button.TabIndex = 40;
            this.search_button.Text = "Найти";
            this.search_button.UseVisualStyleBackColor = true;
            this.search_button.Click += new System.EventHandler(this.search_button_Click);
            // 
            // add_button
            // 
            this.add_button.Location = new System.Drawing.Point(116, 255);
            this.add_button.Name = "add_button";
            this.add_button.Size = new System.Drawing.Size(75, 23);
            this.add_button.TabIndex = 39;
            this.add_button.Text = "Добавить";
            this.add_button.UseVisualStyleBackColor = true;
            this.add_button.Click += new System.EventHandler(this.add_button_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(10, 284);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(760, 205);
            this.dataGridView1.TabIndex = 38;
            this.dataGridView1.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView1_CellBeginEdit);
            this.dataGridView1.RowValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_RowValidated);
            // 
            // Ad_record_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 557);
            this.Controls.Add(this.exit_button);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Second_numericUpDown);
            this.Controls.Add(this.Minut_numericUpDown);
            this.Controls.Add(this.Hours_numericUpDown);
            this.Controls.Add(this.textBoxTitle);
            this.Controls.Add(this.delete_button);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.search_button);
            this.Controls.Add(this.add_button);
            this.Controls.Add(this.dataGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Ad_record_form";
            this.Text = "Реклама";
            ((System.ComponentModel.ISupportInitialize)(this.Second_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Minut_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Hours_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button exit_button;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown Second_numericUpDown;
        private System.Windows.Forms.NumericUpDown Minut_numericUpDown;
        private System.Windows.Forms.NumericUpDown Hours_numericUpDown;
        private System.Windows.Forms.TextBox textBoxTitle;
        private System.Windows.Forms.Button delete_button;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button search_button;
        private System.Windows.Forms.Button add_button;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}