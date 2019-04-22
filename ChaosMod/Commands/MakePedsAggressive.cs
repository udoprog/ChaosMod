using System;
using GTA;
using GTA.Native;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Set all nearby peds on fire.
	/// </summary>
	public class MakePedsAggressive : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			Ped player = Game.Player.Character;

			if (player == null)
			{
				mod.ShowText($"{from} tried to make EVERYONE attack you!");
				return;
			}

			var peds = World.GetNearbyPeds(player.Position, 5000f);
			var count = 0;

			foreach (var ped in peds)
			{
				if (!ped.Exists() || !ped.IsHuman || !ped.IsAlive || ped == player)
				{
					continue;
				}

				ped.NeverLeavesGroup = true;
				ped.RelationshipGroup = mod.HateGroup.GroupId;

				// var blip = ped.AddBlip();
				// mod.AddTicker(new EnemyBlipTicker(blip, ped));

				ped.SetCombatAttributes(CombatAttributes.AlwaysFight, true);
				ped.SetCombatAttributes(CombatAttributes.CanFightArmedPedsWhenNotArmed, true);

				ped.Task.ClearAll();
				ped.Task.FightAgainst(player);

				count += 1;
			}

			if (count == 0)
			{
				mod.ShowText($"{from} tried to set pedestrians on you, but there are none :(");
				return;
			}

			var timer = mod.AnonymousTimer(30f);
			mod.AddUniqueTicker(TickerId.SuppressShocking, new SuppressShockingTicker(timer));
			mod.ShowText($"{from} set {count} pedestrians on you!");
		}
	}
}
