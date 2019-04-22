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

			var duration = rest.GetEnumerator().NextFloatOrDefault(30f);
			var timer = mod.Timer("Fireproof", duration);

			if (mod.AddUniqueTicker(TickerId.MakeFireProof, new FireProofTicker(player, timer)))
			{
				player.IsFireProof = true;
			}

			mod.ShowText($"{from} made you fire proof for {duration} seconds!");
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
		Timer timer;

		public FireProofTicker(Ped player, Timer timer)
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
			if (!timer.Tick())
			{
				return false;
			}

			this.player.IsFireProof = false;
			return true;
		}
	}
}
