using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace VideoPlayer
{
    public partial class MainWindow : Window
    {
        public static DispatcherTimer vTimer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            vTimer.Interval = TimeSpan.FromMilliseconds(1);
            vTimer.Tick += new EventHandler(timerTick);
        }
        public void timerTick(object sender, EventArgs e)
        {
            mySlider.Value = myMedia.Position.TotalMilliseconds;
            this.Title = $"{myMedia.Position.Hours}:{myMedia.Position.Minutes}:{myMedia.Position.Seconds}";
        }
        private void openFile(object sender, RoutedEventArgs e)
        {
            string filePath = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "avi files (*.avi)|*.avio|All files (*.*)|*.*";
            openFileDialog.ShowDialog();
            filePath = openFileDialog.FileName;
            if(filePath!=null)
                myMedia.Source = new Uri(filePath);
            myMedia.LoadedBehavior = MediaState.Manual;
            myMedia.Play();
            vTimer.Start();
        }
        private void Play(object sender, RoutedEventArgs e)
        {
            if (myMedia.Source != null)
            {
                myMedia.Play();
                vTimer.Start();
            }
        }
        private void Pause(object sender, RoutedEventArgs e)
        {
            if (myMedia.Source != null)
            {
                myMedia.Pause();
                vTimer.Stop();
            }
        }
        private void Stop(object sender, RoutedEventArgs e)
        {
            if (myMedia.Source != null)
            {
                myMedia.Stop();
                vTimer.Stop();
                mySlider.Value = 0;
                this.Title = "VideoPlayer";
            }
        }

        private void myMedia_MediaOpened(object sender, RoutedEventArgs e)
        {
            mySlider.Minimum = 0;
            mySlider.Maximum = myMedia.NaturalDuration.TimeSpan.TotalMilliseconds;
        }

        private void mySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(Mouse.LeftButton==MouseButtonState.Pressed)
            {
                vTimer.Stop();
                myMedia.Pause();
                myMedia.Position = TimeSpan.FromMilliseconds(mySlider.Value);
                vTimer.Start();
                myMedia.Play();
            }
        }
    }
}
