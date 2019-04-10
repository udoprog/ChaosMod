using System;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Super boost the current vehicle.
	/// </summary>
	public class SuperBoost : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			mod.Boost(true);
			mod.ShowText($"{from} caused the vehicle to SUPER boost!");
		}
	}
}
