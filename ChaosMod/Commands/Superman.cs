using System;
using GTA;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Call the necessary commands to make the character into superman.
	/// </summary>
	public class Superman : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			mod.Call(from, "internal", "exploding-punches", rest);
			mod.Call(from, "internal", "super-speed", rest);
			mod.Call(from, "internal", "super-jump", rest);
			mod.Call(from, "internal", "super-swim", rest);
			mod.Call(from, "internal", "invincibility", rest);
		}
	}
}
