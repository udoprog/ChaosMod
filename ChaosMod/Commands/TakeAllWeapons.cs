using System;
using GTA;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Take all the weapons from the player.
	/// </summary>
	public class TakeAllWeapons : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			Ped player = Game.Player.Character;

			if (player == null)
			{
				mod.ShowText($"{from} tried to take your weapons!");
				return;
			}

			player.Weapons.RemoveAll();
			mod.ShowText($"{from} took ALL your weapons!");
		}
	}
}