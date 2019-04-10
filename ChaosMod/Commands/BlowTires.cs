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

			vehicle.BurstTire(0);
			vehicle.BurstTire(1);
			vehicle.BurstTire(4);
			vehicle.BurstTire(5);
			mod.ShowText($"{from} blew out your tires!");
		}
	}
}
