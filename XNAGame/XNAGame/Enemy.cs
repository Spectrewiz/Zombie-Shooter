using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ZombieShooter
{
    public class Enemy : Obj
    {
        int health;
        private Vector2 dest;
        private bool[,] Map;
        private List<Point> path = new List<Point>();
        private int pathIndex = 0;
        private bool wroteMap = false, queued = false, finding = false;
        private int queTimer = 0, queTime = 60;
        private Thread t;

        public Enemy(Vector2 position)
            : base(position)
        {
            this.position = position;
            dest = position;
            this.name = "Enemy";
            this.velocity = new Vector2(2, 2);
            health = 50;
        }

        public override void Update()
        {
            if (!alive) return;

            queTimer++;
            if (queTimer > queTime)
            {
                queTimer = 0;
                dest = Player.player.position;
                setPath();
            }

            moveToDest();

            UpdateArea();
            this.PushTo(velocity, rotation);
        }

        public void findPath()
        {
            Map = pathFinder.writeMap();
            pathFinder finder;
            finder = new pathFinder(Map);
            path = finder.findPath(position, dest);
        }

        private void setPath()
        {
            queued = true;
            if (pathFinder.queue < 1 || finding)
            {
                if (!finding)
                {
                    pathFinder.queue++;
                    t = new Thread(findPath);
                    t.Start();
                    finding = true;
                }
                //threads
                if (!t.IsAlive)
                {
                    t.Abort();
                    finding = false;
                    queued = false;
                    pathIndex = 0;
                    pathFinder.queue--;
                }
            }
        }

        private void moveToDest()
        {
            if (path == null) return;
            if (pathIndex > path.Count)
            {
                if (stepToPoint(path[pathIndex]))
                {
                    pathIndex++;
                }
            }
        }

        private bool stepToPoint(Point point)
        {
            if (pointDistance(this.position, new Vector2(point.X, point.Y)) < pathFinder.gridSize) { velocity = Vector2.Zero; return true; }

            //face destination
            rotation = pointDirection(position, new Vector2(point.X, point.Y));
            velocity = new Vector2(2, 2);

            return false;
        }

        public override void PushTo(Vector2 velocity, float direction)
        {
            float newX = (float)Math.Cos(MathHelper.ToRadians(direction));
            float newY = (float)Math.Sin(MathHelper.ToRadians(direction));
            newX *= velocity.X;
            newY *= velocity.Y;
            if (!Collision(new Vector2(newX, newY), new Wall(new Vector2(0, 0), 1)))
                base.PushTo(velocity, direction);
        }
    }
}
