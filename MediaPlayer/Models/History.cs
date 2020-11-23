namespace MediaPlayer.Models
{
    public class History
    {
        public int Id { get; set; }
        public int TrackId { get; set; }
        public string Name { get; set; }
        public History()
        {

        }
        public History(int id, int trackId)
        {
            Id = id;
            TrackId = trackId;
        }
        public History(int id, int trackId, string name)
        {
            Id = id;
            TrackId = trackId;
            Name = name;
        }
    }
}
