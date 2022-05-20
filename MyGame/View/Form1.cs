using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyGame.Controller;
using MyGame.Model;
using MyGame.View;

namespace MyGame
{
    public partial class Form1 : Form
    {
        private Button AddButton;
        private Button AttackButton;
        private Button FallBackButton;
        private Button SuppliesButton;
        private Button OrderAttackButton;
        private Label PlayerMoneyLabel;
        private FlowLayoutPanel UpperInterfacePanel;
        private FlowLayoutPanel LowerInterfacePanel;
        private const int SpawnSizeConst = 60;
        public Timer UpdateTimer;
        public Timer CleanCorpsTimer;
        public Timer AddMoneyTimer;

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
        }

        public void MakeUpdateFunction()
        {
            UpdateTimer = new Timer {Interval = 50};
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

                if (Map.Trenches.Any(x => x == unit.PosX && unit.IsReadyToClean == false) && !unit.IsShooting)
                {
                    unit.IsInTrench = true;
                    unit.CurrentAnimation = 0;
                    unit.CurrentLimit = unit.IdleFrames;
                }
            }
            CheckInterface();
            PlayerMoneyLabel.Text = $"Текущее количество средств : {GameModel.PlayerMoney}";
            Invalidate();
        }

        public void CheckInterface()
        {
            if (GameModel.PlayerMoney < 25 || ButtonController.PreparingToGetSupplies)
            {
                AddButton.Enabled = false;
            }
            else
            {
                AddButton.Enabled = true;
            }
        }
        public void MakeFormBorders()
        {
            BackgroundImage = Map.MapImage;
            Height = Map.MapHeight+Interface.ButtonsHeight;
            Width = Map.MapWidth;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
        }

        public void MakeInterface()
        {
            MouseClick += ButtonController.OnMouseClick;

            LowerInterfacePanel = new FlowLayoutPanel
            {
                Location = new Point(0,Map.MapHeight - Interface.ButtonsHeight),
                Height = Interface.ButtonsHeight + 20,
                Width = Map.MapWidth,
                BackColor = Color.DarkGreen
            };
            Controls.Add(LowerInterfacePanel);

            AddButton = new Button{ Size = new Size(Interface.ButtonsWidth, Interface.ButtonsHeight),
                Location = new Point(Interface.ButtonsEmptyBorders - Interface.ButtonsWidth/2,Map.MapHeight - Interface.ButtonsHeight) };
            AddButton.Text = "Призвать резервы";
            AddButton.Click += ButtonController.AddButtonOnClick;
            AddButton.BackColor = Color.Gainsboro;
            LowerInterfacePanel.Controls.Add(AddButton);

            AttackButton = new Button
            {
                Size = new Size(Interface.ButtonsWidth, Interface.ButtonsHeight),
                Location = new Point(Interface.ButtonsEmptyBorders*2 - Interface.ButtonsWidth / 2, Map.MapHeight - Interface.ButtonsHeight)
            };
            AttackButton.Text = "В атаку!";
            AttackButton.BackColor = Color.Gainsboro;
            AttackButton.Click += ButtonController.AttackButton_Click;
            LowerInterfacePanel.Controls.Add(AttackButton);

            FallBackButton = new Button
            {
                Size = new Size(Interface.ButtonsWidth, Interface.ButtonsHeight),
                Location = new Point(Interface.ButtonsEmptyBorders * 3 - Interface.ButtonsWidth / 2, Map.MapHeight - Interface.ButtonsHeight)
            };
            FallBackButton.Text = "Отступаем!";
            FallBackButton.BackColor = Color.Gainsboro;
            FallBackButton.Click += ButtonController.FallBackButtonOnClick;
            LowerInterfacePanel.Controls.Add(FallBackButton);

            SuppliesButton = new Button
            {
                Size = new Size(Interface.ButtonsWidth, Interface.ButtonsHeight),
                Location = new Point(Interface.ButtonsEmptyBorders * 4 - Interface.ButtonsWidth / 2, Map.MapHeight - Interface.ButtonsHeight)
            };
            SuppliesButton.Text = "Стянуть подкрепления";
            SuppliesButton.Click += ButtonController.SuppliesButtonOnClick;
            SuppliesButton.BackColor = Color.Gainsboro;
            LowerInterfacePanel.Controls.Add(SuppliesButton);

            OrderAttackButton = new Button
            {
                Size = new Size(Interface.ButtonsWidth, Interface.ButtonsHeight),
                Location = new Point(Interface.ButtonsEmptyBorders * 5 - Interface.ButtonsWidth / 2, Map.MapHeight - Interface.ButtonsHeight)
            };
            OrderAttackButton.Text = "Убить кого-нибудь";
            OrderAttackButton.Click += ButtonController.OrderAttackButtonOnClick;
            OrderAttackButton.BackColor = Color.Gainsboro;
            LowerInterfacePanel.Controls.Add(OrderAttackButton);

            UpperInterfacePanel = new FlowLayoutPanel
            {
                Location = new Point(0,0),
                BackColor = Color.DarkGreen,
                Height = Interface.ButtonsHeight,
                Width = Map.MapWidth
            };
            Controls.Add(UpperInterfacePanel);

            PlayerMoneyLabel = new Label()
            {
                Text = $"Текущее количество средств : {GameModel.PlayerMoney}",
                Height = Interface.ButtonsHeight,
                Width = Interface.ButtonsWidth * 2,
                TextAlign = ContentAlignment.MiddleLeft
            };
            UpperInterfacePanel.Controls.Add(PlayerMoneyLabel);
        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            var graphics = e.Graphics;
            foreach (var unit in GameModel.AllUnits)
            {
                unit.PlayAnimation(graphics);
            }
            var mouseX = MousePosition.X - Location.X;
            var mouseY = MousePosition.Y - Location.Y - 7;
            if (ButtonController.PreparingToGetSupplies)
            {
                if (mouseY <= Interface.ButtonsHeight + SpawnSizeConst)
                {
                    graphics.DrawLine(new Pen(Color.Gold, 3f),new Point(0, Interface.ButtonsHeight + 2*SpawnSizeConst),new Point(mouseX, Interface.ButtonsHeight + 2*SpawnSizeConst));
                    ButtonController.LowerSuppliesPosition = Interface.ButtonsHeight + ViewGraphics.SpriteRectangleSize;
                    ButtonController.UpperSuppliesPosition = Interface.ButtonsHeight + 2*SpawnSizeConst;
                }
                else if(mouseY >= Map.MapHeight - Interface.ButtonsHeight - SpawnSizeConst)
                {
                    graphics.DrawLine(new Pen(Color.Gold, 3f), new Point(0, Map.MapHeight - Interface.ButtonsHeight - 2*SpawnSizeConst), new Point(mouseX, Map.MapHeight - Interface.ButtonsHeight - 2*SpawnSizeConst));
                    ButtonController.LowerSuppliesPosition = Map.MapHeight - Interface.ButtonsHeight - SpawnSizeConst*2 - ViewGraphics.SpriteRectangleSize;
                    ButtonController.UpperSuppliesPosition = Map.MapHeight - Interface.ButtonsHeight;
                }
                else
                {
                    graphics.DrawLine(new Pen(Color.Gold, 3f), new Point(0, mouseY + SpawnSizeConst), new Point(mouseX, mouseY + SpawnSizeConst));
                    graphics.DrawLine(new Pen(Color.Gold, 3f), new Point(0, mouseY - SpawnSizeConst), new Point(mouseX, mouseY - SpawnSizeConst));
                    ButtonController.UpperSuppliesPosition = mouseY + SpawnSizeConst;
                    ButtonController.LowerSuppliesPosition = mouseY - SpawnSizeConst;
                }
            }
        }

    }
}
