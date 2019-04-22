using System;
using GTA;
using GTA.Native;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Slow down time.
	/// </summary>
	public class SlowDownTime : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			var timer = mod.Timer("Time Slowdown", 5f);

			if (mod.AddUniqueTicker(TickerId.SlowDownTime, new SlowDownTimeTicker(timer)))
			{
				Function.Call(Hash.SET_TIME_SCALE, 0.25f);
			}

			mod.ShowText($"{from} slowed down time!");
		}
	}

	class SlowDownTimeTicker : ITicker
	{
		Timer timer;

		public SlowDownTimeTicker(Timer timer)
		{
			this.timer = timer;
		}

		public override void Stop()
		{
			timer.Stop();
		}

		public override bool Tick()
		{
			if (!timer.Tick())
			{
				return false;
			}

			Function.Call(Hash.SET_TIME_SCALE, 1.0f);
			return true;
		}
	}
}
