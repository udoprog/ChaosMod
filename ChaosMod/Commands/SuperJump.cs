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
			var player = Game.Player;

			mod.AddUniqueTicker(TickerId.SuperJump, new SuperJumpTicker(time, player));
			mod.ShowText($"{from} gave you with super jump for {time} seconds");
		}
	}

	class SuperJumpTicker : ITicker
	{
		private float timer;
		private Player player;

		public SuperJumpTicker(float timer, Player player)
		{
			this.timer = timer;
			this.player = player;
		}

		public bool Tick()
		{
			var delta = Game.LastFrameTime;
			timer -= delta;
			if (timer <= 0)
			{
				return true;
			}

			player.SetSuperJumpThisFrame();
			return false;
		}

		public String What()
		{
			return "Super Jump";
		}
	}
}
