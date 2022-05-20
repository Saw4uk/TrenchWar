using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGame.View;

namespace MyGame.Model
{
    public static class GameModel
    {
        public static List<Entity> AllUnits = new List<Entity>();
        public static List<Entity> PlayerUnits = new List<Entity>();
        public static List<Entity> EnemyUnits = new List<Entity>();
        public static Random HitRandom = new Random();
        public static int PlayerMoney = 100;

        public static double GetDistance(Point point1, Point point2)
        {
            return Math.Sqrt(Math.Pow(point2.X - point1.X, 2) + Math.Pow(point2.Y - point1.Y, 2));

        }

        public static void SpawnUnits(int numberOfUnits, int lowerPos, int upperPos)
        {
            for (var x = 0; x < numberOfUnits; x++)
            {
                var entityToAdd = new Entity
                {
                    PosX = 0 + ViewGraphics.SpriteRectangleSize,
                    PosY = HitRandom.Next(lowerPos, upperPos),
                    //(ViewGraphics.SpriteRectangleSize + Interface.ButtonsHeight, Map.MapHeight - ViewGraphics.SpriteRectangleSize - Interface.ButtonsHeight)
                    IdleFrames = 5,
                    RunFrames = 8,
                    DeadFrames = 7,
                    CurrentLimit = 5,
                    AttackFrames = 7,
                    SpriteList = ViewGraphics.FriendlyUnitSprite,

                };
                GameModel.AllUnits.Add(entityToAdd);
                GameModel.PlayerUnits.Add(entityToAdd);
                entityToAdd.MoveToNextTrench();
            }
        }
        public static void CleanCorps(object sender, EventArgs e)
        {
            var newAllUnits = AllUnits.Where(x => x.IsReadyToClean == false).ToList();
            AllUnits.Clear();
            AllUnits = newAllUnits;

            var newPlayerUnits = PlayerUnits.Where(x => x.IsReadyToClean == false).ToList();
            PlayerUnits.Clear();
            PlayerUnits = newPlayerUnits;

            var newEnemyUnits = EnemyUnits.Where(x => x.IsReadyToClean == false).ToList();
            EnemyUnits.Clear();
            EnemyUnits = newEnemyUnits;
        }

        public static void AddMoney(object sender, EventArgs e)
        {
            PlayerMoney += 50;
        }
    }
}
