using System;
using GTA;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Causes the current vehicle to VERY suddenly break.
	/// </summary>
	public class Brake : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			Ped player = Game.Player.Character;
			Vehicle vehicle = player.CurrentVehicle;

			if (vehicle == null)
			{
				mod.ShowText($"{from} tried to brake your vehicle!");
				return;
			}

			mod.AddTicker(new BrakeTicker(vehicle, 5f));
			mod.ShowText($"{from} caused the vehicle to brake!");
		}
	}

	class BrakeTicker : ITicker
	{
		/// <summary>
		/// Vehicle being braked.
		/// </summary>
		Vehicle vehicle;
		/// <summary>
		/// Time the vehicle is being braked for.
		/// </summary>
		float timer;

		public BrakeTicker(Vehicle vehicle, float timer)
		{
			this.vehicle = vehicle;
			this.timer = timer;
			this.vehicle.HandbrakeOn = true;
		}

		public bool Tick()
		{
			timer -= Game.LastFrameTime;

			if (timer <= 0)
			{
				timer = 0;
				vehicle.HandbrakeOn = false;
				return true;
			}

			return false;
		}

		public String What()
		{
			return "braking your vehicle";
		}
	}
}
