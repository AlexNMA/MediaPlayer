using MediaPlayer.Db;
using MediaPlayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace MediaPlayer
{

    public partial class MainWindow : Window
    {
        List<History> histories;
        int historypozition;

        public MainWindow()
        {
            InitializeComponent();


            _playbackTimer = new DispatcherTimer();
            _playbackTimer.Interval = TimeSpan.FromSeconds(1);
            _playbackTimer.Tick += _playbackTimer_Tick;
            Timer1Label.ContentStringFormat = "{0:mm\\:ss}";
            histories = new List<History>();
            historypozition = 0;
        }

        private void _playbackTimer_Tick(object sender, EventArgs e)
        {
            TrackBarSlider.Value = MediaplayerElement.Position.TotalSeconds;
            Timer1Label.Content = MediaplayerElement.Position.Duration();

        }

        private Repository _repository = new Repository();
        private DispatcherTimer _playbackTimer;

        private void MediaPlayerWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LibraryRB.IsChecked = true;
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {

            MediaplayerElement.Play();
            PauseButton.Visibility = Visibility.Visible;
            PlayButton.Visibility = Visibility.Collapsed;

            _playbackTimer.Start();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            MediaplayerElement.Pause();
            PauseButton.Visibility = Visibility.Collapsed;
            PlayButton.Visibility = Visibility.Visible;

            _playbackTimer.Stop();
        }

        private void LibraryRB_Checked(object sender, RoutedEventArgs e)
        {
            SearchPanel.Visibility = Visibility.Visible;
            DataGridLibrary.Visibility = Visibility.Visible;
            DataGridPlaylis.Visibility = Visibility.Hidden;
            PlaylistLbox.Visibility = Visibility.Hidden;
            List<Track> tracks = _repository.GetTracks();
            DataGridLibrary.ItemsSource = tracks;
        }

        private void PlayListRB_Checked(object sender, RoutedEventArgs e)
        {
            SearchPanel.Visibility = Visibility.Hidden;
            DataGridLibrary.Visibility = Visibility.Hidden;
            DataGridPlaylis.Visibility = Visibility.Visible;
            PlaylistLbox.Visibility = Visibility.Visible;

            List<Playlist> playlists = _repository.GetPlaylists();
            PlaylistLbox.DisplayMemberPath = "Name";
            PlaylistLbox.SelectedValuePath = "Id";
            PlaylistLbox.ItemsSource = playlists;
        }


        private void PlaylistLbox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            List<Track> tracks = _repository.GetTracksFromPlaylist(PlaylistLbox.SelectedValue);
            DataGridPlaylis.ItemsSource = tracks;
        }

        private void EndTrackBtn_Click(object sender, RoutedEventArgs e)
        {
            MediaplayerElement.Stop();

        }

        private void FirstTrackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MediaplayerElement.Position.TotalSeconds > 5)
            {
                MediaplayerElement.Position = TimeSpan.Zero;
            }
            else
            {
                List<Track> tracks = new List<Track>();
                foreach (History index in histories)
                {
                    if (index.Id == (historypozition - 2))
                    {
                        tracks = _repository.GetOneTrack(index.TrackId);
                        foreach (Track track in tracks)
                        {
                            Uri trackFiles = _repository.GetTrackFiles(track.Id);
                            Uri artAlbum = _repository.GetTrackArt(track.Id);
                            AlbumArtImage.Source = new BitmapImage(artAlbum);
                            TrackAndArtistLabel.Content = track.Name + " - " + track.Artist;
                            AlbumLabel.Content = track.Album;
                            MediaplayerElement.Source = trackFiles;
                            MediaplayerElement.Play();
                            historypozition--;
                        }
                    }
                }

            }

        }

        private void TrackBarSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }



        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
        private void MenuItem_Click_addToPlaylis(object sender, RoutedEventArgs e)
        {
            Track selectedTrack = DataGridLibrary.SelectedItem as Track;
            _repository.AddInPlaylist(selectedTrack.Id, 1);
        }

        private void MenuItem_Click_RemoveFromPlaylist(object sender, RoutedEventArgs e)
        {
            Track selectedTrack = DataGridPlaylis.SelectedItem as Track;
            _repository.RemoveFromPlaylist(selectedTrack.Id);
        }
        private void DataGridLibrary_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Track selectedTrack = DataGridLibrary.SelectedItem as Track;
            PlayTrack(selectedTrack.Id, selectedTrack.Name, selectedTrack.Artist, selectedTrack.Album);
            historypozition++;
        }

        private void DataGridPlaylis_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Track selectedTrack = DataGridPlaylis.SelectedItem as Track;
            PlayTrack(selectedTrack.Id, selectedTrack.Name, selectedTrack.Artist, selectedTrack.Album);
            historypozition++;
        }

        private void PlayTrack(int trackId, string trackName, string ArtistName, string albumName)
        {
            histories.Add(new History(historypozition, trackId));

            Uri trackFiles = _repository.GetTrackFiles(trackId);
            Uri artAlbum = _repository.GetTrackArt(trackId);
            AlbumArtImage.Source = new BitmapImage(artAlbum);
            TrackAndArtistLabel.Content = trackName + " - " + ArtistName;
            AlbumLabel.Content = albumName;
            MediaplayerElement.Source = trackFiles;
            MediaplayerElement.Play();

        }

        private void MediaplayerElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            TimeSpan time = MediaplayerElement.NaturalDuration.TimeSpan;
            TrackBarSlider.Minimum = 0;
            TrackBarSlider.Maximum = time.TotalSeconds;
            Timer2Label.ContentStringFormat = "{0:mm\\:ss}";
            Timer2Label.Content = time;
            TrackBarSlider.Value = 0;

            PauseButton.Visibility = Visibility.Visible;
            PlayButton.Visibility = Visibility.Collapsed;


            _playbackTimer.Start();

        }
        private void SearchTb_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var filter = (DataGridLibrary.ItemsSource as List<Track>).Where(t => t.Name.Contains(SearchTb.Text));
            if (filter != null)
                DataGridLibrary.ItemsSource = filter;
        }

    }
}
