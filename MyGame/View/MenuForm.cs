using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Media;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media;
using AxWMPLib;
using MyGame.Controller;
using MyGame.Model;

namespace MyGame.View
{
    public partial class MenuForm : Form
    {
        private Button PlayButton;
        private Button SettingsButton;
        private Button StudingButton;
        private Button PlayVideoButton;
        private Button TrainingButton;
        private Button BackButton;
        public MenuForm()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            axWindowsMediaPlayer1.Hide();
            MakeFormBorders();
            SetFirstTypeOfInterface();
            Interface.PlayMusic();

        }


        public void SetFirstTypeOfInterface()
        {
            PlayButton = new Button
            {
                Location = new Point(447,285),
                Size = ViewGraphics.PlayButtonImage.Size,
                Image = ViewGraphics.PlayButtonImage
            };
            PlayButton.Click += PlayButtonOnClick;
            Controls.Add(PlayButton);

            StudingButton = new Button
            {
                Location = new Point(447, PlayButton.Location.Y + 160),
                Size = ViewGraphics.StudingButtonImage.Size,
                Image = ViewGraphics.StudingButtonImage
            };
            StudingButton.Click += StudingButtonOnClick;
            Controls.Add(StudingButton);

            SettingsButton = new Button
            {
                Location = new Point(447, StudingButton.Location.Y + 160),
                Size = ViewGraphics.SettingsButtonImage.Size,
                Image = ViewGraphics.SettingsButtonImage
            };
            SettingsButton.Click += SettingsButtonOnClick;
            Controls.Add(SettingsButton);
        }

        private void SettingsButtonOnClick(object sender, EventArgs e)
        {
            Interface.PlayClickSound();
        }

        private void SetSecondTypeOfInterface()
        {
            PlayButton.Hide();
            StudingButton.Hide();
            SettingsButton.Hide();
            PlayVideoButton = new Button
            {
                Location = new Point(447, 285),
                Size = ViewGraphics.MovieButtonImage.Size,
                Image = ViewGraphics.MovieButtonImage
            };
            PlayVideoButton.Click += PlayVideoButtonOnClick;
            Controls.Add(PlayVideoButton);

            TrainingButton = new Button
            {
                Location = new Point(447, PlayVideoButton.Location.Y + 160),
                Size = ViewGraphics.TrainingButtonImage.Size,
                Image = ViewGraphics.TrainingButtonImage
            };
            TrainingButton.Click += TrainingButtonOnClick;
            Controls.Add(TrainingButton);

            BackButton = new Button
            {
                Location = new Point(447, TrainingButton.Location.Y + 160),
                Size = ViewGraphics.BackButtonImage.Size,
                Image = ViewGraphics.BackButtonImage
            };
            BackButton.Click += BackButtonOnClick;
            Controls.Add(BackButton);
        }

        private void PlayVideoButtonOnClick(object sender, EventArgs e)
        {
            Interface.MainPlayer.Pause();
            axWindowsMediaPlayer1.openPlayer(ViewGraphics.FullVideo);
            Interface.PlayClickSound();
        }

        public void BackButtonOnClick(object sender, EventArgs e)
        {
            Interface.MainPlayer.Play();
            Interface.PlayClickSound();
            PlayVideoButton.Hide(); 
            TrainingButton.Hide(); 
            BackButton.Hide();
            SetFirstTypeOfInterface();
        }

        public void TrainingButtonOnClick(object sender, EventArgs e)
        {
            Interface.PlayClickSound();
        }

        private void StudingButtonOnClick(object sender, EventArgs e)
        {
            Interface.PlayClickSound();
            SetSecondTypeOfInterface();
        }

        private void PlayButtonOnClick(object sender, EventArgs e)
        {
            Interface.PlayClickSound();
            var gameModel = new GameModel();
            var buttonController = new ButtonController(gameModel);
            var enemyAi = new EnemyAI(gameModel, buttonController);
            var game = new Form1(gameModel,buttonController,enemyAi)
            {
                Location = new Point(Location.X, Location.Y)
            };
            game.Top = Top;
            game.Left = Left;
            game.Show();
            Hide();
        }

        public void MakeFormBorders()
        {
            BackgroundImage = ViewGraphics.SecondFormImage;
            Height = ViewGraphics.SecondFormImage.Height+7;
            Width = ViewGraphics.SecondFormImage.Width + 20;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
        }
    }
}
