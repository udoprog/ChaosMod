using System;
using System.Collections.Generic;
using GTA;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Open the parachute.
	/// </summary>
	public class CloseParachute : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			var player = Game.Player.Character;

			if (player == null)
			{
				mod.ShowText($"{from} tried to open your parachute!");
				return;
			}

			player.Task.UseParachute();
			mod.ShowText($"{from} opened your parachute!");
		}
	}
}
