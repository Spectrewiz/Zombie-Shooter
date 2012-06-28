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
    public class ObjList
    {
        public static List<Obj> objList = new List<Obj>();
        public static List<Obj> placableObjects;

        public static void Initialize()
        {
            /*
            //Walls and other inanimate objects:
            for (int i = 1; i <= 4; i++)
                objList.Add(new Wall(new Vector2(64, 64 + ((i * 32) - (i * 3))), 1));
            for (int i = 1; i <= 4; i++)
                objList.Add(new Wall(new Vector2(200, 64 + ((i * 32) - (i * 3))), 1));
            objList.Add(new Barricade(new Vector2(400, 400)));

            //Player:
            objList.Add(new Player(new Vector2(128, 128)));

            //Enemies:
            objList.Add(new EnemySpawner(new Vector2(250, 250)));
            */
        }

        public static void LoadContent(ContentManager content)
        {
            for (int i = 0; i < objList.Count; i++)
                objList[i].LoadContent(content);
        }

        public static void Update()
        {
            for (int i = 0; i < objList.Count; i++)
                objList[i].Update();
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < objList.Count; i++)
                objList[i].Draw(spriteBatch);
        }

        public static void Reset()
        {
            for (int i = 0; i < objList.Count; i++)
                objList[i].alive = false;
        }
    }
}
