using System;
using GTA;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Make the player drunk.
	/// 
	/// EXPERIMENTAL - currently does not work.
	/// </summary>
	public class Drunk : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			var player = Game.Player.Character;

			if (player == null)
			{
				return;
			}

			if (!WorldExtension.HasAnimationSetLoaded("move_m@drunk@verydrunk"))
			{
				WorldExtension.RequestAnimationSet("move_m@drunk@verydrunk");
			}

			player.SetPedMovementClipset("move_m@drunk@verydrunk");
			mod.AddTicker(new DrunkTicker(10, player));
			mod.ShowText($"{from} made you drunk!");
		}
	}

	class DrunkTicker : ITicker
	{
		private float timer;
		private Ped ped;

		public DrunkTicker(float timer, Ped ped)
		{
			this.timer = timer;
			this.ped = ped;
			this.ped.SetPedIsDrunk(true);
		}

		public bool Tick()
		{
			var delta = Game.LastFrameTime;
			timer -= delta;

			if (timer <= 0)
			{
				return true;
			}

			this.ped.ResetPedMovementClipset();
			this.ped.SetPedIsDrunk(false);
			return false;
		}

		public String What()
		{
			return "Drunkenness";
		}
	}
}
