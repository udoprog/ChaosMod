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

			var timer = mod.Timer("Handbrake On", 5f);
			mod.AddTicker(new BrakeTicker(vehicle, timer));
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
		Timer timer;

		public BrakeTicker(Vehicle vehicle, Timer timer)
		{
			this.vehicle = vehicle;
			this.timer = timer;
			this.vehicle.HandbrakeOn = true;
		}

		public override void Stop()
		{
			timer.Stop();
		}

		public override bool Tick()
		{
			vehicle.HandbrakeOn = false;
			return timer.Tick();
		}
	}
}
