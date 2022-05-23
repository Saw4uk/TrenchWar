using System;
using System.Collections.Generic;
using System.Drawing;
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
        private static Entity CurrentGunner;
        public static int GunnerNewPosition;
        public static bool PreparingToGetSupplies;
        public static bool GunnerWaitOrders;
        public static bool IsWaitingForArtilleryFire;
        public static int NumOfRiflemansToSupply;
        public static int NumOfGunnersToSupply;
        public static int LowerSuppliesPosition;
        public static int UpperSuppliesPosition;
        public static int SpawnSizeConst;
        public static void LittleRiflemanButtonOnClick(object sender, EventArgs e)
        {
            GameModel.PlayerMoney -= 25;
            PreparingToGetSupplies = true;
            NumOfRiflemansToSupply = 3;
            SpawnSizeConst = 60;
            NumOfGunnersToSupply = 0;
        }

        public static void MiddleRiflemanButtonOnClick(object sender, EventArgs e)
        {
            GameModel.PlayerMoney -= 35;
            PreparingToGetSupplies = true;
            NumOfRiflemansToSupply = 5;
            SpawnSizeConst = 70;
            NumOfGunnersToSupply = 0;
        }

        public static void LargeRiflemanButtonOnClick(object sender, EventArgs e)
        {
            GameModel.PlayerMoney -= 65;
            PreparingToGetSupplies = true;
            NumOfRiflemansToSupply = 10;
            SpawnSizeConst = 100;
            NumOfGunnersToSupply = 0;
        }
        public static void OnMouseClick(object sender, MouseEventArgs e)
        {
            if(PreparingToGetSupplies)
            {
                GameModel.SpawnRiflemans(NumOfRiflemansToSupply, LowerSuppliesPosition, UpperSuppliesPosition);
                GameModel.SpawnGunners(NumOfGunnersToSupply, LowerSuppliesPosition, UpperSuppliesPosition);
                PreparingToGetSupplies = false;
                Interface.PlayStepsSound();
            }

            if (GunnerWaitOrders)
            {
                GunnerWaitOrders = false;
                CurrentGunner.OrderedPosition = GunnerNewPosition;
            }

            if (IsWaitingForArtilleryFire)
            {
                IsWaitingForArtilleryFire = false;
                GameModel.PlayerArtillery.PlayShootAnimation();
            }
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

        public static void LittleGunnerSquadButtonOnClick(object sender, EventArgs e)
        {
            GameModel.PlayerMoney -= 50;
            GameModel.PlayerSecretDocuments -= 1;
            PreparingToGetSupplies = true;
            NumOfRiflemansToSupply = 2;
            NumOfGunnersToSupply = 1;
            SpawnSizeConst = 40;
        }

        public static void LargeGunnerSquadButtonOnClick(object sender, EventArgs e)
        {
            GameModel.PlayerMoney -= 100;
            GameModel.PlayerSecretDocuments -= 1;
            PreparingToGetSupplies = true;
            NumOfRiflemansToSupply = 9;
            NumOfGunnersToSupply = 1;
            SpawnSizeConst = 100;
        }

        public static void GunnerButtonOnClick(object sender, EventArgs e,Entity gunner)
        {
            GunnerWaitOrders = true;
            CurrentGunner = gunner;
        }

        public static void ArtilleryFireButtonOnClick(object sender, EventArgs e)
        {
            IsWaitingForArtilleryFire = true;
            GameModel.PlayerArtillery.OrderedShoots = 1;
            GameModel.PlayerArtillery.FireAccuracy = 30;
            GameModel.PlayerMoney -= 50;
            GameModel.PlayerSecretDocuments -= 1;

        }

        public static void ArtilleryThreeFireButtonOnClick(object sender, EventArgs e)
        {
            IsWaitingForArtilleryFire = true;
            GameModel.PlayerArtillery.OrderedShoots = 3;
            GameModel.PlayerArtillery.FireAccuracy = 100;
            GameModel.PlayerMoney -= 100;
            GameModel.PlayerSecretDocuments -= 2;
        }
    }
}
