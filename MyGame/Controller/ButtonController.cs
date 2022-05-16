using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGame.Model;
using MyGame.View;

namespace MyGame.Controller
{
    public static class ButtonController
    {
        private static Random rnd = new Random();
        public static void AddButtonOnClick(object sender, EventArgs e)
        {
            var entityToAdd = new Entity
            {
                PosX = 0,
                PosY = rnd.Next(ViewGraphics.SpriteRectangleSize, Map.MapHeight - ViewGraphics.SpriteRectangleSize - Interface.ButtonsHeight),
                IdleFrames = 5,
                RunFrames = 8,
                SpriteList = ViewGraphics.FriendlyUnitSprite
                
            };
            GameModel.AllUnits.Add(entityToAdd);
            GameModel.PlayerUnits.Add(entityToAdd);
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
            var maxTrenchCoord = GameModel.PlayerUnits.Where(x => Map.Trenches.Any(i => x.PosX >= i)).Max(x => x.PosX);
            foreach (var unit in GameModel.PlayerUnits.Where(unit => unit.IsInTrench))
            {
                if(unit.PosX != maxTrenchCoord)
                    unit.MoveToNextTrench();
            }
        }
    }
}
