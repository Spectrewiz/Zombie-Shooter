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
    public class Grid
    {
        public int G = 0, H = 0, F = 0;
        public Point Parent = new Point(0, 0);
        public bool Walkable = false, Closed = false;

        public void Reset()
        {
            G = 0; H = 0; F = 0;
            Parent = new Point(0, 0);
            Closed = false;
        }
    }
}
