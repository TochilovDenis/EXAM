using System;
using System.Windows.Forms;

namespace MusicStore
{
    public partial class Form1 : Form
    {
        private readonly RecordRepository repository;
        private readonly BindingSource bindingSource;
        private readonly UserRepository userRepository;

        public Form1()
        {
            InitializeComponent();

            repository = new RecordRepository();
            bindingSource = new BindingSource();
            userRepository = new UserRepository();


            // Проверяем, есть ли сохранённый пользователь
            if (!string.IsNullOrEmpty(Properties.Settings.Default.CurrentUserLogin))
            {
                using (var loginForm = new LoginForm())
                {
                    if (loginForm.ShowDialog() != DialogResult.OK)
                    {
                        Application.Exit();
                        return;
                    }
                }
            }
          
            if (dgvRecords == null)
            {
                dgvRecords = new DataGridView();
                this.Controls.Add(dgvRecords);
            }
            InitControls();
            LoadData();
        }

        private void InitControls()
        {
            dgvRecords.AutoGenerateColumns = false;
            dgvRecords.DataSource = bindingSource;

            dgvRecords.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Id",
                DataPropertyName = "Id"
            });

            dgvRecords.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Название",
                DataPropertyName = "Title"
            });

            dgvRecords.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Исполнитель",
                DataPropertyName = "Artist"
            });

            dgvRecords.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Издатель",
                DataPropertyName = "Publisher"
            });

            dgvRecords.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Количество треков",
                DataPropertyName = "TrackCount"
            });

            dgvRecords.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Жанр",
                DataPropertyName = "Genre"
            });

            dgvRecords.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Год выпуска",
                DataPropertyName = "ReleaseYear"
            });

            dgvRecords.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Себестоимость",
                DataPropertyName = "CostPrice"
            });

            dgvRecords.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Цена продажи",
                DataPropertyName = "SellingPrice"
            });
        }
        private async void LoadData()
        {
            try
            {
                var records = await repository.GetAllRecordsAsync().ConfigureAwait(false);
                if (dgvRecords.InvokeRequired)
                {
                    dgvRecords.BeginInvoke(new Action(() =>
                        bindingSource.DataSource = records
                    ));
                }
                else
                {
                    bindingSource.DataSource = records;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnAdd_Click_1(object sender, EventArgs e)
        {
            try
            {
                var form = new RecordForm(null);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    await repository.AddRecordAsync(form.Record);
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении записи: {ex.Message}");
            }
        }

        private async void btnEdit_Click_1(object sender, EventArgs e)
        {
            if (bindingSource.Current != null)
            {
                var record = (Record)bindingSource.Current;
                var form = new RecordForm(record);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    await repository.UpdateRecord(form.Record);
                    LoadData();
                }
            }
        }

        private async void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (bindingSource.Current != null)
            {
                var record = (Record)bindingSource.Current;
                if (MessageBox.Show($"Удалить запись '{record.Title}'?",
                    "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    await repository.DeleteRecord(record.Id);
                    LoadData();
                }
            }
        }

        private async void btnSale_Click_Click(object sender, EventArgs e)
        {
            if (bindingSource.Current != null)
            {
                var record = (Record)bindingSource.Current;
                var saleForm = new SaleForm(record);

                if (saleForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        int? customerId = null;
                        if (!string.IsNullOrEmpty(Properties.Settings.Default.CurrentUserLogin))
                        {
                            var user = await userRepository.GetUserAsync(Properties.Settings.Default.CurrentUserLogin);
                            customerId = user.Id;
                        }

                        await repository.SaleRecordAsync(record.Id, customerId, saleForm.Price);
                        LoadData();
                        MessageBox.Show("Продажа успешно оформлена!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnStock_Click(object sender, EventArgs e)
        {
            var stockForm = new StockOperationsForm();
            stockForm.ShowDialog();
        }

        private void btnPromotions_Click(object sender, EventArgs e)
        {
            var promotionsForm = new PromotionsForm();
            promotionsForm.ShowDialog();

        }

        private async void btnReserve_Click(object sender, EventArgs e)
        {
            if (bindingSource.Current != null)
            {
                var record = (Record)bindingSource.Current;
                var reserveForm = new ReserveForm(record);
                if (reserveForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        await repository.ReserveRecordAsync(record.Id, reserveForm.CustomerName, reserveForm.ExpireDate);
                        MessageBox.Show("Запись успешно забронирована!");
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private async void search_Click(object sender, EventArgs e)
        {
            using (var searchForm = new SearchForm())
            {
                if (searchForm.ShowDialog() == DialogResult.OK)
                {
                    var records = await repository.SearchRecordsAsync(
                        searchForm.Title,
                        searchForm.Artist,
                        searchForm.Genre
                    );
                    bindingSource.DataSource = records;
                }
            }
        }

        private void btnStatistics_Click(object sender, EventArgs e)
        {
            var statsForm = new StatisticsForm();
            statsForm.ShowDialog();
        }
    } 
}
