using System;
using GTA;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Leak fuel from the current vehicle.
	/// </summary>
	public class FuelLeakage : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			Ped player = Game.Player.Character;
			Vehicle vehicle = player.CurrentVehicle;

			if (vehicle == null)
			{
				mod.ShowText($"{from} tried to cause your fuel tank to leak :(");
				return;
			}

			var initial = vehicle.FuelLevel / 100f;
			var timer = mod.AnonymousTimer(30f * initial, 30f);
			var gauge = mod.Gauge("Fuel Level");
			mod.AddTicker(new FuelLeakageTicker(vehicle, gauge, timer));
			mod.ShowText($"{from} caused your fuel tank to leak!");
		}
	}

	public class FuelLeakageTicker : ITicker
	{
		private Vehicle vehicle;
		private Gauge gauge;
		private AnonymousTimer timer;
		private float lastLevel;

		public FuelLeakageTicker(Vehicle vehicle, Gauge gauge, AnonymousTimer timer)
		{
			this.vehicle = vehicle;
			this.gauge = gauge;
			this.timer = timer;
			this.lastLevel = vehicle.FuelLevel;
		}

		public override void Stop()
		{
			gauge.Stop();
			timer.Stop();
		}

		public override bool Tick()
		{
			if (vehicle.FuelLevel > lastLevel)
			{
				return true;
			}

			lastLevel = vehicle.FuelLevel;

			var level = 0f;

			if (lastLevel > 0)
			{
				level = 1f - timer.Percentage();
				vehicle.FuelLevel = level * 100;
				gauge.Set(level);

				if (vehicle.Acceleration != 0 && timer.Tick())
				{
					gauge.Clear("Out Of Fuel");
					vehicle.FuelLevel = 0;
				}
			}

			Ped player = Game.Player.Character;

			if (gauge.Visible)
			{
				if (player.CurrentVehicle != vehicle)
				{
					gauge.Visible = false;
				}
			}
			else
			{
				if (player.CurrentVehicle == vehicle)
				{
					gauge.Visible = true;
				}
			}

			return !vehicle.IsAlive;
		}
	}
}
