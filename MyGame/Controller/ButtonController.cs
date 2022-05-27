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
    public class ButtonController
    {
        public GameModel GameModel;

        public ButtonController(GameModel gameModel)
        {
            GameModel = gameModel;
        }

        public void LittleRiflemanButtonOnClick(object sender, EventArgs e)
        {
            Interface.PlayClickSound();
            GameModel.PlayerMoney -= 25;
            GameModel.PreparingToGetSupplies = true;
            GameModel.NumOfRiflemansToSupply = 3;
            GameModel.SpawnSizeConst = 60;
            GameModel.NumOfGunnersToSupply = 0;
        }

        public void MiddleRiflemanButtonOnClick(object sender, EventArgs e)
        {
            Interface.PlayClickSound();
            GameModel.PlayerMoney -= 35;
            GameModel.PreparingToGetSupplies = true;
            GameModel.NumOfRiflemansToSupply = 5;
            GameModel.SpawnSizeConst = 70;
            GameModel.NumOfGunnersToSupply = 0;
        }

        public void LargeRiflemanButtonOnClick(object sender, EventArgs e)
        {
            Interface.PlayClickSound();
            GameModel.PlayerMoney -= 65;
            GameModel.PreparingToGetSupplies = true;
            GameModel.NumOfRiflemansToSupply = 10;
            GameModel.SpawnSizeConst = 100;
            GameModel.NumOfGunnersToSupply = 0;
        }
        public void OnMouseClick(object sender, MouseEventArgs e)
        {
            if(GameModel.PreparingToGetSupplies)
            {
                GameModel.SpawnRiflemans(GameModel.NumOfRiflemansToSupply, GameModel.LowerSuppliesPosition, GameModel.UpperSuppliesPosition);
                GameModel.SpawnGunners(GameModel.NumOfGunnersToSupply, GameModel.LowerSuppliesPosition, GameModel.UpperSuppliesPosition);
                GameModel.PreparingToGetSupplies = false;
                Interface.PlayStepsSound();
            }

            if (GameModel.GunnerWaitOrders)
            {
                GameModel.GunnerWaitOrders = false;
                GameModel.CurrentGunner.OrderedPosition = GameModel.GunnerNewPosition;
            }

            if (GameModel.IsWaitingForArtilleryFire)
            {
                GameModel.IsWaitingForArtilleryFire = false;
                GameModel.PlayerArtillery.PlayShootAnimation();
            }
        }

        public void AttackButton_Click(object sender, EventArgs e)
        {
            foreach (var unit in GameModel.PlayerUnits)
            {
                unit.MoveToNextTrench();
            }
            Interface.PlayMorzeSound();
        }

        public void FallBackButtonOnClick(object sender, EventArgs e)
        {
            foreach (var unit in GameModel.PlayerUnits.Where(unit => !unit.IsInTrench))
            {
                unit.FallBack();
            }
            Interface.PlayMorzeSound();
        }

        public void SuppliesButtonOnClick(object sender, EventArgs e)
        {
            var maxTrenchCord = GameModel.PlayerUnits.Where(x => Map.Trenches.Any(i => x.PosX >= i && x.IsAlive == 1 && x.IsInTrench)).Max(x => x.PosX);
            foreach (var unit in GameModel.PlayerUnits.Where(unit => unit.IsInTrench))
            {
                unit.MoveToAllyTrench(maxTrenchCord);
            }
            Interface.PlayMorzeSound();
        }

        public void LittleGunnerSquadButtonOnClick(object sender, EventArgs e)
        {
            Interface.PlayClickSound();
            GameModel.PlayerMoney -= 50;
            GameModel.PlayerSecretDocuments -= 1;
            GameModel.PreparingToGetSupplies = true;
            GameModel.NumOfRiflemansToSupply = 2;
            GameModel.NumOfGunnersToSupply = 1;
            GameModel.SpawnSizeConst = 40;
        }

        public void LargeGunnerSquadButtonOnClick(object sender, EventArgs e)
        {
            Interface.PlayClickSound();
            GameModel.PlayerMoney -= 100;
            GameModel.PlayerSecretDocuments -= 1;
            GameModel.PreparingToGetSupplies = true;
            GameModel.NumOfRiflemansToSupply = 9;
            GameModel.NumOfGunnersToSupply = 1;
            GameModel.SpawnSizeConst = 100;
        }


        public void ArtilleryFireButtonOnClick(object sender, EventArgs e)
        {
            Interface.PlayMorzeSound();
            GameModel.IsWaitingForArtilleryFire = true;
            GameModel.PlayerArtillery.OrderedShoots = 1;
            GameModel.PlayerArtillery.FireAccuracy = 30;
            GameModel.PlayerMoney -= 50;
            GameModel.PlayerSecretDocuments -= 1;

        }

        public void ArtilleryThreeFireButtonOnClick(object sender, EventArgs e)
        {
            Interface.PlayMorzeSound();
            GameModel.IsWaitingForArtilleryFire = true;
            GameModel.PlayerArtillery.OrderedShoots = 3;
            GameModel.PlayerArtillery.FireAccuracy = 100;
            GameModel.PlayerMoney -= 100;
            GameModel.PlayerSecretDocuments -= 2;
        }
    }
}
