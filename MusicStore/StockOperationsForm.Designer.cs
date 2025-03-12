namespace MusicStore
{
    partial class StockOperationsForm
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
            this.lblOperationType = new System.Windows.Forms.Label();
            this.rbIn = new System.Windows.Forms.RadioButton();
            this.rbOut = new System.Windows.Forms.RadioButton();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.numQuantity = new System.Windows.Forms.NumericUpDown();
            this.lblReason = new System.Windows.Forms.Label();
            this.txtReason = new System.Windows.Forms.TextBox();
            this.btnAddOperation = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).BeginInit();
            this.SuspendLayout();
            // 
            // cbRecords
            // 
            this.cbRecords.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRecords.FormattingEnabled = true;
            this.cbRecords.Location = new System.Drawing.Point(139, 15);
            this.cbRecords.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbRecords.Name = "cbRecords";
            this.cbRecords.Size = new System.Drawing.Size(265, 24);
            this.cbRecords.TabIndex = 0;
            // 
            // lblRecord
            // 
            this.lblRecord.AutoSize = true;
            this.lblRecord.Location = new System.Drawing.Point(16, 18);
            this.lblRecord.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRecord.Name = "lblRecord";
            this.lblRecord.Size = new System.Drawing.Size(57, 16);
            this.lblRecord.TabIndex = 1;
            this.lblRecord.Text = "Запись:";
            // 
            // lblOperationType
            // 
            this.lblOperationType.AutoSize = true;
            this.lblOperationType.Location = new System.Drawing.Point(16, 54);
            this.lblOperationType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOperationType.Name = "lblOperationType";
            this.lblOperationType.Size = new System.Drawing.Size(102, 16);
            this.lblOperationType.TabIndex = 2;
            this.lblOperationType.Text = "Тип операции:";
            // 
            // rbIn
            // 
            this.rbIn.AutoSize = true;
            this.rbIn.Checked = true;
            this.rbIn.Location = new System.Drawing.Point(139, 52);
            this.rbIn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbIn.Name = "rbIn";
            this.rbIn.Size = new System.Drawing.Size(91, 20);
            this.rbIn.TabIndex = 3;
            this.rbIn.TabStop = true;
            this.rbIn.Text = "Поставка";
            this.rbIn.UseVisualStyleBackColor = true;
            // 
            // rbOut
            // 
            this.rbOut.AutoSize = true;
            this.rbOut.Location = new System.Drawing.Point(241, 52);
            this.rbOut.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbOut.Name = "rbOut";
            this.rbOut.Size = new System.Drawing.Size(74, 20);
            this.rbOut.TabIndex = 4;
            this.rbOut.Text = "Расход";
            this.rbOut.UseVisualStyleBackColor = true;
            // 
            // lblQuantity
            // 
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Location = new System.Drawing.Point(16, 89);
            this.lblQuantity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(88, 16);
            this.lblQuantity.TabIndex = 5;
            this.lblQuantity.Text = "Количество:";
            // 
            // numQuantity
            // 
            this.numQuantity.Location = new System.Drawing.Point(139, 86);
            this.numQuantity.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numQuantity.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numQuantity.Name = "numQuantity";
            this.numQuantity.Size = new System.Drawing.Size(267, 22);
            this.numQuantity.TabIndex = 6;
            this.numQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numQuantity.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblReason
            // 
            this.lblReason.AutoSize = true;
            this.lblReason.Location = new System.Drawing.Point(16, 121);
            this.lblReason.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReason.Name = "lblReason";
            this.lblReason.Size = new System.Drawing.Size(68, 16);
            this.lblReason.TabIndex = 7;
            this.lblReason.Text = "Причина:";
            // 
            // txtReason
            // 
            this.txtReason.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtReason.Location = new System.Drawing.Point(139, 117);
            this.txtReason.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtReason.Name = "txtReason";
            this.txtReason.Size = new System.Drawing.Size(265, 22);
            this.txtReason.TabIndex = 8;
            // 
            // btnAddOperation
            // 
            this.btnAddOperation.Location = new System.Drawing.Point(139, 155);
            this.btnAddOperation.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAddOperation.Name = "btnAddOperation";
            this.btnAddOperation.Size = new System.Drawing.Size(100, 28);
            this.btnAddOperation.TabIndex = 9;
            this.btnAddOperation.Text = "Добавить";
            this.btnAddOperation.UseVisualStyleBackColor = true;
            this.btnAddOperation.Click += new System.EventHandler(this.btnAddOperation_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(305, 155);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 28);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // StockOperationsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 207);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAddOperation);
            this.Controls.Add(this.txtReason);
            this.Controls.Add(this.lblReason);
            this.Controls.Add(this.numQuantity);
            this.Controls.Add(this.lblQuantity);
            this.Controls.Add(this.rbOut);
            this.Controls.Add(this.rbIn);
            this.Controls.Add(this.lblOperationType);
            this.Controls.Add(this.lblRecord);
            this.Controls.Add(this.cbRecords);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "StockOperationsForm";
            this.Text = "Операции с запасом";
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }

        #endregion
}