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
        public static Dictionary<string, Texture2D> objSpriteDB = new Dictionary<string, Texture2D>();

        public Vector2 position;// = new Vector2(50,50);
        public Vector2 velocity = Vector2.Zero;
        public float rotation = 0.0f;
        public Texture2D sprite;
        public string name = "black";
        public float scale = 1.0f;
        public bool alive = true, draw = false, Solid;
        public Rectangle area;
        public int health = 0;

        public Rectangle imageArea;
        public Point frame;
        protected int imageNumber = 1;
        protected float imageSpeed = 1f;
        public float imageIndex = 0f;

        public Obj(Vector2 position)
        {
            this.position = position;
            SnapToGrid();
        }

        public static void InitSpriteDB(ContentManager content)
        {
            objSpriteDB.Add("black", content.Load<Texture2D>("Sprites/black"));
            objSpriteDB.Add("Barricade", content.Load<Texture2D>("Sprites/Barricade"));
            objSpriteDB.Add("Bullet", content.Load<Texture2D>("Sprites/Bullet"));
            objSpriteDB.Add("Cursor", content.Load<Texture2D>("Sprites/Cursor"));
            objSpriteDB.Add("Enemy", content.Load<Texture2D>("Sprites/Enemy"));
            objSpriteDB.Add("floor", content.Load<Texture2D>("Sprites/floor"));
            objSpriteDB.Add("gunner", content.Load<Texture2D>("Sprites/gunner"));
            objSpriteDB.Add("HUD", content.Load<Texture2D>("Sprites/HUD"));
            objSpriteDB.Add("Player", content.Load<Texture2D>("Sprites/Player"));
            objSpriteDB.Add("Wall1", content.Load<Texture2D>("Sprites/Wall1"));
        }

        public virtual void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("Sprites/" + name);
            area = new Rectangle(0, 0, sprite.Width / imageNumber, sprite.Height);
            frame = new Point(sprite.Width / imageNumber, sprite.Height);
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
            if (!draw && !alive) return;
            imageIndex += (imageIndex < (imageNumber - 1)) ? imageSpeed : -imageIndex;
            imageArea = new Rectangle((int)imageIndex * frame.X, 0, frame.X, frame.Y);
            spriteBatch.Draw(sprite, position, imageArea, Color.White, MathHelper.ToRadians(rotation), new Vector2(sprite.Width / 2, sprite.Height / 2), scale, SpriteEffects.None, 0);
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
        public Obj Collision(Obj obj)
        {
            for (int i = 0; i < ObjList.objList.Count; i++)
                if (ObjList.objList[i].GetType() == obj.GetType() && ObjList.objList[i].alive)
                    if (ObjList.objList[i].area.Intersects(area))
                        return ObjList.objList[i];

            for (int i = 0; i < EnemySpawner.Enemies.Count; i++)
                if (EnemySpawner.Enemies[i].GetType() == obj.GetType() && EnemySpawner.Enemies[i].alive)
                    if (EnemySpawner.Enemies[i].area.Intersects(area))
                        return EnemySpawner.Enemies[i];

            return new Obj(Vector2.Zero);
        }
        public bool Collision(Vector2 pos, bool checkSolids)
        {
            Rectangle newArea = new Rectangle(area.X, area.Y, area.Width, area.Height);
            newArea.X += (int)pos.X;
            newArea.Y += (int)pos.Y;

            for (int i = 0; i < ObjList.objList.Count; i++)
                if (ObjList.objList[i].Solid || !checkSolids)
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

        public virtual void Damage(int damage)
        {
            health -= damage;
        }

        public virtual void SnapToGrid()
        {
            position = new Vector2(
                (int)(position.X / pathFinder.gridSize) * pathFinder.gridSize,
                (int)(position.Y / pathFinder.gridSize) * pathFinder.gridSize);
        }
    }
}
