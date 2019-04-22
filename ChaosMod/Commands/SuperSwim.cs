using System;
using System.Collections.Generic;
using GTA;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Increase swim speed multiplier for a time.
	/// </summary>
	public class SuperSwim : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			var duration = rest.GetEnumerator().NextFloatOrDefault(30f);
			var timer = mod.Timer("Super Swim", duration);
			mod.AddUniqueTicker(TickerId.SuperSwim, new SuperSwimTicker(timer, 10f));
			mod.ShowText($"{from} gave you with super swim for {duration} seconds");
		}

		class SuperSwimTicker : ITicker
		{
			private Timer timer;

			public SuperSwimTicker(Timer timer, float multiplier)
			{
				this.timer = timer;
			}

			public override void Stop()
			{
				timer.Stop();
			}

			public override bool Tick()
			{
				Game.Player.SetSwimSpeedMultThisFrame(1f);
				return timer.Tick();
			}
		}
	}
}
