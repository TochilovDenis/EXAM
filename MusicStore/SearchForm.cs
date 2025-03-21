using System;
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

        private void btnSearch_Click_1(object sender, EventArgs e)
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

        private void btnCancel_Click_1(object sender, EventArgs e)
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

