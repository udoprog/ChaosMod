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

			for (var i = 0; i < wheels.Count; i++)
			{
				wheels[i].Burst();
			}

			mod.ShowText($"{from} blew out your tires!");
		}
	}
}
