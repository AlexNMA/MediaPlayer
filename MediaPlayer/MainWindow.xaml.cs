using MediaPlayer.Db;
using MediaPlayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace MediaPlayer
{

    public partial class MainWindow : Window
    {
        int previous = 1;
        public MainWindow()
        {
            InitializeComponent();
            MediaplayerElement.Source = new Uri("C:/tracksFolder/Skillet - Save Me.mp3");
            
            _playbackTimer = new DispatcherTimer();
            _playbackTimer.Interval = TimeSpan.FromSeconds(1);
            _playbackTimer.Tick += _playbackTimer_Tick;
        }

        private void _playbackTimer_Tick(object sender, EventArgs e)
        {
            TrackBarSlider.Value = MediaplayerElement.Position.TotalSeconds;
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
            MediaplayerElement.Stop();
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
            if (previous == 1)
            {
                MediaplayerElement.Position = TimeSpan.Zero;
                previous++;
            }
            else
            {
                previous = 1;
            }

        }

        private void TrackBarSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TrackBarSlider.Maximum = MediaplayerElement.Position.TotalSeconds;
            MediaplayerElement.Position = new TimeSpan((int)TrackBarSlider.Value);
        }



        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DataGridLibrary_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Track selectedTrack = DataGridLibrary.SelectedItem as Track;
            PlayTrack(selectedTrack.Id);
        }

        private void DataGridPlaylis_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Track selectedTrack = DataGridPlaylis.SelectedItem as Track;
            PlayTrack(selectedTrack.Id);
        }

        private void PlayTrack(int trackId)
        {
            Uri trackFiles = _repository.GetTrackFiles(trackId);
            
            TimeSpan time = MediaplayerElement.NaturalDuration.TimeSpan;
            TrackBarSlider.Minimum = 0;
            TrackBarSlider.Maximum = time.TotalSeconds;
            TrackBarSlider.Value = 0;
            
            PauseButton.Visibility = Visibility.Visible;
            PlayButton.Visibility = Visibility.Collapsed;

            MediaplayerElement.Source = trackFiles;
            MediaplayerElement.Play();
            
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
