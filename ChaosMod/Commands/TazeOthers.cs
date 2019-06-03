using System;
using GTA;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Taze other characters.
	/// </summary>
	public class TazeOthers : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			var player = Game.Player.Character;
			var peds = World.GetNearbyPeds(player.Position, 1000f);

			foreach (var ped in peds)
			{
				if (ped == player || !ped.IsHuman || !ped.IsAlive)
				{
					continue;
				}

				ped.Euphoria.Electrocute.ResetArguments();
				ped.Euphoria.Electrocute.Start(3_000 + (int) (mod.Rnd.NextDouble() * 1_000f));
			}

			var timer = mod.Timer("Tazed Others", 3);
			mod.AddUniqueTicker(TickerId.ElectrocutedOthers, timer);
			mod.ShowText($"{from} tazed others!");
		}
	}
}
