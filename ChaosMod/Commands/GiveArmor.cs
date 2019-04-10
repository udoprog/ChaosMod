using System;
using GTA;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Give armor to player.
	/// </summary>
	public class GiveArmor : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			Ped player = Game.Player.Character;

			if (player == null)
			{
				mod.ShowText($"{from} tried to give you armor!");
				return;
			}

			player.Armor = 100;
			mod.ShowText($"{from} gave you armor!");
		}
	}
}