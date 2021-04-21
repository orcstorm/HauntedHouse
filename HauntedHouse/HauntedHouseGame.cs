using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace HauntedHouse
{
    public class HauntedHouseGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Eyes eyes;
        private Texture2D logoTexture;
        private Vector2 logoLocation;
        private Urn urn;
        private Color backgroundColor;
        private enum GameStates { Menu, Active, Paused }
        private GameStates gameState;
        private Vector2 pauseLocation;
        private Texture2D pauseTexture;

        public HauntedHouseGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            backgroundColor = Color.Black;
            var backgroundBuffer = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

            // Load Eye Textures
            var eye_textures = new Texture2D[]{ 
                Content.Load<Texture2D>("Eyes-Center"),
                Content.Load<Texture2D>("Eyes-Up"),
                Content.Load<Texture2D>("Eyes-Down"),
                Content.Load<Texture2D>("Eyes-Left"),
                Content.Load<Texture2D>("Eyes-Right"),
                Content.Load<Texture2D>("Match")
            };

            //Create Eyes object
            eyes = new Eyes(eye_textures, backgroundBuffer, 500f);
            
            //Load Urn Textures
            var urnTextures = new Texture2D[]
            {
                Content.Load<Texture2D>("Urn-Handle-L"),
                Content.Load<Texture2D>("Urn-C"),
                Content.Load<Texture2D>("Urn-Handle-R")
            };

            //Create Urn Object
            urn = new Urn(backgroundBuffer, urnTextures); 
            
            logoLocation = new Vector2(_graphics.PreferredBackBufferWidth / 2f, _graphics.PreferredBackBufferHeight / 2f);
            pauseLocation = new Vector2(_graphics.PreferredBackBufferWidth / 2f, _graphics.PreferredBackBufferHeight / 2f);
            gameState = GameStates.Menu;
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            logoTexture = Content.Load<Texture2D>("Text");
            pauseTexture = Content.Load<Texture2D>("Pause");
        }

        protected override void Update(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();

            switch (gameState)
            {
                case GameStates.Menu:
                    if(kstate.IsKeyDown(Keys.Enter))
                    {
                        gameState = GameStates.Active;
                    }
                    break;
                case GameStates.Active:
                    //Check that all three pieces are found, end game
                    backgroundColor = Color.Black;

                    if (urn.UrnCenterIsFound && urn.UrnHandleLIsFound && urn.UrnHandleRIsFound)
                    {
                        Initialize();
                        gameState = GameStates.Menu;
                    }


                    if (kstate.IsKeyDown(Keys.Escape))
                    {
                        gameState = GameStates.Paused;
                    }

                    //check for collisions between eyes and urn pieces
                    urn.CheckCollisions(eyes);

                    //update the eye location and texture
                    eyes.UpdateEyes(kstate, gameTime);
                    break;
                case GameStates.Paused:
                    if (kstate.IsKeyDown(Keys.Escape))
                    {
                        gameState = GameStates.Active;
                    }
                    break;
            }


        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backgroundColor);

            _spriteBatch.Begin();
            
            switch(gameState)
            {
                case GameStates.Menu:
                    DrawMenu(gameTime);
                    break;
                case GameStates.Active:
                    DrawGame(gameTime);
                    break;
                case GameStates.Paused:
                    DrawPaused(gameTime);
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawMenu(GameTime gameTime)
        {
            //Draw the logo
            _spriteBatch.Draw(
                logoTexture,
                logoLocation,
                null,
                Color.White,
                0f,
                new Vector2(logoTexture.Width / 2f, logoTexture.Height / 2f),
                Vector2.One,
                SpriteEffects.None,
                0f
            );
        }

        private void DrawPaused(GameTime gameTime)
        {
            _spriteBatch.Draw(
                pauseTexture,
                pauseLocation,
                null,
                Color.White,
                0f,
                new Vector2(pauseTexture.Width / 2f, pauseTexture.Height / 2f),
                Vector2.One,
                SpriteEffects.None,
                0f
            );

        }

        private void DrawGame(GameTime game)
        {
           
            
            if (eyes.MatchIsLit == true)
            {
                _spriteBatch.Draw(
                    eyes.MatchTexture,
                    eyes.Location,
                    null,
                    Color.White,
                    0f,
                    new Vector2(eyes.MatchTexture.Width / 2, eyes.MatchTexture.Height / 2),
                    Vector2.One,
                    SpriteEffects.None,
                    0f);
            }

            //Draw the eyes
            _spriteBatch.Draw(
                eyes.CurrentTexture,
                eyes.Location,
                null,
                Color.White,
                0f,
                new Vector2(eyes.CurrentTexture.Width / 2, eyes.CurrentTexture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f);

            //Draw the Urn Pieces
            if (eyes.MatchIsLit == true)
            {
                
                if (urn.UrnCenterIsFound == false)
                {
                    if(urn.IsCenterLit(eyes.GetBoundingCircle())) {
                        _spriteBatch.Draw(
                            urn.UrnCenterTexture,
                            urn.UrnCenterLocation,
                            null,
                            Color.White,
                            0f,
                            new Vector2(urn.UrnCenterTexture.Width / 2f, urn.UrnCenterTexture.Height / 2f),
                            Vector2.One,
                            SpriteEffects.None,
                            0f
                        );
                    }

                }

                if (urn.UrnHandleLIsFound == false)
                {
                    if (urn.IsLeftLit(eyes.GetBoundingCircle()))
                    {
                        _spriteBatch.Draw(
                            urn.UrnHandleLTexture,
                            urn.UrnHandleLLocation,
                            null,
                            Color.White,
                            0f,
                            new Vector2(urn.UrnHandleLTexture.Width / 2f, urn.UrnHandleLTexture.Height / 2f),
                            Vector2.One,
                            SpriteEffects.None,
                            0f
                        );
                    }
                }

                if (urn.UrnHandleRIsFound == false)
                {
                    if (urn.IsRightLit(eyes.GetBoundingCircle()))
                    {
                        _spriteBatch.Draw(
                            urn.UrnHandleRTexture,
                            urn.UrnHandleRLocation,
                            null,
                            Color.White,
                            0f,
                            new Vector2(urn.UrnHandleRTexture.Width / 2f, urn.UrnHandleRTexture.Height / 2f),
                            Vector2.One,
                            SpriteEffects.None,
                            0f
                        );
                    }
                }
            }

        }

    }
}
