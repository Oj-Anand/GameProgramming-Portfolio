using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace lesson21_MosquitoAttack_Final;

public class MosquitoAttack : Game
{
    private const int _WindowWidth = 550, _WindowHeight = 400, _NumMosquitoes = 10; //edited to 10 for playability
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _background;
    private SpriteFont _arial;

    private Cannon _cannon;
    private Texture2D _hudBackground;
    private Rectangle _hudRectangle = new Rectangle(0, 0, _WindowWidth, 30); 
    // (0,0) at top-left, width matches window, height is 30 pixels

    private Mosquito [] _mosquitoes;

    private enum GameState { Menu, Level01, Level02, Paused, Over }
    private GameState _gameState;
    private GameState _previousGameState;

    private KeyboardState _kbPreviousState;
    private string _status = "";

    public MosquitoAttack()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = _WindowWidth;
        _graphics.PreferredBackBufferHeight = _WindowHeight;
        _graphics.ApplyChanges();

        _cannon = new Cannon();

        _mosquitoes = new Mosquito[_NumMosquitoes];
        for(int c = 0; c < _NumMosquitoes; c++)
        {
            _mosquitoes[c] = new Mosquito(); 
        }
        base.Initialize(); //this method call invokes LoadContent, thereby making cannon._animationSequence exist

        Rectangle gameBoundingBox = new Rectangle(0, 0, _WindowWidth, _WindowHeight);
        _cannon.Initialize(new Vector2(50, 325), gameBoundingBox);

        Random random = new Random();
        foreach(Mosquito mosquito in _mosquitoes)
        {
            int direction = random.Next(1, 3);
            if(direction == 2)
            {
                direction = -1;
            }
            int xPosition = random.Next(1, _WindowWidth - mosquito.BoundingBox.Width);
            int speed = random.Next(50, 251);
            mosquito.Initialize(new Vector2(xPosition, 25), gameBoundingBox, speed, new Vector2(direction, 0));
        }
        _gameState = GameState.Menu;
        _kbPreviousState = Keyboard.GetState();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _background = Content.Load<Texture2D>("Background");
        _arial = Content.Load<SpriteFont>("SystemArialFont");
        _cannon.LoadContent(Content);
        
        foreach(Mosquito mosquito in _mosquitoes)
        {
            mosquito.LoadContent(Content);
        }
        _hudBackground = new Texture2D(GraphicsDevice, 1, 1);
        _hudBackground.SetData(new[] { Color.Black * 0.7f }); // Slight opacity

    }

    protected override void Update(GameTime gameTime)
    {
        KeyboardState kbState = Keyboard.GetState();

        switch(_gameState)
        {
            case GameState.Menu:
                _status = "Press ENTER to start";
                if(kbState.IsKeyDown(Keys.Enter) && _kbPreviousState.IsKeyUp(Keys.Enter))
                {
                    StartLevel(1); 
                }
                break;
            case GameState.Level01:
                UpdateGameplay(gameTime, kbState);
                break;
            case GameState.Level02:
                UpdateGameplay(gameTime, kbState);
                break;
            case GameState.Paused:
                _status = "Paused - Press P to resume";
                if(kbState.IsKeyDown(Keys.P)&& _kbPreviousState.IsKeyUp(Keys.P))
                {
                   _gameState = _previousGameState; //player presses p to start playing again 
                }
                break;
            case GameState.Over:
                _status = "Game Over - Press Enter to start again";
                if (kbState.IsKeyDown(Keys.Enter) && _kbPreviousState.IsKeyUp(Keys.Enter))
                _gameState = GameState.Menu;
                break; 
        }
    
        _kbPreviousState = kbState;
        base.Update(gameTime);
    }

    private void UpdateGameplay(GameTime gameTime, KeyboardState kbState)
    {
        //handle cannon movement 
        if (kbState.IsKeyDown(Keys.Left))
            _cannon.Direction = new Vector2(-1, 0);
        else if (kbState.IsKeyDown(Keys.Right))
            _cannon.Direction = new Vector2(1, 0);
        else
            _cannon.Direction = Vector2.Zero;
        
        //shooting logic
        if(kbState.IsKeyDown(Keys.Space) && _kbPreviousState.IsKeyUp(Keys.Space))
        {
            _cannon.Shoot();
        }
        if(kbState.IsKeyDown(Keys.F) && _kbPreviousState.IsKeyUp(Keys.F))
        {
            _cannon.ShootFastBullet();
        }
        _cannon.Update(gameTime);

        bool allMosqitoesDead = true; 
        foreach(Mosquito mosquito in _mosquitoes)
        {
            mosquito.Update(gameTime);
            if(mosquito.Alive)
            {
                allMosqitoesDead = false;
                if(_cannon.ProcessCollision(mosquito.BoundingBox) )
                {
                    mosquito.Die();
                }

                //fast bullet collision
                foreach(var bullet in _cannon.FastBullets)
                {
                    if(bullet.ProcessCollision(mosquito.BoundingBox))
                    {
                        mosquito.Die();
                    }
                }
            }

            if(mosquito.FireballIsActive && mosquito.FIreballBoundingBox.Intersects(_cannon.BoundingBox))
            {
                _cannon.TakeDamage();

                if (_cannon.Lives > 0)
                {
                    // respawn cannon at initial position
                    _cannon.Respawn(new Vector2(50, 325));
                }
                else
                {
                    // set game state to Game Over if no lives left
                    _gameState = GameState.Over;
                }

                mosquito.DeactivateFireball();
            }
        }

        if(allMosqitoesDead)
        {
            if(_gameState == GameState.Level01)
            {
                StartLevel(2);
            }
            else if(_gameState == GameState.Level02)
            {
                _gameState = GameState.Over;
                _status = "Game Over!";
            }

        }

        // Pausing gameplay
        if (kbState.IsKeyDown(Keys.P) && _kbPreviousState.IsKeyUp(Keys.P))
        {
            _previousGameState = _gameState;
            _gameState = GameState.Paused;
        }

        //if the player gets hit X number of times, game over
        if(_cannon.Lives == 0)
        {
            _gameState = GameState.Over;
            _status = "Game Over! You ran out of lives.";
        }

        //reloading logic 
        if (kbState.IsKeyDown(Keys.R) && _kbPreviousState.IsKeyUp(Keys.R))
        {
            _cannon.Reload();
        }

    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        _spriteBatch.Draw(_background, Vector2.Zero, Color.White);

        switch(_gameState)
        {
            case GameState.Menu:
                _spriteBatch.DrawString(_arial, _status, new Vector2(20, 50), Color.White);
                break;
            case GameState.Level01:
            case GameState.Level02:
                _cannon.Draw(_spriteBatch);
                
                foreach(Mosquito mosquito in _mosquitoes)
                {
                    mosquito.Draw(_spriteBatch);
                }
                break;
            case GameState.Paused:
                _spriteBatch.DrawString(_arial, _status, new Vector2(20, 50), Color.White);
                break;
            case GameState.Over:
                _spriteBatch.DrawString(_arial, _status, new Vector2(20, 50), Color.White);
                break;
        }
        // Drawing HUD Background 
        _spriteBatch.Draw(_hudBackground, _hudRectangle, Color.White);

        // Drawing HUD Text 
        string livesText = $"Lives: {_cannon.Lives}";
        string levelText = _gameState == GameState.Level01 ? "Level: 1" : "Level: 2";
        string ammoText = _cannon.IsOutOfAmmo ? "Reload! (Press R)" : $"Ammo: {_cannon.CurrentAmmo}";

        // Positioning text 
        _spriteBatch.DrawString(_arial, livesText, new Vector2(10, 5), Color.White);
        _spriteBatch.DrawString(_arial, levelText, new Vector2(150, 5), Color.White);
        _spriteBatch.DrawString(_arial, ammoText, new Vector2(290, 5), _cannon.IsOutOfAmmo ? Color.Yellow : Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }

private void StartLevel(int levelNumber)
{
    Rectangle gameBoundingBox = new Rectangle(0, 0, _WindowWidth, _WindowHeight);
    _cannon.Initialize(new Vector2(50, 325), gameBoundingBox);

    Random random = new Random();

    foreach (Mosquito mosquito in _mosquitoes)
    {
        int direction = random.Next(1, 3) == 2 ? -1 : 1;
        int xPosition = random.Next(1, _WindowWidth - mosquito.BoundingBox.Width);
        int speed = random.Next(50, 251);
        mosquito.Initialize(new Vector2(xPosition, 25), gameBoundingBox, speed, new Vector2(direction, 0));
    }

    if (levelNumber == 1)
    {
        _cannon.Reset();
        _gameState = GameState.Level01;
    }
    else if (levelNumber == 2)
        _gameState = GameState.Level02;
}
}
