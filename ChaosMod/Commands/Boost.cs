using System;
using System.Collections.Generic;
using GTA;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Boost the current vehicle.
	/// </summary>
	public class Boost : Command
	{
		/// <summary>
		/// If we are super boosting.
		/// </summary>
		private bool super;

		public Boost(bool super)
		{
			this.super = super;
		}

		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			Ped player = Game.Player.Character;

			if (player == null)
			{
				mod.ShowText($"{from} tried to boost you :(");
				return;
			}

			var what = "Boost";
			float time = 30f;
			float factor = 20f;

			if (super)
			{
				factor = 200f;
				what = "Super Boost";
			}

			var timer = mod.Timer(what, time);
			mod.AddTicker(new BoostTicker(player, timer, factor));
			mod.ShowText($"{from} caused the vehicle to boost!");
		}

		class BoostTicker : ITicker
		{
			/// <summary>
			/// Player being boosted.
			/// </summary>
			Ped player;
			/// <summary>
			/// Time the vehicle is being boosted for.
			/// </summary>
			Timer timer;
			/// <summary>
			/// Force to apply for the boost.
			/// </summary>
			float factor;

			public BoostTicker(Ped player, Timer timer, float factor)
			{
				this.player = player;
				this.timer = timer;
				this.factor = factor;
			}

			public override void Stop()
			{
				timer.Stop();
			}

			public override bool Tick()
			{
				if (timer.Tick() || !player.IsAlive)
				{
					return true;
				}

				var vehicle = player.CurrentVehicle;
				var delta = Game.LastFrameTime;

				if (vehicle != null)
				{
					var a = vehicle.Acceleration;
					var forward = GTA.Math.Vector3.WorldNorth;
					vehicle.ApplyForceRelative(forward * delta * a * factor);
				}

				return false;
			}
		}
	}
}
