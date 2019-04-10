using System;
using GTA;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Kill the engine of the current car.
	/// </summary>
	public class KillEngine : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			Ped player = Game.Player.Character;
			Vehicle vehicle = player.CurrentVehicle;

			if (vehicle == null)
			{
				mod.ShowText($"{from} tried to kill your engine!");
				return;
			}

			vehicle.EngineHealth = 0;
			mod.ShowText($"{from} killed your engine!");
		}
	}
}