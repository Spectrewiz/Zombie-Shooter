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
    public class MenuComponent
    {
        public KeyboardState key, prevKey;
        public MouseState mouse, prevMouse;
        public SpriteFont spriteFont;

        public Rectangle areaOfMenu;

        public int selected = 0;

        public List<MenuItems> buttonList = new List<MenuItems>();
        public string[] strings;

        public MenuComponent(params string[] strings)
        {
            foreach (string str in strings)
                buttonList.Add(new MenuItems(str, new Rectangle()));

            this.strings = strings;
        }

        public virtual void LoadContent(ContentManager content)
        {
            spriteFont = content.Load<SpriteFont>("MenuFont");
        }

        public virtual void Update(GameTime gameTime)
        {
            key = Keyboard.GetState();
            mouse = Mouse.GetState();

            if (checkKeyboard(Keys.Up) || checkKeyboard(Keys.W))
                if (selected > 0) selected--;

            if (checkKeyboard(Keys.Down) || checkKeyboard(Keys.S))
                if (selected < buttonList.Count - 1) selected++;

            if (checkKeyboard(Keys.Enter))
                Game.switchGameState(selected);

            for (int i = 0; i < buttonList.Count; i++)
                if (checkMousePos(Cursor.cursor.position).Intersects(buttonList[i].area))
                {
                    selected = i;
                    if (checkMouseClick()) Game.switchGameState(selected);
                }
            prevKey = key;
            prevMouse = mouse;
        }

        public Rectangle checkMousePos(Vector2 center)
        {
            return new Rectangle((int)center.X, (int)center.Y, 0, 0);
        }
        public bool checkMouseClick()
        {
            return (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released);
        }

        public bool checkKeyboard(Keys keyDown)
        {
            return (key.IsKeyDown(keyDown) && prevKey.IsKeyUp(keyDown));
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Color color;
            int linePadding = 3;
            areaOfMenu = new Rectangle();

            for (int i = 0; i < buttonList.Count; i++)
            {
                Vector2 pos = new Vector2((Game.screen.Width / 2) - (spriteFont.MeasureString(buttonList[i].text).X / 2), (Game.screen.Height / 2) - (spriteFont.LineSpacing * buttonList.Count / 2) + ((spriteFont.LineSpacing + linePadding) * i));

                buttonList[i].area = new Rectangle((int)pos.X, (int)pos.Y, (int)spriteFont.MeasureString(buttonList[i].text).X, (int)spriteFont.MeasureString(buttonList[i].text).Y);

                color = (i == selected) ? buttonList[i].selectColor : buttonList[i].normColor;

                spriteBatch.DrawString(spriteFont, buttonList[i].text, pos, color);
                
                if (i == buttonList.Count - 1)
                {
                    areaOfMenu.X = buttonList[indexOfLargestString(spriteFont, strings)].area.X - 25;
                    areaOfMenu.Y = buttonList[0].area.Y - 25;
                    areaOfMenu.Width = (int)measureLargestString(spriteFont, strings) + 50;
                    areaOfMenu.Height = ((spriteFont.LineSpacing + linePadding) * buttonList.Count) + 50;
                }
            }
        }

        public float measureLargestString(SpriteFont spriteFont, string[] strings)
        {
            float length = 0;
            foreach (string str in strings)
                if (spriteFont.MeasureString(str).X > length) length = spriteFont.MeasureString(str).X;
            return length;
        }
        public int indexOfLargestString(SpriteFont spriteFont, string[] strings)
        {
            float length = 0;
            int index = 0;
            for (int i = 0; i < strings.Length; i++)
                if (spriteFont.MeasureString(strings[i]).X > length) { length = spriteFont.MeasureString(strings[i]).X; index = i; }
            return index;
        }
    }
}
