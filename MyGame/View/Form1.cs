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
        public Timer UpdateTimer;
        public Timer CleanCorpsTimer;
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
            Invalidate();
        }


        public void MakeFormBorders()
        {
            BackgroundImage = Map.MapImage;
            Height = Map.MapHeight+Interface.ButtonsHeight;
            Width = Map.MapWidth;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
        }

        public void MakeInterface()
        {
            AddButton = new Button{ Size = new Size(Interface.ButtonsWidth, Interface.ButtonsHeight),
                Location = new Point(Interface.ButtonsEmptyBorders - Interface.ButtonsWidth/2,Map.MapHeight - Interface.ButtonsHeight) };
            AddButton.Text = "Призвать резервы";
            AddButton.Click += ButtonController.AddButtonOnClick;
            Controls.Add(AddButton);

            AttackButton = new Button
            {
                Size = new Size(Interface.ButtonsWidth, Interface.ButtonsHeight),
                Location = new Point(Interface.ButtonsEmptyBorders*2 - Interface.ButtonsWidth / 2, Map.MapHeight - Interface.ButtonsHeight)
            };
            AttackButton.Text = "В атаку!";
            AttackButton.Click += ButtonController.AttackButton_Click;
            Controls.Add(AttackButton);

            FallBackButton = new Button
            {
                Size = new Size(Interface.ButtonsWidth, Interface.ButtonsHeight),
                Location = new Point(Interface.ButtonsEmptyBorders * 3 - Interface.ButtonsWidth / 2, Map.MapHeight - Interface.ButtonsHeight)
            };
            FallBackButton.Text = "Отступаем!";
            FallBackButton.Click += ButtonController.FallBackButtonOnClick;
            Controls.Add(FallBackButton);

            SuppliesButton = new Button
            {
                Size = new Size(Interface.ButtonsWidth, Interface.ButtonsHeight),
                Location = new Point(Interface.ButtonsEmptyBorders * 4 - Interface.ButtonsWidth / 2, Map.MapHeight - Interface.ButtonsHeight)
            };
            SuppliesButton.Text = "Стянуть подкрепления";
            SuppliesButton.Click += ButtonController.SuppliesButtonOnClick;
            Controls.Add(SuppliesButton);

            OrderAttackButton = new Button
            {
                Size = new Size(Interface.ButtonsWidth, Interface.ButtonsHeight),
                Location = new Point(Interface.ButtonsEmptyBorders * 5 - Interface.ButtonsWidth / 2, Map.MapHeight - Interface.ButtonsHeight)
            };
            OrderAttackButton.Text = "Убить кого-нибудь";
            OrderAttackButton.Click += ButtonController.OrderAttackButtonOnClick;
            Controls.Add(OrderAttackButton);
        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            var graphics = e.Graphics;
            foreach (var unit in GameModel.AllUnits)
            {
                unit.PlayAnimation(graphics);
            }
        }

    }
}
