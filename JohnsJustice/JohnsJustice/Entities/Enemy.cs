using JohnsJustice.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace JohnsJustice.Entities
{
	public class Enemy : IGameEntity
	{
		public Vector2 Position { get; set; }

		private Sprite _idleSprite1;
		private Sprite _idleSprite2;
		private Sprite _idleSprite3;
		private Sprite _idleSprite4;

		private Sprite _punchSprite1;
		private Sprite _punchSprite2;
		private Sprite _punchSprite3;

		private SpriteAnimation _idleAnimation;
		private SpriteAnimation _punchAnimation;

		public int DrawOrder => 1;

		private Texture2D _texture;

		private int _health = 100;

		public Rectangle CollisionBox { get; set;}

		public Enemy(Texture2D spriteSheet, Vector2 position)
		{
			Position = position;
			_idleSprite1 = new Sprite(spriteSheet, 26, 14, 37, 50);
			_idleSprite2 = new Sprite(spriteSheet, 122, 14, 37, 50);
			_idleSprite3 = new Sprite(spriteSheet, 218, 15, 37, 50);
			_idleSprite4 = new Sprite(spriteSheet, 314, 14, 38, 50);

			_idleAnimation = new SpriteAnimation();
			_idleAnimation.AddFrame(_idleSprite1, 0);
			_idleAnimation.AddFrame(_idleSprite2, 0.2f);
			_idleAnimation.AddFrame(_idleSprite3, 0.5f);
			_idleAnimation.AddFrame(_idleSprite4, 0.75f);
			_idleAnimation.Play();

			_punchSprite1 = new Sprite(spriteSheet, 797, 14, 35, 50);
			_punchSprite2 = new Sprite(spriteSheet, 897, 14, 33, 50);
			_punchSprite3 = new Sprite(spriteSheet, 968, 16, 55, 50);

			_punchAnimation = new SpriteAnimation();
			_punchAnimation.ShouldLoop = false;
			_punchAnimation.AddFrame(_punchSprite1, 0);
			_punchAnimation.AddFrame(_punchSprite2, 0.1f);
			_punchAnimation.AddFrame(_punchSprite3, 0.4f);
			_punchAnimation.AddFrame(_punchSprite1, 0.6f);
			_punchAnimation.Play();


			CollisionBox = new Rectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), 40, 55);

			_texture = new Texture2D(spriteSheet.GraphicsDevice, 1, 1);
			_texture.SetData(new Color[] { Color.MonoGameOrange });


		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			spriteBatch.Draw(_texture, CollisionBox, Color.White);

			_idleAnimation.Draw(spriteBatch, Position);
		}

		public void Update(GameTime gameTime)
		{
			_idleAnimation.Update(gameTime);
		}

		public void Hurt(int damage)
		{
			_health -= damage;

			if (_health <= 0)
			{
				CollisionBox = Rectangle.Empty;

				_idleAnimation.Stop();

				_health = 0;
			}
		}
	}
}
