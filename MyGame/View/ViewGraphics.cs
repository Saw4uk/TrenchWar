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
        public static Image FriendlyUnitSprite = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "Sprites\\Dwarf.png"));
        public static Image EnemyUnitSprite = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "Sprites\\Gladiator.png"));
    }
}
