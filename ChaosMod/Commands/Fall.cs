using System;
using System.Collections.Generic;
using GTA;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Cause the character to fall.
	/// </summary>
	public class Fall : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			Ped player = Game.Player.Character;
			player.Euphoria.HighFall.Start(1000);
			mod.ShowText($"{from} caused you to fall!");
		}
	}
}
