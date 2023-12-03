using JohnsJustice.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection.Metadata.Ecma335;

namespace JohnsJustice.Entities
{
	public class Player : IGameEntity
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

		public PlayerState State { get; set; }

		public int DrawOrder => 1;

		public Player(Texture2D spriteSheet, Vector2 position)
		{
			State = PlayerState.Idle;

			Position = position;

			_idleSprite1 = new Sprite(spriteSheet, 30, 14, 35, 50);
			_idleSprite2 = new Sprite(spriteSheet, 128, 14, 31, 50);
			_idleSprite3 = new Sprite(spriteSheet, 223, 16, 32, 50);
			_idleSprite4 = new Sprite(spriteSheet, 319, 14, 32, 50);

			_idleAnimation = new SpriteAnimation();
			_idleAnimation.AddFrame(_idleSprite1, 0);
			_idleAnimation.AddFrame(_idleSprite2, 0.2f);
			_idleAnimation.AddFrame(_idleSprite3, 0.5f);
			_idleAnimation.AddFrame(_idleSprite4, 0.75f);
			_idleAnimation.Play();

			_punchSprite1 = new Sprite(spriteSheet, 802, 14, 35, 50);
			_punchSprite2 = new Sprite(spriteSheet, 892, 15, 35, 50);
			_punchSprite3 = new Sprite(spriteSheet, 985, 17, 48, 50);

			_punchAnimation = new SpriteAnimation();
			_punchAnimation.ShouldLoop = false;
			_punchAnimation.AddFrame(_punchSprite1, 0);
			_punchAnimation.AddFrame(_punchSprite2, 0.1f);
			_punchAnimation.AddFrame(_punchSprite3, 0.4f);
			_punchAnimation.AddFrame(_idleSprite1, 0.6f);
			_punchAnimation.Play();

		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			if (State == PlayerState.Idle)
			{
				_idleAnimation.Draw(spriteBatch, Position);
			}
			else if (State == PlayerState.Punching)
			{
				_punchAnimation.Draw(spriteBatch, Position);
			}
		}

		public void Update(GameTime gameTime)
		{
			if (State == PlayerState.Punching)
			{
				_punchAnimation.Update(gameTime);

				if (_punchAnimation.PlaybackProgress == 0 || _punchAnimation.IsPlaying == false)
				{
					State = PlayerState.Idle;
				}
			}
			else if (State == PlayerState.Idle)
			{
				_idleAnimation.Update(gameTime);
			}
		}

		public bool BeginPunch()
		{
			if (State == PlayerState.Punching)
				return false;

			State = PlayerState.Punching;

			return true;
		}
	}

	public enum PlayerState
	{
		Idle,
		Punching
	}
}
