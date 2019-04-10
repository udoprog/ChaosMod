using System;
using GTA;
using System.Collections.Generic;
using System.Linq;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Change the license plate of the current vehicle.
	/// </summary>
	public class License : Command
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

			var license = String.Join(" ", rest);

			if (license.Length == 0)
			{
				license = from;
			}

			license = String.Concat(license.Take(8));

			vehicle.NumberPlate = license;
			mod.ShowText($"{from} set your license plate to \"{license}\"!");
		}
	}
}
