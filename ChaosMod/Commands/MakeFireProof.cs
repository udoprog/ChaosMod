using System;
using GTA;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Make the player fire proof.
	/// </summary>
	public class MakeFireProof : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			Ped player = Game.Player.Character;

			if (player == null)
			{
				mod.ShowText($"{from} tried to make you fire proof!");
				return;
			}

			if (mod.AddUniqueTicker(TickerId.MakeFireProof, new FireProofTicker(player, 10f)))
			{
				player.IsFireProof = true;
			}

			mod.ShowText($"{from} made you fire proof!");
		}
	}

	class FireProofTicker : ITicker
	{
		/// <summary>
		/// Player being boosted.
		/// </summary>
		Ped player;
		/// <summary>
		/// The timer to tick.
		/// </summary>
		float timer;

		public FireProofTicker(Ped player, float timer)
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

			this.player.IsFireProof = false;
			return true;
		}

		public String What()
		{
			return "Fire proof";
		}
	}
}
