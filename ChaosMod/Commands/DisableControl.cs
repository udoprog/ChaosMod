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

			mod.AddTicker(new Ticker(10, control, what));
			mod.ShowText($"{from} disabled the {what} control!");
		}

		/// <summary>
		/// Disable the specified control for a given time.
		/// </summary>
		class Ticker : ITicker
		{
			private float timer;
			private DisabledControl control;
			private String what;

			public Ticker(float timer, DisabledControl control, String what)
			{
				this.timer = timer;
				this.control = control;
				this.what = what;
			}

			public bool Tick()
			{
				timer -= Game.LastFrameTime;

				if (timer > 0)
				{
					switch (control)
					{
						case DisabledControl.Steering:
							Game.DisableControlThisFrame(0, Control.VehicleMoveLeftRight);
							break;
					}

					return false;
				}

				return true;
			}

			public String What()
			{
				return $"Disabling of {what}";
			}
		}
	}
}
