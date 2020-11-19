﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MediaPlayer.Models
{
    public class Track
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string AlbumArt { get; set; }
        public int GenreId { get; set; }
    }
}
