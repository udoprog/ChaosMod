using System;
using GTA;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Enable fire ammo.
	/// </summary>
	public class FireAmmo : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			var duration = rest.GetEnumerator().NextFloatOrDefault(30f);
			var timer = mod.Timer("Fire Ammo", duration);
			mod.AddUniqueTicker(TickerId.ExplodingBullets, new FireAmmoTicker(timer));
			mod.ShowText($"{from} enabled fire ammo!");
		}
	}

	class FireAmmoTicker : ITicker
	{
		Timer timer;

		public FireAmmoTicker(Timer timer)
		{
			this.timer = timer;
		}

		public override void Stop()
		{
			this.timer.Stop();
		}

		public override bool Tick()
		{
			Game.Player.SetFireAmmoThisFrame();
			return timer.Tick();
		}
	}
}
