using System;
using GTA;
using System.Collections.Generic;
using GTA.Native;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Take the current weapon from the player.
	/// </summary>
	public class TakeWeapon : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			Ped player = Game.Player.Character;
			Weapon weapon = player.Weapons.Current;

			if (weapon != null)
			{
				player.Weapons.Remove(weapon);
			}
			else
			{
				var weapons = mod.PlayerWeapons();

				if (weapons.Count == 0)
				{
					mod.ShowText($"{from} tried to take your weapons, but no weapons to take :(");
					return;
				}

				var take = weapons[mod.Rnd.Next(0, weapons.Count - 1)];
				// take a random weapon.
				player.Weapons.Remove(take);
			}

			player.Weapons.Select(WeaponHash.Unarmed, true);
			// mod.ShowText($"{from} took your weapon!");
		}

		public ParachuteState GetParachuteState(int ped)
		{
			return Function.Call<ParachuteState>(Hash.GET_PED_PARACHUTE_STATE, ped);
		}
	}
}
