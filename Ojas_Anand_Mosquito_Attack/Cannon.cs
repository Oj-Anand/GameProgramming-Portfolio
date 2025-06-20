using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;



namespace lesson21_MosquitoAttack_Final;

public class Cannon 
{
    private const float _Speed = 250;
    private const int _NumCannonBalls = 10;
    private CelAnimationSequence _animationSequence;
    private CelAnimationPlayer _animationPlayer;
    private Vector2 _position, _direction;
    private float _speed;
    //private float _scale;
    private Rectangle _gameBoundingBox;
    public Vector2 Direction { set => _direction = value; }
    private int _lives = 3; 
    public int Lives => _lives;
    private const int _MaxAmmo = 10;
    private int _currentAmmo = _MaxAmmo;

    public bool IsOutOfAmmo => _currentAmmo <= 0;
    public int CurrentAmmo => _currentAmmo;

    private enum State { Ready, Reloading, Empty }
    private State _state = State.Ready;


    //FastBullet variables
    private const int _NumFastBullets = 10;
    private FastBullet[] _fastBullets;
    public FastBullet[] FastBullets => _fastBullets; // public getter
    public Rectangle BoundingBox 
        {get => new Rectangle((int) _position.X, (int) _position.Y, _animationSequence.CelWidth, _animationSequence.CelHeight);}
    private CannonBall[] _cannonBalls;
    
    public Cannon()
    {
        _cannonBalls = new CannonBall[_NumCannonBalls];
        for(int c = 0; c < _NumCannonBalls; c++)
        {
            _cannonBalls[c] = new CannonBall();
        }
        
        _fastBullets = new FastBullet[_NumFastBullets];
        for (int i = 0; i < _NumFastBullets; i++)
        {
            _fastBullets[i] = new FastBullet();
        }
            
    }
    
    internal void Initialize(Vector2 initialPosition, Rectangle gameBoundingBox)
    {
        //_scale = 1.0f;
        _position = initialPosition;
        _animationPlayer = new CelAnimationPlayer();
        _animationPlayer.Play(_animationSequence);
        _speed = _Speed; //we have a _speed data member in case we want to add _scale later
        _gameBoundingBox = gameBoundingBox;
        _state = State.Ready;

        foreach(CannonBall cBall in _cannonBalls)
        {
            cBall.Initialize(gameBoundingBox);
        }
        foreach(FastBullet bullet in _fastBullets)
        {
            bullet.Initialize(gameBoundingBox);
        }
    }
    internal void LoadContent(ContentManager content)
    {
        _animationSequence = 
            new CelAnimationSequence(content.Load<Texture2D>("Cannon"), 40, 1 / 8.0f);
        foreach(CannonBall cBall in _cannonBalls)
        {
            cBall.LoadContent(content);
        }
        foreach(FastBullet bullet in _fastBullets)
        {
            bullet.LoadContent(content);
        }
    }
    internal void Update(GameTime gameTime)
    {
        _position += _direction * _speed * (float) gameTime.ElapsedGameTime.TotalSeconds;
        if(BoundingBox.Left < _gameBoundingBox.Left)
        {
            _position.X = _gameBoundingBox.Left;
        }
        else if(BoundingBox.Right > _gameBoundingBox.Right)
        {
            _position.X = _gameBoundingBox.Right - BoundingBox.Width;
        }
        else//if(!_direction.Equals(Vector2.Zero))
        {
            //another way: if(_direction.X != 0)
            //if we're in this "else", the cannon is not on the sides
            if(!_direction.Equals(Vector2.Zero))
            {
                //if we're in this "if", the cannon is moving
                _animationPlayer.Update(gameTime);
            }
        }
        foreach(CannonBall cBall in _cannonBalls)
        {
            cBall.Update(gameTime);
        }
        foreach(FastBullet bullet in _fastBullets)
        {
            bullet.Update(gameTime);
        }
    }
    internal void Draw(SpriteBatch spriteBatch)
    {
        _animationPlayer.Draw(spriteBatch, _position, SpriteEffects.None);
        foreach(CannonBall cBall in _cannonBalls)
        {
            cBall.Draw(spriteBatch);
        }
        foreach(FastBullet bullet in _fastBullets)
        {
            bullet.Draw(spriteBatch);
        }
    }

    internal void Shoot()
    {
        if (_state == State.Empty) return;//dont shoot if out of ammo

        int cannonBallIndex = 0;
        bool shot = false;
         
        while(cannonBallIndex < _NumCannonBalls && !shot)
        {
            shot = _cannonBalls[cannonBallIndex].Shoot(new Vector2(BoundingBox.Center.X, BoundingBox.Top), new Vector2(0, -1), 50);
            cannonBallIndex++;
        }
        if(shot)
        {
            _currentAmmo--; //decrease ammo once a cannonball is shot
            if (_currentAmmo <= 0)
            {
                _state = State.Empty; 
            }
            
        }
    }
    internal bool ProcessCollision(Rectangle boundingBox)
    {
        bool hit = false;
        int c = 0;
        while(!hit && c < _cannonBalls.Length)
        {
            hit = _cannonBalls[c].ProcessCollision(boundingBox);
            c++;
        }
        return hit;
    }


    //Fast bullet methods
    internal void ShootFastBullet()
    {
        int bulletIndex = 0;
        bool shot = false;

        while(bulletIndex < _NumFastBullets && !shot)
        {
            shot = _fastBullets[bulletIndex].Shoot(
                new Vector2(BoundingBox.Center.X, BoundingBox.Top), 
                new Vector2(0, -1), 
                200 // Faster than cannonballs (50)
            );
            bulletIndex++;
        }
    }

    //HELPER METHODS
    public void TakeDamage(int amount = 1)
    {
        _lives -= amount;
        if (_lives < 0) _lives = 0; //no negative life value
        Console.WriteLine($"[DEBUG] Player hit! Lives left: {_lives}");
    }
    public void Reset()
    {
        _lives = 3;
        _currentAmmo = _MaxAmmo;
        _state = State.Ready; // Ready on reset
    }
    internal void Respawn(Vector2 respawnPosition)
    {
        _position = respawnPosition;
        _direction = Vector2.Zero;

        // (optional) Reset cannonballs, animations, etc.
    }

    internal void Reload()
    {
        _state = State.Reloading;
        foreach (CannonBall ball in _cannonBalls)
            ball.SetInactive();

        _currentAmmo = _MaxAmmo;
        FinishReloading();
    }

    internal void FinishReloading()
    {
        if (_currentAmmo > 0)
            _state = State.Ready;
    }

    internal void ResetFastBullets()
    {
        foreach (FastBullet bullet in _fastBullets)
            bullet.SetInactive();
    }



}