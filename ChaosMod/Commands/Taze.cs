using System;
using GTA;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Taze the current character.
	/// </summary>
	public class Taze : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			Ped player = Game.Player.Character;

			if (player == null)
			{
				mod.ShowText($"{from} tried to taze you!");
				return;
			}

			player.Euphoria.Electrocute.Start(3_000);
			var timer = mod.Timer("Tazing", 3);
			mod.AddUniqueTicker(TickerId.Electrocuted, timer);
			mod.ShowText($"{from} tazed you!");
		}
	}
}
