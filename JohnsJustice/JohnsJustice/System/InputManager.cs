using JohnsJustice.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JohnsJustice.System
{
	public class InputManager
	{
		public bool _isBlocked;
		private Player _player;
		private KeyboardState _previousKeyboardState;

		public InputManager(Player player)
		{
			_player = player;
		}

		public void ProcessControls(GameTime gameTime)
		{
			KeyboardState keyboardState = Keyboard.GetState();

			bool isPunchKeyPressed = keyboardState.IsKeyDown(Keys.Space);
			bool wasPunchKeyPressed = _previousKeyboardState.IsKeyDown(Keys.Space);

			if (!wasPunchKeyPressed && isPunchKeyPressed)
			{
				if (_player.State != PlayerState.Punching)
				{
					_player.BeginPunch();
				}				
			}
		}
	}
}
