using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JohnsJustice.Entities
{
	public class HealthText : IGameEntity
	{
		protected SpriteFont _font;
		public string Text { get; set; } = "Health: 100";

		public int DrawOrder => 100;

		public HealthText(SpriteFont font)
		{
			_font = font;
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			spriteBatch.DrawString(_font, Text, new Vector2(10, 10), Color.White);
		}

		public void Update(GameTime gameTime)
		{
			
		}
	}
}
