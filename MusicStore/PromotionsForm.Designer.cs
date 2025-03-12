using System;

namespace MusicStore
{
    partial class PromotionsForm
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
            this.cbRecords = new System.Windows.Forms.ComboBox();
            this.lblRecord = new System.Windows.Forms.Label();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.lblEndDate = new System.Windows.Forms.Label();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.lblDiscount = new System.Windows.Forms.Label();
            this.numDiscount = new System.Windows.Forms.NumericUpDown();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.btnAddPromotion = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();

            this.SuspendLayout();

            this.cbRecords.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRecords.FormattingEnabled = true;
            this.cbRecords.Location = new System.Drawing.Point(104, 12);
            this.cbRecords.Name = "cbRecords";
            this.cbRecords.Size = new System.Drawing.Size(200, 21);
            this.cbRecords.TabIndex = 0;

            this.lblRecord.AutoSize = true;
            this.lblRecord.Location = new System.Drawing.Point(12, 15);
            this.lblRecord.Name = "lblRecord";
            this.lblRecord.Size = new System.Drawing.Size(57, 13);
            this.lblRecord.TabIndex = 1;
            this.lblRecord.Text = "Запись:";

            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Location = new System.Drawing.Point(12, 44);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(71, 13);
            this.lblStartDate.TabIndex = 2;
            this.lblStartDate.Text = "Дата начала:";

            this.dtpStartDate.Location = new System.Drawing.Point(104, 40);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(200, 20);
            this.dtpStartDate.TabIndex = 3;

            this.lblEndDate.AutoSize = true;
            this.lblEndDate.Location = new System.Drawing.Point(12, 72);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(73, 13);
            this.lblEndDate.Text = "Дата окончания:";

            this.dtpEndDate.Location = new System.Drawing.Point(104, 68);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(200, 20);
            this.dtpEndDate.TabIndex = 4;

            this.lblDiscount.AutoSize = true;
            this.lblDiscount.Location = new System.Drawing.Point(12, 100);
            this.lblDiscount.Name = "lblDiscount";
            this.lblDiscount.Size = new System.Drawing.Size(46, 13);
            this.lblDiscount.Text = "Скидка (%):";

            this.numDiscount.DecimalPlaces = 2;
            this.numDiscount.Location = new System.Drawing.Point(104, 96);
            this.numDiscount.Name = "numDiscount";
            this.numDiscount.Size = new System.Drawing.Size(200, 20);
            this.numDiscount.TabIndex = 5;
            this.numDiscount.Value = new decimal(new int[] { 10, 0, 0, 0 });

            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(12, 128);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(60, 13);
            this.lblDescription.Text = "Описание:";

            this.txtDescription.Location = new System.Drawing.Point(104, 124);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(200, 20);
            this.txtDescription.TabIndex = 6;

            this.btnAddPromotion.Location = new System.Drawing.Point(104, 154);
            this.btnAddPromotion.Name = "btnAddPromotion";
            this.btnAddPromotion.Size = new System.Drawing.Size(75, 23);
            this.btnAddPromotion.TabIndex = 7;
            this.btnAddPromotion.Text = "Добавить";
            this.btnAddPromotion.UseVisualStyleBackColor = true;
            this.btnAddPromotion.Click += new System.EventHandler(this.btnAddPromotion_Click);

            this.btnCancel.Location = new System.Drawing.Point(229, 154);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 194);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAddPromotion);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.numDiscount);
            this.Controls.Add(this.lblDiscount);
            this.Controls.Add(this.dtpEndDate);
            this.Controls.Add(this.lblEndDate);
            this.Controls.Add(this.dtpStartDate);
            this.Controls.Add(this.lblStartDate);
            this.Controls.Add(this.lblRecord);
            this.Controls.Add(this.cbRecords);
            this.Name = "PromotionsForm";
            this.Text = "Акции";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        

        #endregion
    }
}