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
			var duration = rest.GetEnumerator().NextFloatOrDefault(30f);
			var timer = mod.Timer("Super Speed", duration);

			if (mod.AddUniqueTicker(TickerId.SuperSpeed, new SuperSpeedTicker(timer)))
			{
				// NB: in spite of its name, it should only be called once.
				Game.Player.SetRunSpeedMultThisFrame(10f);
			}

			mod.ShowText($"{from} gave you super speed for {duration} seconds");
		}

		class SuperSpeedTicker : ITicker
		{
			private Timer timer;

			public SuperSpeedTicker(Timer timer)
			{
				this.timer = timer;
			}

			public override void Stop()
			{
				timer.Stop();
			}

			public override bool Tick()
			{
				if (!timer.Tick())
				{
					return false;
				}

				Game.Player.SetRunSpeedMultThisFrame(1f);
				return true;
			}
		}
	}
}
