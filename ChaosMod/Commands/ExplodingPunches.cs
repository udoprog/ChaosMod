using System;
using GTA;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Enable exploding punches for 10 seconds.
	/// </summary>
	public class ExplodingPunches : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			mod.AddUniqueTicker(TickerId.ExplodingPunches, new ExplodingPunchesTicker(20));
			mod.ShowText($"{from} enabled exploding punches!");
		}
	}

	class ExplodingPunchesTicker : ITicker
	{
		float timer;

		public ExplodingPunchesTicker(float timer)
		{
			this.timer = timer;
		}

		public bool Tick()
		{
			var delta = Game.LastFrameTime;
			timer -= delta;

			if (timer <= 0)
			{
				return true;
			}

			Game.Player.SetExplosiveMeleeThisFrame();
			return false;
		}

		public String What()
		{
			return "Exploding punches";
		}
	}
}
