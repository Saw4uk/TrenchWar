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
        public static Image MapImage = new Bitmap(Path.Combine(Interface.CurrentDirectory, "Sprites\\Map.png"));
        public static int MapHeight = MapImage.Height;
        public static int MapWidth = MapImage.Width;

        public static int[] Trenches =
        {
            86,
            250,
            500,
            750,
            1000
        };
    }
}
