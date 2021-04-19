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

        public HauntedHouseGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            var eye_textures = new Texture2D[]{ 
                Content.Load<Texture2D>("Eyes-Center"),
                Content.Load<Texture2D>("Eyes-Up"),
                Content.Load<Texture2D>("Eyes-Down"),
                Content.Load<Texture2D>("Eyes-Left"),
                Content.Load<Texture2D>("Eyes-Right"),
                Content.Load<Texture2D>("Match")
            };
            logoLocation = new Vector2(_graphics.PreferredBackBufferWidth / 2, 2f);
            eyes = new Eyes(eye_textures, new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), 500f);
            
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
            );

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
