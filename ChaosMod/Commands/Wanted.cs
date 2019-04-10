using System;
using GTA;
using GTA.Native;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	public class Wanted : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			var r = rest.GetEnumerator();

			if (!r.MoveNext())
			{
				return;
			}

			var level = int.Parse(r.Current);

			String what;

			if (level == 0)
			{
				what = $"{from} cleared your wanted level!";
			}
			else if (level == 1)
			{
				what = $"{from} set your wanted level to one star!";
			}
			else
			{
				what = $"{from} set your wanted level to {level} stars!";
			}

			Game.Player.WantedLevel = level;
			mod.ShowText(what);
		}
	}
}
