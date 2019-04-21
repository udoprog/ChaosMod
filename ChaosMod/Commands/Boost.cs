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

			float time = 0.5f;
			float factor = 2f;
			var wheelsOnGround = true;

			if (super)
			{
				time = 2f;
				factor = 10f;
				wheelsOnGround = true;

				if (mod.AddUniqueTicker(TickerId.Invincibility, new InvincibilityTicker(10f, player)))
				{
					player.IsInvincible = true;
				}
			}

			mod.AddTicker(new BoostTicker(player, time, factor, wheelsOnGround));
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
			float timer;
			/// <summary>
			/// Force to apply for the boost.
			/// </summary>
			GTA.Math.Vector3 force;
			/// <summary>
			/// Does the boost require wheels to be on ground or not.
			/// </summary>
			bool wheelsOnGround;

			public BoostTicker(Ped player, float timer, float factor, bool wheelsOnGround)
			{
				this.player = player;
				this.timer = timer;
				this.force = GTA.Math.Vector3.WorldNorth * 100f * factor;
				this.wheelsOnGround = wheelsOnGround;
			}

			public bool Tick()
			{
				var vehicle = player.CurrentVehicle;

				if (vehicle == null)
				{
					return true;
				}

				var delta = Game.LastFrameTime;

				var isFlying = player.IsInFlyingVehicle && vehicle.IsInAir;
				var isGrounded = !wheelsOnGround && vehicle.IsInAir || vehicle.IsOnAllWheels;

				if (isFlying || isGrounded)
				{
					vehicle.ApplyForceRelative(this.force * delta);
				}

				timer -= delta;

				if (timer > 0)
				{
					return false;
				}

				return true;
			}

			public String What()
			{
				return null;
			}
		}
	}
}
