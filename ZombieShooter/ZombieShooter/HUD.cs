using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ZombieShooter
{
    public class HUD
    {
        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Game.spriteFont, "Ammo: " + Player.player.ammo, new Vector2(5, 0), Color.White);
            if (Player.player.health > 30)
                spriteBatch.DrawString(Game.spriteFont, "HP: " + Player.player.health + "/" + Player.player.maxHealth, new Vector2(5, Game.spriteFont.LineSpacing), Color.Green);
            else
                spriteBatch.DrawString(Game.spriteFont, "HP: " + Player.player.health + "/" + Player.player.maxHealth, new Vector2(5, Game.spriteFont.LineSpacing), Color.Red);
            if (Player.player.reloading)
                spriteBatch.DrawString(Game.spriteFont, "Reloading...", new Vector2(5, Game.spriteFont.LineSpacing * 2), Color.Red);
        }
    }
}
