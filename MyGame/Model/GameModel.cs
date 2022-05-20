using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Model
{
    public static class GameModel
    {
        public static List<Entity> AllUnits = new List<Entity>();
        public static List<Entity> PlayerUnits = new List<Entity>();
        public static List<Entity> EnemyUnits = new List<Entity>();
        public static Random HitRandom = new Random();

        public static double GetDistance(Point point1, Point point2)
        {
            return Math.Sqrt(Math.Pow(point2.X - point1.X, 2) + Math.Pow(point2.Y - point1.Y, 2));

        }

        public static void CleanCorps(object sender, EventArgs e)
        {
            var newAllUnits = AllUnits.Where(x => x.IsReadyToClean == false).ToList();
            AllUnits = newAllUnits;

            var newPlayerUnits = PlayerUnits.Where(x => x.IsReadyToClean == false).ToList();
            PlayerUnits = newPlayerUnits;

            var newEnemyUnits = EnemyUnits.Where(x => x.IsReadyToClean == false).ToList();
            EnemyUnits = newEnemyUnits;
        }
    }
}
