using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using MyGame.Model;

namespace MyGame.View
{
    public static class ViewGraphics
    {
        public static int SpriteRectangleSize = 32;
        public static int ArtillerySpriteHeight = 100;
        public static int ArtillerySpriteWidth = 60;
        public static int ExplosiveSpriteRectangleSize = 60;
        public static Image FriendlyUnitSprite = new Bitmap(Path.Combine(Interface.CurrentDirectory, "Sprites\\AllySoldierSpriteSheet.png"));
        public static Image AllyGunnerSpriteList = new Bitmap(Path.Combine(Interface.CurrentDirectory, "Sprites\\AllyGunnerSpriteList.png"));
        public static Image EnemyUnitSprite = new Bitmap(Path.Combine(Interface.CurrentDirectory, "Sprites\\EnemySoldierSpriteSheet.png"));
        public static Image LittleRiflemanSquadImage = new Bitmap(Path.Combine(Interface.CurrentDirectory, "Sprites\\LittleRiflemanSqad.png"));
        public static Image LittleGunnerSquadImage = new Bitmap(Path.Combine(Interface.CurrentDirectory, "Sprites\\LittleGunnerSqad.png"));
        public static Image MiddleRiflemanSquadImage = new Bitmap(Path.Combine(Interface.CurrentDirectory, "Sprites\\MiddleRiflemanSqad.png"));
        public static Image LargeRiflemanSquadImage = new Bitmap(Path.Combine(Interface.CurrentDirectory, "Sprites\\LargeRiflemanSqad.png"));
        public static Image MoneyImage = new Bitmap(Path.Combine(Interface.CurrentDirectory, "Sprites\\Money.png"));
        public static Image DocumentsImage = new Bitmap(Path.Combine(Interface.CurrentDirectory, "Sprites\\Documents.png"));
        public static Image AttackButtonImage = new Bitmap(Path.Combine(Interface.CurrentDirectory, "Sprites\\Attack.png"));
        public static Image FallBackButtonImage = new Bitmap(Path.Combine(Interface.CurrentDirectory, "Sprites\\FallBack.png"));
        public static Image ToMainTrenchImage = new Bitmap(Path.Combine(Interface.CurrentDirectory, "Sprites\\ToMainTrench.png"));
        public static Image XPosChangeImage = new Bitmap(Path.Combine(Interface.CurrentDirectory, "Sprites\\XPosChange.png"));
        public static Image ExplosiveSpriteSheet = new Bitmap(Path.Combine(Interface.CurrentDirectory, "Sprites\\ExplosiveSheet.png"));
        public static Image ArtillerySpriteSheet = new Bitmap(Path.Combine(Interface.CurrentDirectory, "Sprites\\ArtillerySpriteSheett.png"));
        public static Image ArtilleryOneShoot = new Bitmap(Path.Combine(Interface.CurrentDirectory, "Sprites\\ArtilleryShoot.png"));
        public static Image ArtilleryThreeShoots = new Bitmap(Path.Combine(Interface.CurrentDirectory, "Sprites\\ArtilleryShoots.png"));
        public static Image LargeGunnerSquad = new Bitmap(Path.Combine(Interface.CurrentDirectory, "Sprites\\LargeGunnerSqad.png"));
        public static Image SecondFormImage = new Bitmap(Path.Combine(Interface.CurrentDirectory, "Sprites\\SecondForm.png"));
        public static Image PlayButtonImage = new Bitmap(Path.Combine(Interface.CurrentDirectory, "Sprites\\PlayButton.png"));
        public static Image StudingButtonImage = new Bitmap(Path.Combine(Interface.CurrentDirectory, "Sprites\\StudingButton.png"));
        public static Image SettingsButtonImage = new Bitmap(Path.Combine(Interface.CurrentDirectory, "Sprites\\SettingsButton.png"));
        public static Image TrainingButtonImage = new Bitmap(Path.Combine(Interface.CurrentDirectory, "Sprites\\TrainingButton.png"));
        public static Image BackButtonImage = new Bitmap(Path.Combine(Interface.CurrentDirectory, "Sprites\\BackButton.png"));
        public static Image MovieButtonImage = new Bitmap(Path.Combine(Interface.CurrentDirectory, "Sprites\\Movie.png"));
        public static string FullVideo = Path.Combine(Interface.CurrentDirectory, "Sprites\\FullVersion.wmv");
    }
}
