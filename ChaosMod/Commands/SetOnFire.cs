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

			if (mod.AddUniqueTicker(TickerId.SetOnFire, new SetOnFireTicker(player, 5f)))
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
		float timer;

		public SetOnFireTicker(Ped player, float timer)
		{
			this.player = player;
			this.timer = timer;
		}

		public bool Tick()
		{
			timer -= Game.LastFrameTime;

			if (timer > 0)
			{
				return false;
			}

			player.StopEntityFire();
			return true;
		}

		public String What()
		{
			return null;
		}
	}
}
