using JohnsJustice.Engine.Sound;
using JohnsJustice.Entities;
using JohnsJustice.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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

		public const int WINDOW_WIDTH = 600;
		public const int WINDOW_HEIGHT = 150;
		public const int PLAYER_START_POS_Y = WINDOW_HEIGHT - 65;
		public const int PLAYER_START_POS_X = 5;

		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;

		private Player _player;
		private Enemy _enemy;
		private Enemy2 _enemy2;
		private Texture2D _playerSpriteSheet;
		private Texture2D _enemySpriteSheet;
		private Texture2D _enemy2SpriteSheet;
		private SoundEffect _music;
		private SoundEffect _music1;

		private SoundManager _soundManager;
		private InputManager _inputManager;

		public MainGame()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			// TODO: Add your initialization logic here
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

			_music = Content.Load<SoundEffect>(MUSIC);
			_music1 = Content.Load<SoundEffect>(MUSIC1);

			var track1 = _music.CreateInstance();
			var track2 = _music1.CreateInstance();
			_soundManager.SetSoundtrack(new List<SoundEffectInstance>() { track1, track2 });

			_player = new Player(_playerSpriteSheet, new Vector2(PLAYER_START_POS_X, PLAYER_START_POS_Y));

			_enemy = new Enemy(_enemySpriteSheet, new Vector2(200, PLAYER_START_POS_Y));
			_enemy2 = new Enemy2(_enemy2SpriteSheet, new Vector2(400, PLAYER_START_POS_Y));

			_inputManager = new InputManager(_player);
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();
			
			_soundManager.PlaySoundtrack();
			_inputManager.ProcessControls(gameTime);
			KeepPlayerInBounds();

			_player.Update(gameTime);
			_enemy.Update(gameTime);
			_enemy2.Update(gameTime);

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			_spriteBatch.Begin();

			_player.Draw(_spriteBatch, gameTime);
			_enemy.Draw(_spriteBatch, gameTime);
			_enemy2.Draw(_spriteBatch, gameTime);

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
	}
}
