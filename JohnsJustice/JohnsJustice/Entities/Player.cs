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

		private Texture2D _texture;

		private Enemy _enemy;
		private Enemy _enemy1;
		private Enemy _enemy2;
		private Enemy _enemy3;

		private HealthText _healthText;

		private int _health = 100;

		public bool IsDead = false;

		public EventHandler OnDeath;
		public EventHandler OnVictory;

		public PlayerState State { get; set; }

		public int DrawOrder => 1;

		public Rectangle CollisionBox
		{
			get
			{
				Rectangle box = new Rectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), 40 * 2, 55 * 2);

				return box;
			}
		}


		public Player(Texture2D spriteSheet, Vector2 position, SoundEffectInstance hit, SoundEffectInstance miss, 
			Enemy enemy, Enemy enemy1, Enemy enemy2, Enemy enemy3,
			HealthText healthText)
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
			_walkingAnimation.AddFrame(_walkingSprite3, 0.4f);
			_walkingAnimation.AddFrame(_walkingSprite4, 0.6f);
			_walkingAnimation.AddFrame(_walkingSprite1, 0.8f);
			_walkingAnimation.Play();

			_hurtSprite1 = new Sprite(spriteSheet, 1081, 14, 33, 50);
			_hurtSprite2 = new Sprite(spriteSheet, 1170, 14, 33, 50);
			_hurtSprite3 = new Sprite(spriteSheet, 1261, 17, 52, 50);
			_hurtSprite4 = new Sprite(spriteSheet, 1352, 14, 65, 50);

			_hurtAnimation = new SpriteAnimation();
			_hurtAnimation.ShouldLoop = false;
			_hurtAnimation.AddFrame(_hurtSprite1, 0);
			_hurtAnimation.AddFrame(_hurtSprite2, 0.2f);
			_hurtAnimation.AddFrame(_hurtSprite1, 0.4f);
			_hurtAnimation.Play();

			_koAnimation = new SpriteAnimation();
			_koAnimation.ShouldLoop = false;
			_koAnimation.AddFrame(_hurtSprite1, 0);
			_koAnimation.AddFrame(_hurtSprite2, 0.2f);
			_koAnimation.AddFrame(_hurtSprite3, 0.4f);
			_koAnimation.AddFrame(_hurtSprite4, 0.6f);
			_koAnimation.Play();

			_texture = new Texture2D(spriteSheet.GraphicsDevice, 1, 1);
			_texture.SetData(new Color[] { Color.MonoGameOrange });
			_enemy = enemy;
			_enemy1 = enemy1;
			_enemy2 = enemy2;
			_enemy3 = enemy3;

			_healthText = healthText;
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			// spriteBatch.Draw(_texture, CollisionBox, Color.White);

			if (IsDead)
			{
				_hurtSprite4.Draw(spriteBatch, Position);
				return;
			}

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
			else if (State == PlayerState.Hit)
			{
				_hurtAnimation.Draw(spriteBatch, Position);
			}
			else if (State == PlayerState.KO)
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

			CheckIfEnemiesAreDead();

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
						_enemy.Hurt(30);

						_hit.Play();
					}
					else if (EnemyCollision1() && _hit.State != SoundState.Playing)
					{
						_enemy1.Hurt(30);

						_hit.Play();
					}
					else if (EnemyCollision2() && _hit.State != SoundState.Playing)
					{
						_enemy2.Hurt(30);

						_hit.Play();
					}
					else if (EnemyCollision3() && _hit.State != SoundState.Playing)
					{
						_enemy3.Hurt(30);

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
			else if (State == PlayerState.Hit)
			{
				_hurtAnimation.Update(gameTime);

				if (!_hurtAnimation.IsPlaying)
				{
					State = PlayerState.Idle;
				}
			}
			else if (State == PlayerState.KO)
			{
				_koAnimation.Update(gameTime);

				if (!_koAnimation.IsPlaying)
				{
					IsDead = true;
					OnDeath?.Invoke(this, EventArgs.Empty);
				}
			}
		}	

		public void Hurt(int damage)
		{
			_health -= damage;

			if (_health <= 0)
			{
				State = PlayerState.KO;

				_health = 0;

				_koAnimation.Play();
				_healthText.Text = "Health: " + _health;
			}
			else
			{
				State = PlayerState.Hit;
				_hurtAnimation.Play();

				_healthText.Text = "Health: " + _health;
			}
		}

		public void Reset(Vector2 position)
		{
			Position = position;

			_health = 100;

			IsDead = false;

			State = PlayerState.Idle;

			_idleAnimation.Play();

			_healthText.Text = "Health: " + _health;
		}

		private void CheckIfEnemiesAreDead()
		{			
			if (_enemy.IsDead && _enemy1.IsDead && _enemy2.IsDead && _enemy3.IsDead)
			{				
				_healthText.Text = "You Win!";
				OnVictory?.Invoke(this, EventArgs.Empty);
			}
		}

		private bool EnemyCollision()
		{
			if (CollisionBox.Intersects(_enemy.CollisionBox))
			{
				if (_enemy.CanPunch == false)
				{
					_enemy.StartFight();
				}

				return true;
			}

			return false;
		}

		private bool EnemyCollision1()
		{
			if (CollisionBox.Intersects(_enemy1.CollisionBox))
			{
				if (_enemy1.CanPunch == false)
				{
					_enemy1.StartFight();
				}

				return true;
			}

			return false;
		}

		private bool EnemyCollision2()
		{
			if (CollisionBox.Intersects(_enemy2.CollisionBox))
			{
				if (_enemy2.CanPunch == false)
				{
					_enemy2.StartFight();
				}

				return true;
			}

			return false;
		}

		private bool EnemyCollision3()
		{
			if (CollisionBox.Intersects(_enemy3.CollisionBox))
			{
				if (_enemy3.CanPunch == false)
				{
					_enemy3.StartFight();
				}

				return true;
			}

			return false;
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

			if (!(EnemyCollision() || EnemyCollision1() || EnemyCollision2() || EnemyCollision3()))
			{
				Position = new Vector2(Position.X + 70f * (float)gameTime.ElapsedGameTime.TotalSeconds, Position.Y);
			}

			return true;
		}

		public bool WalkLeft(GameTime gameTime)
		{
			State = PlayerState.Walking;
			_walkingAnimation.Play();

			Position = new Vector2(Position.X - 105f * (float)gameTime.ElapsedGameTime.TotalSeconds, Position.Y);

			return true;
		}
	}

	public enum PlayerState
	{
		Idle,
		Punching,
		Walking,
		Hit,
		KO
	}
}
