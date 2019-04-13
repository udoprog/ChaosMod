using System;
using System.Collections.Generic;
using GTA;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Take all character health.
	/// </summary>
	public class TakeHealth : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			Ped player = Game.Player.Character;

			if (player == null)
			{
				return;
			}

			player.Stumble(500);
			mod.TakeHealth();
		}
	}
}
