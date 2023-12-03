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

		private SpriteAnimation _idleAnimation;

		public int DrawOrder => 1;

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
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			_idleAnimation.Draw(spriteBatch, Position);
		}

		public void Update(GameTime gameTime)
		{
			_idleAnimation.Update(gameTime);
		}
	}
}
