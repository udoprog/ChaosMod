using System;
using GTA;
using GTA.Native;

namespace ChaosMod
{
	/// <summary>
	/// Identifier for a unique ticker.
	/// </summary>
	public enum TickerId
	{
		SuppressShocking,
		SetOnFire,
		MakeFireProof,
		SuperSpeed,
		SuperJump,
		SuperSwim,
		Invincibility,
		ExplodingPunches,
		ExplodingBullets,
		SlowDownTime,
		Levitation,
		Drunk,
		LevitatingEntities,
	}

	public abstract class ITicker
	{
		/// <summary>
		/// Tick the ticker.
		/// </summary>
		public abstract bool Tick();

		/// <summary>
		/// Stop the ticker from the outside. Cleaning up any resource associated with it.
		/// </summary>
		public abstract void Stop();
	}

	/// <summary>
	/// A ticker that simply runs a timer.
	/// </summary>
	class TimerTicker : ITicker
	{
		Timer timer;

		public TimerTicker(Timer timer)
		{
			this.timer = timer;
		}

		public override void Stop()
		{
			timer.Stop();
		}

		public override bool Tick()
		{
			return timer.Tick();
		}
	}

	class SuppressShockingTicker : ITicker
	{
		/// <summary>
		/// Time the vehicle is being boosted for.
		/// </summary>
		Timer timer;

		public SuppressShockingTicker(Timer timer)
		{
			this.timer = timer;
		}

		public override void Stop()
		{
			timer.Stop();
		}

		public override bool Tick()
		{
			WorldExtension.SuppressShockingEventsNextFrame();
			return timer.Tick();
		}
	}

	public class LevitateController : ITicker
	{
		private Timer timer;
		private Entity entity;
		private float height;
		private Random rnd;
		private float? jitter;
		private bool initialWheels;

		public LevitateController(Timer timer, Entity entity, float height, Random rnd, float? jitter)
		{
			this.timer = timer;
			this.entity = entity;
			this.height = height;
			this.rnd = rnd;
			this.jitter = jitter;
			this.initialWheels = true;
		}

		public override void Stop()
		{
			timer.Stop();
		}

		public override bool Tick()
		{
			if (timer.Tick())
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
				}
				else
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
	}
}
