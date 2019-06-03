using System;
using System.Collections.Generic;
using GTA;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Shoot the player up in the air.
	/// </summary>
	public class Skyfall : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			var player = Game.Player.Character;

			if (player == null)
			{
				mod.ShowText($"{from} tried to enable matrix slam :(");
				return;
			}

			// parameters when in vehicle
			if (player.CurrentVehicle != null)
			{
				player.CurrentVehicle.Delete();
			}

			if (player.HeightAboveGround > 2)
			{
				return;
			}

			var timer = mod.Timer("Skyfall", 10f);
			player.Weapons.Give(WeaponHash.Parachute, 1, true, true);
			player.Euphoria.HighFall.Start(11_000);
			mod.AddTicker(new SkyfallTicker(player, timer));
		}

		class SkyfallTicker : ITicker
		{
			GTA.Math.Vector3 facing;
			Timer timer;
			Ped player;

			public SkyfallTicker(Ped player, Timer timer)
			{
				this.facing = player.ForwardVector;
				this.player = player;
				this.timer = timer;
			}

			public override void Stop()
			{
				timer.Stop();
			}

			public override bool Tick()
			{
				var factor = timer.Remaining / timer.Duration;
				var force = GTA.Math.Vector3.WorldUp * 3_000f * factor + facing * 1_000f * (1 - factor);
				player.ApplyForce(force);
				return timer.Tick();
			}
		}
	}
}
