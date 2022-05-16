using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyGame.Model;

namespace MyGame.View
{
    public static class ViewGraphics
    {
        public static int SpriteRectangleSize = 31;
        public static Size SpriteSize = new Size(SpriteRectangleSize, SpriteRectangleSize);
        public static Image FriendlyUnitSprite = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "Sprites\\Dwarf.png"));
        public static Timer MoveTimer = new Timer
        {
            Interval = 20
        };
    }
}
