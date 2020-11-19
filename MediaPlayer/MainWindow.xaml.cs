using System;
using System.Windows;

namespace MediaPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            PauseButton.Visibility = Visibility.Visible;
            PlayButton.Visibility = Visibility.Collapsed;
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            PauseButton.Visibility = Visibility.Collapsed;
            PlayButton.Visibility = Visibility.Visible;
        }

        private void LibraryRB_Checked(object sender, RoutedEventArgs e)
        {
            SearchPanel.Visibility = Visibility.Visible;
            DataGridLibrary.Visibility = Visibility.Visible;
            DataGridPlaylis.Visibility = Visibility.Hidden;
            PlayListLB.Visibility = Visibility.Hidden;
        }

        private void PlayListRB_Checked(object sender, RoutedEventArgs e)
        {
            SearchPanel.Visibility = Visibility.Hidden;
            DataGridLibrary.Visibility = Visibility.Hidden;
            DataGridPlaylis.Visibility = Visibility.Visible;
            PlayListLB.Visibility = Visibility.Visible;
        }

        private void MediaPlayerWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LibraryRB.IsChecked = true;
        }
    }
}
