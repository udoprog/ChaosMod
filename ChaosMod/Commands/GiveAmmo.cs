using System;
using GTA;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Give ammo for current weapon.
	/// </summary>
	public class GiveAmmo : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			Ped player = Game.Player.Character;
			Weapon weapon = player.Weapons.Current;

			if (weapon == null)
			{
				mod.ShowText($"{from} tried to give you ammo!");
				return;
			}

			weapon.AmmoInClip = weapon.MaxAmmoInClip;
			weapon.Ammo = weapon.MaxAmmo;
			mod.ShowText($"{from} gave you ammo!");
		}
	}
}