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
		private GamePadState _previousGamePadState;

		public InputManager(Player player)
		{
			_player = player;
		}

		public void ProcessControls(GameTime gameTime)
		{
			KeyboardState keyboardState = Keyboard.GetState();
			GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

			bool isPunchKeyPressed = keyboardState.IsKeyDown(Keys.Space);
			bool isPunchButtonPressed = gamePadState.Buttons.A == ButtonState.Pressed;
			bool wasPunchKeyPressed = _previousKeyboardState.IsKeyDown(Keys.Space);
			bool wasPunchButtonPressed = _previousGamePadState.Buttons.A == ButtonState.Pressed;

			if ((wasPunchKeyPressed || wasPunchButtonPressed) && (isPunchKeyPressed || isPunchButtonPressed))
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

			_previousGamePadState = gamePadState;
			_previousKeyboardState = keyboardState;
		}
	}
}
