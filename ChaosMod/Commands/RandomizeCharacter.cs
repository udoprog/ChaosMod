using System;
using GTA;
using GTA.Native;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Set random character traits.
	/// </summary>
	public class RandomizeCharacter : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			Ped player = Game.Player.Character;

			if (player == null)
			{
				return;
			}

			/// Currently disabled for player characters in scripthookvdotnet.
			player.Style.RandomizeOutfit();
			player.Style.RandomizeProps();
			mod.ShowText($"{from} scrambled your character!");
		}
	}
}
