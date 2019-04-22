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

			var timer = mod.AnonymousTimer(2f);
			var controller = new MatrixController(entities, player, timer);

			mod.AddTicker(controller);
			mod.ShowText($"{from} performed a matrix slam!");
		}

		class MatrixController : ITicker
		{
			private Entity[] entities;
			private Ped player;
			private Timer timer;

			public MatrixController(Entity[] entities, Ped player, Timer timer)
			{
				this.entities = entities;
				this.player = player;
				this.timer = timer;
			}

			public override void Stop()
			{
				timer.Stop();
			}

			public override bool Tick()
			{
				var delta = Game.LastFrameTime;

				var vehicle = player.CurrentVehicle;

				if (timer.Tick())
				{
					return true;
				}

				if (timer.Remaining > 1)
				{
					var fa = GTA.Math.Vector3.WorldUp * 100f * delta;
					var ra = GTA.Math.Vector3.WorldNorth * 10f;

					foreach (var entity in entities)
					{
						if (entity == player || entity == vehicle)
						{
							continue;
						}

						if (entity is Ped)
						{
							var ped = (Ped)entity;

							switch (ped.GetRelationshipWithPed(player))
							{
								case Relationship.Like:
									continue;
								default:
									break;
							}
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
		}
	}
}
