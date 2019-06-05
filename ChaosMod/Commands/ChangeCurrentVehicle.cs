using System;
using GTA;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Change the current vehicle to a random vehicle.
	/// </summary>
	public class ChangeCurrentVehicle : Command
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

			var r = rest.GetEnumerator();

			if (!r.MoveNext())
			{
				return;
			}

			var id = r.Current;

			for (var i = 0; i < 5; i++)
			{
				var randomVehicle = mod.PickVehicle(id);

				if (!randomVehicle.HasValue)
				{
					continue;
				}

				var model = new Model(randomVehicle.Value);
				var newVehicle = World.CreateVehicle(model, vehicle.Position, vehicle.Heading);

				if (newVehicle == null)
				{
					continue;
				}

				if (newVehicle.PassengerCapacity < vehicle.PassengerCount)
				{
					newVehicle.Delete();
					continue;
				}

				if (vehicle.IsOnAllWheels)
				{
					newVehicle.PlaceOnGround();
				}

				newVehicle.Velocity = vehicle.Velocity;
				newVehicle.Quaternion = vehicle.Quaternion;
				newVehicle.Throttle = vehicle.Throttle;
				newVehicle.IsEngineRunning = vehicle.IsEngineRunning;

				for (var seatIndex = -1; seatIndex < newVehicle.PassengerCapacity; seatIndex++)
				{
					var seat = (VehicleSeat)seatIndex;
					var ped = vehicle.GetPedOnSeat(seat);

					if (ped == null)
					{
						continue;
					}

					if (newVehicle.IsSeatFree(seat))
					{
						ped.SetIntoVehicle(newVehicle, seat);
					}
				}

				newVehicle.PreviouslyOwnedByPlayer = true;
				vehicle.Delete();
				mod.ShowText($"{from} changed your current vehicle!");
				return;
			}

			mod.ShowText($"{from} tried to change current vehicle, but failed");
		}
	}
}
