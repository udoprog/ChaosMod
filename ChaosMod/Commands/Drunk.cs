using System;
using GTA;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Make the player drunk.
	/// 
	/// EXPERIMENTAL - currently does not work.
	/// </summary>
	public class Drunk : Command
	{
		private bool veryDrunk;
		/// <summary>
		/// How many effects are stacked.
		/// </summary>
		private int stacked;

		public Drunk(bool veryDrunk)
		{
			this.veryDrunk = veryDrunk;
			this.stacked = 0;
		}

		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			var player = Game.Player.Character;

			if (player == null)
			{
				return;
			}

			this.stacked += 1;

			var id = TickerId.Drunk;
			float amplitude = 1f;
			string animationSet = "move_m@drunk@moderatedrunk";
			string what = "drunk";

			if (veryDrunk)
			{
				amplitude = 5f;
				animationSet = "move_m@drunk@verydrunk";
				what = "VERY drunk";
			}

			var timer = mod.Timer(what, 10f);

			if (!WorldExtension.HasAnimationSetLoaded(animationSet))
			{
				WorldExtension.RequestAnimationSet(animationSet);
			}

			GameplayCamera.Shake(CameraShake.Drunk, amplitude);
			player.SetPedIsDrunk(true);
			player.SetConfigFlag(100, true);
			player.SetPedMovementClipset(animationSet);
			mod.AddUniqueTicker(id, new DrunkTicker(timer, player));
			mod.ShowText($"{from} made you {what}!");
		}

		class DrunkTicker : ITicker
		{
			private Timer timer;
			private Ped ped;

			public DrunkTicker(Timer timer, Ped ped)
			{
				this.timer = timer;
				this.ped = ped;
			}

			public override void Stop()
			{
				timer.Stop();
			}

			public override bool Tick()
			{
				if (timer.Tick())
				{
					GameplayCamera.StopShaking();
					ped.ResetPedMovementClipset();
					ped.SetPedIsDrunk(false);
					ped.ResetConfigFlag(100);
					return true;
				}

				return false;
			}
		}
	}
}
