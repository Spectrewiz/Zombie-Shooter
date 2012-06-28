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
        const int maxHealth = 50;
        private Vector2 dest;
        private bool[,] Map;
        private List<Point> path = new List<Point>();
        private int pathIndex = 0;
        private bool wroteMap = false, finding = false;
        private int hitTimer = 0, hitTime = 60;
        private int pathTimer = 0, pathTime = 20;
        private int DamageAmt;

        public Enemy(Vector2 position)
            : base(position)
        {
            this.position = position;
            dest = position;
            name = "Enemy";
            velocity = new Vector2(1, 1);
            health = maxHealth;
            DamageAmt = 10;
        }

        public override void Update()
        {
            if (Game.getGameState() == Game.GameState.map_edit) alive = false;
            if (!alive) return;
            IncrementTimers();

            if (pointDistance(Player.player.position, this.position) < Game.screen.Width / 2 && pathTimer >= pathTime)
            {
                velocity = new Vector2(1, 1);
                moveToDest();
            }
            else velocity = Vector2.Zero;

            tryToHitPlayer();

            if (health <= 0)
            {
                alive = false;
                health = maxHealth;
            }

            base.Update();
        }

        public void IncrementTimers()
        {
            hitTimer++;
            pathTimer++;
        }

        public void tryToHitPlayer()
        {
            if (pointDistance(this.position, Player.player.position) < pathFinder.gridSize)
                if (hitTimer >= hitTime)
                {
                    if (Player.player.health <= 0)
                        return;
                    Player.player.Damage(DamageAmt);
                    hitTimer = 0;
                }
        }

        public void findPath()
        {
            Map = pathFinder.writeMap(position);
            pathFinder finder;
            finder = new pathFinder(Map);
            path = finder.findPath(position, dest);
        }

        private void setPath()
        {
            if (Game.t[0] != null)
                if (Game.t[0].IsAlive == true)
                    return;

            dest = Player.player.position;

            if (pathFinder.queue < 1 || finding)
            {
                if (!finding)
                {
                    Game.t[0] = new Thread(findPath);
                    Game.t[0].Start();
                    finding = true;
                    pathFinder.queue++;
                }
                //threads
                if (!Game.t[0].IsAlive)
                {
                    Game.t[0].Abort();
                    Game.t[0] = null;
                    finding = false;
                    pathFinder.queue--;
                    pathIndex = 0;
                }
            }
        }

        private void moveToDest()
        {
            setPath();
            /*if (path == null && finding == false)
            {
                pathIndex = 0;
                dest = Player.player.position;
                setPath();
                return;
            }*/
            if (path != null)
            {
                if (pathIndex < path.Count)
                {
                    if (stepToPoint(path[pathIndex]))
                    {
                        pathIndex++;
                    }
                }
                else if (path.Count >= 0)
                {
                    path = null;
                    pathIndex = 0;
                    dest = Player.player.position;
                    setPath();
                }
            }
        }

        private bool stepToPoint(Point point)
        {
            if (pointDistance(this.position, new Vector2(point.X, point.Y)) < pathFinder.gridSize) { velocity = Vector2.Zero; return true; } //set the pathFinder.gridSize / 8, if it glitches l8r take that out.

            //face destination
            rotation = pointDirection(position, new Vector2(point.X, point.Y));
            velocity = new Vector2(1, 1);
            PushTo(velocity, rotation);

            return false;
        }

        public override void PushTo(Vector2 velocity, float direction)
        {
            float newX = (float)Math.Cos(MathHelper.ToRadians(direction));
            float newY = (float)Math.Sin(MathHelper.ToRadians(direction));
            newX *= velocity.X;
            newY *= velocity.Y;

            if (!Collision(velocity, true))
                base.PushTo(velocity, direction);
            else
            {
                Obj collisionObj = Collision(new Wall(Vector2.Zero, 1));
                float tempDir = pointDirection(collisionObj.position, position);
                base.PushTo(velocity, tempDir);
                base.PushTo(velocity, direction);
            }
        }
    }
}
