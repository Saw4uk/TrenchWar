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
        public static int ButtonsWidth = 100;
        public static int ButtonsHeight = 125;
        public static int ButtonsEmptyBorders = 150;
        public static PrivateFontCollection fontCollection = new PrivateFontCollection ();
        public static SoundPlayer ShootMediaPlayer = new SoundPlayer();
        public static SoundPlayer ButtonsMediaPlayer = new SoundPlayer();
        public static SoundPlayer MoneyMediaPlayer = new SoundPlayer();
        public static FontFamily SimpleFont;
        public static FontFamily BoldFont;
        public static Font MainFont;
        public static Font BigFont;
        public static Color ButtonsColor = Color.FromArgb(114, 125, 113);
        public static Color InterfaceColor = Color.FromArgb(171, 196, 171);
        public static Color EventsColor = Color.FromArgb(220, 201, 182);
        public static List<Button> GunnersButtons = new List<Button>();
        public static int CurrentTrack = 0;

        public static string Shoot =
            Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
                "Sprites\\RifleShoot.wav");

        public static string Steps =
            Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
                "Sprites\\Steps.wav");

        public static string Morze =
            Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
                "Sprites\\Morze.wav");

        public static string Money =
            Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
                "Sprites\\MoneySound.wav");

        public static string Artillery =
            Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
                "Sprites\\ArtilleryShoot.wav");

        public static string Explosive =
            Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
                "Sprites\\Explosive.wav");

        public static string[] Tracks = {
            Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "Sprites\\Track1.mp3"),
            Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "Sprites\\Track2.mp3")
        };
        
        public static void AddFonts()
        {
            fontCollection.AddFontFile(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "Sprites\\Pixel Times.ttf"));
            SimpleFont = fontCollection.Families[0];
            MainFont = new Font(SimpleFont, 10);
            BigFont = new Font(SimpleFont, 25);
        }

        public static void PlayShootSound()
        {
            LoadAsyncSound();
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
