using JohnsJustice.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JohnsJustice
{
	public class MainGame : Game
	{
		private const string PLAYER_SPRITE_SHEET = "Sprites/player";
		private const string ENEMY_SPRITE_SHEET = "Sprites/enemy-punk";
		private const string ENEMY2_SPRITE_SHEET = "Sprites/enemy-punk2";

		public const int WINDOW_WIDTH = 600;
		public const int WINDOW_HEIGHT = 150;
		public const int PLAYER_START_POS_Y = WINDOW_HEIGHT - 65;
		public const int PLAYER_START_POS_X = 5;

		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;

		private Player _player;
		private Texture2D _playerSpriteSheet;
		private Texture2D _enemySpriteSheet;
		private Texture2D _enemy2SpriteSheet;

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
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			_playerSpriteSheet = Content.Load<Texture2D>(PLAYER_SPRITE_SHEET);
			_enemySpriteSheet = Content.Load<Texture2D>(ENEMY_SPRITE_SHEET);
			_enemy2SpriteSheet = Content.Load<Texture2D>(ENEMY2_SPRITE_SHEET);

			_player = new Player(_playerSpriteSheet, new Vector2(PLAYER_START_POS_X, PLAYER_START_POS_Y));
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			// TODO: Add your update logic here

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			_spriteBatch.Begin();

			_player.Draw(_spriteBatch, gameTime);

			_spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
