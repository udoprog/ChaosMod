using System;
using GTA;
using System.Collections.Generic;
namespace ChaosMod.Commands
{
	/// <summary>
	/// Levitate the current player or vehicle.
	/// </summary>
	public class EuphoriaTest : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			var player = Game.Player.Character;

			player.Euphoria.HighFall.Start(10_000);
		}
	}
}
