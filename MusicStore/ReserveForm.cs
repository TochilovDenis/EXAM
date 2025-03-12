using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicStore
{
    public partial class ReserveForm: Form
    {

        private Label lblRecord;
        private TextBox txtRecord;
        private Label lblArtist;
        private TextBox txtArtist;
        private Label lblCustomerName;
        private TextBox txtCustomerName;
        private Label lblExpireDate;
        private DateTimePicker dtpExpireDate;
        private Button btnOK;
        private Button btnCancel;
        private readonly RecordRepository _repository;

        public string CustomerName { get; private set; }
        public DateTime ExpireDate { get; private set; }

        public ReserveForm(Record record)
        {
            InitializeComponent();
            _repository = new RecordRepository();
            txtRecord.Text = record.Title;
            txtArtist.Text = record.Artist;
            dtpExpireDate.Value = DateTime.Now.AddDays(1);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                CustomerName = txtCustomerName.Text.Trim();
                ExpireDate = dtpExpireDate.Value;
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtCustomerName.Text))
            {
                MessageBox.Show("Введите имя покупателя");
                return false;
            }

            if (dtpExpireDate.Value <= DateTime.Now)
            {
                MessageBox.Show("Дата истечения брони должна быть в будущем");
                return false;
            }

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
