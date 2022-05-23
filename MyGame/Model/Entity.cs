using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyGame.Controller;
using MyGame.View;

namespace MyGame.Model
{
    public class Entity
    {
        public int PosX;
        public int PosY;
        public Point Location => new Point(PosX, PosY);
        public Button GunnerButton;
        public Image SpriteList;
        public int IdleFrames;
        public int AttackFrames;
        public int RunFrames;
        public int DeadFrames;
        public int OrderedPosition;
        public int IsAlive = 1;
        public bool IsEnemy;
        public bool IsReadyToClean;
        public bool IsAttacking;
        public bool IsShooting;
        public bool IsButtonAddedToControls;
        public bool IsFallingBack;
        public bool IsGunner;
        public bool IsInTrench = true;
        public int Flip = 1;
        public int FireRange = 130;
        public int PercentOfHit = 70;
        public int CurrentAnimation;
        public int CurrentFrame;
        public int CurrentLimit;

        public void PlayAnimation(Graphics graphics)
        {
            if (CurrentAnimation == 2 && CurrentFrame == AttackFrames - 1)
            {
                CurrentFrame = 0;
                if (IsInTrench)
                {
                    CurrentAnimation = 0;
                    CurrentLimit = IdleFrames;
                    IsShooting = false;
                }
                else
                {
                    CurrentAnimation = 1;
                    CurrentLimit = RunFrames;
                    IsShooting = false;
                }
            }
            graphics.DrawImage(
                SpriteList,
                new Rectangle(
                    new Point(PosX - Flip*ViewGraphics.SpriteRectangleSize/2,PosY), 
                    new Size(ViewGraphics.SpriteRectangleSize*Flip,ViewGraphics.SpriteRectangleSize)),
                ViewGraphics.SpriteRectangleSize * CurrentFrame,
                ViewGraphics.SpriteRectangleSize * CurrentAnimation,
                ViewGraphics.SpriteRectangleSize,
                ViewGraphics.SpriteRectangleSize,
                GraphicsUnit.Pixel);
            if (CurrentFrame < CurrentLimit - 1 && IsReadyToClean == false)
                CurrentFrame++;
            else if (IsReadyToClean == false)
            {
                CurrentFrame = 0;
            }
        }
        public void Move(int dx, int dy)
        {
            PosX += dx;
            PosY += dy;
        }

        public void MoveToOrderedPosition()
        {
            if (!IsInTrench) return;
            if (OrderedPosition > PosY)
                Move(0, 1);
            else if (OrderedPosition < PosY)
            {
                Move(0, -1);
            }
        }

        public void MoveToNextTrench(bool ShouldGunnersGo = false)
        {
            if(IsGunner && !ShouldGunnersGo)
                return;
            if (IsShooting) return;
            CurrentAnimation = 1;
            CurrentLimit = RunFrames;
            if (IsEnemy)
                Flip = -1;
            else
            {
                Flip = 1;
            }

            IsInTrench = false;
            IsFallingBack = false;
            IsAttacking = true;
        }

        public void FallBack()
        {
            if (IsShooting) return;
            CurrentAnimation = 1;
            CurrentLimit = RunFrames;
            if (IsEnemy)
                Flip = 1;
            else
            {
                Flip = -1;
            }
            IsInTrench = false;
            IsAttacking = false;
            IsFallingBack = true;
        }

        public void MoveToAllyTrench(int maxTrenchCord)
        {
            if (PosX != maxTrenchCord)
                MoveToNextTrench(true);
        }

        public void FindAndTryToKillEnemy()
        {
            if(IsShooting || IsAlive == 0)
                return;
            if(IsGunner && !IsInTrench)
                return;
            var enemyUnits = (IsEnemy) ? GameModel.PlayerUnits : GameModel.EnemyUnits;
            var target = enemyUnits.FirstOrDefault(unit => GameModel.GetDistance(Location, unit.Location) <= FireRange && unit.IsAlive == 1);
            if (target != null)
            {
                double currentPercentOfHit = PercentOfHit;
                if (target.IsInTrench)
                    currentPercentOfHit /= 1.5;
                if(target.IsGunner) 
                    currentPercentOfHit -= 15;
                PlayShootAnimation();
                if (target.CheckHit(currentPercentOfHit))
                {
                    target.IsAlive = 0;
                    target.MakeAllLogicFalse();
                    if (target.IsEnemy)
                    {
                        GameModel.EnemyKilled++;
                        GameModel.PlayerMoney += 5;
                    }
                    else
                    {
                        GameModel.PlayerUnitsKilled++;
                        GameModel.PlayerMoney += 2;
                    }
                }
            }
        }
        public bool CheckHit(double percent)
        {
            return percent >= GameModel.HitRandom.Next(0, 100);
        }

        public void DieWithHonor()
        {
            if (CurrentAnimation == 3 && CurrentFrame == DeadFrames - 1)
            {
                IsReadyToClean = true;
            }
            else
            {
                CurrentAnimation = 3;
                CurrentLimit = DeadFrames;
                IsInTrench = false;
                IsAttacking = false;
                IsShooting = false;
                IsFallingBack = false;
            }
        }

        public void PlayShootAnimation()
        {
            Interface.PlayShootSound();
            IsShooting = true;
            CurrentAnimation = 2;
            CurrentFrame = 0;
            CurrentLimit = AttackFrames;
        }

        public void MakeAllLogicFalse()
        {
            IsInTrench = false;
            IsAttacking = false;
            IsShooting = false;
            IsFallingBack = false;
        }


        public void InitGunnerButton()
        {
            GunnerButton = new Button
            {
                Location = new Point(PosX - ViewGraphics.SpriteRectangleSize - 8, PosY + ViewGraphics.SpriteRectangleSize + 16 ),
                Size = new Size(20,20),
                BackColor = Interface.EventsColor,
                Image = ViewGraphics.XPosChangeImage
            };
            GunnerButton.Click += (sender, args) =>
            {
                ButtonController.GunnerButtonOnClick(sender,args,this);
            };
            Interface.GunnersButtons.Add(GunnerButton);
        }
    }
}
