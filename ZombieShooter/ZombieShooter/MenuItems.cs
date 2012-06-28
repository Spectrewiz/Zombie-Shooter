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
    public class MenuItems
    {
        public string text;
        public Rectangle area;
        public Color selectColor, normColor;

        public MenuItems(string text, Rectangle area, Color selectColor, Color normColor)
        {
            this.text = text;
            this.area = area;
            this.selectColor = selectColor;
            this.normColor = normColor;
        }
        public MenuItems(string text, Rectangle area)
        {
            this.text = text;
            this.area = area;
            this.selectColor = Color.Yellow;
            this.normColor = Color.Red;
        }
    }
}
