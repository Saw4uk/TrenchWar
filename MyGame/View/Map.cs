using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.View
{
    public static class Map
    {
        public static Image MapImage = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName,"Sprites\\Map.png"));
        public static int MapHeight = MapImage.Height;
        public static int MapWidth = MapImage.Width;

        public static int[] Trenches = new int[]
        {
            100,
            400,
            800,
            1000
        };
    }
}
