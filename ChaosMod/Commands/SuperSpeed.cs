using System;
using System.Collections.Generic;
using GTA;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Cause the current character to super speed.
	/// </summary>
	public class SuperSpeed : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			var r = rest.GetEnumerator();

			if (!r.MoveNext())
			{
				return;
			}

			var time = float.Parse(r.Current);
			mod.AddTicker(new SuperSpeedTicker(time, 10f));
			mod.ShowText($"{from} gave you super speed for {time} seconds");
		}
	}

	class SuperSpeedTicker : ITicker
	{
		float timer;

		public SuperSpeedTicker(float timer, float multiplier)
		{
			this.timer = timer;
			Game.Player.SetRunSpeedMultThisFrame(multiplier);
		}

		public bool Tick()
		{
			var delta = Game.LastFrameTime;
			timer -= delta;
			if (timer <= 0)
			{
				Game.Player.SetRunSpeedMultThisFrame(1f);
				return true;
			}

			return false;
		}

		public String What()
		{
			return "Super Speed";
		}
	}
}
