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

        private readonly GameModel GameModel;
        private readonly ButtonController ButtonController;
        private readonly EnemyAI EnemyAI;
        private List<KeyValuePair<Entity, Button>> GunnersButtons = new List<KeyValuePair<Entity, Button>>();
        private Button LittleRiflemanSquadButton;
        private Button MiddleRiflemanSquadButton;
        private Button LargeRiflemanSquadButton;
        private Button LittleGunnerSquadButton;
        private Button LargeGunnerSquadButton;
        private Button AttackButton;
        private Button FallBackButton;
        private Button SuppliesButton;
        private Button ArtilleryFireButton;
        private Button ArtilleryThreeFireButton;
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

        public Form1(GameModel gameModel, ButtonController buttonController, EnemyAI enemyAi)
        {
            GameModel = gameModel;
            ButtonController = buttonController;
            EnemyAI = enemyAi;
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            MakeInterface();
            MakeFormBorders();
            MakeUpdateFunction();
            GameModel.DrawArtillery();
            EnemyAI.StartWar();
            Interface.CurrentTrack = 0;
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
            Button buttonToAdd;
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

                if(unit.PosY != unit.OrderedPosition && !unit.IsShooting)
                    unit.MoveToOrderedPosition();

                if (Map.Trenches.Any(x => x == unit.PosX && unit.IsReadyToClean == false) && !unit.IsShooting && !unit.IsInTrench)
                {
                    unit.IsInTrench = true;
                    unit.CurrentAnimation = 0;
                    unit.CurrentFrame = 0;
                    unit.CurrentLimit = unit.IdleFrames;
                }

                if (unit.IsGunner && !unit.IsButtonAddedToControls)
                {
                    buttonToAdd = new Button
                    {
                        Location = new Point(unit.PosX - ViewGraphics.SpriteRectangleSize - 5,
                            unit.PosY + 10),
                        Size = new Size(20, 20),
                        BackColor = Interface.EventsColor,
                        Image = ViewGraphics.XPosChangeImage
                    };
                    buttonToAdd.Click += (o, args) =>
                    {
                        GameModel.GunnerWaitOrders = true;
                        GameModel.CurrentGunner = unit;
                    };
                    Controls.Add(buttonToAdd);
                    GunnersButtons.Add(new KeyValuePair<Entity, Button>(unit,buttonToAdd));
                    unit.IsButtonAddedToControls = true;
                }
            }

            foreach (var pair in GunnersButtons)
            {
                var unit = pair.Key;
                pair.Value.Location = new Point(unit.PosX - ViewGraphics.SpriteRectangleSize - 5,
                    unit.PosY + 10);
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
            if (GameModel.PlayerMoney < 25 || GameModel.PreparingToGetSupplies)
            {
                LittleRiflemanSquadButton.Enabled = false;
            }
            else
            {
                LittleRiflemanSquadButton.Enabled = true;
            }

            if (GameModel.PlayerMoney < 35 || GameModel.PreparingToGetSupplies)
            {
                MiddleRiflemanSquadButton.Enabled = false;
            }
            else
            {
                MiddleRiflemanSquadButton.Enabled = true;
            }

            if (GameModel.PlayerMoney < 65 || GameModel.PreparingToGetSupplies)
            {
                LargeRiflemanSquadButton.Enabled = false;
            }
            else
            {
                LargeRiflemanSquadButton.Enabled = true;
            }

            if (GameModel.PlayerMoney < 50 || GameModel.PlayerSecretDocuments < 1 || GameModel.PreparingToGetSupplies)
            {
                LittleGunnerSquadButton.Enabled = false;
            }
            else
            {
                LittleGunnerSquadButton.Enabled = true;
            }

            if (GameModel.PlayerMoney < 100 || GameModel.PlayerSecretDocuments < 1 || GameModel.PreparingToGetSupplies)
            {
                LargeGunnerSquadButton.Enabled = false;
            }
            else
            {
                LargeGunnerSquadButton.Enabled = true;
            }

            if (GameModel.PlayerArtillery.IsShooting || GameModel.PlayerMoney < 50 || GameModel.PlayerSecretDocuments < 1)
            {
                ArtilleryFireButton.Enabled = false;
            }
            else
            {
                ArtilleryFireButton.Enabled = true;
            }

            if (GameModel.PlayerArtillery.IsShooting || GameModel.PlayerMoney < 100 || GameModel.PlayerSecretDocuments < 2)
            {
                ArtilleryThreeFireButton.Enabled = false;
            }
            else
            {
                ArtilleryThreeFireButton.Enabled = true;
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

            Interface.ShootMediaPlayer.LoadCompleted += Interface.ShootMediaPlayerOnLoadCompleted;

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

            LittleGunnerSquadButton = new Button
            {
                Size = new Size(Interface.ButtonsWidth, Interface.ButtonsHeight),
            };
            LittleGunnerSquadButton.Image = ViewGraphics.LittleGunnerSquadImage;
            LittleGunnerSquadButton.Click += ButtonController.LittleGunnerSquadButtonOnClick;
            LowerInterfacePanel.Controls.Add(LittleGunnerSquadButton);

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

            LargeGunnerSquadButton = new Button
            {
                Size = new Size(Interface.ButtonsWidth, Interface.ButtonsHeight),
            };
            LargeGunnerSquadButton.Image = ViewGraphics.LargeGunnerSquad;
            LargeGunnerSquadButton.Click += ButtonController.LargeGunnerSquadButtonOnClick;
            LowerInterfacePanel.Controls.Add(LargeGunnerSquadButton);

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

            ArtilleryFireButton = new Button
            {
                Size = new Size(Interface.ButtonsWidth, Interface.ButtonsHeight),
                Image = ViewGraphics.ArtilleryOneShoot
            };
            ArtilleryFireButton.Click += ButtonController.ArtilleryFireButtonOnClick;
            LowerInterfacePanel.Controls.Add(ArtilleryFireButton);

            ArtilleryThreeFireButton = new Button
            {
                Size = new Size(Interface.ButtonsWidth, Interface.ButtonsHeight),
                Image = ViewGraphics.ArtilleryThreeShoots
            };
            ArtilleryThreeFireButton.Click += ButtonController.ArtilleryThreeFireButtonOnClick;
            LowerInterfacePanel.Controls.Add(ArtilleryThreeFireButton);

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
            var spawnSizeConst = GameModel.SpawnSizeConst;
            var graphics = e.Graphics;

            GameModel.PlayerArtillery.PlayAnimation(graphics);
            GameModel.PlayerArtillery.Explosive.PlayAnimation(graphics);
            foreach (var unit in GameModel.AllUnits)
            {
                unit.PlayAnimation(graphics);
            }

            var mouseX = MousePosition.X - Location.X;
            var mouseY = MousePosition.Y - Location.Y - 7;
            if (GameModel.PreparingToGetSupplies)
            {
                if (mouseY <= Interface.ButtonsHeight/2 + spawnSizeConst)
                {
                    graphics.DrawLine(new Pen(Color.Gold, 3f),new Point(0, Interface.ButtonsHeight/2 + 2*spawnSizeConst),new Point(mouseX, Interface.ButtonsHeight/2 + 2*spawnSizeConst));
                    GameModel.LowerSuppliesPosition = Interface.ButtonsHeight + ViewGraphics.SpriteRectangleSize;
                    GameModel.UpperSuppliesPosition = Interface.ButtonsHeight + 2*spawnSizeConst;
                }
                else if(mouseY >= Map.MapHeight + Interface.ButtonsHeight/2 - spawnSizeConst)
                {
                    graphics.DrawLine(new Pen(Color.Gold, 3f), new Point(0, Map.MapHeight + Interface.ButtonsHeight/2 - 2*spawnSizeConst), new Point(mouseX, Map.MapHeight + Interface.ButtonsHeight/2 - 2*spawnSizeConst));
                    GameModel.LowerSuppliesPosition = Map.MapHeight - Interface.ButtonsHeight - spawnSizeConst*2 - ViewGraphics.SpriteRectangleSize;
                    GameModel.UpperSuppliesPosition = Map.MapHeight - Interface.ButtonsHeight;
                }
                else
                {
                    graphics.DrawLine(new Pen(Color.Gold, 3f), new Point(0, mouseY + spawnSizeConst), new Point(mouseX, mouseY + spawnSizeConst));
                    graphics.DrawLine(new Pen(Color.Gold, 3f), new Point(0, mouseY - spawnSizeConst), new Point(mouseX, mouseY - spawnSizeConst));
                    GameModel.UpperSuppliesPosition = mouseY + spawnSizeConst;
                    GameModel.LowerSuppliesPosition = mouseY - spawnSizeConst;
                }
            }

            if (GameModel.GunnerWaitOrders)
            {
                if (mouseY >= Interface.ButtonsHeight / 2 && mouseY <= Map.MapHeight + Interface.ButtonsHeight)
                {
                    graphics.DrawLine(new Pen(Color.Gold, 3f),
                        new Point(0, mouseY),
                        new Point(mouseX, mouseY));
                    GameModel.GunnerNewPosition = mouseY;
                }
            }

            if (GameModel.IsWaitingForArtilleryFire)
            {
                graphics.DrawLine(new Pen(Color.Red, 3f), new Point(mouseX - 50, mouseY), new Point(mouseX + 50, mouseY));
                graphics.DrawLine(new Pen(Color.Red, 3f), new Point(mouseX, mouseY + 50), new Point(mouseX, mouseY - 50));
                GameModel.PlayerArtillery.FireTarget = new Point(mouseX, mouseY);
            }
        }

    }
}
