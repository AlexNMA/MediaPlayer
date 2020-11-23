using MediaPlayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MediaPlayer
{
    public class QueueManager : INotifyPropertyChanged
    {
        public QueueManager()
        {

        }

        private List<Track> history;
        public List<Track> History { get => history; set => history = value; }

        private ObservableCollection<Track> queue;
        public ObservableCollection<Track> Queue
        {
            get => queue; set
            {
                queue = value;
                OnPropertyChanged("Queue");
            }
        }

        private Track currentTrack;
        public Track CurrentTrack
        {
            get { return currentTrack; }
            set { currentTrack = value; OnPropertyChanged("CurrentTrack"); }
        }


        public void Enqueue(Track track)
        {
            Queue.Add(track);
        }

        public void SetQueue(IEnumerable<Track> tracks, List<Track> history)
        {
            Queue = new ObservableCollection<Track>(tracks);
            History = history;
        }

        public Track CurrentTrackEnded()
        {
            if (CurrentTrack != null)
            {
                History.Add(CurrentTrack);
            }

            Track nextTrack = Queue.FirstOrDefault();
            if (nextTrack != null)
            {
                Queue.RemoveAt(0);
            }
            return nextTrack;
        }

        public Track NextTrack()
        {
            return CurrentTrackEnded();
        }

        public Track PreviousTrack()
        {
            Track lastTrack = History.LastOrDefault();
            if (lastTrack != null)
            {
                History.RemoveAt(History.Count - 1);
                
                Queue.Insert(0, currentTrack);
            }
            return lastTrack;
        }

        public void RemoveFromQueue(Track track)
        {
            Queue.Remove(track);
        }

        #region INotifyPropertyChanged Members
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
