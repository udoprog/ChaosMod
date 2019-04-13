using System;
using System.Collections.Generic;
using GTA;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Make the character stumble.
	/// </summary>
	public class Stumble : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			Ped player = Game.Player.Character;

			if (player == null)
			{
				return;
			}

			player.Stumble(2000);
			mod.ShowText($"{from} tripped you!");
		}
	}
}
