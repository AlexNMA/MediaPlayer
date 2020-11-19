using MediaPlayer.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace MediaPlayer.Db
{

    public class Repository
    {
        public Repository()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["mediaPlayer"].ConnectionString;
            _con = new SqlConnection(connectionString);
            _con.Open();
        }
        private SqlConnection _con;


        public List<Track> GetTracks()
        {
            List<Track> tracks = new List<Track>();

            SqlCommand command = _con.CreateCommand();
            command.CommandText = "select * from Track";

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Track t = new Track
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Artist = reader.GetString(2),
                    Album = reader.GetString(3),
                    AlbumArt = reader.GetString(4),
                    GenreId = reader.GetInt32(5)
                };
                tracks.Add(t);
            }

            return tracks;
        }

        public List<PLaylist> GetPLaylists()
        {

        }

    }
}
