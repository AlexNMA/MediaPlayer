using MediaPlayer.Db;
using MediaPlayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;

namespace MediaPlayer
{

    public partial class MainWindow : Window
    {
        int previous = 1;
        public MainWindow()
        {
            InitializeComponent();

        }

        private Repository _repository = new Repository();

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            PauseButton.Visibility = Visibility.Visible;
            PlayButton.Visibility = Visibility.Collapsed;
            // MediaplayerElement.Play();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            PauseButton.Visibility = Visibility.Collapsed;
            PlayButton.Visibility = Visibility.Visible;
            // MediaplayerElement.Pause();
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

        private void MediaPlayerWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LibraryRB.IsChecked = true;
        }

        private void PlaylistLbox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            List<Track> tracks = _repository.GetTracksFromPlaylist(PlaylistLbox.SelectedValue);


            DataGridPlaylis.DisplayMemberPath = "Name";
            DataGridPlaylis.SelectedValuePath = "Id";
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

        }

        

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void DataGridLibrary_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void SearchTb_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var filter = (DataGridLibrary.ItemsSource as List<Track>).Where(t => t.Name.Contains(SearchTb.Text));
            if (filter != null)
                DataGridLibrary.ItemsSource = filter;
        }
    }
}
