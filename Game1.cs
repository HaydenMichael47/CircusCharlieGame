using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace CircusCharlieGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Model player;
        Model barrel;
        SpriteFont gameFont;

        int score;
        bool gameOver;
        bool gameStart;

        Matrix proj;
        Matrix view;
        Matrix world;

        float difficulty;

        Vector3 camPos;
        Vector3 playerPos;

        float playerRotY;
        float playerRotX;

        Vector3 barrelPos;
        Matrix barrelWorld;

        Random rand;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            score = 0;
            difficulty = 0.05f;
            gameOver = false;
            gameStart = false;

            rand = new Random();

            camPos = new Vector3(0, 3, 10);
            playerPos = new Vector3(-2, 0, 5);
            playerRotX = 0f;
            playerRotY = 90f;

            barrelPos = new Vector3(3, 0, 5);

            proj = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(60), 1, 0.001f, 1000f);
            view = Matrix.CreateLookAt(camPos, Vector3.Zero, Vector3.Up);
            world = Matrix.CreateScale(0.002f) *
                    Matrix.CreateRotationY(MathHelper.ToRadians(playerRotY)) *
                    Matrix.CreateTranslation(playerPos);

            barrelWorld = Matrix.CreateScale(0.04f) *
                    Matrix.CreateRotationX(MathHelper.ToRadians(90)) *
                    Matrix.CreateTranslation(barrelPos);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            player = Content.Load<Model>("Animal_Rigged_Zebu_01");
            barrel = Content.Load<Model>("Barrel_Sealed_01");
            gameFont = Content.Load<SpriteFont>("gamefont");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            if(gameStart == false)
            {
                if(Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    gameStart = true;
                }
            }
            if(gameStart == true)
            {
                if (gameOver == false)
                {


                    if (score % 500 == 0)
                    {
                        difficulty = difficulty * 1.25f;
                    }

                    barrelPos.X -= difficulty;
                    if (barrelPos.X < -4)
                    {
                        barrelPos.X = 3f;
                        barrelPos.Z = rand.Next(4, 6);
                    }


                    if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    {

                        playerPos.Z -= .2f;
                        if (playerPos.Z < 0)
                            playerPos.Z = 0f;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    {
                        playerPos.Z += .2f;
                        if (playerPos.Z > 5)
                            playerPos.Z = 5f;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        playerPos.Y += .40f;
                    }
                    if (playerPos.Y > 0)
                    {
                        playerPos.Y -= .2f;
                        if (playerPos.Y < 0)
                            playerPos.Y = 0f;
                    }
                    if (playerPos.Y > 2)
                        playerPos.Y = 2f;

                    if (Vector3.Distance(barrelPos, playerPos) < 1f)
                    {
                        playerRotX = -90;
                        gameOver = true;
                    }


                    world = Matrix.CreateScale(0.002f) *
                            Matrix.CreateRotationY(MathHelper.ToRadians(playerRotY)) *
                            Matrix.CreateRotationX(MathHelper.ToRadians(playerRotX)) *
                            Matrix.CreateTranslation(playerPos);

                    barrelWorld = Matrix.CreateScale(0.04f) *
                            Matrix.CreateRotationY(MathHelper.ToRadians(playerRotY)) *
                            Matrix.CreateTranslation(barrelPos);

                    score++;
                }
            }
            

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            if(gameStart==true)
            {
                if (gameOver == false)
                {
                    _spriteBatch.Begin();

                    _spriteBatch.DrawString(gameFont, "Score = " + score.ToString(), new Vector2(20, 20), Color.Black);

                    //_spriteBatch.DrawString(gameFont, "barrelX = " + barrelPos.X.ToString(), new Vector2(20, 80), Color.Black);
                    //_spriteBatch.DrawString(gameFont, "barrelY = " + barrelPos.Y.ToString(), new Vector2(20, 120), Color.Black);
                    //_spriteBatch.DrawString(gameFont, "barrelZ = " + barrelPos.Z.ToString(), new Vector2(20, 160), Color.Black);

                    //_spriteBatch.DrawString(gameFont, "playerX = " + playerPos.X.ToString(), new Vector2(150, 80), Color.Black);
                    //_spriteBatch.DrawString(gameFont, "PlaterY = " + playerPos.Y.ToString(), new Vector2(150, 120), Color.Black);
                    //_spriteBatch.DrawString(gameFont, "PlayerZ = " + playerPos.Z.ToString(), new Vector2(150, 160), Color.Black);

                    _spriteBatch.End();
                }
                else
                {
                    _spriteBatch.Begin();


                    _spriteBatch.DrawString(gameFont, "Game Over!", new Vector2(200, 200), Color.Black);
                    _spriteBatch.DrawString(gameFont, "Score = " + score.ToString(), new Vector2(200, 225), Color.Black);



                    _spriteBatch.End();
                }







                barrel.Draw(barrelWorld, view, proj);
                player.Draw(world, view, proj);
            }
            else
            {
                _spriteBatch.Begin();


                _spriteBatch.DrawString(gameFont, "Press the Space bar to start.", new Vector2(350, 200), Color.Black);
                



                _spriteBatch.End();
            }
            



            base.Draw(gameTime);
        }
    }
}
