using System;
using System.Collections.Generic;
using GTA;

namespace ChaosMod.Commands
{
	public enum DisabledControl
	{
		Steering,
	}

	/// <summary>
	/// Command to disable a single control.
	/// </summary>
	public class DisableControl : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			var r = rest.GetEnumerator();

			if (!r.MoveNext())
			{
				return;
			}

			String what = null;
			DisabledControl control = DisabledControl.Steering;

			var id = r.Current;

			switch (id)
			{
				case "steering":
					control = DisabledControl.Steering;
					what = "Steering";
					break;
				default:
					throw new Exception($"bad disable-control id `{id}`");
			}

			var timer = mod.Timer($"Disabled {what}", 10f);
			mod.AddTicker(new DisableControlTicker(timer, control));
			mod.ShowText($"{from} disabled the {what} control!");
		}

		/// <summary>
		/// Disable the specified control for a given time.
		/// </summary>
		class DisableControlTicker : ITicker
		{
			private Timer timer;
			private DisabledControl control;

			public DisableControlTicker(Timer timer, DisabledControl control)
			{
				this.timer = timer;
				this.control = control;
			}

			public override void Stop()
			{
				timer.Stop();
			}

			public override bool Tick()
			{
				switch (control)
				{
					case DisabledControl.Steering:
						Game.DisableControlThisFrame(0, Control.VehicleMoveLeftRight);
						break;
				}

				return timer.Tick();
			}
		}
	}
}
