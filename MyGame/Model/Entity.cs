using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MyGame.View;

namespace MyGame.Model
{
    public class Entity
    {
        public int PosX;
        public int PosY;
        public Point Location => new Point(PosX, PosY);

        public Image SpriteList;
        public int IdleFrames;
        public int AttackFrames;
        public int RunFrames;
        public int DeadFrames;
        public int IsAlive = 1;
        public bool IsEnemy;
        public bool IsReadyToClean;
        public int Flip = 1;
        public int FireRange = 150;
        public int PercentOfHit = 70;
        public bool IsAttacking;
        public bool IsShooting;
        public bool IsFallingBack;
        public bool IsInTrench = true;
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
                }
                else
                {
                    CurrentAnimation = 1;
                    CurrentLimit = RunFrames;
                }
                IsShooting = false;
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

        public void MoveToNextTrench()
        {
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
                MoveToNextTrench();
        }

        public void FindAndTryToKillEnemy()
        {
            if(IsShooting || IsAlive == 0)
                return;
            var enemyUnits = (IsEnemy) ? GameModel.PlayerUnits : GameModel.EnemyUnits;
            var target = enemyUnits.FirstOrDefault(unit => GameModel.GetDistance(Location, unit.Location) <= FireRange && unit.IsAlive == 1);
            if (target != null)
            {
                var currentPercentOfHit = PercentOfHit;
                if (target.IsInTrench)
                    currentPercentOfHit /= 2;
                PlayShootAnimation();
                if (target.CheckHit(currentPercentOfHit))
                {
                    target.IsAlive = 0;
                    if (target.IsEnemy)
                    {
                        GameModel.PlayerMoney += 5;
                    }
                    else
                    {
                        GameModel.PlayerMoney += 2;
                    }
                }
            }
        }
        public bool CheckHit(int percent)
        {
            return percent >= GameModel.HitRandom.Next(0, 100);
        }

        public void DieWithHonor()
        {
            if (CurrentAnimation == 4 && CurrentFrame == DeadFrames - 1)
            {
                IsReadyToClean = true;
            }
            else
            {
                CurrentAnimation = 4;
                CurrentLimit = DeadFrames;
                IsInTrench = false;
                IsAttacking = false;
                IsFallingBack = false;
            }
        }

        public void PlayShootAnimation()
        {
            IsShooting = true;
            CurrentAnimation = 2;
            CurrentFrame = 0;
            CurrentLimit = AttackFrames;
        }
    }
}
