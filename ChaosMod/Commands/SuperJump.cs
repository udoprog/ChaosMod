using System;
using System.Collections.Generic;
using GTA;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Cause the current character to super jump.
	/// </summary>
	public class SuperJump : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			var r = rest.GetEnumerator();

			if (!r.MoveNext())
			{
				return;
			}

			var time = float.Parse(r.Current);
			mod.AddTicker(new SuperJumpTicker(time));
			mod.ShowText($"{from} gave you with super jump for {time} seconds");
		}
	}

	class SuperJumpTicker : ITicker
	{
		float timer;

		public SuperJumpTicker(float timer)
		{
			this.timer = timer;
			Game.Player.SetSuperJumpThisFrame();
		}

		public bool Tick()
		{
			var delta = Game.LastFrameTime;
			timer -= delta;
			if (timer <= 0)
			{
				return true;
			}

			Game.Player.SetSuperJumpThisFrame();
			return false;
		}

		public String What()
		{
			return "Super Jump";
		}
	}
}
