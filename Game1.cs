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
        bool notAtMaxHeight;


        Matrix proj;
        Matrix view;
        Matrix world;

        Matrix worldGround;

        float difficulty;
        float timerDifficulty;

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
            timerDifficulty = 0f;

            gameOver = false;
            gameStart = false;
            notAtMaxHeight = true;
       

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

           

            if(gameStart == false)
            {
                if(Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    gameStart = true;
                    //countDown = true;
                }
            }

            //reset the game
            if(gameOver == true)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    gameOver = false;
                    gameStart = true;
                    barrelsPos.Clear();
                    playerRotX = 0f;
                    playerPos = new Vector3(-2, 0, 5);
                    score = 0;
                    difficulty = 0.05f;
                    timerDifficulty = 0f;
                }
            }
            if(gameStart == true)
            {
                if (gameOver == false)
                {
                    //used to change the difficulty
                    //Every five hundred points, make the barrels move faster and spawn more often
                    if (score != 0 && score % 500 == 0)
                    {
                        difficulty = difficulty * 1.25f;
                        if (timerDifficulty < 1000f)
                            timerDifficulty += 200;



                    }

                    if (Math.Floor(gameTime.TotalGameTime.TotalMilliseconds) % (2000-timerDifficulty) == 0) //add another barrel
                    {
                        barrelsPos.Add(new Vector3(3.5f, 0, rand.Next(4,6)));
                    }
                    


                    

                    //move each barrel towards the player
                    for (int b = 0; b < barrelsPos.Count; b++)
                    {
                        barrelsPos[b] -= new Vector3((float)(difficulty), 0, 0);
                        if (barrelsPos[b].X < -4)
                        {
                            barrelsPos.RemoveAt(b); //remove the barrel when off the screen
                           
                        }

                        
                    }

                    if (playerPos.Y == 0 && notAtMaxHeight == false) //used to determine when player should fall after a jump
                        notAtMaxHeight = true;

                    //update movement
                    if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    {

                        playerPos.Z -= .2f;
                        if (playerPos.Z < 3)
                            playerPos.Z = 3f;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    {
                        playerPos.Z += .2f;
                        if (playerPos.Z > 5.5f)
                            playerPos.Z = 5.5f;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Space) && notAtMaxHeight == true)
                    {
                        playerPos.Y += .30f; //jump if not already in the air
                    }
                    if (playerPos.Y > 0) //activate 'gravity' if in the air
                    {
                        playerPos.Y -= .05f;
                        if (playerPos.Y < 0)
                            playerPos.Y = 0f;
                    }
                    if (playerPos.Y > 2.5)
                    {
                        playerPos.Y = 2.5f;
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

                 
                    //update world matrices and score
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


                //Draw the player
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

                if (gameOver == false) //While the player is playing, display the score on the screen
                {
                    

                    _spriteBatch.Begin();

                    _spriteBatch.DrawString(gameFont, "Score = " + score.ToString(), new Vector2(20, 20), Color.Black);
                    //_spriteBatch.DrawString(gameFont, "Time = " + (Math.Floor(gameTime.TotalGameTime.TotalMilliseconds) % (2000 - timerDifficulty)).ToString(), new Vector2(20, 50), Color.Black);


                    _spriteBatch.End();
                }
                else //The game is over so display results
                {
                    _spriteBatch.Begin();


                    _spriteBatch.DrawString(gameFont, "Game Over!", new Vector2(325, 50), Color.Black);
                    _spriteBatch.DrawString(gameFont, "Score = " + score.ToString(), new Vector2(325, 75), Color.Black);
                    _spriteBatch.DrawString(gameFont, "Press the Enter key to play again.", new Vector2(225, 150), Color.Black);

                    _spriteBatch.DrawString(developedFont, "Developed by Hayden Michael", new Vector2(550, 450), Color.Black);

                    _spriteBatch.End();
                }





                
                
            }
            else //Messages to begin the game
            {
                _spriteBatch.Begin();

                _spriteBatch.DrawString(gameFont, "Controls", new Vector2(350, 100), Color.Black);
                _spriteBatch.DrawString(gameFont, "Up Arrow and Down Arrow: Moves the player up or down", new Vector2(150, 125), Color.Black);
                _spriteBatch.DrawString(gameFont, "Spacebar: Makes the player jump then fall down", new Vector2(150, 150), Color.Black);
                _spriteBatch.DrawString(gameFont, "Press the Enter key to start.", new Vector2(250, 275), Color.Black);
                
                

                _spriteBatch.End();
            }
            



            base.Draw(gameTime);
        }
    }
}
