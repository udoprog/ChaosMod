using System;
using GTA;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Kill the engine of the current car.
	/// </summary>
	public class Repair : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			Ped player = Game.Player.Character;
			Vehicle vehicle = player.CurrentVehicle;

			if (vehicle == null)
			{
				mod.ShowText($"{from} tried to repair your car!");
				return;
			}

			vehicle.Repair();
			mod.ShowText($"{from} repaired your car!");
		}
	}
}
