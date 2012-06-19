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
    public class Obj
    {
        public Vector2 position;// = new Vector2(50,50);
        public Vector2 velocity = Vector2.Zero;
        public float rotation = 0.0f;
        public Texture2D sprite;
        public string name = "black";
        public float scale = 1.0f;
        public bool alive = true, Solid;
        public Rectangle area;

        public Obj(Vector2 position)
        {
            this.position = position;
        }

        public virtual void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("Sprites/" + name);
            area = new Rectangle(0, 0, sprite.Width, sprite.Height);
        }

        public virtual void Update()
        {
            if (!alive) return;
            UpdateArea();
            PushTo(velocity, rotation);
        }
        public void UpdateArea()
        {
            area.X = (int)position.X - (sprite.Width / 2);
            area.Y = (int)position.Y - (sprite.Height / 2);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!alive) return;
            spriteBatch.Begin();
            spriteBatch.Draw(sprite, position, null, Color.White, MathHelper.ToRadians(rotation), new Vector2(sprite.Width / 2, sprite.Height / 2), scale, SpriteEffects.None, 0);
            spriteBatch.End();
        }

        public bool Collision(Vector2 pos, Obj obj)
        {
            Rectangle newArea = new Rectangle(area.X, area.Y, area.Width, area.Height);
            newArea.X += (int)pos.X;
            newArea.Y += (int)pos.Y;

            for (int i = 0; i < ObjList.objList.Count; i++)
                if (ObjList.objList[i].GetType() == obj.GetType() && ObjList.objList[i].Solid)
                    if (ObjList.objList[i].area.Intersects(newArea))
                        return true;

            return false;
        }

        public virtual void PushTo(Vector2 velocity, float direction)
        {
            float newX = (float)Math.Cos(MathHelper.ToRadians(direction));
            float newY = (float)Math.Sin(MathHelper.ToRadians(direction));
            position.X += velocity.X * newX;
            position.Y += velocity.Y * newY;
        }

        public float pointDistance(Vector2 first, Vector2 second)
        {
            float newX = (first.X - second.X) * (first.X - second.X);
            float newY = (first.Y - second.Y) * (first.Y - second.Y);
            double hRect = newX + newY;
            float dist = (float)Math.Sqrt(hRect);
            return dist;
        }

        public float pointDirection(Vector2 first, Vector2 second)
        {
            float adj = first.X - second.X;
            float opp = first.Y - second.Y;
            float res = MathHelper.ToDegrees((float)Math.Atan2(opp, adj));
            res = (res - 180) % 360;
            if (res < 0) { res += 360; }
            return res;
        }
    }
}
