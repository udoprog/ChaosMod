using System;
using GTA;
using GTA.Native;
using System.Collections.Generic;

namespace ChaosMod
{
	public interface Command
	{
		void Handle(Chaos mod, String from, IEnumerable<String> rest);
	}
}
