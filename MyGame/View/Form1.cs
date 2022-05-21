using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyGame.Controller;
using MyGame.Model;
using MyGame.View;
using Color = System.Drawing.Color;
using Pen = System.Drawing.Pen;

namespace MyGame
{
    public partial class Form1 : Form
    {
        private Button LittleRiflemanSquadButton;
        private Button MiddleRiflemanSquadButton;
        private Button LargeRiflemanSquadButton;
        private Button AttackButton;
        private Button FallBackButton;
        private Button SuppliesButton;
        private PictureBox MoneyPictureBox;
        private PictureBox DocumentPictureBox;
        private Label PlayerMoneyLabel;
        private Label PlayerDocumentsLabel;
        private Label InfoLabel;
        private FlowLayoutPanel UpperInterfacePanel;
        private FlowLayoutPanel LowerInterfacePanel;
        public Timer UpdateTimer;
        public Timer CleanCorpsTimer;
        public Timer AddMoneyTimer;
        public MediaPlayer MainPlayer;

        public Form1()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            MakeInterface();
            MakeFormBorders();
            MakeUpdateFunction();
            EnemyAI.StartWar();
            MainPlayer = new MediaPlayer();
            MainPlayer.Open(new Uri(Interface.Tracks[Interface.CurrentTrack],UriKind.Relative));
            MainPlayer.Play();
            MainPlayer.MediaEnded += MainPlayerOnMediaEnded;
        }

        private void MainPlayerOnMediaEnded(object sender, EventArgs e)
        {
            if (Interface.CurrentTrack == Interface.Tracks.Length - 1)
            {
                Interface.CurrentTrack = 0;
            }
            else
            {
                Interface.CurrentTrack++;
            }
        }

        public void MakeUpdateFunction()
        {
            UpdateTimer = new Timer {Interval = 75};
            UpdateTimer.Tick += Update;
            UpdateTimer.Start();

            CleanCorpsTimer = new Timer {Interval = 30000};
            CleanCorpsTimer.Tick += GameModel.CleanCorps;
            CleanCorpsTimer.Start();

            AddMoneyTimer = new Timer { Interval = 45000 };
            AddMoneyTimer.Tick += GameModel.AddMoney;
            AddMoneyTimer.Start();
        }


        private void Update(object sender, EventArgs e)
        {

            foreach (var unit in GameModel.AllUnits)
            {
                unit.FindAndTryToKillEnemy();
            }

            foreach (var unit in GameModel.AllUnits)
            {
                if (unit.IsAlive == 0)
                {
                    unit.DieWithHonor();
                    continue;
                }

                if (unit.IsInTrench && !unit.IsShooting)
                {
                    if (unit.IsFallingBack)
                    {
                        if(unit.IsEnemy)
                            unit.Flip = -1;
                        else
                        {
                            unit.Flip = 1;
                        }
                        unit.CurrentAnimation = 0;
                        unit.IsFallingBack = false;
                    }
                    unit.CurrentAnimation = 0;
                    unit.IsAttacking = false;
                    unit.IsInTrench = true;
                    unit.CurrentLimit = unit.IdleFrames;
                }

                if (unit.IsAttacking && !unit.IsShooting)
                {
                    if(unit.IsEnemy)
                        unit.Move(-1,0);
                    else
                    {
                        unit.Move(1,0);
                    }
                }
                
                if (unit.IsFallingBack && !unit.IsShooting)
                {
                    if(unit.IsEnemy)
                        unit.Move(1,0);
                    else
                    {
                        unit.Move(-1,0);
                    }
                }

                if (Map.Trenches.Any(x => x == unit.PosX && unit.IsReadyToClean == false) && !unit.IsShooting && !unit.IsInTrench)
                {
                    unit.IsInTrench = true;
                    unit.CurrentAnimation = 0;
                    unit.CurrentFrame = 0;
                    unit.CurrentLimit = unit.IdleFrames;
                }
            }
            CheckInterface();
            PlayerMoneyLabel.Text = $"{GameModel.PlayerMoney}";
            PlayerDocumentsLabel.Text = $"{GameModel.PlayerSecretDocuments}";
            InfoLabel.Text =
                $"Ваши потери : {GameModel.PlayerUnitsKilled} ч. \nПотери противника : {GameModel.EnemyKilled} ч.";
            Invalidate();
        }

        public void CheckInterface()
        {
            if (GameModel.PlayerMoney < 25 || ButtonController.PreparingToGetSupplies)
            {
                LittleRiflemanSquadButton.Enabled = false;
            }
            else
            {
                LittleRiflemanSquadButton.Enabled = true;
            }

            if (GameModel.PlayerMoney < 35 || ButtonController.PreparingToGetSupplies)
            {
                MiddleRiflemanSquadButton.Enabled = false;
            }
            else
            {
                MiddleRiflemanSquadButton.Enabled = true;
            }

            if (GameModel.PlayerMoney < 65 || ButtonController.PreparingToGetSupplies)
            {
                LargeRiflemanSquadButton.Enabled = false;
            }
            else
            {
                LargeRiflemanSquadButton.Enabled = true;
            }
        }
        public void MakeFormBorders()
        {
            BackgroundImage = Map.MapImage;
            Height = Map.MapHeight+Interface.ButtonsHeight+ Interface.ButtonsHeight/2 - 7;
            Width = Map.MapWidth;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
        }

        public void MakeInterface()
        {
            Interface.AddFonts();
            MouseClick += ButtonController.OnMouseClick;

            LowerInterfacePanel = new FlowLayoutPanel
            {
                Location = new Point(0,Map.MapHeight),
                Height = Interface.ButtonsHeight + 20,
                Width = Map.MapWidth,
                BackColor = Interface.InterfaceColor
            };
            Controls.Add(LowerInterfacePanel);

            LittleRiflemanSquadButton = new Button{ Size = new Size(Interface.ButtonsWidth, Interface.ButtonsHeight),
            };
            LittleRiflemanSquadButton.Image = ViewGraphics.LittleRiflemanSquadImage;
            LittleRiflemanSquadButton.Click += ButtonController.LittleRiflemanButtonOnClick;
            LowerInterfacePanel.Controls.Add(LittleRiflemanSquadButton);

            MiddleRiflemanSquadButton = new Button
            {
                Size = new Size(Interface.ButtonsWidth, Interface.ButtonsHeight),
            };
            MiddleRiflemanSquadButton.Image = ViewGraphics.MiddleRiflemanSquadImage; 
            MiddleRiflemanSquadButton.Click += ButtonController.MiddleRiflemanButtonOnClick;
            LowerInterfacePanel.Controls.Add(MiddleRiflemanSquadButton);


            LargeRiflemanSquadButton = new Button
            {
                Size = new Size(Interface.ButtonsWidth, Interface.ButtonsHeight),
            };
            LargeRiflemanSquadButton.Image = ViewGraphics.LargeRiflemanSquadImage;
            LargeRiflemanSquadButton.Click += ButtonController.LargeRiflemanButtonOnClick;
            LowerInterfacePanel.Controls.Add(LargeRiflemanSquadButton);

            AttackButton = new Button
            {
                Size = new Size(Interface.ButtonsWidth, Interface.ButtonsHeight),
                Image = ViewGraphics.AttackButtonImage
            };
            AttackButton.Click += ButtonController.AttackButton_Click;
            LowerInterfacePanel.Controls.Add(AttackButton);

            FallBackButton = new Button
            {
                Size = new Size(Interface.ButtonsWidth, Interface.ButtonsHeight),
                Image = ViewGraphics.FallBackButtonImage
            };
            FallBackButton.Click += ButtonController.FallBackButtonOnClick;
            LowerInterfacePanel.Controls.Add(FallBackButton);

            SuppliesButton = new Button
            {
                Size = new Size(Interface.ButtonsWidth, Interface.ButtonsHeight),
                Image = ViewGraphics.ToMainTrenchImage

            };
            SuppliesButton.Click += ButtonController.SuppliesButtonOnClick;
            LowerInterfacePanel.Controls.Add(SuppliesButton);


            UpperInterfacePanel = new FlowLayoutPanel
            {
                Location = new Point(0,0),
                BackColor = Interface.ButtonsColor,
                Height = Interface.ButtonsHeight/2,
                Width = Map.MapWidth
            };
            Controls.Add(UpperInterfacePanel);

            MoneyPictureBox = new PictureBox 
            { 
                Height = 55,
                Width = 55,
                Image = ViewGraphics.MoneyImage
            };
            UpperInterfacePanel.Controls.Add(MoneyPictureBox);

            PlayerMoneyLabel = new Label()
            {
                Text = $"{GameModel.PlayerMoney}",
                Height = Interface.ButtonsHeight/2,
                Width = Interface.ButtonsWidth,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Interface.ButtonsColor,
                Font = Interface.BigFont
            };
            UpperInterfacePanel.Controls.Add(PlayerMoneyLabel);

            DocumentPictureBox = new PictureBox
            {
                Height = 55,
                Width = 55,
                Image = ViewGraphics.DocumentsImage
            };
            UpperInterfacePanel.Controls.Add(DocumentPictureBox);

            PlayerDocumentsLabel = new Label
            {
                Text = $"{GameModel.PlayerSecretDocuments}",
                Height = Interface.ButtonsHeight/2,
                Width = Interface.ButtonsWidth * 3,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Interface.ButtonsColor,
                Font = Interface.BigFont
            };
            UpperInterfacePanel.Controls.Add(PlayerDocumentsLabel);

            InfoLabel = new Label
            {
                Text = $"Ваши потери : {GameModel.PlayerUnitsKilled} ч. \nПотери противника : {GameModel.EnemyKilled} ч.",
                Height = Interface.ButtonsHeight / 2,
                Width = Interface.ButtonsWidth * 3,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Interface.ButtonsColor,
                Font = Interface.MainFont
            };
            UpperInterfacePanel.Controls.Add(InfoLabel);
        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            var spawnSizeConst = ButtonController.SpawnSizeConst;
            var graphics = e.Graphics;
            foreach (var unit in GameModel.AllUnits)
            {
                unit.PlayAnimation(graphics);
            }
            var mouseX = MousePosition.X - Location.X;
            var mouseY = MousePosition.Y - Location.Y - 7;
            if (ButtonController.PreparingToGetSupplies)
            {
                if (mouseY <= Interface.ButtonsHeight/2 + spawnSizeConst)
                {
                    graphics.DrawLine(new Pen(Color.Gold, 3f),new Point(0, Interface.ButtonsHeight/2 + 2*spawnSizeConst),new Point(mouseX, Interface.ButtonsHeight/2 + 2*spawnSizeConst));
                    ButtonController.LowerSuppliesPosition = Interface.ButtonsHeight + ViewGraphics.SpriteRectangleSize;
                    ButtonController.UpperSuppliesPosition = Interface.ButtonsHeight + 2*spawnSizeConst;
                }
                else if(mouseY >= Map.MapHeight + Interface.ButtonsHeight/2 - spawnSizeConst)
                {
                    graphics.DrawLine(new Pen(Color.Gold, 3f), new Point(0, Map.MapHeight + Interface.ButtonsHeight/2 - 2*spawnSizeConst), new Point(mouseX, Map.MapHeight + Interface.ButtonsHeight/2 - 2*spawnSizeConst));
                    ButtonController.LowerSuppliesPosition = Map.MapHeight - Interface.ButtonsHeight - spawnSizeConst*2 - ViewGraphics.SpriteRectangleSize;
                    ButtonController.UpperSuppliesPosition = Map.MapHeight - Interface.ButtonsHeight;
                }
                else
                {
                    graphics.DrawLine(new Pen(Color.Gold, 3f), new Point(0, mouseY + spawnSizeConst), new Point(mouseX, mouseY + spawnSizeConst));
                    graphics.DrawLine(new Pen(Color.Gold, 3f), new Point(0, mouseY - spawnSizeConst), new Point(mouseX, mouseY - spawnSizeConst));
                    ButtonController.UpperSuppliesPosition = mouseY + spawnSizeConst;
                    ButtonController.LowerSuppliesPosition = mouseY - spawnSizeConst;
                }
            }
        }

    }
}
