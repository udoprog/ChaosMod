using System;
using System.Collections.Generic;
using GTA;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Upgrade the current vehicle.
	/// </summary>
	public class ModVehicle : Command
	{
		public static VehicleModType[] ALL_MOD_TYPES = (VehicleModType[])Enum.GetValues(typeof(VehicleModType));

		enum ModId
		{
			Random,
			LowTier,
			MidTier,
			HighTier,
		}

		/// <summary>
		/// Parse a modification ID.
		/// </summary>
		private ModId ParseModId(string id)
		{
			switch (id)
			{
				case "random":
					return ModId.Random;
				case "low-tier":
					return ModId.LowTier;
				case "mid-tier":
					return ModId.MidTier;
				case "high-tier":
					return ModId.HighTier;
				default:
					throw new Exception($"bad mod id: {id}");
			}
		}

		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			var stringId = rest.GetEnumerator().NextOrDefault("random");
			var id = ParseModId(stringId);

			var player = Game.Player.Character;

			if (player == null)
			{
				mod.ShowText($"{from} tried to open your parachute!");
				return;
			}

			var vehicle = player.CurrentVehicle;

			if (vehicle == null)
			{
				mod.ShowText($"{from} tried to upgrade your vehicle!");
				return;
			}

			vehicle.Mods.InstallModKit();

			vehicle.RandomizeColors(mod.Rnd);
			vehicle.RandomizeLightsOn(mod.Rnd);

			var turbo = vehicle.Mods[VehicleToggleModType.XenonHeadlights];

			vehicle.Mods[VehicleToggleModType.XenonHeadlights].IsInstalled = mod.Rnd.NextBoolean();
			vehicle.Mods[VehicleToggleModType.TireSmoke].IsInstalled = mod.Rnd.NextBoolean();

			switch (id)
			{
				case ModId.MidTier:
					// 50% chance of getting a turbo.
					turbo.IsInstalled = mod.Rnd.NextBoolean();
					break;
				case ModId.HighTier:
					// 75% chance of getting a turbo.
					turbo.IsInstalled = mod.Rnd.Next(0, 4) != 0;
					break;
				default:
					turbo.IsInstalled = false;
					break;
			}

			foreach (var m in ALL_MOD_TYPES)
			{
				VehicleMod modType = vehicle.Mods[m];
				var count = modType.ModCount;

				if (count == 0)
				{
					continue;
				}

				var bottom = 0;
				var ceiling = count + 1;

				switch (id)
				{
					case ModId.Random:
						break;
					case ModId.LowTier:
						bottom = 0;
						ceiling = Math.Max(1, count / 3);
						break;
					case ModId.MidTier:
						bottom = count / 3;
						ceiling = Math.Max(bottom + 1, 2 * (count / 3));
						break;
					case ModId.HighTier:
						bottom = Math.Min(2 * (count / 3), count - 1);
						ceiling = count;
						break;
				}

				var index = mod.Rnd.Next(bottom, ceiling);
				modType.Index = index;
			}

			mod.ShowText($"{from} modded your vehicle ({id})!");
		}
	}
}
