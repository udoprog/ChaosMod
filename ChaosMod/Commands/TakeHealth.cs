using System;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Take all character health.
	/// </summary>
	public class TakeHealth : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			mod.Stumble(500);
			mod.TakeHealth();
		}
	}
}
