using System;
using GTA;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Put the player on fire.
	/// </summary>
	public class SetOnFire : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			Ped player = Game.Player.Character;

			if (player == null)
			{
				mod.ShowText($"{from} tried to put you on fire!");
				return;
			}

			var timer = mod.Timer("On Fire", 5f);

			if (mod.AddUniqueTicker(TickerId.SetOnFire, new SetOnFireTicker(player, timer)))
			{
				player.StartEntityFire();
			}

			mod.ShowText($"{from} put you on fire!");
		}
	}

	class SetOnFireTicker : ITicker
	{
		/// <summary>
		/// Player being set on fire.
		/// </summary>
		Ped player;
		/// <summary>
		/// The timer to tick.
		/// </summary>
		Timer timer;

		public SetOnFireTicker(Ped player, Timer timer)
		{
			this.player = player;
			this.timer = timer;
		}

		public override void Stop()
		{
			timer.Stop();
		}

		public override bool Tick()
		{
			if (timer.Tick())
			{
				player.StopEntityFire();
				return true;
			}

			return false;
		}
	}
}
