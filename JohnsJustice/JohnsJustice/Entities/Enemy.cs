using JohnsJustice.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace JohnsJustice.Entities
{
	public class Enemy : IGameEntity
	{
		public Player Player { get; set; }

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

		private Sprite _hurtSprite1;
		private Sprite _hurtSprite2;
		private Sprite _hurtSprite3;
		private Sprite _hurtSprite4;

		private SpriteAnimation _idleAnimation;
		private SpriteAnimation _punchAnimation;
		private SpriteAnimation _walkingAnimation;
		private SpriteAnimation _hurtAnimation;
		private SpriteAnimation _koAnimation;

		private SoundEffectInstance _hit;
		private SoundEffectInstance _miss;
		public int DrawOrder => 1;

		//private Texture2D _texture;

		private int _health = 100;

		public bool IsDead = false;

		public bool CanPunch { get; set; } = false;

		public EnemyState State { get; set; }

		public Rectangle CollisionBox { get; set; }

		private Random _random;

		public Enemy(Texture2D spriteSheet, Vector2 position, SoundEffectInstance hit, SoundEffectInstance miss)
		{
			Position = position;
			_hit = hit;
			_miss = miss;

			_idleSprite1 = new Sprite(spriteSheet, 29, 14, 32, 50);
			_idleSprite2 = new Sprite(spriteSheet, 122, 14, 37, 50);
			_idleSprite3 = new Sprite(spriteSheet, 218, 15, 37, 50);
			_idleSprite4 = new Sprite(spriteSheet, 314, 14, 38, 50);

			_idleAnimation = new SpriteAnimation();
			_idleAnimation.AddFrame(_idleSprite1, 0);
			_idleAnimation.AddFrame(_idleSprite2, 0.2f);
			_idleAnimation.AddFrame(_idleSprite3, 0.5f);
			_idleAnimation.AddFrame(_idleSprite4, 0.75f);
			_idleAnimation.Play();

			_punchSprite1 = new Sprite(spriteSheet, 797, 14, 33, 50);
			_punchSprite2 = new Sprite(spriteSheet, 897, 14, 33, 50);
			_punchSprite3 = new Sprite(spriteSheet, 971, 16, 50, 50);

			_punchAnimation = new SpriteAnimation();
			_punchAnimation.ShouldLoop = false;
			_punchAnimation.AddFrame(_punchSprite1, 0);
			_punchAnimation.AddFrame(_punchSprite2, 0.1f);
			_punchAnimation.AddFrame(_punchSprite3, 0.3f);
			_punchAnimation.AddFrame(_punchSprite1, 0.8f);
			_punchAnimation.Play();

			_walkingSprite1 = new Sprite(spriteSheet, 415, 14, 35, 50);
			_walkingSprite2 = new Sprite(spriteSheet, 512, 14, 35, 50);
			_walkingSprite3 = new Sprite(spriteSheet, 608, 13, 35, 50);
			_walkingSprite4 = new Sprite(spriteSheet, 705, 14, 35, 50);

			_walkingAnimation = new SpriteAnimation();
			_walkingAnimation.ShouldLoop = false;
			_walkingAnimation.AddFrame(_walkingSprite1, 0);
			_walkingAnimation.AddFrame(_walkingSprite2, 0.2f);
			_walkingAnimation.AddFrame(_walkingSprite3, 0.4f);
			_walkingAnimation.AddFrame(_walkingSprite4, 0.6f);
			_walkingAnimation.AddFrame(_walkingSprite1, 0.8f);
			_walkingAnimation.Play();

			_hurtSprite1 = new Sprite(spriteSheet, 1084, 14, 37, 50);
			_hurtSprite2 = new Sprite(spriteSheet, 1183, 14, 37, 50);
			_hurtSprite3 = new Sprite(spriteSheet, 1271, 15, 50, 50);
			_hurtSprite4 = new Sprite(spriteSheet, 1354, 14, 64, 50);

			_hurtAnimation = new SpriteAnimation();
			_hurtAnimation.ShouldLoop = false;
			_hurtAnimation.AddFrame(_hurtSprite1, 0);
			_hurtAnimation.AddFrame(_hurtSprite2, 0.1f);
			_hurtAnimation.AddFrame(_hurtSprite1, 0.3f);
			_hurtAnimation.Play();

			_koAnimation = new SpriteAnimation();
			_koAnimation.ShouldLoop = false;
			_koAnimation.AddFrame(_hurtSprite1, 0);
			_koAnimation.AddFrame(_hurtSprite2, 0.1f);
			_koAnimation.AddFrame(_hurtSprite3, 0.3f);
			_koAnimation.AddFrame(_hurtSprite4, 0.5f);
			_koAnimation.Play();

			CollisionBox = new Rectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), 40 * 2, 55 * 2);

			//_texture = new Texture2D(spriteSheet.GraphicsDevice, 1, 1);
			//_texture.SetData(new Color[] { Color.MonoGameOrange });

			State = EnemyState.Idle;
			_random = new Random();
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			//spriteBatch.Draw(_texture, CollisionBox, Color.White);

			if (IsDead)
			{
				_hurtSprite4.Draw(spriteBatch, Position);
				return;
			}

			if (State == EnemyState.Idle)
			{
				_idleAnimation.Draw(spriteBatch, Position);
			}
			else if (State == EnemyState.Punching)
			{
				_punchAnimation.Draw(spriteBatch, Position);
			}
			else if (State == EnemyState.Walking)
			{
				_walkingAnimation.Draw(spriteBatch, Position);
			}
			else if (State == EnemyState.Hit)
			{
				_hurtAnimation.Draw(spriteBatch, Position);
			}
			else if (State == EnemyState.KO)
			{
				_koAnimation.Draw(spriteBatch, Position);
			}
		}

		public void Update(GameTime gameTime)
		{
			if (IsDead)
			{
				_koAnimation.Update(gameTime);
				return;
			}		

			if (State == EnemyState.Idle)
			{
				_idleAnimation.Update(gameTime);

				if (CanPunch)
				{				
					if (PlayerCollision())
					{
						if (_random.Next(0, 100) > 96)
						{
							State = EnemyState.Punching;
							_punchAnimation.Play();
						}	
					}
					else
					{
						if (_random.Next(0, 100) > 97)
						{
							State = EnemyState.Walking;
							_walkingAnimation.Play();
						}
					}
				}
			}
			else if (State == EnemyState.Punching)
			{
				if (!_punchAnimation.IsPlaying)
				{
					State = EnemyState.Idle;

					_idleAnimation.Play();
				}

				if (_punchAnimation.CurrentFrame == _punchAnimation.GetFrame(2))
				{
					if (PlayerCollision() && _hit.State != SoundState.Playing)
					{
						Player.Hurt(10);

						_hit.Play();
					}
					else if (_miss.State != SoundState.Playing)
					{
						_miss.Play();
					}
				}

				_punchAnimation.Update(gameTime);
			}
			else if (State == EnemyState.Walking)
			{
				_walkingAnimation.Update(gameTime);

				if (!PlayerCollision())
				{
					Position = new Vector2(Position.X - 75 * (float)gameTime.ElapsedGameTime.TotalSeconds, Position.Y);
					CollisionBox = new Rectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), 40 * 2, 55 * 2);
				}				

				if (!_walkingAnimation.IsPlaying)
				{
					State = EnemyState.Idle;

					_idleAnimation.Play();
				}
			}
			else if (State == EnemyState.Hit)
			{
				_hurtAnimation.Update(gameTime);

				if (!_hurtAnimation.IsPlaying)
				{
					State = EnemyState.Idle;
				}
			}
			else if (State == EnemyState.KO)
			{
				_koAnimation.Update(gameTime);

				if (!_koAnimation.IsPlaying)
				{
					IsDead = true;
				}
			}
		}

		public void Hurt(int damage)
		{
			_health -= damage;

			if (_health <= 0)
			{
				State = EnemyState.KO;
				CollisionBox = Rectangle.Empty;

				_health = 0;
				CanPunch = false;

				_koAnimation.Play();
			}
			else
			{
				State = EnemyState.Hit;
				_hurtAnimation.Play();
			}
		}

		public void Reset(Vector2 position)
		{
			Position = position;

			_health = 100;

			CollisionBox = new Rectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), 40 * 2, 55 * 2);

			IsDead = false;
			CanPunch = false;

			State = EnemyState.Idle;

			_idleAnimation.Play();			
		}

		public void StartFight()
		{
			CanPunch = true;
		}

		private bool PlayerCollision()
		{
			return CollisionBox.Intersects(Player.CollisionBox);
		}

		public bool WalkLeft(GameTime gameTime)
		{
			State = EnemyState.Walking;
			_walkingAnimation.Play();

			Position = new Vector2(Position.X - 75 * (float)gameTime.ElapsedGameTime.TotalSeconds, Position.Y);

			return true;
			
		}

	}

	public enum EnemyState
	{
		Idle,
		Punching,
		Walking,
		Hit,
		KO
	}
}
