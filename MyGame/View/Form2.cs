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

namespace MyGame.View
{
    public partial class Form2 : Form
    {
        private Button PlayButton;
        private Button SettingsButton;
        private Button StudingButton;
        private Button PlayVideoButton;
        private Button TrainingButton;
        private Button BackButton;
        public SoundPlayer ButtonClickPlayer;
        public Form2()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
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
            throw new NotImplementedException();
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
            PlayVideoButton.Click += StudingButtonOnClick;
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

        public void BackButtonOnClick(object sender, EventArgs e)
        { 
            PlayVideoButton.Hide(); 
            TrainingButton.Hide(); 
            BackButton.Hide();
            SetFirstTypeOfInterface();
        }

        public void TrainingButtonOnClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void StudingButtonOnClick(object sender, EventArgs e)
        {
            SetSecondTypeOfInterface();
        }

        private void PlayButtonOnClick(object sender, EventArgs e)
        {
            ActiveForm?.Hide();
            var game = new Form1
            {
                Location = new Point(Location.X, Location.Y)
            };
            game.TopMost = true;
            game.ShowDialog();
            Close();
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
