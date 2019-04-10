using System;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Make the character stumble.
	/// </summary>
	public class Stumble : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			mod.Stumble(2000);
			mod.ShowText($"{from} tripped you!");
		}
	}
}
