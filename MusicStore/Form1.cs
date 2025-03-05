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
    public partial class Form1 : Form
    {
        private readonly RecordRepository repository;
        private readonly BindingSource bindingSource;
        public Form1()
        {
            InitializeComponent();

            repository = new RecordRepository();
            bindingSource = new BindingSource();

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

        private void LoadData()
        {
            bindingSource.DataSource = repository.GetAllRecords();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var form = new RecordForm(null);
            if (form.ShowDialog() == DialogResult.OK)
            {
                repository.AddRecord(form.Record);
                LoadData();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (bindingSource.Current != null)
            {
                var record = (Record)bindingSource.Current;
                var form = new RecordForm(record);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    repository.UpdateRecord(form.Record);
                    LoadData();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (bindingSource.Current != null)
            {
                var record = (Record)bindingSource.Current;
                if (MessageBox.Show($"Удалить запись '{record.Title}'?",
                    "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    repository.DeleteRecord(record.Id);
                    LoadData();
                }
            }
        }
    }
}
