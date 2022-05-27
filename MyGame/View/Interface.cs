using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Media;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Color = System.Drawing.Color;
using FontFamily = System.Drawing.FontFamily;

namespace MyGame.View
{
    public static class Interface
    {
        public static string CurrentDirectory =
            new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        public static int ButtonsWidth = 100;
        public static int ButtonsHeight = 125;
        public static PrivateFontCollection fontCollection = new PrivateFontCollection ();
        public static SoundPlayer ShootMediaPlayer = new SoundPlayer();
        public static SoundPlayer ButtonsMediaPlayer = new SoundPlayer();
        public static SoundPlayer MoneyMediaPlayer = new SoundPlayer();
        public static FontFamily SimpleFont;
        public static Font MainFont;
        public static Font BigFont;
        public static MediaPlayer MainPlayer;
        public static Color ButtonsColor = Color.FromArgb(114, 125, 113);
        public static Color InterfaceColor = Color.FromArgb(171, 196, 171);
        public static Color EventsColor = Color.FromArgb(220, 201, 182);
        public static int CurrentTrack;

        public static string Shoot =
            Path.Combine(CurrentDirectory,
                "Sprites\\RifleShoot.wav");

        public static string Click =
            Path.Combine(CurrentDirectory,
                "Sprites\\Click.wav");

        public static string Steps =
            Path.Combine(CurrentDirectory,
                "Sprites\\Steps.wav");

        public static string Morze =
            Path.Combine(CurrentDirectory,
                "Sprites\\Morze.wav");

        public static string Money =
            Path.Combine(CurrentDirectory,
                "Sprites\\MoneySound.wav");

        public static string Artillery =
            Path.Combine(CurrentDirectory,
                "Sprites\\ArtilleryShoot.wav");

        public static string Explosive =
            Path.Combine(CurrentDirectory,
                "Sprites\\Explosive.wav");

        public static string Menu =
            Path.Combine(CurrentDirectory,
                "Sprites\\Menu.wav");

        public static string[] Tracks = {
            Path.Combine(CurrentDirectory, "Sprites\\Track6.mp3"),
            Path.Combine(CurrentDirectory, "Sprites\\Track7.mp3"),
            Path.Combine(CurrentDirectory, "Sprites\\Track3.mp3"),
            Path.Combine(CurrentDirectory, "Sprites\\Track4.mp3"),
            Path.Combine(CurrentDirectory, "Sprites\\Track5.mp3")
        };

        public static void PlayMusic()
        {
            MainPlayer = new MediaPlayer();
            MainPlayer.Open(new Uri(Tracks[CurrentTrack], UriKind.Relative));
            MainPlayer.Play();
            MainPlayer.MediaEnded += MainPlayerOnMediaEnded;
        }

        private static void MainPlayerOnMediaEnded(object sender, EventArgs e)
        {
            if (CurrentTrack == Tracks.Length - 1)
            {
                CurrentTrack = 0;
                MainPlayer.Open(new Uri(Tracks[CurrentTrack], UriKind.Relative));
                MainPlayer.Play();
            }
            else
            {
                CurrentTrack++;
                MainPlayer.Open(new Uri(Tracks[CurrentTrack], UriKind.Relative));
                MainPlayer.Play();
            }
        }
        public static void AddFonts()
        {
            fontCollection.AddFontFile(Path.Combine(CurrentDirectory, "Sprites\\Pixel Times.ttf"));
            SimpleFont = fontCollection.Families[0];
            MainFont = new Font(SimpleFont, 10);
            BigFont = new Font(SimpleFont, 25);
        }

        public static void PlayShootSound()
        {
            LoadAsyncSound();
        }

        public static void PlayClickSound()
        {
            ButtonsMediaPlayer.SoundLocation = Click;
            ButtonsMediaPlayer.Play();
        }

        public static void LoadAsyncSound()
        {
            try
            {
                // Replace this file name with a valid file name.
                ShootMediaPlayer.SoundLocation = Shoot;
                ShootMediaPlayer.LoadAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error loading sound");
            }
        }

        public static void ShootMediaPlayerOnLoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (ShootMediaPlayer.IsLoadCompleted)
            {
                try
                {
                    ShootMediaPlayer.Play();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error playing sound");
                }
            }
        }


        public static void PlayMorzeSound()
        {
            ButtonsMediaPlayer.SoundLocation = Morze;
            ButtonsMediaPlayer.Play();
        }

        public static void PlayStepsSound()
        {
            ButtonsMediaPlayer.SoundLocation = Steps;
            ButtonsMediaPlayer.Play();
        }

        public static void PlayMoneySound()
        {
            MoneyMediaPlayer.SoundLocation = Money;
            MoneyMediaPlayer.Play();
        }

        public static void PlayArtilleryShootSound()
        {
            MoneyMediaPlayer.SoundLocation = Artillery;
            MoneyMediaPlayer.Play();
        }

        public static void PlayExplosiveSound()
        {
            MoneyMediaPlayer.SoundLocation = Explosive;
            MoneyMediaPlayer.Play();
        }
    }
}
