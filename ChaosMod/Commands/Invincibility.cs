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

			var player = Game.Player.Character;

			if (player == null)
			{
				mod.ShowText($"{from} tried to give you invincibility :(");
				return;
			}

			var time = float.Parse(r.Current);

			if (mod.AddUniqueTicker(TickerId.Invincibility, new InvincibilityTicker(time, player)))
			{
				Game.Player.IsInvincible = true;
			}

			mod.ShowText($"{from} gave you invincibility for {time} seconds!");
		}
	}

	class InvincibilityTicker : ITicker
	{
		private float timer;
		private Ped ped;

		public InvincibilityTicker(float timer, Ped ped)
		{
			this.timer = timer;
			this.ped = ped;
		}

		public bool Tick()
		{
			var delta = Game.LastFrameTime;
			timer -= delta;

			if (timer <= 0)
			{
				ped.IsInvincible = false;
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
