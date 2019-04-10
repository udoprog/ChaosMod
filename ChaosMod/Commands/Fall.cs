using System;
using System.Collections.Generic;
using GTA;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Cause the character to fall.
	/// </summary>
	public class Fall : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			Ped player = Game.Player.Character;

			player.Euphoria.ApplyImpulse.ResetArguments();
			player.Euphoria.ApplyImpulse.Impulse = player.ForwardVector * 500;
			player.Euphoria.ApplyImpulse.Start(2000);

			mod.ShowText($"{from} caused you to fall!");
		}
	}
}
