using System;
using GTA;
using GTA.Native;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Reverse controls.
	/// 
	/// EXPERIMENTAL: currently does not work.
	///
	/// Disabling controls while simulating them is not easy.
	/// </summary>
	public class ReverseControls : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			var controller = new ReverseControlsTicker(20f);
			mod.AddTicker(controller);
			mod.AddKeyDown(controller);
			mod.ShowText($"{from} reversed your controls!");
		}

		class ReverseControlsTicker : IKeyDown, ITicker
		{
			private float timer;

			public ReverseControlsTicker(float timer)
			{
				this.timer = timer;
			}

			public bool KeyDown()
			{
				return timer <= 0;
			}

			public bool Tick()
			{
				timer -= Game.LastFrameTime;

				if (timer > 0)
				{;
					return false;
				}

				return true;
			}

			public String What()
			{
				return "Reversed controls";
			}
		}
	}
}
