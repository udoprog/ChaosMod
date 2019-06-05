using System;
using GTA;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Enable exploding punches for 20 seconds.
	/// </summary>
	public class LockAbility : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			Player player = Game.Player;
			Model playerModel = Game.Player.Character.Model;

			player.ToggleSpecialAbility(false);
		}
	}
}