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
		private const string MUSIC = "Music/NoSurvivors";
		private const string MUSIC1 = "Music/CircuitBreaker";
		private const string HIT = "SFX/Hit";
		private const string MISS = "SFX/Miss";
		private const string TextFont = "Fonts/File";
		private const string BACKGROUND = "Graphics/background";
		private const string MENU_SCREEN = "Graphics/jjtitle";

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
		private Texture2D _playerSpriteSheet;
		private Texture2D _enemySpriteSheet;
		private Texture2D _enemy2SpriteSheet;
		private Texture2D _menuScreen;
		private Texture2D _background;

		private SoundEffect _music;
		private SoundEffect _music1;

		private SoundEffect _hit;
		private SoundEffect _miss;

		private SoundManager _soundManager;
		private InputManager _inputManager;

		private HealthText _healthText;

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
			_menuScreen = Content.Load<Texture2D>(MENU_SCREEN);
			_background = Content.Load<Texture2D>(BACKGROUND);

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

			_player = new Player(_playerSpriteSheet, new Vector2(PLAYER_START_POS_X, PLAYER_START_POS_Y), 
				hitInstance, missInstance, _enemy, _enemy2, _enemy3, _healthText);

			_enemy.Player = _player;
			_enemy2.Player = _player;
			_enemy3.Player = _player;

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
				HandleMenuInput(gameTime);
			}

			if (GameState == GameState.Playing)
			{
				if (_player.State != PlayerState.KO)
				{
					_inputManager.ProcessControls(gameTime);
				}
				KeepPlayerInBounds();

				_player.Update(gameTime);
				_enemy.Update(gameTime);
				_enemy2.Update(gameTime);
				_enemy3.Update(gameTime);
			}			

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

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

				_healthText.Draw(_spriteBatch, gameTime);
			}
			else if (GameState == GameState.GameOver)
			{
				GraphicsDevice.Clear(Color.Red);
			}
			else if (GameState == GameState.Credits)
			{
				GraphicsDevice.Clear(Color.Green);
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

		private void HandleMenuInput(GameTime gameTime)
		{
			KeyboardState keyboardState = Keyboard.GetState();
			GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

			bool isPunchKeyPressed = keyboardState.IsKeyDown(Keys.Space);

			if (isPunchKeyPressed || gamePadState.Buttons.A == ButtonState.Pressed)
			{
				GameState = GameState.Playing;
			}
		}


		private void Reset()
		{
			_player.Reset(new Vector2(PLAYER_START_POS_X, PLAYER_START_POS_Y));

			_enemy.Reset(new Vector2(200, PLAYER_START_POS_Y));

			_enemy2.Reset(new Vector2(400, PLAYER_START_POS_Y));

			_enemy3.Reset(new Vector2(600, PLAYER_START_POS_Y));
		}

	}
}
