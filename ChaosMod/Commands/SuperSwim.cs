using System;
using System.Collections.Generic;
using GTA;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Increase swim speed multiplier for a time.
	/// </summary>
	public class SuperSwim : Command
	{
		private int stacked = 0;

		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			var r = rest.GetEnumerator();

			if (!r.MoveNext())
			{
				return;
			}

			var time = float.Parse(r.Current);
			stacked += 1;
			mod.AddTicker(new SuperSwimTicker(time, this, 10f));
			mod.ShowText($"{from} gave you with super swim for {time} seconds");
		}

		class SuperSwimTicker : ITicker
		{
			private float timer;
			private SuperSwim parent;

			public SuperSwimTicker(float timer, SuperSwim parent, float multiplier)
			{
				this.timer = timer;
				this.parent = parent;
				Game.Player.SetSwimSpeedMultThisFrame(multiplier);
			}

			public bool Tick()
			{
				timer -= Game.LastFrameTime;

				if (timer > 0)
				{
					return false;
				}

				parent.stacked -= 1;

				if (parent.stacked == 0)
				{
					Game.Player.SetSwimSpeedMultThisFrame(1f);
				}

				return true;
			}

			public String What()
			{
				return "Super Swim";
			}
		}
	}
}
