using System;
using GTA;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Blow out tires of current vehicle.
	/// </summary>
	public class Kill : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			Ped player = Game.Player.Character;

			if (player == null)
			{
				mod.ShowText($"{from} tried to kill you!");
				return;
			}

			player.Kill();
			mod.ShowText($"{from} killed you!");
		}
	}
}
