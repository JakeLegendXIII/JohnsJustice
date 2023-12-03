using JohnsJustice.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JohnsJustice.Entities
{
	public class Player : IGameEntity
	{
		public Vector2 Position { get; set; }

		private Sprite _idleSprite1;
		private Sprite _idleSprite2;
		private Sprite _idleSprite3;
		private Sprite _idleSprite4;

		public int DrawOrder => 1;

		public Player(Texture2D spriteSheet, Vector2 position)
		{
			Position = position;
			// 13,34  and 65/64
			_idleSprite1 = new Sprite(spriteSheet, 30, 14, 35, 50);
			_idleSprite2 = new Sprite(spriteSheet, 30, 14, 35, 50);
			_idleSprite3 = new Sprite(spriteSheet, 30, 14, 35, 50);
			_idleSprite4 = new Sprite(spriteSheet, 30, 14, 35, 50);
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			_idleSprite1.Draw(spriteBatch, Position);
		}

		public void Update(GameTime gameTime)
		{
			
		}
	}
}
