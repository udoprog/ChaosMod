using System;
using GTA;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Give the player invincibility.
	/// </summary>
	public class Invincibility : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			var r = rest.GetEnumerator();

			if (!r.MoveNext())
			{
				return;
			}

			var time = float.Parse(r.Current);
			mod.AddTicker(new InvincibilityTicker(time));
			mod.ShowText($"{from} gave you invincibility for {time} seconds!");
		}
	}

	class InvincibilityTicker : ITicker
	{
		float timer;

		public InvincibilityTicker(float timer)
		{
			this.timer = timer;
			Game.Player.IsInvincible = true;
		}

		public bool Tick()
		{
			var delta = Game.LastFrameTime;
			timer -= delta;

			if (timer <= 0)
			{
				Game.Player.IsInvincible = false;
				return true;
			}

			return false;
		}

		public String What()
		{
			return "Invincibility";
		}
	}
}
