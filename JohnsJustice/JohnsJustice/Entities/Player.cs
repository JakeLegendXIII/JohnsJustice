using JohnsJustice.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

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

		private Sprite _walkingSprite1;
		private Sprite _walkingSprite2;
		private Sprite _walkingSprite3;
		private Sprite _walkingSprite4;

		private SpriteAnimation _idleAnimation;
		private SpriteAnimation _punchAnimation;
		private SpriteAnimation _walkingAnimation;

		private SoundEffectInstance _hit;
		private SoundEffectInstance _miss;

		private Texture2D _texture;

		private Enemy _enemy;

		public PlayerState State { get; set; }

		public int DrawOrder => 1;

		public Rectangle CollisionBox
		{
			get
			{
				Rectangle box = new Rectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), 40, 55);

				return box;
			}
		}


		public Player(Texture2D spriteSheet, Vector2 position, SoundEffectInstance hit, SoundEffectInstance miss, Enemy enemy)
		{
			State = PlayerState.Idle;

			Position = position;
			_hit = hit;
			_miss = miss;

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
			_punchAnimation.AddFrame(_punchSprite1, 0.6f);
			_punchAnimation.Play();

			_walkingSprite1 = new Sprite(spriteSheet, 415, 14, 35, 50);
			_walkingSprite2 = new Sprite(spriteSheet, 512, 14, 35, 50);
			_walkingSprite3 = new Sprite(spriteSheet, 608, 13, 35, 50);
			_walkingSprite4 = new Sprite(spriteSheet, 705, 14, 35, 50);

			_walkingAnimation = new SpriteAnimation();
			_walkingAnimation.ShouldLoop = false;
			_walkingAnimation.AddFrame(_walkingSprite1, 0);
			_walkingAnimation.AddFrame(_walkingSprite2, 0.2f);
			_walkingAnimation.AddFrame(_walkingSprite3, 0.5f);
			_walkingAnimation.AddFrame(_walkingSprite4, 0.75f);
			_walkingAnimation.Play();

			_texture = new Texture2D(spriteSheet.GraphicsDevice, 1, 1);
			_texture.SetData(new Color[] { Color.MonoGameOrange });
			_enemy = enemy;
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{

			spriteBatch.Draw(_texture, CollisionBox, Color.White);

			if (State == PlayerState.Idle)
			{
				_idleAnimation.Draw(spriteBatch, Position);
			}
			else if (State == PlayerState.Punching)
			{
				_punchAnimation.Draw(spriteBatch, Position);
			}
			else if (State == PlayerState.Walking)
			{
				_walkingAnimation.Draw(spriteBatch, Position);
			}
		}

		public void Update(GameTime gameTime)
		{
			if (State == PlayerState.Punching)
			{
				if (!_punchAnimation.IsPlaying)
				{
					State = PlayerState.Idle;

					_idleAnimation.Play();
				}

				if (_punchAnimation.CurrentFrame == _punchAnimation.GetFrame(2))
				{
					if (EnemyCollision() && _hit.State != SoundState.Playing)
					{
						// Damage Event to Enemy? Something here

						_hit.Play();
					}
					else if (_miss.State != SoundState.Playing)
					{
						_miss.Play();
					}
				}

				_punchAnimation.Update(gameTime);

			}
			else if (State == PlayerState.Idle)
			{
				_idleAnimation.Update(gameTime);

				if (!_idleAnimation.IsPlaying)
				{
					_idleAnimation.Play();
				}
			}
			else if (State == PlayerState.Walking)
			{
				_walkingAnimation.Update(gameTime);

				if (!_walkingAnimation.IsPlaying)
				{
					State = PlayerState.Idle;

					_idleAnimation.Play();
				}
			}
		}

		private bool EnemyCollision()
		{
			return CollisionBox.Intersects(_enemy.CollisionBox);
		}

		public bool BeginPunch()
		{
			if (State == PlayerState.Punching)
				return false;

			State = PlayerState.Punching;
			_punchAnimation.Play();

			return true;
		}

		public bool WalkRight(GameTime gameTime)
		{
			State = PlayerState.Walking;
			_walkingAnimation.Play();

			if (!EnemyCollision())
			{
				Position = new Vector2(Position.X + 35f * (float)gameTime.ElapsedGameTime.TotalSeconds, Position.Y);
			}

			return true;
		}

		public bool WalkLeft(GameTime gameTime)
		{
			State = PlayerState.Walking;
			_walkingAnimation.Play();

			Position = new Vector2(Position.X - 65f * (float)gameTime.ElapsedGameTime.TotalSeconds, Position.Y);

			return true;
		}
	}

	public enum PlayerState
	{
		Idle,
		Punching,
		Walking,
		Hit
	}
}
