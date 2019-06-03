using System;
using GTA;
using System.Collections.Generic;
using System.Linq;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Spawn a vehicle.
	/// </summary>
	public class SpawnVehicle : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			Ped player = Game.Player.Character;

			if (player == null)
			{
				return;
			}

			var r = rest.GetEnumerator();

			if (!r.MoveNext())
			{
				return;
			}

			var id = r.Current;

			var picked = mod.PickVehicle(id);

			if (!picked.HasValue)
			{
				return;
			}

			var p = picked.Value;

			Vehicle vehicle = World.CreateVehicle(p, player.Position + player.ForwardVector * 10.0f, player.Heading + 90);

			if (vehicle == null)
			{
				mod.ShowText($"{from} failed to create vehicle :(");
				return;
			}

			vehicle.PlaceOnGround();
			vehicle.Mods.LicensePlate = String.Concat(from.Take(8));
			vehicle.Detach();
			vehicle.MarkAsNoLongerNeeded();
			mod.ShowText($"{from} gave you a vehicle! ({id})");
		}
	}
}
