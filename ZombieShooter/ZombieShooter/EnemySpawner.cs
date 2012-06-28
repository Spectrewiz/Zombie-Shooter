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
    public class EnemySpawner : Obj
    {
        public static List<Obj> Enemies = new List<Obj>();
        private int SpawnTimer = 0, EnemyCount = 16;
        private const int SpawnTime = 60*5;

        public EnemySpawner(Vector2 position)
            : base(position)
        {
            this.position = position;
            for (int i = 0; i < EnemyCount; i++)
            {
                Obj o = new Enemy(this.position);
                o.alive = false;
                Enemies.Add(o);
            }
        }

        public override void Update()
        {
            if (!alive) return;
            IncrementTimers();

            if (SpawnTimer >= SpawnTime)
            {
                SpawnTimer = 0;

                for (int i = 0; i < ObjList.objList.Count; i++)
                    if (!Enemies[i].alive)
                    {
                        Enemies[i].alive = true;
                        Enemies[i].position = this.position;

                        break;
                    }
            }
            if (Enemies.Count > 0)
            {
                for (int i = 0; i < Enemies.Count; i++)
                    Enemies[i].Update();
            }
        }

        public override void LoadContent(ContentManager content)
        {
            if (Enemies.Count > 0)
            {
                for (int i = 0; i < Enemies.Count; i++)
                    Enemies[i].LoadContent(content);
            }
            base.LoadContent(content);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Enemies.Count > 0)
            {
                for (int i = 0; i < Enemies.Count; i++)
                    Enemies[i].Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
        }

        private void IncrementTimers()
        {
            SpawnTimer++;
        }
    }
}
