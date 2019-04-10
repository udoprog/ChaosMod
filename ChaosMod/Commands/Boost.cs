using System;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Boost the current vehicle.
	/// </summary>
	public class Boost : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			mod.Boost(false);
			mod.ShowText($"{from} caused the vehicle to boost!");
		}
	}
}
