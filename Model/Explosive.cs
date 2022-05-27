using System;
using System.Drawing;
using MyGame.View;

namespace MyGame.Model
{
    public class Explosive
    {
        public int PosX;
        public int PosY;
        public Point Location => new Point(PosX, PosY);
        public bool ShouldPlayAnimation;
        public Image SpriteList => ViewGraphics.ExplosiveSpriteSheet;
        public int IdleFrames;
        public int CurrentAnimation;
        public int CurrentFrame;
        public int CurrentLimit;

        public void PlayAnimation(Graphics graphics)
        {
            graphics.DrawImage(
                SpriteList,
                new Rectangle(
                    new Point(PosX, PosY),
                    new Size(ViewGraphics.ExplosiveSpriteRectangleSize, ViewGraphics.ExplosiveSpriteRectangleSize)),
                ViewGraphics.ExplosiveSpriteRectangleSize * CurrentFrame,
                ViewGraphics.ExplosiveSpriteRectangleSize * CurrentAnimation,
                ViewGraphics.ExplosiveSpriteRectangleSize,
                ViewGraphics.ExplosiveSpriteRectangleSize,
                GraphicsUnit.Pixel);
            if (CurrentFrame < IdleFrames - 1)
                CurrentFrame++;
            else
            {
                ShouldPlayAnimation = false;
            }
        }
    }
}