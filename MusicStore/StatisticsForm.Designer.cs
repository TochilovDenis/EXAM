namespace MusicStore
{
    partial class StatisticsForm
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
            this.cbTimePeriod = new System.Windows.Forms.ComboBox();
            this.btnShowStats = new System.Windows.Forms.Button();
            this.dgvStatistics = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatistics)).BeginInit();
            this.SuspendLayout();
            // 
            // cbTimePeriod
            // 
            this.cbTimePeriod.FormattingEnabled = true;
            this.cbTimePeriod.Location = new System.Drawing.Point(12, 12);
            this.cbTimePeriod.Name = "cbTimePeriod";
            this.cbTimePeriod.Size = new System.Drawing.Size(121, 24);
            this.cbTimePeriod.TabIndex = 0;
            // 
            // btnShowStats
            // 
            this.btnShowStats.Location = new System.Drawing.Point(218, 10);
            this.btnShowStats.Name = "btnShowStats";
            this.btnShowStats.Size = new System.Drawing.Size(100, 26);
            this.btnShowStats.TabIndex = 1;
            this.btnShowStats.Text = "Показать статистику";
            this.btnShowStats.UseVisualStyleBackColor = true;
            this.btnShowStats.Click += new System.EventHandler(this.btnShowStats_Click_1);
            // 
            // dgvStatistics
            // 
            this.dgvStatistics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStatistics.Location = new System.Drawing.Point(12, 42);
            this.dgvStatistics.Name = "dgvStatistics";
            this.dgvStatistics.RowHeadersWidth = 51;
            this.dgvStatistics.RowTemplate.Height = 24;
            this.dgvStatistics.Size = new System.Drawing.Size(658, 400);
            this.dgvStatistics.TabIndex = 2;
            // 
            // StatisticsForm
            // 
            this.ClientSize = new System.Drawing.Size(684, 459);
            this.Controls.Add(this.dgvStatistics);
            this.Controls.Add(this.btnShowStats);
            this.Controls.Add(this.cbTimePeriod);
            this.Name = "StatisticsForm";
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatistics)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbTimePeriod;
        private System.Windows.Forms.Button btnShowStats;
        private System.Windows.Forms.DataGridView dgvStatistics;
    }
}