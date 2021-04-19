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

        public HauntedHouseGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

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

            //Create and Urn Object
            urn = new Urn(backgroundBuffer, urnTextures); 
            logoLocation = new Vector2(_graphics.PreferredBackBufferWidth / 2, 2f);
            
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            logoTexture = Content.Load<Texture2D>("logo");
        }

        protected override void Update(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();
            if(kstate.IsKeyDown(Keys.LeftControl) && kstate.IsKeyDown(Keys.Q))
            {
                Exit();
            }
            //if an arrow key is pushed down, update the eye location and texture
            eyes.UpdateEyes(kstate, gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            //Draw the Match if lit
            if(eyes.MatchIsLit == true)
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
                0f
            );

            //Draw the Urn Pieces
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

            /*
            //Draw the logo
            _spriteBatch.Draw(
                logoTexture,
                logoLocation,
                null,
                Color.White,
                0f,
                new Vector2(logoTexture.Width / 2f, 0),
                Vector2.One,
                SpriteEffects.None,
                0f
            );*/

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
