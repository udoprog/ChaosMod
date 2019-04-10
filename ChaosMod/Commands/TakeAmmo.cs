using System;
using GTA;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Take all the ammo from the current weapon.
	/// </summary>
	public class TakeAmmo : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			Ped player = Game.Player.Character;
			Weapon weapon = player.Weapons.Current;

			if (weapon == null)
			{
				mod.ShowText($"{from} tried to take your ammo!");
				return;
			}

			weapon.AmmoInClip = 0;
			weapon.Ammo = 0;
			mod.ShowText($"{from} took all your ammo!");
		}
	}
}