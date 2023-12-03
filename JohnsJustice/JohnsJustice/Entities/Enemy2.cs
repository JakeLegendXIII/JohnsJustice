using JohnsJustice.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace JohnsJustice.Entities
{
	public class Enemy2 : IGameEntity
	{
		public Vector2 Position { get; set; }

		private Sprite _idleSprite1;

		public int DrawOrder => 1;

		public Enemy2(Texture2D spriteSheet, Vector2 position)
		{
			Position = position;
			_idleSprite1 = new Sprite(spriteSheet, 26, 14, 37, 50);
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
