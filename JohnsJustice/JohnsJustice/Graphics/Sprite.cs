using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JohnsJustice.Graphics
{
	public class Sprite
	{
		public Texture2D Texture { get; set; }
		public int X { get; set; }
		public int Y { get; set; }

		public int Width { get; set; }
		public int Height { get; set; }
		public Color TintColor { get; set; } = Color.White;

		public Sprite(Texture2D texture, int x, int y, int width, int height)
		{
			Texture = texture;
			X = x;
			Y = y;
			Width = width;
			Height = height;
		}

		public void Draw(SpriteBatch spriteBatch, Vector2 position)
		{
			spriteBatch.Draw(Texture, new Rectangle((int)position.X, (int)position.Y, Width * 4, Height *4), new Rectangle(X, Y, Width, Height), Color.White);
		}
	}
}
