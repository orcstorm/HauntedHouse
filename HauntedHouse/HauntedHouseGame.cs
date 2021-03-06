using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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
        private SoundEffectInstance soundEffectInstance;
        private SoundEffectInstance soundEffectInstance2;
        private SoundEffectInstance soundEffectInstance3;
        private SoundEffectInstance soundEffectInstance4;
        private SoundEffectInstance currentSoundEffect;
        private Texture2D vwall;
        private Texture2D hwall;
        private Room currentRoom;
        private Vector2 backgroundBuffer;

        public HauntedHouseGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            backgroundColor = Color.Black;
            backgroundBuffer = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

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

            //create a Room;
            vwall = Content.Load<Texture2D>("walls/vertical-wall");
            hwall = Content.Load<Texture2D>("walls/horizontal-wall");
            currentRoom = GetRoom1();
            
            
            //Load Urn Textures
            var urnTextures = new Texture2D[]
            {
                Content.Load<Texture2D>("Urn-Handle-L"),
                Content.Load<Texture2D>("Urn-C"),
                Content.Load<Texture2D>("Urn-Handle-R")
            };

            //Create Urn Object
            urn = new Urn(backgroundBuffer, urnTextures, Content.Load<SoundEffect>("audio/Found")); 
            
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
            soundEffectInstance = Content.Load<SoundEffect>("audio/SquareBass").CreateInstance();
            soundEffectInstance2 = Content.Load<SoundEffect>("audio/SB2").CreateInstance();
            soundEffectInstance3 = Content.Load<SoundEffect>("audio/SB3").CreateInstance();
            soundEffectInstance4 = Content.Load<SoundEffect>("audio/SB4").CreateInstance();
            currentSoundEffect = soundEffectInstance;
            MediaPlayer.IsRepeating = true; 
        }

        protected override void Update(GameTime gameTime)
        {

            var kstate = Keyboard.GetState();
            var piecesFound = urn.PiecesFound();

            switch (gameState)
            {
                case GameStates.Menu:
                    if(kstate.IsKeyDown(Keys.Enter))
                    {
                        
                        gameState = GameStates.Active;
                    }
                    break;
                case GameStates.Active:
                    
                    backgroundColor = Color.Black;
                    
                    //play an audio effect based on how many pieces have been found
                    if(currentSoundEffect.State == SoundState.Stopped)
                    {
                        switch (piecesFound)
                        {
                            case 0:
                           
                                currentSoundEffect.Play();
                                break;
                            case 1:
                                currentSoundEffect = soundEffectInstance2;
                                currentSoundEffect.Play();
                                break;
                            case 2:
                                currentSoundEffect = soundEffectInstance3;
                                currentSoundEffect.Play();
                                break;
                            case 3:
                                currentSoundEffect = soundEffectInstance4;
                                currentSoundEffect.Play();
                                break;
                        }
                    }


                    if (urn.UrnCenterIsFound && urn.UrnHandleLIsFound && urn.UrnHandleRIsFound)
                    {
                        MediaPlayer.Stop(); 
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
                    UpdateEyes(gameTime, kstate);

                    //check if we've left the room
                    CheckRoomLeft();

                    break;
                case GameStates.Paused:
                    if (kstate.IsKeyDown(Keys.Escape))
                    {
                        gameState = GameStates.Active;
                    }

                    if (kstate.IsKeyDown(Keys.Q))
                    {
                        Exit();
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

            //Draw the match light circle if lit. 
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

            //Draw the walls
            DrawWalls();

            //Draw the eyes
            _spriteBatch.Draw(
                eyes.CurrentTexture,
                eyes.Location,
                null,
                Color.White,
                0f,
                new Vector2(eyes.CurrentTexture.Width / 2f, eyes.CurrentTexture.Height / 2f),
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

        private void DrawWalls()
        {
            foreach(Wall wall in currentRoom.walls)
            {
                _spriteBatch.Draw(
                    wall.texture,
                    wall.location,
                    null,
                    Color.White,
                    0f,
                    new Vector2(0,0),
                    Vector2.One,
                    SpriteEffects.None,
                    0f);
            }
        }

        private void UpdateEyes(GameTime gameTime, KeyboardState kstate)
        {
            var seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var ticks = gameTime.TotalGameTime.Ticks;

            //check if the match should be extinguished
            if (ticks > eyes.MatchLitTick + eyes.MatchLitDuration)
            {
                eyes.MatchIsLit = false;
            }

            //check direction keys
            if (kstate.IsKeyDown(Keys.Up))
            {
                eyes.CurrentTexture = eyes.Textures[1];
                var nextY = eyes.Location.Y - eyes.Speed * seconds;
                var nextRectangle = eyes.GetBoundingBox();
                nextRectangle.Y = (int)(nextY - eyes.CurrentTexture.Height / 2f);
                nextRectangle.X = (int)(nextRectangle.X - eyes.CurrentTexture.Width / 2f);
                if (currentRoom.checkCollisions(nextRectangle) == false)
                {
                    eyes.Location.Y = nextY;
                } else
                {
                    eyes.Location.Y = hwall.Height + eyes.CurrentTexture.Height / 2f;
                }
            }
            else if (kstate.IsKeyDown(Keys.Down))
            {
                eyes.CurrentTexture = eyes.Textures[2];
                var nextY = eyes.Location.Y + eyes.Speed * seconds;
                var nextRectangle = eyes.GetBoundingBox();
                nextRectangle.Y = (int)(nextY + eyes.CurrentTexture.Height / 2f);
                nextRectangle.X = (int)(nextRectangle.X - eyes.CurrentTexture.Width / 2f);
                if (currentRoom.checkCollisions(nextRectangle) == false)
                {
                    eyes.Location.Y = nextY;
                } else
                {
                    eyes.Location.Y = _graphics.PreferredBackBufferHeight - hwall.Height - eyes.CurrentTexture.Height / 2f;
                
                }

            }
            else if (kstate.IsKeyDown(Keys.Left))
            {
                eyes.CurrentTexture = eyes.Textures[3];
                var nextX = eyes.Location.X - eyes.Speed * seconds;
                var nextRectangle = eyes.GetBoundingBox();
                nextRectangle.X = (int)(nextX - eyes.CurrentTexture.Width / 2f);
                if (currentRoom.checkCollisions(nextRectangle) == false)
                {
                    eyes.Location.X = nextX;
                }
            }
            else if (kstate.IsKeyDown(Keys.Right))
            {
                eyes.CurrentTexture = eyes.Textures[4];
                var nextX = eyes.Location.X + eyes.Speed * seconds;
                var nextRectangle = eyes.GetBoundingBox();
                nextRectangle.X = (int)(nextX - eyes.CurrentTexture.Width / 2f);
                if (currentRoom.checkCollisions(nextRectangle) == false)
                {
                    eyes.Location.X = nextX;
                }
            }
            else
            {
                eyes.CurrentTexture = eyes.Textures[0];
            }

            if (kstate.IsKeyDown(Keys.Space) && eyes.MatchIsLit == false)
            {
                eyes.MatchIsLit = true;
                eyes.MatchLitTick = ticks;
            }
        }

        private Room GetRoom1()
        {
            var room = new Room(backgroundBuffer, 1);
            
            room.AddWall(new Vector2(0, 0), vwall);
            room.AddWall(new Vector2(0, backgroundBuffer.Y * 0.333f), vwall);
            room.AddWall(new Vector2(0, backgroundBuffer.Y * 0.666f), vwall);
            var voffset = backgroundBuffer.X - vwall.Width;
            room.AddWall(new Vector2(voffset, 0), vwall);
            room.AddWall(new Vector2(voffset, backgroundBuffer.Y * 0.666f), vwall);

            room.AddWall(new Vector2(0, 0), hwall);
            room.AddWall(new Vector2(backgroundBuffer.X * 0.333f, 0), hwall);
            room.AddWall(new Vector2(backgroundBuffer.X * 0.666f, 0), hwall);
            var hoffset = backgroundBuffer.Y - hwall.Height;
            room.AddWall(new Vector2(0, hoffset), hwall);
            room.AddWall(new Vector2(backgroundBuffer.X * 0.666f, hoffset), hwall);

            return room;
        }

        private Room GetRoom2()
        {
            var room = new Room(backgroundBuffer, 2);

            room.AddWall(new Vector2(0, 0), vwall);
            room.AddWall(new Vector2(0, backgroundBuffer.Y * 0.666f), vwall);
            var voffset = backgroundBuffer.X - vwall.Width;
            room.AddWall(new Vector2(voffset, 0), vwall);
            room.AddWall(new Vector2(voffset, backgroundBuffer.Y * 0.333f), vwall);
            room.AddWall(new Vector2(voffset, backgroundBuffer.Y * 0.666f), vwall);

            room.AddWall(new Vector2(0, 0), hwall);
            room.AddWall(new Vector2(backgroundBuffer.X * 0.333f, 0), hwall);
            room.AddWall(new Vector2(backgroundBuffer.X * 0.666f, 0), hwall);
            var hoffset = backgroundBuffer.Y - hwall.Height;
            room.AddWall(new Vector2(0, hoffset), hwall);
            room.AddWall(new Vector2(backgroundBuffer.X * 0.666f, hoffset), hwall);

            return room;
        }

        private void CheckRoomLeft()
        {
            switch(currentRoom.id) {
                case 1:
                    if(eyes.Location.X > backgroundBuffer.X)
                    {
                        currentRoom = GetRoom2();
                        eyes.Location.X = 0;
                    }
                    break;
                case 2:
                    if (eyes.Location.X < 0)
                    {
                        currentRoom = GetRoom1();
                        eyes.Location.X = backgroundBuffer.X;
                    }
                    break;
            }
        }

    }


}
