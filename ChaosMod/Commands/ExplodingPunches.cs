using System;
using GTA;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Enable exploding punches for 20 seconds.
	/// </summary>
	public class ExplodingPunches : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			var duration = rest.GetEnumerator().NextFloatOrDefault(30f);
			var timer = mod.Timer("Exploding Punches", duration);
			mod.AddUniqueTicker(TickerId.ExplodingPunches, new ExplodingPunchesTicker(timer));
			mod.ShowText($"{from} enabled exploding punches for {duration} seconds!");
		}
	}

	class ExplodingPunchesTicker : ITicker
	{
		Timer timer;

		public ExplodingPunchesTicker(Timer timer)
		{
			this.timer = timer;
		}

		public override void Stop()
		{
			timer.Stop();
		}

		public override bool Tick()
		{
			Game.Player.SetExplosiveMeleeThisFrame();
			return timer.Tick();
		}
	}
}
