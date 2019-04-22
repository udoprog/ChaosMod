using System;
using GTA;
using GTA.Native;
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

		public Drunk(bool veryDrunk)
		{
			this.veryDrunk = veryDrunk;
		}

		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			var player = Game.Player.Character;

			if (player == null)
			{
				return;
			}

			float amplitude = 1f;
			string animationSet = "move_m@drunk@moderatedrunk";
			string what = "drunk";

			if (veryDrunk)
			{
				amplitude = 5f;
				animationSet = "move_m@drunk@verydrunk";
				what = "VERY drunk";
				Function.Call(Hash._START_SCREEN_EFFECT, "DrugsDrivingOut", 0, 0);
				Function.Call(Hash._0x293220DA1B46CEBC, 4.0, 12.0, 4);
			}

			var timer = mod.Timer(what, 20f);
			var stumbleTimer = mod.RandomTimer(2f, 5f);

			if (!WorldExtension.HasAnimationSetLoaded(animationSet))
			{
				WorldExtension.RequestAnimationSet(animationSet);
			}

			GameplayCamera.Shake(CameraShake.Drunk, amplitude);
			player.SetPedIsDrunk(true);
			player.SetConfigFlag(100, true);
			player.SetPedMovementClipset(animationSet);
			mod.AddUniqueTicker(TickerId.Drunk, new DrunkTicker(mod.Rnd, timer, stumbleTimer, player));
			mod.ShowText($"{from} made you {what}!");
		}

		class DrunkTicker : ITicker
		{
			private Random rnd;
			private Timer timer;
			private Timer stumbleTimer;
			private Ped ped;

			public DrunkTicker(Random rnd, Timer timer, Timer stumbleTimer, Ped ped)
			{
				this.rnd = rnd;
				this.timer = timer;
				this.stumbleTimer = stumbleTimer;
				this.ped = ped;
			}

			public override void Stop()
			{
				stumbleTimer.Stop();
				timer.Stop();
			}

			public override bool Tick()
			{
				if (stumbleTimer.Tick())
				{
					ped.Euphoria.LeanRandom.Start(rnd.Next(500, 1000));
					ped.Euphoria.BodyBalance.Start();
				}

				if (timer.Tick())
				{
					GameplayCamera.StopShaking();
					ped.ResetPedMovementClipset();
					ped.SetPedIsDrunk(false);
					ped.ResetConfigFlag(100);
					Function.Call(Hash._STOP_SCREEN_EFFECT, "DrugsDrivingOut");
					return true;
				}

				return false;
			}
		}
	}
}
