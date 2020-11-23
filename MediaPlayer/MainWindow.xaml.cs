using MediaPlayer.Db;
using MediaPlayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace MediaPlayer
{

    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            QueueManager = new QueueManager();
            _playbackTimer = new DispatcherTimer();
            _playbackTimer.Interval = TimeSpan.FromSeconds(1);
            _playbackTimer.Tick += _playbackTimer_Tick;
            Timer1Label.ContentStringFormat = "{0:mm\\:ss}";
            _queueState = true;
            this.DataContext = this;
        }

        private bool _queueState;
        private Repository _repository = new Repository();
        private DispatcherTimer _playbackTimer;

        private QueueManager _queueManager;
        public QueueManager QueueManager { get => _queueManager; set => _queueManager = value; }

        private void _playbackTimer_Tick(object sender, EventArgs e)
        {
            TrackBarSlider.Value = MediaplayerElement.Position.TotalSeconds;
            Timer1Label.Content = MediaplayerElement.Position.Duration();

        }

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

        private void PreviousTrackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MediaplayerElement.Position.TotalSeconds > 5)
            {
                MediaplayerElement.Position = TimeSpan.Zero;
            }
            else
            {
                PlayTrack(QueueManager.PreviousTrack());
            }

        }

        private void NextTrackBtn_Click(object sender, RoutedEventArgs e)
        {
            PlayTrack(QueueManager.NextTrack());
        }

        private void LibraryRB_Checked(object sender, RoutedEventArgs e)
        {

            DataGridLibrary.Visibility = Visibility.Visible;
            DataGridPlaylis.Visibility = Visibility.Hidden;
            PlaylistLbox.Visibility = Visibility.Hidden;
            List<Track> tracks = _repository.GetTracks();
            DataGridLibrary.ItemsSource = tracks;
        }

        private void PlayListRB_Checked(object sender, RoutedEventArgs e)
        {

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
            PlayTrack(selectedTrack);

            List<Track> tracks = DataGridLibrary.ItemsSource as List<Track>;

            List<Track> nextTracks = tracks.Skip(DataGridLibrary.SelectedIndex + 1).ToList();
            List<Track> previousTracks = tracks.Take(DataGridLibrary.SelectedIndex).ToList();
            QueueManager.SetQueue(nextTracks, previousTracks);
        }

        private void DataGridPlaylis_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Track selectedTrack = DataGridPlaylis.SelectedItem as Track;
            PlayTrack(selectedTrack);

            List<Track> tracks = DataGridPlaylis.ItemsSource as List<Track>;

            List<Track> nextTracks = tracks.Skip(DataGridPlaylis.SelectedIndex + 1).ToList();
            List<Track> previousTracks = tracks.Take(DataGridLibrary.SelectedIndex).ToList();
            QueueManager.SetQueue(nextTracks, previousTracks);
        }

        private void QueueGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            
        }

        private void PlayTrack(Track track)
        {
            if (track == null)
                return;
            Uri trackFiles = _repository.GetTrackFiles(track.Id);
            Uri artAlbum = _repository.GetTrackArt(track.Id);
            AlbumArtImage.Source = new BitmapImage(artAlbum);
            TrackAndArtistLabel.Content = track.Name + " - " + track.Artist;
            AlbumLabel.Content = track.Album;
            MediaplayerElement.Source = trackFiles;
            MediaplayerElement.Play();
            QueueManager.CurrentTrack = track;
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
            if (LibraryRB.IsChecked == true)
            {
                var filter = (DataGridLibrary.ItemsSource as IEnumerable<Track>).Where(t => t.Name.Contains(SearchTb.Text));
                if (filter != null)
                    DataGridLibrary.ItemsSource = filter;
            }
            else if (PlayListRB.IsChecked == true)
            {
                var filter = (DataGridPlaylis.ItemsSource as List<Track>).Where(t => t.Name.Contains(SearchTb.Text));
                if (filter != null)
                    DataGridPlaylis.ItemsSource = filter;
            }
        }

        private void QueueButton_Click(object sender, RoutedEventArgs e)
        {
            if (_queueState == false)
            {

                QueueGrid.Visibility = Visibility.Visible;
                _queueState = true;
            }
            else
            {
                QueueGrid.Visibility = Visibility.Hidden;
                _queueState = false;
            }

        }

        private void TrackBarSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void MenuItem_Click_RemoveFromQueue(object sender, RoutedEventArgs e)
        {
            QueueManager.RemoveFromQueue(QueueGrid.SelectedItem as Track);
        }

        private void MenuItem_Click_AddToQueuePlaylis(object sender, RoutedEventArgs e)
        {
            Track selectedTrack = DataGridPlaylis.SelectedItem as Track;
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MediaplayerElement.Volume = VolumeSlider.Value;
        }
    }
}
