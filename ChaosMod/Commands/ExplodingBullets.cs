using System;
using GTA;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Enable exploding bullets for 10 seconds?
	/// </summary>
	public class ExplodingBullets : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			var duration = rest.GetEnumerator().NextFloatOrDefault(30f);
			var timer = mod.Timer("Exploding Bullets", duration);
			mod.AddUniqueTicker(TickerId.ExplodingBullets, new ExplodingBulletsTicker(timer));
			mod.ShowText($"{from} enabled exploding bullets!");
		}
	}

	class ExplodingBulletsTicker : ITicker
	{
		Timer timer;

		public ExplodingBulletsTicker(Timer timer)
		{
			this.timer = timer;
		}

		public override void Stop()
		{
			this.timer.Stop();
		}

		public override bool Tick()
		{
			Game.Player.SetExplosiveAmmoThisFrame();
			return timer.Tick();
		}
	}
}
