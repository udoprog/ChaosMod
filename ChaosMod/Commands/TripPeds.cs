using System;
using GTA;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	public class TripPeds : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			mod.ShowText("Tripping everyone");
			Ped player = Game.Player.Character;


			var peds = World.GetNearbyPeds(player.Position, 1000f);

			foreach (var ped in peds)
			{
				if (ped.IsHuman && ped.IsAlive && ped != player)
				{
					ped.Task.ClearAll();
					ped.SetPedToRagdoll(1000, 1500, 0);

				}
			}
		}
	}
}