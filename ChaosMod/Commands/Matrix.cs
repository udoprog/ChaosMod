using System;
using GTA;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Matrix effect. Pick up props and pedestrians and slam them into the ground.
	/// </summary>
	public class MatrixSlam : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			var player = Game.Player.Character;

			if (player == null)
			{
				mod.ShowText($"{from} tried to enable matrix slam :(");
				return;
			}

			var entities = World.GetNearbyEntities(player.Position, 100f);

			var controller = new MatrixController(entities, player);

			mod.AddTicker(controller);
			mod.ShowText($"{from} performed a matrix slam!");
		}

		class MatrixController : ITicker
		{
			private float timer;
			private Entity[] entities;
			private Ped player;

			public MatrixController(Entity[] entities, Ped player)
			{
				this.timer = 0;
				this.entities = entities;
				this.player = player;
			}

			public bool Tick()
			{
				var delta = Game.LastFrameTime;
				timer += Game.LastFrameTime;

				var vehicle = player.CurrentVehicle;

				if (timer > 2)
				{
					return true;
				}

				if (timer < 1)
				{
					var fa = GTA.Math.Vector3.WorldUp * 100f * delta;
					var ra = GTA.Math.Vector3.WorldNorth * 10f;

					foreach (var entity in entities)
					{
						if (entity == player || entity == vehicle)
						{
							continue;
						}

						entity.ApplyForce(fa, ra);
					}

					return false;
				}

				var fb = GTA.Math.Vector3.WorldDown * 1000f * delta;
				var rb = GTA.Math.Vector3.Zero;

				foreach (var entity in entities)
				{
					if (entity == player || entity == vehicle)
					{
						continue;
					}

					entity.ApplyForce(fb, rb);
				}

				return false;
			}

			public String What()
			{
				return "Matrix Mode";
			}
		}
	}
}
