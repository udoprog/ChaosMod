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
			var duration = rest.GetEnumerator().NextFloatOrDefault(30f);

			var player = Game.Player;

			var timer = mod.Timer("Super Jump", duration);

			mod.AddUniqueTicker(TickerId.SuperJump, new SuperJumpTicker(timer, player));
			mod.ShowText($"{from} gave you with super jump for {duration} seconds");
		}
	}

	class SuperJumpTicker : ITicker
	{
		private Timer timer;
		private Player player;

		public SuperJumpTicker(Timer timer, Player player)
		{
			this.timer = timer;
			this.player = player;
		}

		public override void Stop()
		{
			timer.Stop();
		}

		public override bool Tick()
		{
			player.SetSuperJumpThisFrame();
			return timer.Tick();
		}
	}
}
