using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Media;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media;

namespace MyGame.View
{
    public partial class Form2 : Form
    {
        private Button PlayButton;
        private Button SettingsButton;
        private Button StudingButton;
        public SoundPlayer MainPlayer;
        public Form2()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            MakeFormBorders();
            CreateInterface();
            Interface.PlayMusic();

        }


        public void CreateInterface()
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

        private void StudingButtonOnClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void PlayButtonOnClick(object sender, EventArgs e)
        {
            ActiveForm?.Hide();
            var game = new Form1();
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
