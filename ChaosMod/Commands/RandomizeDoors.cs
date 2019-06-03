using System;
using GTA;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Randomize the state of doors and windows on the current vehicle.
	/// </summary>
	public class RandomizeDoors : Command
	{
		static VehicleWindowIndex[] ALL_WINDOWS = (VehicleWindowIndex[])Enum.GetValues(typeof(VehicleWindowIndex));
		static VehicleDoorIndex[] ALL_DOORS = (VehicleDoorIndex[])Enum.GetValues(typeof(VehicleDoorIndex));

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

			foreach (var door in ALL_DOORS)
			{
				var d = vehicle.Doors[door];

				if (mod.Rnd.Next(0, 2) == 0)
				{
					d.Open();
				}
				else
				{
					d.Close();
				}
			}

			foreach (var window in ALL_WINDOWS)
			{
				var w = vehicle.Windows[window];

				if (mod.Rnd.Next(0, 2) == 0)
				{
					w.RollDown();
				}
				else
				{
					w.RollUp();
				}
			}

			mod.ShowText($"{from} randomized your vehicle doors and windows!");
		}
	}
}
