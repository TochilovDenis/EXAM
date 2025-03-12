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
    public partial class SearchForm: Form
    {
        private TextBox txtTitle;
        private TextBox txtArtist;
        private TextBox txtGenre;
        private Button btnSearch;
        private Button btnCancel;

        public event EventHandler<SearchEventArgs> SearchRequested;

        public string Title => txtTitle.Text.Trim();
        public string Artist => txtArtist.Text.Trim();
        public string Genre => txtGenre.Text.Trim();

        public SearchForm()
        {
            InitializeComponent();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            var args = new SearchEventArgs(
                txtTitle.Text.Trim(),
                txtArtist.Text.Trim(),
                txtGenre.Text.Trim()
            );
            SearchRequested?.Invoke(this, args);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }

    public class SearchEventArgs : EventArgs
    {
        public string Title { get; }
        public string Artist { get; }
        public string Genre { get; }

        public SearchEventArgs(string title, string artist, string genre)
        {
            Title = title;
            Artist = artist;
            Genre = genre;
        }
    }
}

