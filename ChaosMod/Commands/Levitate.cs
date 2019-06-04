using System;
using GTA;
using System.Collections.Generic;
namespace ChaosMod.Commands
{
	/// <summary>
	/// Levitate the current player or vehicle.
	/// </summary>
	public class Levitate : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			var player = Game.Player.Character;

			if (player == null)
			{
				mod.ShowText($"{from} tried to enable matrix slam :(");
				return;
			}

			var time = 10f;
			var extraHeight = 4f;
			var entity = (Entity) player;
			float? jitter = null;

			// parameters when in vehicle
			if (player.CurrentVehicle != null)
			{
				entity = player.CurrentVehicle;
				extraHeight = 10f + (float) (mod.Rnd.NextDouble() * 5f);
			} else
			{
				time = 7f;
				player.Euphoria.HighFall.Start(10_000);
				jitter = 0.05f;
				extraHeight += (float) (mod.Rnd.NextDouble() * 2f);
			}

			var height = entity.Position.Z + extraHeight;

			var timer = mod.Timer("Levitation", time);
			mod.AddUniqueTicker(TickerId.Levitation, new LevitateController(timer, entity, height, mod.Rnd, jitter));
			mod.ShowText($"{from} caused you to levitate!");
		}
	}
}
