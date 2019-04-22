using System;
using GTA;
using System.Collections.Generic;
namespace ChaosMod.Commands
{
	/// <summary>
	/// Levitate entities surrounding the player.
	/// </summary>
	public class LevitateEntities : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			var player = Game.Player.Character;

			if (player == null)
			{
				mod.ShowText($"{from} tried to enable matrix slam :(");
				return;
			}

			var entities = World.GetNearbyEntities(player.Position, 1000f);

			foreach (var entity in entities)
			{
				if (entity == player)
				{
					continue;
				}

				if (entity is Ped)
				{
					var ped = (Ped)entity;
					mod.AddTicker(ped.Levitate(mod.Rnd, mod.AnonymousTimer(10f)));
					continue;
				}

				if (entity is Vehicle)
				{
					var vehicle = (Vehicle)entity;
					mod.AddTicker(vehicle.Levitate(mod.Rnd, mod.AnonymousTimer(10f)));
					continue;
				}
			}

			mod.AddUniqueTicker(TickerId.LevitatingEntities, new TimerTicker(mod.Timer("Levitating things", 10f)));
		}
	}
}
