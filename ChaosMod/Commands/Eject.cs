using System;
using GTA;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Cause the character to leave the current vehicle.
	/// </summary>
	public class Eject : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			var player = Game.Player.Character;
			var vehicle = player?.CurrentVehicle;

			if (player == null || vehicle == null)
			{
				mod.ShowText($"{from} tried to eject from vehicle :(");
				return;
			}

			player.Task.LeaveVehicle();
			mod.ShowText($"{from} caused you to eject from your vehicle!");
		}
	}
}
