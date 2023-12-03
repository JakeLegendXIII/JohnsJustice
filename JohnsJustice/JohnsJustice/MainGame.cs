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

		// 13,34  and 65/64

		public const int WINDOW_WIDTH = 600;
		public const int WINDOW_HEIGHT = 200;
		public const int PLAYER_START_POS_Y = WINDOW_HEIGHT - 16;
		public const int PLAYER_START_POS_X = 5;

		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;

		public MainGame()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			// TODO: Add your initialization logic here
			_graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
			_graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;

			base.Initialize();
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			_spriteBatch = Content.Load<SpriteBatch>(PLAYER_SPRITE_SHEET);
			_spriteBatch = Content.Load<SpriteBatch>(ENEMY_SPRITE_SHEET);
			_spriteBatch = Content.Load<SpriteBatch>(ENEMY2_SPRITE_SHEET);

			// TODO: use this.Content to load your game content here
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



			_spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
