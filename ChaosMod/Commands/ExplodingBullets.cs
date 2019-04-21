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
			mod.AddUniqueTicker(TickerId.ExplodingBullets, new ExplodingBulletsTicker(10));
			mod.ShowText($"{from} enabled exploding bullets!");
		}
	}

	class ExplodingBulletsTicker : ITicker
	{
		float timer;

		public ExplodingBulletsTicker(float timer)
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

			Game.Player.SetExplosiveAmmoThisFrame();
			return false;
		}

		public String What()
		{
			return "Exploding bullets";
		}
	}
}
