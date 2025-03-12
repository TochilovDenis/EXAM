namespace MusicStore
{
    public class Record
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Publisher { get; set; }
        public int TrackCount { get; set; }
        public string Genre { get; set; }
        public int ReleaseYear { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }

        public Record() { }

        public Record(string title, string artist, string publisher, int trackCount,
                     string genre, int releaseYear, decimal costPrice, decimal sellingPrice)
        {
            Title = title;
            Artist = artist;
            Publisher = publisher;
            TrackCount = trackCount;
            Genre = genre;
            ReleaseYear = releaseYear;
            CostPrice = costPrice;
            SellingPrice = sellingPrice;
        }
    }
}
