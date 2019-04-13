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

			float amplitude = 1f;
			string animationSet = "move_m@drunk@moderatedrunk";
			string what = "drunk";

			if (veryDrunk)
			{
				amplitude = 5f;
				animationSet = "move_m@drunk@verydrunk";
				what = "VERY drunk";
			}

			if (!WorldExtension.HasAnimationSetLoaded(animationSet))
			{
				WorldExtension.RequestAnimationSet(animationSet);
			}

			player.SetPedMovementClipset(animationSet);

			GameplayCamera.Shake(CameraShake.Drunk, amplitude);
			player.SetConfigFlag(100, true);
			mod.AddTicker(new DrunkTicker(10, this, player));
			mod.ShowText($"{from} made you {what}!");
		}

		class DrunkTicker : ITicker
		{
			private float timer;
			private Drunk drunk;
			private Ped ped;

			public DrunkTicker(float timer, Drunk drunk, Ped ped)
			{
				this.timer = timer;
				this.drunk = drunk;
				this.ped = ped;
				this.ped.SetPedIsDrunk(true);
			}

			public bool Tick()
			{
				var delta = Game.LastFrameTime;
				timer -= delta;

				if (timer > 0)
				{
					return false;
				}

				this.drunk.stacked -= 1;

				if (this.drunk.stacked == 0)
				{
					GameplayCamera.StopShaking();
					this.ped.ResetPedMovementClipset();
					this.ped.SetPedIsDrunk(false);
					this.ped.ResetConfigFlag(100);
				}

				return true;
			}

			public String What()
			{
				return "Drunkenness";
			}
		}
	}
}
