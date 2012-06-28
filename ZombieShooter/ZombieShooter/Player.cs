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
        public int maxAmmo = 32, maxHealth = 100;
        public int ammo = 32, shootingTimer = 0, reloadTimer = 0, rate = 20, reload = 60 * 2;
        public bool reloading = false, inMenu = false;

        public Player(Vector2 position)
            : base(position)
        {
            this.position = position;
            this.name = "Player";
            player = this;
            velocity = new Vector2(5, 5);
            for (int i = 0; i < ammo; i++)
            {
                Obj o = new Bullet(this.position);
                o.alive = false;
                currentClip.Add(o);
            }
            this.health = maxHealth;
        }

        public override void Update()
        {
            player = this;
            if (!alive) return;
            if (health <= 0)
            {
                /*alive = false;
                health = 0;*/
                Game.switchGameState(Game.GameState.exit);
            }

            area.X = (int)position.X - (sprite.Width / 2);
            area.Y = (int)position.Y - (sprite.Height / 2);

            mouse = Mouse.GetState();
            key = Keyboard.GetState();

            if ((key.IsKeyDown(Keys.Up) || key.IsKeyDown(Keys.W)) && !Collision(new Vector2(0, -velocity.Y), true))
                position.Y -= velocity.Y;
            if ((key.IsKeyDown(Keys.Down) || key.IsKeyDown(Keys.S)) && !Collision(new Vector2(0, velocity.Y), true))
                position.Y += velocity.Y;
            if ((key.IsKeyDown(Keys.Right) || key.IsKeyDown(Keys.D)) && !Collision(new Vector2(velocity.X, 0), true))
                position.X += velocity.X;
            if ((key.IsKeyDown(Keys.Left) || key.IsKeyDown(Keys.A)) && !Collision(new Vector2(-velocity.X, 0), true))
                position.X -= velocity.X;
            if (key.IsKeyDown(Keys.Escape))
                Menu();

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

            rotation = pointDirection(Camera.globalToLocal(position), new Vector2(mouse.X, mouse.Y));

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
                        Bullet b = (Bullet)currentClip[i];

                        b.position = this.position;
                        b.UpdateArea();
                        b.rotation = this.rotation;
                        b.velocity = new Vector2(25, 25);
                        b.alive = true;
                        b.DamageAmt = 10;
                        break;
                    }
                }
            }
        }

        public void Menu()
        {
            if (!inMenu)
            {
                inMenu = true;
                Game.switchGameState(Game.GameState.game_menu);
            }
            else
            {
                inMenu = false;
                Game.switchGameState(Game.GameState.game);
            }
        }
    }
}
