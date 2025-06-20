using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace lesson21_MosquitoAttack_Final;

public class FastBullet 
{
    private Vector2 _position, _direction;
    private float _speed;
    private Rectangle _gameBoundingBox;
    private Texture2D _texture;

    private enum State { Flying, NotFlying }
    private State _state;
    private float _scale; 

    internal Rectangle BoundingBox => new Rectangle(_position.ToPoint(), new Point(_texture.Width, _texture.Height));

    internal void Initialize(Rectangle gameBoundingBox)
    {
        _gameBoundingBox = gameBoundingBox;
        _state = State.NotFlying;
        _scale = 1.0f;
    }

    internal void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("FastBullet-nobg"); // Make sure you add this asset
    }

    internal void Update(GameTime gameTime)
    {
        if (_state == State.Flying)
        {
            _position += _direction * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!BoundingBox.Intersects(_gameBoundingBox))
                _state = State.NotFlying;
        }
    }

    internal void Draw(SpriteBatch spriteBatch)
    {
        if (_state == State.Flying)
            spriteBatch.Draw(_texture, _position,null, Color.White,0f,Vector2.Zero,_scale,SpriteEffects.None, 0f);
    }

    internal bool Shoot(Vector2 position, Vector2 direction, float speed)
    {
        if (_state == State.NotFlying)
        {
            _position = new Vector2(position.X - _texture.Width / 2, position.Y);
            _direction = direction;
            _speed = speed;
            _state = State.Flying;
            return true;
        }
        return false;
    }

    internal bool ProcessCollision(Rectangle boundingBox)
    {
        if (_state == State.Flying && BoundingBox.Intersects(boundingBox))
        {
            _state = State.NotFlying;
            return true;
        }
        return false;
    }

    internal void SetInactive()
    {
        _state = State.NotFlying;
    }
}
