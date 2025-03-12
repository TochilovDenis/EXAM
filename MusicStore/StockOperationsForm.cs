using System;
using System.Windows.Forms;

namespace MusicStore
{
    public partial class StockOperationsForm: Form
    {
        private ComboBox cbRecords;
        private Label lblRecord;
        private Label lblOperationType;
        private RadioButton rbIn;
        private RadioButton rbOut;
        private Label lblQuantity;
        private NumericUpDown numQuantity;
        private Label lblReason;
        private TextBox txtReason;
        private Button btnAddOperation;
        private Button btnCancel;
        private readonly RecordRepository _repository;

        public StockOperationsForm()
        {
            InitializeComponent();
            _repository = new RecordRepository();
            LoadRecords();
        }

        private async void LoadRecords()
        {
            try
            {
                var records = await _repository.GetAllRecordsAsync();
                cbRecords.DataSource = records;
                cbRecords.DisplayMember = "Title";
                cbRecords.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке записей: {ex.Message}");
            }
        }

        private async void btnAddOperation_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    var recordId = (int)cbRecords.SelectedValue;
                    var operationType = rbIn.Checked ? "IN" : "OUT";
                    var quantity = (int)numQuantity.Value;
                    var reason = txtReason.Text.Trim();

                    await _repository.AddStockOperationAsync(recordId, operationType, quantity, reason);
                    MessageBox.Show("Операция добавлена успешно!");
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении операции: {ex.Message}");
                }
            }
        }

        private bool ValidateInput()
        {
            if (cbRecords.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите запись");
                return false;
            }

            if (numQuantity.Value <= 0)
            {
                MessageBox.Show("Количество должно быть больше 0");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtReason.Text))
            {
                MessageBox.Show("Введите причину операции");
                return false;
            }

            return true;
        }

        private void ClearFields()
        {
            cbRecords.SelectedIndex = -1;
            rbIn.Checked = true;
            numQuantity.Value = 1;
            txtReason.Clear();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
