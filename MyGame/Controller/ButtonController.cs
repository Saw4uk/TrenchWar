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
        public static int NumOfUnitsToSupply;
        public static int LowerSuppliesPosition;
        public static int UpperSuppliesPosition;
        public static int SpawnSizeConst;
        public static void LittleRiflemanButtonOnClick(object sender, EventArgs e)
        {
            GameModel.PlayerMoney -= 25;
            PreparingToGetSupplies = true;
            NumOfUnitsToSupply = 3;
            SpawnSizeConst = 60;
        }

        public static void MiddleRiflemanButtonOnClick(object sender, EventArgs e)
        {
            GameModel.PlayerMoney -= 35;
            PreparingToGetSupplies = true;
            NumOfUnitsToSupply = 5;
            SpawnSizeConst = 70;
        }

        public static void LargeRiflemanButtonOnClick(object sender, EventArgs e)
        {
            GameModel.PlayerMoney -= 65;
            PreparingToGetSupplies = true;
            NumOfUnitsToSupply = 10;
            SpawnSizeConst = 100;
        }
        public static void OnMouseClick(object sender, MouseEventArgs e)
        {
            if(PreparingToGetSupplies)
                GameModel.SpawnUnits(NumOfUnitsToSupply, LowerSuppliesPosition, UpperSuppliesPosition);
            PreparingToGetSupplies = false;
            Interface.PlayStepsSound();
        }

        public static void AttackButton_Click(object sender, EventArgs e)
        {
            foreach (var unit in GameModel.PlayerUnits)
            {
                unit.MoveToNextTrench();
            }
            Interface.PlayMorzeSound();
        }

        public static void FallBackButtonOnClick(object sender, EventArgs e)
        {
            foreach (var unit in GameModel.PlayerUnits.Where(unit => !unit.IsInTrench))
            {
                unit.FallBack();
            }
            Interface.PlayMorzeSound();
        }

        public static void SuppliesButtonOnClick(object sender, EventArgs e)
        {
            var maxTrenchCord = GameModel.PlayerUnits.Where(x => Map.Trenches.Any(i => x.PosX >= i && x.IsAlive == 1 && x.IsInTrench)).Max(x => x.PosX);
            foreach (var unit in GameModel.PlayerUnits.Where(unit => unit.IsInTrench))
            {
                unit.MoveToAllyTrench(maxTrenchCord);
            }
            Interface.PlayMorzeSound();
        }
    }
}
