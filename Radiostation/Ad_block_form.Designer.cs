﻿namespace Radiostation
{
    partial class Ad_block_form
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
            this.searchButton = new System.Windows.Forms.Button();
            this.adBlockTextBox = new System.Windows.Forms.TextBox();
            this.removeAdBlockButton = new System.Windows.Forms.Button();
            this.addAdBlockButton = new System.Windows.Forms.Button();
            this.authorsComboBox = new System.Windows.Forms.ComboBox();
            this.removeAdButton = new System.Windows.Forms.Button();
            this.addAdButton = new System.Windows.Forms.Button();
            this.dataGridViewComposition = new System.Windows.Forms.DataGridView();
            this.dataGridViewAdBlock = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewComposition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAdBlock)).BeginInit();
            this.SuspendLayout();
            // 
            // searchButton
            // 
            this.searchButton.Location = new System.Drawing.Point(413, 77);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(75, 23);
            this.searchButton.TabIndex = 44;
            this.searchButton.Text = "Найти";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // adBlockTextBox
            // 
            this.adBlockTextBox.Location = new System.Drawing.Point(136, 39);
            this.adBlockTextBox.Name = "adBlockTextBox";
            this.adBlockTextBox.Size = new System.Drawing.Size(246, 20);
            this.adBlockTextBox.TabIndex = 43;
            // 
            // removeAdBlockButton
            // 
            this.removeAdBlockButton.Location = new System.Drawing.Point(612, 77);
            this.removeAdBlockButton.Name = "removeAdBlockButton";
            this.removeAdBlockButton.Size = new System.Drawing.Size(75, 23);
            this.removeAdBlockButton.TabIndex = 42;
            this.removeAdBlockButton.Text = "Удалить";
            this.removeAdBlockButton.UseVisualStyleBackColor = true;
            this.removeAdBlockButton.Click += new System.EventHandler(this.removeAdBlockButton_Click);
            // 
            // addAdBlockButton
            // 
            this.addAdBlockButton.Location = new System.Drawing.Point(515, 77);
            this.addAdBlockButton.Name = "addAdBlockButton";
            this.addAdBlockButton.Size = new System.Drawing.Size(75, 23);
            this.addAdBlockButton.TabIndex = 41;
            this.addAdBlockButton.Text = "Добавить";
            this.addAdBlockButton.UseVisualStyleBackColor = true;
            this.addAdBlockButton.Click += new System.EventHandler(this.addAdBlockButton_Click);
            // 
            // authorsComboBox
            // 
            this.authorsComboBox.FormattingEnabled = true;
            this.authorsComboBox.Location = new System.Drawing.Point(169, 402);
            this.authorsComboBox.Name = "authorsComboBox";
            this.authorsComboBox.Size = new System.Drawing.Size(121, 21);
            this.authorsComboBox.TabIndex = 39;
            // 
            // removeAdButton
            // 
            this.removeAdButton.Location = new System.Drawing.Point(612, 419);
            this.removeAdButton.Name = "removeAdButton";
            this.removeAdButton.Size = new System.Drawing.Size(75, 23);
            this.removeAdButton.TabIndex = 38;
            this.removeAdButton.Text = "Удалить";
            this.removeAdButton.UseVisualStyleBackColor = true;
            this.removeAdButton.Click += new System.EventHandler(this.removeAdButton_Click);
            // 
            // addAdButton
            // 
            this.addAdButton.Location = new System.Drawing.Point(515, 419);
            this.addAdButton.Name = "addAdButton";
            this.addAdButton.Size = new System.Drawing.Size(75, 23);
            this.addAdButton.TabIndex = 37;
            this.addAdButton.Text = "Добавить";
            this.addAdButton.UseVisualStyleBackColor = true;
            this.addAdButton.Click += new System.EventHandler(this.addAdButton_Click);
            // 
            // dataGridViewComposition
            // 
            this.dataGridViewComposition.AllowUserToAddRows = false;
            this.dataGridViewComposition.AllowUserToDeleteRows = false;
            this.dataGridViewComposition.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewComposition.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewComposition.Location = new System.Drawing.Point(10, 477);
            this.dataGridViewComposition.MultiSelect = false;
            this.dataGridViewComposition.Name = "dataGridViewComposition";
            this.dataGridViewComposition.Size = new System.Drawing.Size(760, 205);
            this.dataGridViewComposition.TabIndex = 36;
            // 
            // dataGridViewAdBlock
            // 
            this.dataGridViewAdBlock.AllowUserToAddRows = false;
            this.dataGridViewAdBlock.AllowUserToDeleteRows = false;
            this.dataGridViewAdBlock.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewAdBlock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAdBlock.Location = new System.Drawing.Point(12, 126);
            this.dataGridViewAdBlock.MultiSelect = false;
            this.dataGridViewAdBlock.Name = "dataGridViewAdBlock";
            this.dataGridViewAdBlock.Size = new System.Drawing.Size(760, 205);
            this.dataGridViewAdBlock.TabIndex = 35;
            this.dataGridViewAdBlock.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewAdBlock_CellBeginEdit);
            this.dataGridViewAdBlock.RowValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewAdBlock_RowValidated);
            this.dataGridViewAdBlock.SelectionChanged += new System.EventHandler(this.dataGridViewAdBlock_SelectionChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 45;
            this.label1.Text = "Название блока";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 405);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 46;
            this.label2.Text = "Название ролика";
            // 
            // Ad_block_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 705);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.adBlockTextBox);
            this.Controls.Add(this.removeAdBlockButton);
            this.Controls.Add(this.addAdBlockButton);
            this.Controls.Add(this.authorsComboBox);
            this.Controls.Add(this.removeAdButton);
            this.Controls.Add(this.addAdButton);
            this.Controls.Add(this.dataGridViewComposition);
            this.Controls.Add(this.dataGridViewAdBlock);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Ad_block_form";
            this.Text = "Рекламный блок";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewComposition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAdBlock)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.TextBox adBlockTextBox;
        private System.Windows.Forms.Button removeAdBlockButton;
        private System.Windows.Forms.Button addAdBlockButton;
        private System.Windows.Forms.ComboBox authorsComboBox;
        private System.Windows.Forms.Button removeAdButton;
        private System.Windows.Forms.Button addAdButton;
        private System.Windows.Forms.DataGridView dataGridViewComposition;
        private System.Windows.Forms.DataGridView dataGridViewAdBlock;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}