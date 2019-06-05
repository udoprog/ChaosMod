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
				mod.ShowText($"{from} tried to make you skyfall :(");
				return;
			}

			// parameters when in vehicle
			if (player.CurrentVehicle != null)
			{
				player.CurrentVehicle.Delete();
			}

			switch (player.ParachuteState)
			{
				case ParachuteState.Deploying:
				case ParachuteState.Gliding:
					player.Task.UseParachute();
					break;
				default:
					break;
			}

			var timer = mod.Timer("Skyfall", 20f);
			player.Weapons.Give(WeaponHash.Parachute, 1, true, true);
			mod.AddUniqueTicker(TickerId.Skyfall, new SkyfallTicker(player, timer));
		}

		class SkyfallTicker : ITicker
		{
			Timer timer;
			Ped player;
			bool putAwayParachute;

			public SkyfallTicker(Ped player, Timer timer)
			{
				this.player = player;
				this.timer = timer;
				this.putAwayParachute = false;
			}

			public override void Stop()
			{
				timer.Stop();
			}

			public override bool Tick()
			{
				if (!player.IsAlive)
				{
					return true;
				}

				switch (player.ParachuteState)
				{
					case ParachuteState.Deploying:
					case ParachuteState.Gliding:
						if (putAwayParachute)
						{
							return true;
						}
						break;
					case ParachuteState.None:
						player.Task.ClearAll();
						player.Task.Skydive();
						break;
					default:
						putAwayParachute = true;
						break;
				}

				var factor = timer.Remaining / timer.Duration;
				var facing = player.ForwardVector * 10_000f * (1 - factor);
				var up = GTA.Math.Vector3.WorldUp * 1_000f * factor;
				player.ApplyForce(up + facing);
				return timer.Tick();
			}
		}
	}
}
