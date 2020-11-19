using MediaPlayer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace MediaPlayer.Db
{
    public class Respository
    {
        public SqlConnection GetSqlConnection(string connectionString)
        {
            SqlConnection con = new SqlConnection(connectionString);
            return con;
        }

        public List<Track> GetTracks(SqlConnection connection)
        {
            List<Track> tracks = new List<Track>();

            SqlCommand command = connection.CreateCommand();
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
    }
}
