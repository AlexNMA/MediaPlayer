using MediaPlayer.Db;
using MediaPlayer.Models;
using System;
using System.Collections.Generic;
using System.Windows;

namespace MediaPlayer
{
    
    public partial class MainWindow : Window
    {       
       
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private Repository _repository = new Repository();
        
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            PauseButton.Visibility = Visibility.Visible;
            PlayButton.Visibility = Visibility.Collapsed;
            MediaplayerElement.Play();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            PauseButton.Visibility = Visibility.Collapsed;
            PlayButton.Visibility = Visibility.Visible;
            MediaplayerElement.Pause();
        }

        private void LibraryRB_Checked(object sender, RoutedEventArgs e)
        {
            SearchPanel.Visibility = Visibility.Visible;
            DataGridLibrary.Visibility = Visibility.Visible;
            DataGridPlaylis.Visibility = Visibility.Hidden;
            PlaylisDg.Visibility = Visibility.Hidden;
            List<Track> tracks = _repository.GetTracks();
            DataGridLibrary.ItemsSource = tracks;
        }

        private void PlayListRB_Checked(object sender, RoutedEventArgs e)
        {
            SearchPanel.Visibility = Visibility.Hidden;
            DataGridLibrary.Visibility = Visibility.Hidden;
            DataGridPlaylis.Visibility = Visibility.Visible;
            PlaylisDg.Visibility = Visibility.Visible;
            List<Playlist> playlists = _repository.GetPlaylists();
            PlaylisDg.ItemsSource = playlists;
        }

        private void MediaPlayerWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LibraryRB.IsChecked = true;
           
        }

    }
}
