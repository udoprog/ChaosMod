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

			var model = new Model(PedHash.Trevor);

			player.RandomizeOutfit();
			// TODO: implement this.
			mod.ShowText($"{from} scrambled your character!");
		}
	}
}
