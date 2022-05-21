using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyGame.View;

namespace MyGame.Model
{
    public enum Strategy
    {
        PrepareToAttack,
        Attack,
        Defend,
        Rush,
        WaitOrders
    }
    public static class EnemyAI
    {
        private static Timer EnemyOrdersLogicTimer;
        private static Random rnd = new Random();
        public static Strategy CurrentStrategy;
        public static void StartWar()
        {

            EnemyOrdersLogicTimer = new Timer();
            EnemyOrdersLogicTimer.Interval = 5000;
            EnemyOrdersLogicTimer.Tick += GetOrders;
            EnemyOrdersLogicTimer.Start();
            CurrentStrategy = Strategy.Rush;
            GameModel.SpawnUnits(5,Interface.ButtonsHeight + ViewGraphics.SpriteRectangleSize, Map.MapHeight - ViewGraphics.SpriteRectangleSize);
            SpawnEnemies(10);
        }

        private static void GetOrders(object sender, EventArgs e)
        {
            switch (CurrentStrategy)
            {
                case Strategy.WaitOrders:
                    CurrentStrategy = GetStrategy();
                    break;
                case Strategy.PrepareToAttack:
                    PrepareToAttack();
                    break;
                case Strategy.Attack:
                    Attack();
                    CurrentStrategy = Strategy.WaitOrders;
                    break;
                case Strategy.Defend:
                    SpawnEnemies(5);
                    AllSuppliesToMainTrench();
                    CurrentStrategy = Strategy.WaitOrders;
                    break;
                case Strategy.Rush:
                    Attack();
                    CurrentStrategy = Strategy.Attack;
                    break;
            }

        }

        private static Strategy GetStrategy()
        {
            if (GameModel.PlayerUnits.Count > GameModel.EnemyUnits.Count)
            {
                return Strategy.Defend;
            }
            else if(GameModel.PlayerUnits.Count < GameModel.EnemyUnits.Count - 10)
            {
                return Strategy.Attack;
            }
            else if (GameModel.PlayerUnits.Count < GameModel.EnemyUnits.Count)
            {
                return Strategy.PrepareToAttack;
            }
            else if (GameModel.PlayerUnits.Count < GameModel.EnemyUnits.Count - 15)
            {
                return Strategy.Rush;
            }
            else
            {
                return Strategy.Defend;
            }
        }

        private static void PrepareToAttack()
        {
            if (GameModel.PlayerUnits.Count - GameModel.EnemyUnits.Count >= GameModel.EnemyUnits.Count / 2)
            {
                SpawnEnemies(4);
                AllSuppliesToMainTrench();
            }
            else if (GameModel.PlayerUnits.Count <= GameModel.EnemyUnits.Count / 2)
                CurrentStrategy = Strategy.Rush;
            else if (GameModel.PlayerUnits.Count <= GameModel.EnemyUnits.Count)
                CurrentStrategy = Strategy.Attack;
            else
                CurrentStrategy = Strategy.WaitOrders;
            
        }

        private static void Attack()
        {
            foreach (var unit in GameModel.EnemyUnits)
            {
                unit.MoveToNextTrench();
            }
            SpawnEnemies(2);
        }

        private static void AllSuppliesToMainTrench()
        {
            var maxTrenchCord = GameModel.EnemyUnits.Where(x => Map.Trenches.Any(i => x.PosX <= i)).Min(x => x.PosX);
            foreach (var unit in GameModel.EnemyUnits)
            {
                unit.MoveToAllyTrench(maxTrenchCord);
            }
        }
        private static void SpawnEnemies(int numberOfEnemies)
        {
            for (var i = 0; i < numberOfEnemies; i++)
            {
                SpawnEnemyUnit();
            }
        }

        private static void SpawnEnemyUnit()
        {
            var entityToAdd = new Entity
            {
                PosX = Map.MapWidth - ViewGraphics.SpriteRectangleSize,
                PosY = rnd.Next(ViewGraphics.SpriteRectangleSize + Interface.ButtonsHeight, Map.MapHeight - ViewGraphics.SpriteRectangleSize - Interface.ButtonsHeight),
                IdleFrames = 4,
                RunFrames = 8,
                DeadFrames = 7,
                CurrentLimit = 4,
                AttackFrames = 14,
                IsEnemy = true,
                SpriteList = ViewGraphics.EnemyUnitSprite,

            };
            GameModel.AllUnits.Add(entityToAdd);
            GameModel.EnemyUnits.Add(entityToAdd);
            entityToAdd.MoveToNextTrench();
        }
    }
}
