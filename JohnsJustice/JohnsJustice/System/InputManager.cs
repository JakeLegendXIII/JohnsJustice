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
			GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

			bool isPunchKeyPressed = keyboardState.IsKeyDown(Keys.Space);
			bool wasPunchKeyPressed = _previousKeyboardState.IsKeyDown(Keys.Space);

			if (isPunchKeyPressed || gamePadState.Buttons.A == ButtonState.Pressed)
			{
				if (_player.State != PlayerState.Punching)
				{
					_player.BeginPunch();
				}				
			}

			if (keyboardState.IsKeyDown(Keys.A) || gamePadState.ThumbSticks.Left.X < 0 || gamePadState.DPad.Left == ButtonState.Pressed)
			{
				_player.WalkLeft(gameTime);
			}
			else if (keyboardState.IsKeyDown(Keys.D) || gamePadState.ThumbSticks.Left.X > 0 || gamePadState.DPad.Right == ButtonState.Pressed)
			{
				_player.WalkRight(gameTime);
			}
		}
	}
}
