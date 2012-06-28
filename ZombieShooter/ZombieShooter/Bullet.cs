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
        private int damage;

        public Bullet(Vector2 position)
            : base(position)
        {
            this.position = position;
            this.name = "Bullet";
            //set a damage random at some point. REMEMBER!!
        }

        public int DamageAmt
        {
            set { damage = value; }
            get { return damage; }
        }

        public override void Update()
        {
            if (!alive) return;
            //collision with wall:
            if (Collision(Vector2.Zero, new Wall(new Vector2(0, 0), 1)))
                alive = false;

            //collision with zombi3s:
            Obj zombie = Collision(new Enemy(new Vector2(0, 0)));
            Random rndm;
            if (zombie.GetType() == typeof(Enemy))
            {
                alive = false;
                Enemy conv = (Enemy)zombie;
                rndm = new Random();

                conv.Damage(rndm.Next(10, 15));
            }

            if (position.X < 0 || position.X - sprite.Width > Game.stage.Width)
                this.alive = false;
            if (position.Y < 0 || position.Y - sprite.Height > Game.stage.Height)
                this.alive = false;
            
            base.Update();
        }
    }
}
