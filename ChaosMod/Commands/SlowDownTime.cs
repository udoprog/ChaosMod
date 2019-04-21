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
			if (mod.AddUniqueTicker(TickerId.SlowDownTime, new SlowDownTimeTicker(5f)))
			{
				Function.Call(Hash.SET_TIME_SCALE, 0.25f);
			}

			mod.ShowText($"{from} slowed down time!");
		}
	}

	class SlowDownTimeTicker : ITicker
	{
		float timer;

		public SlowDownTimeTicker(float timer)
		{
			this.timer = timer;
		}

		public bool Tick()
		{
			var delta = Game.LastFrameTime;
			timer -= delta;

			if (timer > 0)
			{
				return false;
			}

			Function.Call(Hash.SET_TIME_SCALE, 1.0f);
			return true;
		}

		public String What()
		{
			return "Time slowdown";
		}
	}
}
