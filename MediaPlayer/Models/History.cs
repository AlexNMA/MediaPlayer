using System;
using System.Collections.Generic;
using System.Text;

namespace MediaPlayer.Models
{
    public class History
    {
        public int Id { get; set; }
        public int TrackId { get; set; }
        public History()
        {

        }
        public History(int id, int trackId)
        {
            Id = id;
            TrackId = trackId;
        }
    }
}
