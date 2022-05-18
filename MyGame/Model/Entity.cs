using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGame.View;

namespace MyGame.Model
{
    public class Entity
    {
        public int PosX;
        public int PosY;

        public Image SpriteList;
        public int IdleFrames;
        public int RunFrames;
        public bool IsAlive = true;
        public bool IsEnemy;
        public int Flip = 1;
        public bool IsAttacking;
        public bool IsFallingBack;
        public bool IsInTrench = true;
        public int CurrentAnimation;
        public int CurrentFrame;

        public void PlayAnimation( Graphics graphics)
        {
            graphics.DrawImage(SpriteList, new Rectangle(new Point(PosX - Flip*ViewGraphics.SpriteRectangleSize/2,PosY), new Size(ViewGraphics.SpriteRectangleSize*Flip,ViewGraphics.SpriteRectangleSize)), 32 * CurrentFrame, ViewGraphics.SpriteRectangleSize * CurrentAnimation, ViewGraphics.SpriteRectangleSize, ViewGraphics.SpriteRectangleSize, GraphicsUnit.Pixel);
            if (CurrentFrame < IdleFrames - 1)
                CurrentFrame++;
            else
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
    }
}
