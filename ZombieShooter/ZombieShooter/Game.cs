using System;
using System.Collections.Generic;
using System.Linq;
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
    public class Game : Microsoft.Xna.Framework.Game
    {
        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static SpriteFont spriteFont;
        public static Rectangle stage, screen;
        public enum GameState { menu, game_menu, game, map_edit, exit, inventory, controls, options }
        protected static GameState gameState = GameState.menu;
        public static void switchGameState(GameState newGameState)
        {
            gameState = newGameState;
        }
        public static void switchGameState(int index)
        {
            switch (index)
            {
                case 0:
                    gameState = GameState.game;
                    break;
                case 1:
                    gameState = GameState.controls;
                    break;
                case 2:
                    gameState = GameState.map_edit;
                    break;
                case 3:
                    gameState = GameState.exit;
                    break;
                case 4:
                    gameState = GameState.inventory;
                    break;
                case 5:
                    gameState = GameState.menu;
                    break;
                case 6:
                    gameState = GameState.game_menu;
                    break;
                case 7:
                    gameState = GameState.options;
                    break;
            }
        }
        public static GameState getGameState()
        {
            return gameState;
        }
        public static Camera camera = new Camera();
        public static Obj cursor = new Cursor(new Vector2(0,0));
        MapReader mapReader;
        MapEditor mapEditor;
        MenuComponent menuComponent, gameMenu;

        public static Thread[] t = new Thread[3];

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            stage = new Rectangle(0, 0, graphics.PreferredBackBufferWidth * 4, graphics.PreferredBackBufferHeight * 4);
            screen = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            ObjList.Initialize();
            mapEditor = new MapEditor();
            mapReader = new MapReader();
            menuComponent = new MenuComponent("Play", "Controls", "Map Editor", "Exit");
            gameMenu = new MenuComponent("Resume", "Controls", "Map Editor", "Exit");

            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>("HUDfont");
            mapReader.ReadMap("ObjectDB.xml");
            ObjList.LoadContent(this.Content);
            camera.Update();
            cursor.LoadContent(this.Content);
            mapEditor.LoadContent(Content);
            menuComponent.LoadContent(Content);
            gameMenu.LoadContent(Content);
        }
        protected override void UnloadContent()
        {
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || gameState == GameState.exit)
                this.Exit();

            cursor.Update();
            switch (gameState)
            {
                case GameState.game:
                    ObjList.Update();
                    camera.Update();
                    break;
                case GameState.map_edit:
                    mapEditor.Update(gameTime);
                    break;
                case GameState.game_menu:
                    gameMenu.Update(gameTime);
                    break;
                case GameState.menu:
                    menuComponent.Update(gameTime);
                    break;
            }

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            switch (gameState)
            {
                case GameState.game:
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.Transform(graphics.GraphicsDevice));
                        ObjList.Draw(spriteBatch);
                    spriteBatch.End();
                    spriteBatch.Begin();
                        HUD.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
                case GameState.map_edit:
                    spriteBatch.Begin();
                        mapEditor.Draw(spriteBatch);
                        spriteBatch.DrawString(spriteFont, "Map Editor Alpha (hardly works)", new Vector2(5, screen.Height - spriteFont.MeasureString("Map Editor Alpha (hardly works)").Y), Color.Black);
                    spriteBatch.End();
                    break;
                case GameState.menu:
                    spriteBatch.Begin();
                        menuComponent.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
                case GameState.game_menu:
                    Texture2D texture = new Texture2D(graphics.GraphicsDevice, 1, 1);
                    texture.SetData(new[] { Color.Gray });
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.Transform(graphics.GraphicsDevice));
                        ObjList.Draw(spriteBatch);
                    spriteBatch.End();
                    spriteBatch.Begin();
                        HUD.Draw(spriteBatch);
                        spriteBatch.Draw(texture, screen, new Color(128, 128, 128, 64));
                        spriteBatch.Draw(texture, gameMenu.areaOfMenu, Color.Black);
                    spriteBatch.End();
                    spriteBatch.Begin();
                        gameMenu.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
            }

            spriteBatch.Begin();
                cursor.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public static Vector2 getCameraPosition()
        {
            return camera.position;
        }

        public static Vector2 getCursorPosition()
        {
            return cursor.position;
        }
    }
}
