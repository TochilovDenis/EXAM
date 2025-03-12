using System;
using System.Windows.Forms;

namespace MusicStore
{
    public partial class PromotionsForm: Form
    {
        private ComboBox cbRecords;
        private Label lblRecord;
        private Label lblStartDate;
        private DateTimePicker dtpStartDate;
        private Label lblEndDate;
        private DateTimePicker dtpEndDate;
        private Label lblDiscount;
        private NumericUpDown numDiscount;
        private Label lblDescription;
        private TextBox txtDescription;
        private Button btnAddPromotion;
        private Button btnCancel;
        private readonly RecordRepository _repository;

        public PromotionsForm()
        {
            InitializeComponent();
            _repository = new RecordRepository();
            LoadRecords();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
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

        private async void btnAddPromotion_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    var recordId = (int)cbRecords.SelectedValue;
                    var startDate = dtpStartDate.Value;
                    var endDate = dtpEndDate.Value;
                    var discount = (decimal)numDiscount.Value;
                    var description = txtDescription.Text.Trim();

                    await _repository.CreatePromotionAsync(recordId, startDate, endDate, discount, description);
                    MessageBox.Show("Акция создана успешно!");
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при создании акции: {ex.Message}");
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

            if (dtpEndDate.Value <= dtpStartDate.Value)
            {
                MessageBox.Show("Дата окончания должна быть позже даты начала");
                return false;
            }

            if (numDiscount.Value < 0 || numDiscount.Value > 100)
            {
                MessageBox.Show("Скидка должна быть от 0 до 100%");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Введите описание акции");
                return false;
            }

            return true;
        }

        private void ClearFields()
        {
            cbRecords.SelectedIndex = -1;
            dtpStartDate.Value = DateTime.Now;
            dtpEndDate.Value = DateTime.Now.AddDays(7);
            numDiscount.Value = 10;
            txtDescription.Clear();
        }
    }
}
