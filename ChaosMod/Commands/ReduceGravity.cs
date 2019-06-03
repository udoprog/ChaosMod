using System;
using GTA;
using GTA.Native;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	public class ReduceGravity : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			var timer = mod.Timer("Reduced Gravity", 15);
			World.GravityLevel = 9.8f / 2;
			mod.AddUniqueTicker(TickerId.ReducedGravity, new Ticker(timer));
		}

		class Ticker : ITicker
		{
			private PlayerTimer timer;

			public Ticker(PlayerTimer timer)
			{
				this.timer = timer;
			}

			public override void Stop()
			{
				timer.Stop();
			}

			public override bool Tick()
			{
				if (timer.Tick())
				{
					World.GravityLevel = 9.8f;
					return true;
				}

				return false;
			}
		}
	}
}
