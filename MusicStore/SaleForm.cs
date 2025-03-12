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
    public partial class SaleForm: Form
    {
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox txtTitle;
        private TextBox txtArtist;
        private TextBox txtCustomerName;
        private TextBox txtPrice;
        private Button btnOK;
        private Button btnCancel;
        private Label label4;

        public string CustomerName { get; private set; }
        public decimal Price { get; private set; }

        public SaleForm(Record record)
        {
            InitializeComponent();
            Text = $"Продажа: {record.Title}";
            txtTitle.Text = record.Title;
            txtArtist.Text = record.Artist;
            txtPrice.Text = record.SellingPrice.ToString();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtCustomerName.Text))
            {
                MessageBox.Show("Введите имя покупателя");
                return false;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Введите корректную цену продажи");
                return false;
            }

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            if (ValidateInput())
            {
                CustomerName = txtCustomerName.Text.Trim();
                Price = Convert.ToDecimal(txtPrice.Text);
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
