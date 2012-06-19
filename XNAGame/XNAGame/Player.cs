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
    public class Player : Obj
    {
        public static Player player;

        KeyboardState key, prevKey;
        MouseState mouse, prevMouse;

        List<Obj> currentClip = new List<Obj>();
        const int maxAmmo = 32;
        int ammo = 32, shootingTimer = 0, reloadTimer = 0, rate = 20, reload = 60 * 2;
        bool reloading = false;

        public Player(Vector2 position)
            : base(position)
        {
            this.position = position;
            this.name = "gunner";
            player = this;
            velocity = new Vector2(5, 5);
            for (int i = 0; i < ammo; i++)
            {
                Obj o = new Bullet(new Vector2(0, 0));
                o.alive = false;
                currentClip.Add(o);
            }
        }

        public override void Update()
        {
            player = this;
            if (!alive) return;
            area.X = (int)position.X - (sprite.Width / 2);
            area.Y = (int)position.Y - (sprite.Height / 2);

            mouse = Mouse.GetState();
            key = Keyboard.GetState();

            if ((key.IsKeyDown(Keys.Up) || key.IsKeyDown(Keys.W)) && !Collision(new Vector2(0, -velocity.Y), new Wall(new Vector2(0, 0), 1)))
                position.Y -= velocity.Y;
            if ((key.IsKeyDown(Keys.Down) || key.IsKeyDown(Keys.S)) && !Collision(new Vector2(0, velocity.Y), new Wall(new Vector2(0, 0), 1)))
                position.Y += velocity.Y;
            if ((key.IsKeyDown(Keys.Right) || key.IsKeyDown(Keys.D)) && !Collision(new Vector2(velocity.X, 0), new Wall(new Vector2(0, 0), 1)))
                position.X += velocity.X;
            if ((key.IsKeyDown(Keys.Left) || key.IsKeyDown(Keys.A)) && !Collision(new Vector2(-velocity.X, 0), new Wall(new Vector2(0, 0), 1)))
                position.X -= velocity.X;

            if (position.Y < 0)
                position.Y = 0;
            else if (position.Y > Game.stage.Height)
                position.Y = Game.stage.Height;

            if (position.X < 0)
                position.X = 0;
            else if (position.X > Game.stage.Width)
                position.X = Game.stage.Width;

            shootingTimer++;
            if (mouse.LeftButton == ButtonState.Pressed && !reloading)
            {
                checkTimer();
            }
            if (currentClip.Count > 0)
            {
                for (int i = 0; i < currentClip.Count; i++)
                    currentClip[i].Update();
            }

            if (key.IsKeyDown(Keys.R) || mouse.RightButton == ButtonState.Pressed)
            {
                reloading = true;
            }
            Reload();

            rotation = pointDirection(position, new Vector2(mouse.X, mouse.Y));

            prevMouse = mouse;
            prevKey = key;
        }

        public override void LoadContent(ContentManager content)
        {
            if (currentClip.Count > 0)
            {
                for (int i = 0; i < currentClip.Count; i++)
                    currentClip[i].LoadContent(content);
            }
            base.LoadContent(content);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(Game.spriteFont, "Ammo: " + ammo, Vector2.Zero, Color.White);
            if (reloading)
                spriteBatch.DrawString(Game.spriteFont, "Reloading...", new Vector2(0, Game.spriteFont.LineSpacing), Color.Red);
            spriteBatch.End();
            if (currentClip.Count > 0)
            {
                for (int i = 0; i < currentClip.Count; i++)
                    currentClip[i].Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
        }

        public void Reload()
        {
            if (reloading)
                reloadTimer++;
            if (reloadTimer > reload)
            {
                reloadTimer = 0;
                reloading = false;
                ammo = maxAmmo;
            }
        }

        public void checkTimer()
        {
            if ((shootingTimer > rate) && (ammo > 0))
            {
                shootingTimer = 0;
                Shoot();
            }
        }
        public void Shoot()
        {
            ammo--;
            if (currentClip.Count > 0)
            {
                for (int i = 0; i < currentClip.Count; i++)
                {
                    if (!currentClip[i].alive)
                    {
                        currentClip[i].position = this.position;
                        currentClip[i].UpdateArea();
                        currentClip[i].rotation = this.rotation;
                        currentClip[i].velocity = new Vector2(25, 25);
                        currentClip[i].alive = true;
                        break;
                    }
                }
            }
        }
    }
}
