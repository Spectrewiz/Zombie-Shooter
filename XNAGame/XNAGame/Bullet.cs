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
    public class Bullet : Obj
    {
        public Bullet(Vector2 position)
            : base(position)
        {
            this.position = position;
            this.name = "Bullet";
        }

        public override void Update()
        {
            if (Collision(Vector2.Zero, new Wall(new Vector2(0, 0), 1)))
                alive = false;
            if (position.X < 0 || position.X - sprite.Width > Game.stage.Width)
                this.alive = false;
            if (position.Y < 0 || position.Y - sprite.Height > Game.stage.Height)
                this.alive = false;
            
            base.Update();
        }
    }
}
