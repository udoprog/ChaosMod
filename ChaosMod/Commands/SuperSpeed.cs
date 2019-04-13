using System;
using System.Collections.Generic;
using GTA;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Cause the current character to super speed.
	/// </summary>
	public class SuperSpeed : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			var r = rest.GetEnumerator();

			if (!r.MoveNext())
			{
				return;
			}

			var time = float.Parse(r.Current);

			if (mod.AddUniqueTicker(TickerId.SuperSpeed, new SuperSpeedTicker(time)))
			{
				Game.Player.SetRunSpeedMultThisFrame(time);
			}

			mod.ShowText($"{from} gave you super speed for {time} seconds");
		}

		class SuperSpeedTicker : ITicker
		{
			private float timer;

			public SuperSpeedTicker(float timer)
			{
				this.timer = timer;
			}

			public bool Tick()
			{
				timer -= Game.LastFrameTime;

				if (timer > 0)
				{
					return false;
				}

				Game.Player.SetRunSpeedMultThisFrame(1f);
				return true;
			}

			public String What()
			{
				return "Super Speed";
			}
		}
	}
}
