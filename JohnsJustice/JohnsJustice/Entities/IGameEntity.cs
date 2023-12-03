using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace JohnsJustice.Entities
{
	internal interface IGameEntity
	{
		int DrawOrder { get; }
		void Update(GameTime gameTime);
		void Draw(SpriteBatch spriteBatch, GameTime gameTime);
	}
}
