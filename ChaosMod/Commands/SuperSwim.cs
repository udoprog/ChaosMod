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
			var r = rest.GetEnumerator();

			if (!r.MoveNext())
			{
				return;
			}

			var time = float.Parse(r.Current);
			mod.AddTicker(new SuperSwimTicker(time, 10f));
			mod.ShowText($"{from} gave you with super swim for {time} seconds");
		}
	}

	class SuperSwimTicker : ITicker
	{
		float timer;

		public SuperSwimTicker(float timer, float multiplier)
		{
			this.timer = timer;
			Game.Player.SetSwimSpeedMultThisFrame(multiplier);
		}

		public bool Tick()
		{
			var delta = Game.LastFrameTime;
			timer -= delta;
			if (timer <= 0)
			{
				Game.Player.SetSwimSpeedMultThisFrame(1f);
				return true;
			}

			return false;
		}

		public String What()
		{
			return "Super Swim";
		}
	}
}
