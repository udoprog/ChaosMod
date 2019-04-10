using System;
using GTA;
using GTA.Native;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	public class GiveWeapon : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			Ped player = Game.Player.Character;

			if (player == null)
			{
				return;
			}

			var r = rest.GetEnumerator();

			if (!r.MoveNext())
			{
				return;
			}

			var id = r.Current;

			WeaponHash? weapon = mod.GetWeaponById(id);

			if (!weapon.HasValue)
			{
				mod.ShowText($"{from} tried to give you a weapon :( ({id})");
				return;
			}

			var w = player.Weapons.Give(weapon.Value, 0, true, true);
			w.Ammo = w.MaxAmmo;
			w.AmmoInClip = w.MaxAmmoInClip;
			mod.ShowText($"{from} gave you a weapon! ({id})");
		}
	}
}
