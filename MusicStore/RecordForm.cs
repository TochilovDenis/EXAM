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

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                Record.Title = Record.Title;
                Record.Artist = Record.Artist;
                Record.Publisher = Record.Publisher;
                Record.TrackCount = Record.TrackCount;
                Record.Genre = Record.Genre;
                Record.ReleaseYear = Record.ReleaseYear;
                Record.CostPrice = Record.CostPrice;
                Record.SellingPrice = Record.SellingPrice;

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

            return true;
        }
    }
}
