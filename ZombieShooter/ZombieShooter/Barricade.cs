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
    public class Barricade : Wall
    {
        public Barricade(Vector2 position)
            : base(position, 0)
        {
            this.Solid = true;
            this.position = position;
            this.name = "Barricade";
            imageNumber = 6;
            imageSpeed = 0;
        }
    }
}
