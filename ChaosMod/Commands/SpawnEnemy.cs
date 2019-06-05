﻿using System;
using System.Collections.Generic;
using GTA;
using GTA.Native;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Spawn enemies which are hostile towards the player.
	/// </summary>
	public class SpawnEnemy : Command
	{
		static PedHash[] ENEMY_MODELS = new PedHash[]
		{
			PedHash.FbiSuit01,
			PedHash.FbiSuit01Cutscene,
			PedHash.FibOffice01SMM,
			PedHash.FibOffice02SMM,
		};

		static WeaponHash[] ENEMY_WEAPONS = new WeaponHash[]
		{
			WeaponHash.Pistol,
			WeaponHash.Bat,
			WeaponHash.Hammer,
		};

		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			var player = Game.Player.Character;

			if (player == null)
			{
				return;
			}

			var r = rest.GetEnumerator();

			if (!r.MoveNext())
			{
				return;
			}

			var amount = int.Parse(r.Current);

			for (var i = 0; i < amount; i++)
			{
				var pedHash = ENEMY_MODELS[mod.Rnd.Next(0, ENEMY_MODELS.Length)];
				var weaponHash = ENEMY_WEAPONS[mod.Rnd.Next(0, ENEMY_WEAPONS.Length)];

				var distance = mod.Rnd.Next(10, 20);

				var ped = World.CreatePed(new Model(pedHash), player.Position.Around(distance));
				var weapon = ped.Weapons.Give(weaponHash, 0, true, true);
				weapon.Ammo = weapon.MaxAmmo;

				ped.IsEnemy = true;
				ped.RelationshipGroup = mod.HateGroup.GroupId;

				ped.SetCombatAttributes(CombatAttributes.AlwaysFight, true);
				ped.SetCombatAttributes(CombatAttributes.CanFightArmedPedsWhenNotArmed, true);

				ped.Task.ClearAll();
				ped.Task.FightAgainst(player);

				ped.Detach();
				ped.MarkAsNoLongerNeeded();

				var blip = ped.AttachBlip();
				mod.AddTicker(new EnemyBlipTicker(blip, ped));
			}

			if (amount == 1)
			{
				mod.ShowText($"{from} spawned an fib operative out to get you!");
			}
			else
			{
				mod.ShowText($"{from} spawned {amount} fib operatives out to get you!");
			}
		}
	}

	class EnemyBlipTicker : ITicker
	{
		private Blip blip;
		private Ped ped;

		public EnemyBlipTicker(Blip blip, Ped ped)
		{
			this.blip = blip;
			this.ped = ped;
		}

		public override void Stop()
		{
		}

		public override bool Tick()
		{
			if (this.ped.IsAlive)
			{
				return false;
			}

			this.blip.Delete();
			return true;
		}
	}
}
