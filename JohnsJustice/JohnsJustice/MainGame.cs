using JohnsJustice.Engine.Sound;
using JohnsJustice.Entities;
using JohnsJustice.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace JohnsJustice
{
	public class MainGame : Game
	{
		private const string PLAYER_SPRITE_SHEET = "Sprites/player";
		private const string ENEMY_SPRITE_SHEET = "Sprites/enemy-punk";
		private const string ENEMY2_SPRITE_SHEET = "Sprites/enemy-punk2";
		private const string ENEMY3_SPRITE_SHEET = "Sprites/enemy-punk3";
		private const string MUSIC = "Music/NoSurvivors";
		private const string MUSIC1 = "Music/CircuitBreaker";
		private const string HIT = "SFX/Hit";
		private const string MISS = "SFX/Miss";
		private const string TextFont = "Fonts/File";
		private const string BACKGROUND = "Graphics/background";
		private const string MENU_SCREEN = "Graphics/johnsjusticetitle";
		private const string GAME_OVER_SCREEN = "Graphics/gameover";
		private const string CREDIT_SCREEN = "Graphics/credits";

		public const int WINDOW_WIDTH = 1200; // 600
		public const int WINDOW_HEIGHT = 300; // 150
		public const int PLAYER_START_POS_Y = WINDOW_HEIGHT - 160;
		public const int PLAYER_START_POS_X = 5;

		public GameState GameState;

		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;

		private Player _player;
		private Enemy _enemy;
		private Enemy _enemy2;
		private Enemy _enemy3;
		private Enemy _enemy4;
		private Enemy _enemy5;
		private List<Enemy> _enemyList;
		private Texture2D _playerSpriteSheet;
		private Texture2D _enemySpriteSheet;
		private Texture2D _enemy2SpriteSheet;
		private Texture2D _enemy3SpriteSheet;
		private Texture2D _menuScreen;
		private Texture2D _background;
		private Texture2D _gameOverScreen;
		private Texture2D _creditScreen;

		private SoundEffect _music;
		private SoundEffect _music1;

		private SoundEffect _hit;
		private SoundEffect _miss;

		private SoundManager _soundManager;
		private InputManager _inputManager;

		private HealthText _healthText;

		private const float _victoryDelay = 3f;
		private float _remainingVictoryDelay = _victoryDelay;

		private const float _enemyMoveDelay = 6f;
		private float _remainingEnemyMoveDelay = _enemyMoveDelay;

		public MainGame()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			Window.Title = "John's Justice: Flush with Fury";
			_graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
			_graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
			_graphics.ApplyChanges();

			base.Initialize();
		}

		protected override void LoadContent()
		{
			_soundManager = new SoundManager();

			_spriteBatch = new SpriteBatch(GraphicsDevice);

			_playerSpriteSheet = Content.Load<Texture2D>(PLAYER_SPRITE_SHEET);
			_enemySpriteSheet = Content.Load<Texture2D>(ENEMY_SPRITE_SHEET);
			_enemy2SpriteSheet = Content.Load<Texture2D>(ENEMY2_SPRITE_SHEET);
			_enemy3SpriteSheet = Content.Load<Texture2D>(ENEMY3_SPRITE_SHEET);
			_menuScreen = Content.Load<Texture2D>(MENU_SCREEN);
			_background = Content.Load<Texture2D>(BACKGROUND);
			_gameOverScreen = Content.Load<Texture2D>(GAME_OVER_SCREEN);
			_creditScreen = Content.Load<Texture2D>(CREDIT_SCREEN);

			_music = Content.Load<SoundEffect>(MUSIC);
			_music1 = Content.Load<SoundEffect>(MUSIC1);
			_hit = Content.Load<SoundEffect>(HIT);
			_miss = Content.Load<SoundEffect>(MISS);

			_healthText = new HealthText(Content.Load<SpriteFont>(TextFont));

			var track1 = _music.CreateInstance();
			var track2 = _music1.CreateInstance();
			_soundManager.SetSoundtrack(new List<SoundEffectInstance>() { track1, track2 });

			var hitInstance = _hit.CreateInstance();
			var missInstance = _miss.CreateInstance();

			_enemy = new Enemy(_enemySpriteSheet, new Vector2(200, PLAYER_START_POS_Y), hitInstance, missInstance);
			_enemy2 = new Enemy(_enemy2SpriteSheet, new Vector2(400, PLAYER_START_POS_Y), hitInstance, missInstance);
			_enemy3 = new Enemy(_enemySpriteSheet, new Vector2(600, PLAYER_START_POS_Y), hitInstance, missInstance);
			_enemy4 = new Enemy(_enemy3SpriteSheet, new Vector2(850, PLAYER_START_POS_Y), hitInstance, missInstance);
			_enemy5 = new Enemy(_enemy2SpriteSheet, new Vector2(1000, PLAYER_START_POS_Y), hitInstance, missInstance);

			_enemyList = new List<Enemy>
			{
				_enemy,
				_enemy2,
				_enemy3,
				_enemy4,
				_enemy5
			};

			_player = new Player(_playerSpriteSheet, new Vector2(PLAYER_START_POS_X, PLAYER_START_POS_Y),
				hitInstance, missInstance, _enemyList, _healthText);

			_enemy.Player = _player;
			_enemy2.Player = _player;
			_enemy3.Player = _player;
			_enemy4.Player = _player;
			_enemy5.Player = _player;

			// _player.OnDeath += player_HasDied;

			_inputManager = new InputManager(_player);

			GameState = GameState.Menu;
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			_soundManager.PlaySoundtrack();

			if (GameState == GameState.Menu)
			{
				HandleReplayInput(gameTime);
			}
			else if (GameState == GameState.Playing)
			{
				if (_player.State != PlayerState.KO)
				{
					_inputManager.ProcessControls(gameTime);
				}
				KeepPlayerInBounds();

				CheckIfEnemiesAreDead(gameTime);
				CheckIfPlayerIsDead(gameTime);

				_player.Update(gameTime);
				_enemy.Update(gameTime);
				_enemy2.Update(gameTime);
				_enemy3.Update(gameTime);
				_enemy4.Update(gameTime);
				_enemy5.Update(gameTime);

				MoveEnemyForward(gameTime);
			}
			else if (GameState == GameState.GameOver)
			{
				HandleReplayInput(gameTime);
			}
			else if (GameState == GameState.Credits)
			{				
				HandleReplayInput(gameTime);
			}

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			// GraphicsDevice.Clear(Color.CornflowerBlue);

			_spriteBatch.Begin();

			if (GameState == GameState.Menu)
			{
				_spriteBatch.Draw(_menuScreen, new Vector2(0, 0), Color.White);
			}
			else if (GameState == GameState.Playing)
			{
				_spriteBatch.Draw(_background, new Rectangle(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT), new Rectangle(0, 0, WINDOW_WIDTH / 2, WINDOW_HEIGHT / 2), Color.White);

				_player.Draw(_spriteBatch, gameTime);
				_enemy.Draw(_spriteBatch, gameTime);
				_enemy2.Draw(_spriteBatch, gameTime);
				_enemy3.Draw(_spriteBatch, gameTime);
				_enemy4.Draw(_spriteBatch, gameTime);
				_enemy5.Draw(_spriteBatch, gameTime);

				_healthText.Draw(_spriteBatch, gameTime);
			}
			else if (GameState == GameState.GameOver)
			{
				_spriteBatch.Draw(_gameOverScreen, new Vector2(0, 0), Color.White);
			}
			else if (GameState == GameState.Credits)
			{
				_spriteBatch.Draw(_creditScreen, new Vector2(0, 0), Color.White);
			}

			_spriteBatch.End();

			base.Draw(gameTime);
		}

		// Called during Input handling for movement
		private void KeepPlayerInBounds()
		{
			if (_player.Position.X < 0)
			{
				_player.Position = new Vector2(0, _player.Position.Y);
			}

			if (_player.Position.X > WINDOW_WIDTH - 35)
			{
				_player.Position = new Vector2(WINDOW_WIDTH - 35, _player.Position.Y);
			}

			if (_player.Position.Y < 0)
			{
				_player.Position = new Vector2(_player.Position.X, 0);
			}

			if (_player.Position.Y > WINDOW_HEIGHT - 50)
			{
				_player.Position = new Vector2(_player.Position.X, WINDOW_HEIGHT - 50);
			}
		}

		private void HandleReplayInput(GameTime gameTime)
		{
			KeyboardState keyboardState = Keyboard.GetState();
			GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

			bool isPunchKeyPressed = keyboardState.IsKeyDown(Keys.Enter);

			if (isPunchKeyPressed || gamePadState.Buttons.Start == ButtonState.Pressed)
			{
				Reset();
				GameState = GameState.Playing;
			}
		}

		private void CheckIfEnemiesAreDead(GameTime gameTime)
		{
			bool allEnemiesDead = true;
			foreach (var enemy in _enemyList)
			{
				if (enemy.IsDead == false)
				{
					allEnemiesDead = false;
					break;
				}
			}

			if (allEnemiesDead)
			{
				_healthText.Text = "You Win!";

				var timer = (float)gameTime.ElapsedGameTime.TotalSeconds;

				_remainingVictoryDelay -= timer;

				if (_remainingVictoryDelay <= 0)
				{
					GameState = GameState.Credits;
					_remainingVictoryDelay = _victoryDelay;
				}				
			}
		}

		private void CheckIfPlayerIsDead(GameTime gameTime)
		{
			if (_player.IsDead)
			{
				_healthText.Text = "You Lose!";

				var timer = (float)gameTime.ElapsedGameTime.TotalSeconds;

				_remainingVictoryDelay -= timer;

				if (_remainingVictoryDelay <= 0)
				{
					GameState = GameState.GameOver;
					_remainingVictoryDelay = _victoryDelay;
				}
			}
		}


		private void Reset()
		{
			_player.Reset(new Vector2(PLAYER_START_POS_X, PLAYER_START_POS_Y));

			_enemy.Reset(new Vector2(200, PLAYER_START_POS_Y));

			_enemy2.Reset(new Vector2(400, PLAYER_START_POS_Y));

			_enemy3.Reset(new Vector2(600, PLAYER_START_POS_Y));

			_enemy4.Reset(new Vector2(850, PLAYER_START_POS_Y));

			_enemy5.Reset(new Vector2(1000, PLAYER_START_POS_Y));
		}

		private void MoveEnemyForward(GameTime gameTime)
		{
			if (!_enemy.CanPunch && !_enemy.IsDead)
			{
				var timer = (float)gameTime.ElapsedGameTime.TotalSeconds;
				_remainingEnemyMoveDelay -= timer;
				if (_remainingEnemyMoveDelay <= 0)
				{
					_enemy.WalkLeft();
					_remainingEnemyMoveDelay = _enemyMoveDelay;
				}

				return;
			}

			if (!_enemy2.CanPunch && !_enemy2.IsDead && _enemy.IsDead)
			{
				var timer = (float)gameTime.ElapsedGameTime.TotalSeconds;
				_remainingEnemyMoveDelay -= timer;
				if (_remainingEnemyMoveDelay <= 0)
				{
					_enemy2.WalkLeft();
					_remainingEnemyMoveDelay = _enemyMoveDelay;
				}

				return;
			}

			if (!_enemy3.CanPunch && !_enemy3.IsDead && _enemy2.IsDead)
			{
				var timer = (float)gameTime.ElapsedGameTime.TotalSeconds;

				_remainingEnemyMoveDelay -= timer;

				if (_remainingEnemyMoveDelay <= 0)
				{
					_enemy3.WalkLeft();
					_remainingEnemyMoveDelay = _enemyMoveDelay;
				}

				return;
			}

			if (!_enemy4.CanPunch && !_enemy4.IsDead && _enemy3.IsDead)
			{
				var timer = (float)gameTime.ElapsedGameTime.TotalSeconds;
				_remainingEnemyMoveDelay -= timer;
				if (_remainingEnemyMoveDelay <= 0)
				{
					_enemy4.WalkLeft();
					_remainingEnemyMoveDelay = _enemyMoveDelay;
				}

				return;
			}

			if (!_enemy5.CanPunch && !_enemy5.IsDead && _enemy4.IsDead)
			{
				var timer = (float)gameTime.ElapsedGameTime.TotalSeconds;
				_remainingEnemyMoveDelay -= timer;
				if (_remainingEnemyMoveDelay <= 0)
				{
					_enemy5.WalkLeft();
					_remainingEnemyMoveDelay = _enemyMoveDelay;
				}

				return;
			}
		}

		private void player_HasDied(object sender, EventArgs e)
		{
			GameState = GameState.GameOver;
		}

	}
}
