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
				player.CanRagdoll = true;
				player.SetPedToRagdoll(10000, 11000, 2);
				jitter = 0.05f;
				extraHeight += (float) (mod.Rnd.NextDouble() * 2f);
			}

			var position = new GTA.Math.Vector2(entity.Position.X, entity.Position.Y);
			var height = World.GetGroundHeight(position) + extraHeight;

			mod.AddTicker(new LevitateController(10f, entity, height, mod.Rnd, jitter));
			mod.ShowText($"{from} caused you to levitate!");
		}

		class LevitateController : ITicker
		{
			private float timer;
			private Entity entity;
			private float height;
			private Random rnd;
			private float? jitter;
			private bool initialWheels;

			public LevitateController(float timer, Entity entity, float height, Random rnd, float? jitter)
			{
				this.timer = timer;
				this.entity = entity;
				this.height = height;
				this.rnd = rnd;
				this.jitter = jitter;
				this.initialWheels = true;
			}

			public bool Tick()
			{
				timer -= Game.LastFrameTime;

				if (timer <= 0)
				{
					return true;
				}

				if (entity is Vehicle)
				{
					var vehicle = (Vehicle)entity;

					// Stop levitating if the vehicle lands on the ground.
					// To allow for initial push we need a flag `initialWheels` set to avoid no levitation happening at all.
					if (initialWheels)
					{
						initialWheels = !vehicle.IsOnAllWheels;
					} else
					{
						if (vehicle.IsOnAllWheels)
						{
							return true;
						}
					}
				}

				var target = new GTA.Math.Vector3(entity.Position.X, entity.Position.Y, height);

				var velocity = entity.Velocity;
				velocity.Z = (target - entity.Position).Z;
				entity.Velocity = velocity;

				if (jitter.HasValue)
				{
					entity.ApplyForce(rnd.RandomVector3(jitter.Value));
				}

				return false;
			}

			public String What()
			{
				return "Levitation";
			}
		}
	}
}
