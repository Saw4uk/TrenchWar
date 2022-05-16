using System;
using System.Collections.Generic;
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
    }
}
