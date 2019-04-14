using System;
using GTA;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Randomize the current vehicle.
	/// </summary>
	public class RandomizeVehicle : Command
	{
		static VehicleSeat[] ALL_SEATS = (VehicleSeat[])Enum.GetValues(typeof(VehicleSeat));
		static VehicleWindow[] ALL_WINDOWS = (VehicleWindow[])Enum.GetValues(typeof(VehicleWindow));

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

			// 25% probability of adding a random passenger for every seat.
			foreach (var seat in ALL_SEATS)
			{
				if (vehicle.IsSeatFree(seat))
				{
					if (mod.Rnd.Next(0, 4) == 0)
					{
						vehicle.CreateRandomPedOnSeat(seat);
					}
				}
			}

			foreach (var window in ALL_WINDOWS)
			{
				if (mod.Rnd.Next(0, 2) == 0)
				{
					vehicle.RemoveWindow(window);
				}
				else
				{
					vehicle.FixWindow(window);
				}
			}

			vehicle.RandomizeColors(mod.Rnd);
			mod.ShowText($"{from} chnaged your vehicle randomly!");
		}
	}
}
