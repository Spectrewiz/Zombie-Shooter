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
    public class Camera
    {
        public Vector2 position;
        private float rotation;
        private float zoom;
        public int maxZoom = 0;
        public int minZoom = 4;
        private Matrix m_transform;
        public static float zoomAmount = 0.1f;

        public Camera()
        {
            position = new Vector2(Game.stage.Width / 2, Game.stage.Height / 2);
            rotation = 0.0f;
            zoom = 1.0f;

            float wRatio = (float)Game.stage.Width / (float)Game.screen.Width;
            float hRatio = (float)Game.stage.Height / (float)Game.screen.Height;
            bool lowesRatio = (wRatio < hRatio) ? true : false;

            float rLength = (lowesRatio) ? Game.stage.Width : Game.stage.Height;
            float sLength = (lowesRatio) ? Game.screen.Width : Game.screen.Height;

            float tempZoom = 1.0f;
            int c = 0;
            while (sLength * tempZoom < rLength)
            {
                tempZoom *= 1.0f + (zoomAmount * 2);
                c++;
            }


            maxZoom = c;
            if (maxZoom > 9) maxZoom = 9;
        }

        public void Update()
        {
            position = Player.player.position;
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public float Zoom
        {
            get { return zoom; }
            set
            {
                zoom = value;
                if (zoom < 1.0f - (zoomAmount * maxZoom)) zoom = 1.0f - (zoomAmount * maxZoom);
                if (zoom > 1.0f + (zoomAmount * minZoom)) zoom = 1.0f + (zoomAmount * minZoom);
            }
        }

        public void Move(Vector2 amount)
        {
            position += amount;
        }

        public Matrix Transform(GraphicsDevice graphics)
        {
            float ViewPortWidth = graphics.Viewport.Width;
            float ViewPortHeight = graphics.Viewport.Height;

            m_transform = Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
                Matrix.CreateRotationZ(rotation) * Matrix.CreateScale(zoom) *
                Matrix.CreateTranslation(new Vector3(ViewPortWidth * 0.5f, ViewPortHeight * 0.5f, 0));

            return m_transform;
        }

        public static Vector2 globalToLocal(Vector2 pos)
        {
            pos -= (Game.getCameraPosition() - new Vector2(Game.screen.Width / 2, Game.screen.Height / 2));
            return pos;
        }

        public static Vector2 localToGLobal(Vector2 pos)
        {
            pos += (Game.getCameraPosition() - new Vector2(Game.screen.Width / 2, Game.screen.Height / 2));
            return pos;
        }
    }
}
