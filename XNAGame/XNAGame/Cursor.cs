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
    public class Cursor : Obj
    {
        MouseState mouse;

        public Cursor(Vector2 position)
            : base(position)
        {
            this.position = position;
            this.name = "Cursor";
        }

        public override void Update()
        {
            mouse = Mouse.GetState();

            position = new Vector2(mouse.X, mouse.Y);

            base.Update();
        }
    }
}
