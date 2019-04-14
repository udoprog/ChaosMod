using System;
using GTA;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Set random color of vehicle.
	/// </summary>
	public class RandomizeColor : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			Ped player = Game.Player.Character;

			if (player == null)
			{
				return;
			}

			var vehicle = player.CurrentVehicle;

			if (vehicle == null)
			{
				return;
			}

			var count = vehicle.ColorCombinationCount;

			if (count == 0)
			{
				vehicle.ColorCombination = mod.Rnd.Next(0, count);
			}

			vehicle.RandomizeColors(mod.Rnd);
			mod.ShowText($"{from} scrambled your vehicle colors!");
		}
	}
}
