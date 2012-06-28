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
    public class MapEditor
    {
        KeyboardState key, prevKey;
        MouseState mouse, prevMouse;

        int selected = 2;

        public MapEditor()
        {
            ObjList.placableObjects = new List<Obj>();
            ObjList.placableObjects.Add(new Obj(Vector2.Zero));
            ObjList.placableObjects.Add(new Player(Vector2.Zero));
            ObjList.placableObjects.Add(new Wall(Vector2.Zero, 1));
            ObjList.placableObjects.Add(new Enemy(Vector2.Zero));
            ObjList.placableObjects.Add(new EnemySpawner(Vector2.Zero));
            ObjList.placableObjects.Add(new Barricade(Vector2.Zero));
        }

        public void LoadContent(ContentManager content)
        {
            foreach (Obj o in ObjList.placableObjects)
            {
                o.LoadContent(content);
                o.alive = false;
                o.draw = false;
            }
        }

        public void Update(GameTime gameTime)
        {
            key = Keyboard.GetState();
            mouse = Mouse.GetState();

            selected += (mouse.ScrollWheelValue > prevMouse.ScrollWheelValue) ? 1 
                : (mouse.ScrollWheelValue == prevMouse.ScrollWheelValue) ? 0 
                : -1;
            selected = (selected > ObjList.placableObjects.Count - 1) ? 0 
                : (selected < 0) ? (ObjList.placableObjects.Count - 1) 
                : selected;

            if (checkMouse())
                CreateObj(selected);

            prevKey = key;
            prevMouse = mouse;

            foreach (Obj o in ObjList.placableObjects)
            {
                o.Update();
                o.SnapToGrid();
            }
            ObjList.Update();
        }

        public void CreateObj(int index)
        {
            Obj obj = ObjList.placableObjects[index];
            obj.position = new Vector2(mouse.X, mouse.Y);
            obj.draw = true;
            ObjList.objList.Add(obj);
            ObjList.placableObjects.Add(ObjList.placableObjects[index]);
        }

        public bool checkMouse()
        {
            return (mouse.LeftButton == ButtonState.Pressed);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Obj o in ObjList.placableObjects)
                o.Draw(spriteBatch);
            ObjList.Draw(spriteBatch);
            HUD.Draw(spriteBatch);
        }
    }

    public class Interface
    {

    }
}
