using System;
using System.Windows.Forms;

namespace MusicStore
{
    public partial class RecordForm : Form
    {
        public Record Record { get; private set; }

        public RecordForm(Record record = null)
        {
            InitializeComponent();

            Record = record ?? new Record();
            BindControls();
        }

        private void BindControls()
        {
            txtTitle.DataBindings.Add("Text", Record, "Title");
            txtArtist.DataBindings.Add("Text", Record, "Artist");
            txtPublisher.DataBindings.Add("Text", Record, "Publisher");
            nudTrackCount.DataBindings.Add("Value", Record, "TrackCount");
            txtGenre.DataBindings.Add("Text", Record, "Genre");
            nudReleaseYear.DataBindings.Add("Value", Record, "ReleaseYear");
            nudCostPrice.DataBindings.Add("Value", Record, "CostPrice");
            nudSellingPrice.DataBindings.Add("Value", Record, "SellingPrice");
        }

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                Record.Title = txtTitle.Text.Trim();
                Record.Artist = txtArtist.Text.Trim();
                Record.Publisher = txtPublisher.Text.Trim();
                Record.TrackCount = Convert.ToInt32(nudTrackCount.Value);
                Record.Genre = txtGenre.Text.Trim();
                Record.ReleaseYear = Convert.ToInt32(nudReleaseYear.Value);
                Record.CostPrice = Convert.ToDecimal(nudCostPrice.Value);
                Record.SellingPrice = Convert.ToDecimal(nudSellingPrice.Value);

                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Введите название записи");
                return false;
            }

            if (Record.SellingPrice <= Record.CostPrice)
            {
                MessageBox.Show("Цена продажи должна быть больше себестоимости");
                return false;
            }

            if (nudTrackCount.Value < 1 || nudReleaseYear.Value < 1900)
            {
                MessageBox.Show("Проверьте корректность числовых значений");
                return false;
            }

            return true;
        }
private void btnCancel_Click_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
