using System;
using GTA;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Enable exploding punches for 20 seconds.
	/// </summary>
	public class UnlockAbility : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			Player player = Game.Player;

			player.ToggleSpecialAbility(true);
		}
	}
}