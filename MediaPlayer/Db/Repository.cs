using MediaPlayer.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace MediaPlayer.Db
{

    public class Repository
    {
        public Repository()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["mediaPlayer"].ConnectionString;
            _con = new SqlConnection(connectionString);
            //_con.Open();
        }
        private SqlConnection _con;


        public List<Track> GetTracks()
        {
            _con.Open();
            List<Track> tracks = new List<Track>();

            SqlCommand command = _con.CreateCommand();
            command.CommandText = @"
select t.Id, t.Name, t.Artist, t.Album, g.GenreName
from Track t
join Genre g
on t.GenreId = g.Id";

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Track t = new Track
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Artist = reader.GetString(2),
                    Album = reader.GetString(3),
                    Genre = reader.GetString(4)
                };
                tracks.Add(t);
            }

            _con.Close();
            return tracks;
        }

        public List<Playlist> GetPlaylists()
        {
            _con.Open();
            List<Playlist> playlists = new List<Playlist>();

            SqlCommand command = _con.CreateCommand();
            command.CommandText = "select * from Playlist";

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Playlist p = new Playlist()
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1)
                };
                playlists.Add(p);
            }
            _con.Close();

            return playlists;
        }

        public List<Track> GetTracksFromPlaylist(object index)
        {
            if (index == null)
            {
                return new List<Track>();
            }
            List<Track> tracks = new List<Track>();
            string query = @"
select t.Id, t.Name, t.Artist, t.Album, g.GenreName
from ((TrackPlaylist tp
inner join Track t
    on t.Id = tp.TrackId)
	inner join Genre g
	on t.GenreId = g.Id)
where tp.PlaylistId = @TrackId";
            _con.Open();
            using (SqlCommand command = new SqlCommand(query, _con))
            {
                command.Parameters.AddWithValue("@TrackId", index);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Track t = new Track
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Artist = reader.GetString(2),
                        Album = reader.GetString(3),
                        Genre = reader.GetString(4)
                    };
                    tracks.Add(t);
                }
            }
            _con.Close();


            return tracks;

        }

        public Uri GetTrackFiles(object index)
        {
            if (index == null)
            {
                return new Uri("C:/tracksFolder");
            }
            //List<TrackFile> tracksfile = new List<TrackFile>();
            Uri uri = new Uri("C:/tracksFolder");
            string query = @"select Track.Url
from Track
where Track.Id = @Id";
            _con.Open();
            
            using (SqlCommand command = new SqlCommand(query, _con))
            {
                command.Parameters.AddWithValue("@Id", index);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                     uri = new Uri(reader.GetString(0));
                    
                }
            }
            _con.Close();
            return uri;
        }

    }

}
