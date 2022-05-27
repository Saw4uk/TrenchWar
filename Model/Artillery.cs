using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyGame.Controller;
using MyGame.View;

namespace MyGame.Model
{
    public class Artillery
    {
        public int PosX;
        public int PosY;
        public Point Location => new Point(PosX, PosY);
        public Point FireTarget;
        public Image SpriteList;
        public Random Accuracy;
        public Explosive Explosive;
        public int IdleFrames;
        public int AttackFrames;
        public int Flip = 1;
        public bool IsShooting;
        public int FireAccuracy;
        public int CurrentAnimation;
        public int AmmoDamageDistance = 75;
        public int OrderedShoots;
        public int CurrentFrame;
        public int CurrentLimit;

        public void PlayAnimation(Graphics graphics)
        {

            if (CurrentAnimation == 1 && CurrentFrame == AttackFrames - 2)
            {
                OrderedShoots -= 1;
            }
            if (CurrentAnimation == 1 && CurrentFrame == AttackFrames - 1)
            {
                if(OrderedShoots == 0)
                {
                    CurrentFrame = 0;
                    CurrentAnimation = 0;
                    CurrentLimit = IdleFrames;
                    IsShooting = false;
                    OrderedShoots = 0;
                }
                else
                {
                    CurrentFrame = 0;
                }
            }

            if (CurrentAnimation == 1 && CurrentFrame == 5)
            {
                Interface.PlayArtilleryShootSound();
            }

            if (CurrentAnimation == 1 && CurrentFrame == 32)
            {
                Interface.PlayExplosiveSound();
                CheckTargets();

            }

            graphics.DrawImage(
                SpriteList,
                new Rectangle(
                    new Point(PosX, PosY),
                    new Size(ViewGraphics.ArtillerySpriteWidth * Flip, ViewGraphics.ArtillerySpriteHeight)),
                ViewGraphics.ArtillerySpriteWidth * CurrentFrame,
                ViewGraphics.ArtillerySpriteHeight * CurrentAnimation,
                ViewGraphics.ArtillerySpriteWidth,
                ViewGraphics.ArtillerySpriteHeight,
                GraphicsUnit.Pixel);
            if (CurrentFrame < CurrentLimit - 1)
                CurrentFrame++;
            else
            {
                CurrentFrame = 0;
            }
        }

        public void CheckTargets()
        {
            Accuracy = new Random();
            var detonationDamageCenter = new Point(FireTarget.X + Accuracy.Next(FireAccuracy) - FireAccuracy/2, FireTarget.Y + Accuracy.Next(FireAccuracy) - FireAccuracy/2);
            Explosive = new Explosive
            {
                PosX = detonationDamageCenter.X,
                PosY = detonationDamageCenter.Y,
                CurrentLimit = 16,
                IdleFrames = 16,
                ShouldPlayAnimation = true
            };
            foreach (var unit in GameModel.AllUnits)
            {
                if(GameModel.GetDistance(unit.Location,detonationDamageCenter) <= AmmoDamageDistance)
                    unit.IsAlive = 0;
            }
        }

        public void PlayShootAnimation()
        {
            IsShooting = true;
            CurrentAnimation = 1;
            CurrentFrame = 0;
            CurrentLimit = AttackFrames;
        }
    }
}
