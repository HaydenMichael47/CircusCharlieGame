using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace CircusCharlieGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Model player;
        Model barrel;
        List<Vector3> barrelsPos;

        Model ground;
        SpriteFont gameFont;
        SpriteFont developedFont;

        int score;
        bool gameOver;
        bool gameStart;
        bool countDown;
        bool notAtMaxHeight;
        int count;

        Matrix proj;
        Matrix view;
        Matrix world;

        Matrix worldGround;

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

            barrelsPos = new List<Vector3>();

            score = 0;
            difficulty = 0.05f;
            gameOver = false;
            gameStart = false;
            countDown = false;
            notAtMaxHeight = true;
            count = 10;

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

            worldGround = Matrix.CreateScale(10, 0.001f, 10);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            player = Content.Load<Model>("Animal_Rigged_Zebu_01");
            barrel = Content.Load<Model>("Barrel_Sealed_01");
            ground = Content.Load<Model>("Uneven_Ground_Dirt_01");
            gameFont = Content.Load<SpriteFont>("gamefont");
            developedFont = Content.Load<SpriteFont>("DevelopedFont");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            count--;

            if (count == 0)
                countDown = false;


            if(gameStart == false)
            {
                if(Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    gameStart = true;
                    countDown = true;
                }
            }

            //reset the game
            if(gameOver == true)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    gameOver = false;
                    gameStart = true;
                    barrelsPos.Clear();
                    playerRotX = 0f;
                    playerPos = new Vector3(-2, 0, 5);
                    score = 0;
                    difficulty = 0.05f;
                }
            }
            if(gameStart == true)
            {
                if (gameOver == false)
                {

                    if(Math.Floor(gameTime.TotalGameTime.TotalMilliseconds) % 2000 == 0) //add another barrel
                    {
                        barrelsPos.Add(new Vector3(3.5f, 0, rand.Next(4,6)));
                    }


                    if (score % 500 == 0)
                    {
                        difficulty = difficulty * 1.25f;
                    }

                    //update barrels position

                    //barrelPos.X -= difficulty;
                    //if (barrelPos.X < -4)
                    //{
                    //barrelPos.X = 3f;
                    //barrelPos.Z = rand.Next(4, 6);
                    //}
                    for (int b = 0; b < barrelsPos.Count; b++)
                    {
                        barrelsPos[b] -= new Vector3((float)(difficulty), 0, 0);
                        if (barrelsPos[b].X < -4)
                        {
                            barrelsPos.RemoveAt(b);
                            //barrelsPos[b].Z = rand.Next(4, 6);
                        }

                        
                    }

                    if (playerPos.Y == 0 && notAtMaxHeight == false)
                        notAtMaxHeight = true;

                    if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    {

                        playerPos.Z -= .2f;
                        if (playerPos.Z < 2)
                            playerPos.Z = 2f;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    {
                        playerPos.Z += .2f;
                        if (playerPos.Z > 5)
                            playerPos.Z = 5f;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Space) && notAtMaxHeight == true)
                    {
                        playerPos.Y += .30f;
                    }
                    if (playerPos.Y > 0)
                    {
                        playerPos.Y -= .05f;
                        if (playerPos.Y < 0)
                            playerPos.Y = 0f;
                    }
                    if (playerPos.Y > 2)
                    {
                        playerPos.Y = 2f;
                        notAtMaxHeight = false;
                    }
                        


                    foreach(Vector3 b in barrelsPos)
                    {
                        if (Vector3.Distance(b, playerPos) < 0.9f)
                        {
                            playerRotX = -90;
                            gameOver = true;
                        }
                    }

                    //if (Vector3.Distance(barrelPos, playerPos) < 1f)
                    //{
                        //playerRotX = -90;
                        //gameOver = true;
                    //}


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
           

            if (gameStart==true)
            {
                ground.Draw(worldGround, view, proj);
                //draw a barrel at each position
                foreach (Vector3 b in barrelsPos)
                {
                    barrelWorld = Matrix.CreateScale(0.003f) *
                            Matrix.CreateRotationY(MathHelper.ToRadians(playerRotY)) *
                            Matrix.CreateTranslation(b);
                    // barrel.Draw(barrelWorld, view, proj);

                    foreach (ModelMesh mesh in barrel.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.World = barrelWorld;
                            effect.View = view;
                            effect.Projection = proj;
                            effect.EnableDefaultLighting();
                            effect.LightingEnabled = true;
                            //effect.EmissiveColor = new Vector3(0.5f, 0.5f, 0.5f);
                        }
                        mesh.Draw();
                    }

                }





                //barrel.Draw(barrelWorld, view, proj);
                //player.Draw(world, view, proj);

                foreach (ModelMesh mesh in player.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = world;
                        effect.View = view;
                        effect.Projection = proj;
                        effect.EnableDefaultLighting();
                        effect.LightingEnabled = true;
                        // effect.EmissiveColor = new Vector3(0.5f, 0.5f, 0.5f);
                    }
                    mesh.Draw();
                }

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


                    _spriteBatch.DrawString(gameFont, "Game Over!", new Vector2(300, 200), Color.Black);
                    _spriteBatch.DrawString(gameFont, "Score = " + score.ToString(), new Vector2(300, 225), Color.Black);
                    _spriteBatch.DrawString(gameFont, "Press the Space bar to play again.", new Vector2(300, 250), Color.Black);

                    _spriteBatch.DrawString(developedFont, "Developed by Hayden Michael", new Vector2(550, 450), Color.Black);

                    _spriteBatch.End();
                }





                
                
            }
            else
            {
                _spriteBatch.Begin();

                _spriteBatch.DrawString(gameFont, "Press the Space bar to start.", new Vector2(275, 225), Color.Black);
                
                

                _spriteBatch.End();
            }
            



            base.Draw(gameTime);
        }
    }
}
