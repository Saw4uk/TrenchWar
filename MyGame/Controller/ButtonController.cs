using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyGame.Model;
using MyGame.View;

namespace MyGame.Controller
{
    public static class ButtonController
    {
        private static Random rnd = new Random();
        public static bool PreparingToGetSupplies;
        public static int LowerSuppliesPosition;
        public static int UpperSuppliesPosition;
        public static void AddButtonOnClick(object sender, EventArgs e)
        {
            GameModel.PlayerMoney -= 25;
            PreparingToGetSupplies = true;
        }

        public static void OnMouseClick(object sender, MouseEventArgs e)
        {
            PreparingToGetSupplies = false;
            GameModel.SpawnUnits(3,LowerSuppliesPosition,UpperSuppliesPosition);
        }

        public static void AttackButton_Click(object sender, EventArgs e)
        {
            foreach (var unit in GameModel.PlayerUnits)
            {
                unit.MoveToNextTrench();
            }
        }

        public static void FallBackButtonOnClick(object sender, EventArgs e)
        {
            foreach (var unit in GameModel.PlayerUnits.Where(unit => !unit.IsInTrench))
            {
                unit.FallBack();
            }
        }

        public static void SuppliesButtonOnClick(object sender, EventArgs e)
        {
            var maxTrenchCord = GameModel.PlayerUnits.Where(x => Map.Trenches.Any(i => x.PosX >= i)).Max(x => x.PosX);
            foreach (var unit in GameModel.PlayerUnits.Where(unit => unit.IsInTrench))
            {
                unit.MoveToAllyTrench(maxTrenchCord);
            }
        }

        public static void OrderAttackButtonOnClick(object sender, EventArgs e)
        {
            EnemyAI.CurrentStrategy = Strategy.Rush;
        }
    }
}
