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
        public static int SpriteRectangleSize = 32;
        public static Image FriendlyUnitSprite = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "Sprites\\AllySoldierSpriteSheet.png"));
        public static Image EnemyUnitSprite = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "Sprites\\EnemySoldierSpriteSheet.png"));
        public static Image LittleRiflemanSquadImage = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "Sprites\\LittleRiflemanSqad.png"));
        public static Image MiddleRiflemanSquadImage = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "Sprites\\MiddleRiflemanSqad.png"));
        public static Image LargeRiflemanSquadImage = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "Sprites\\LargeRiflemanSqad.png"));
        public static Image MoneyImage = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "Sprites\\Money.png"));
        public static Image DocumentsImage = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "Sprites\\Documents.png"));
        public static Image AttackButtonImage = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "Sprites\\Attack.png"));
        public static Image FallBackButtonImage = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "Sprites\\FallBack.png"));
        public static Image ToMainTrenchImage = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "Sprites\\ToMainTrench.png"));
    }
}
