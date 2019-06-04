using System;
using GTA;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Blow out tires of current vehicle.
	/// </summary>
	public class BlowTires : Command
	{
		static int[] WHEELS = new int[] { 0, 1, 2, 3, 4, 5, 45, 47 };

		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			Ped player = Game.Player.Character;
			Vehicle vehicle = player.CurrentVehicle;

			if (vehicle == null)
			{
				mod.ShowText($"{from} tried to blow out your tires!");
				return;
			}

			if (!vehicle.CanTiresBurst)
			{
				vehicle.CanTiresBurst = true;
			}

			var wheels = vehicle.Wheels;

			foreach (var i in WHEELS)
			{
				if (vehicle.IsVehicleTyreBurst(i, true))
				{
					continue;
				}

				if (mod.Rnd.NextBoolean())
				{
					wheels[i].Burst();
				}
			}

			mod.ShowText($"{from} blew out your tires!");
		}
	}
}
