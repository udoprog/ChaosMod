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
			var duration = rest.GetEnumerator().NextFloatOrDefault(30f);
			var player = Game.Player.Character;

			if (player == null)
			{
				mod.ShowText($"{from} tried to give you invincibility :(");
				return;
			}

			var timer = mod.Timer("Invincibility", duration);

			if (mod.AddUniqueTicker(TickerId.Invincibility, new InvincibilityTicker(timer, player)))
			{
				Game.Player.IsInvincible = true;
			}

			mod.ShowText($"{from} gave you invincibility for {duration} seconds!");
		}
	}

	class InvincibilityTicker : ITicker
	{
		private Timer timer;
		private Ped ped;

		public InvincibilityTicker(Timer timer, Ped ped)
		{
			this.timer = timer;
			this.ped = ped;
		}

		public override void Stop()
		{
			timer.Stop();
		}

		public override bool Tick()
		{
			if (timer.Tick())
			{
				ped.IsInvincible = false;
				return true;
			}

			return false;
		}
	}
}
