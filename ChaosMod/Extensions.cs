using System;
using GTA;
using GTA.Native;

namespace ChaosMod
{
	public enum ParachuteState
	{
		None = -1,
		FreeFalling,
		Deploying,
		Gliding,
		LandingOrFallingToDoom,
	}

	public static class WorldExtension
	{
		/// <summary>
		/// Test if the given animation set is loaded for the character.
		/// </summary>
		public static bool HasAnimationSetLoaded(string set)
		{
			return Function.Call<bool>(Hash.HAS_ANIM_SET_LOADED, set);
		}

		/// <summary>
		/// Request the given animation set to be loaded.
		/// </summary>
		public static void RequestAnimationSet(string set)
		{
			Function.Call(Hash.REQUEST_ANIM_SET, set);
		}

		/// <summary>
		/// Start a script fire.
		/// </summary>
		public static int StartScriptFire(GTA.Math.Vector3 pos, int maxChildren, bool isGasFire)
		{
			return Function.Call<int>(Hash.START_SCRIPT_FIRE, pos.X, pos.Y, pos.Z, maxChildren, isGasFire);
		}

		/// <summary>
		/// Suppress shocking events next frame.
		/// </summary>
		public static void SuppressShockingEventsNextFrame()
		{
			Function.Call(Hash.SUPPRESS_SHOCKING_EVENTS_NEXT_FRAME);
		}

		/// <summary>
		/// Remove the scripted fire.
		/// </summary>
		public static void RemoveScriptFire(int handle)
		{
			Function.Call(Hash.REMOVE_SCRIPT_FIRE, handle);
		}
	}

	public static class PedExtension
	{
		/// <summary>
		/// Get the parachute state of a ped.
		/// </summary>
		public static ParachuteState GetParachuteState(this Ped ped)
		{
			var value = Function.Call<int>(Hash.GET_PED_PARACHUTE_STATE, ped.Handle);
			return (ParachuteState)value;
		}

		/// <summary>
		/// Set the parachute state of a ped.
		/// </summary>
		public static void SetParachuteState(this Ped ped, ParachuteState state)
		{
			Function.Call<int>(Hash.SET_PARACHUTE_TASK_TARGET, ped.Handle, (int)state);
		}

		/// <summary>
		/// Make the pedestrian drunk.
		/// </summary>
		public static void SetPedIsDrunk(this Ped ped, bool drunk)
		{
			Function.Call<int>(Hash.SET_PED_IS_DRUNK, ped.Handle, drunk);
		}

		/// <summary>
		/// Set which clipset to use when moving.
		/// </summary>
		public static void SetPedMovementClipset(this Ped ped, string set)
		{
			Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, ped.Handle, set, 1f);
		}

		/// <summary>
		/// Clear the current clipset.
		/// </summary>
		public static void ResetPedMovementClipset(this Ped ped)
		{
			Function.Call(Hash.RESET_PED_MOVEMENT_CLIPSET, ped.Handle, 1f);
		}

		/// <summary>
		/// Set the given ped on fire.
		/// 
		/// NOTE: This doesn't work very well.
		/// It seems like the game has a fixed limit on how many fires it can run at any given time, and there's no consistent way to clean it up.
		/// Even StopEntityFire doesn't help.
		/// </summary>
		public static Ped StartEntityFire(this Ped ped)
		{
			var handle = Function.Call<int>(Hash.START_ENTITY_FIRE, ped.Handle);
			return new Ped(handle);
		}

		/// <summary>
		/// Put out the fire on the current player.
		/// </summary>
		public static void StopEntityFire(this Ped ped)
		{
			Function.Call(Hash.STOP_ENTITY_FIRE, ped.Handle);
		}

		/// <summary>
		/// Make the ped stumble.
		/// </summary>
		public static void Stumble(this Ped ped, int durationMs)
		{
			ped.Euphoria.ArmsWindmillAdaptive.ResetArguments();

			ped.Euphoria.ArmsWindmillAdaptive.AngSpeed = 6f;
			ped.Euphoria.ArmsWindmillAdaptive.Amplitude = 1.5f;
			ped.Euphoria.ArmsWindmillAdaptive.LeftElbowAngle = 0.1f;
			ped.Euphoria.ArmsWindmillAdaptive.RightElbowAngle = 0.1f;
			ped.Euphoria.ArmsWindmillAdaptive.DisableOnImpact = true;
			ped.Euphoria.ArmsWindmillAdaptive.BendLeftElbow = true;
			ped.Euphoria.ArmsWindmillAdaptive.BendRightElbow = true;

			ped.Euphoria.ArmsWindmillAdaptive.Start(durationMs);

			ped.Euphoria.ApplyImpulse.ResetArguments();
			ped.Euphoria.ApplyImpulse.Impulse = ped.ForwardVector * -100;
			ped.Euphoria.ApplyImpulse.Start(durationMs);

			ped.Euphoria.BodyBalance.Start();
		}

		/// <summary>
		/// Set flee attributes.
		/// </summary>
		public static void SetFleeAttributes(this Ped ped, int attributes, bool p2)
		{
			Function.Call(Hash.SET_PED_FLEE_ATTRIBUTES, ped.Handle, attributes, p2);
		}

		/// <summary>
		/// Set combat attributes.
		/// </summary>
		public static void SetCombatAttributes(this Ped ped, CombatAttributes attributes, bool enabled)
		{
			Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped.Handle, (int) attributes, enabled);
		}

		/// <summary>
		/// Set the ped as falling from a great height.
		/// </summary>
		public static void SetHighFallTask(this Ped ped)
		{
			Function.Call(Hash.SET_HIGH_FALL_TASK, ped.Handle, 0, 0, 0);
		}

		/// <summary>
		/// Set the ped as falling from a great height.
		/// </summary>
		public static void SetPedToRagdoll(this Ped ped, int timeMillis, int standupMillis, int ragdollType)
		{
			Function.Call(Hash.SET_PED_TO_RAGDOLL, ped.Handle, timeMillis, standupMillis, ragdollType, false, false, false);
		}
	}

	public static class VehicleExtension
	{
		public static VehicleNeonLight[] NEON_LIGHTS = (VehicleNeonLight[])Enum.GetValues(typeof(VehicleNeonLight));
		public static VehicleColor[] VEHICLE_COLORS = (VehicleColor[])Enum.GetValues(typeof(VehicleColor));

		/// <summary>
		/// Randomize lights on vehicle.
		/// </summary>
		public static void RandomizeLightsOn(this Vehicle vehicle, Random rnd)
		{
			foreach (var neon in NEON_LIGHTS)
			{
				var on = false;

				if (rnd.Next(0, 2) == 1)
				{
					on = true;
				}

				vehicle.SetNeonLightsOn(neon, on);
			}
		}

		/// <summary>
		/// Randomize all vehicle colors.
		/// </summary>
		public static void RandomizeColors(this Vehicle vehicle, Random rnd)
		{
			if (rnd.NextBoolean())
			{
				vehicle.CustomPrimaryColor = rnd.NextColor();
			}
			else
			{
				vehicle.ClearCustomPrimaryColor();
				vehicle.PrimaryColor = VEHICLE_COLORS[rnd.Next(0, VEHICLE_COLORS.Length)];
			}

			if (rnd.NextBoolean())
			{
				vehicle.CustomSecondaryColor = rnd.NextColor();
			} else
			{
				vehicle.ClearCustomSecondaryColor();
				vehicle.SecondaryColor = VEHICLE_COLORS[rnd.Next(0, VEHICLE_COLORS.Length)];
			}

			vehicle.NeonLightsColor = rnd.NextColor();
			vehicle.TireSmokeColor = rnd.NextColor();
			vehicle.RimColor = VEHICLE_COLORS[rnd.Next(0, VEHICLE_COLORS.Length)];
			// this causes the game to crash.
			// vehicle.TrimColor = VEHICLE_COLORS[rnd.Next(0, VEHICLE_COLORS.Length)];
			vehicle.PearlescentColor = VEHICLE_COLORS[rnd.Next(0, VEHICLE_COLORS.Length)];
		}
	}

	public static class RandomExtension
	{
		/// <summary>
		/// Get next random boolean.
		/// </summary>
		public static bool NextBoolean(this Random rnd)
		{
			return rnd.Next(0, 2) == 0;
		}

		/// <summary>
		/// Generate a random color.
		/// </summary>
		public static System.Drawing.Color NextColor(this Random rnd)
		{
			var r = rnd.Next(0, 256);
			var g = rnd.Next(0, 256);
			var b = rnd.Next(0, 256);
			return System.Drawing.Color.FromArgb(r, g, b);
		}

		/// <summary>
		/// Generate a random color.
		/// </summary>
		public static GTA.Math.Vector3 RandomVector3(this Random rnd, float magnitude)
		{
			var x = (float)rnd.NextDouble() - 0.5f;
			var y = (float)rnd.NextDouble() - 0.5f;
			var z = (float)rnd.NextDouble() - 0.5f;
			return new GTA.Math.Vector3(x, y, z).Normalized * magnitude;
		}
	}

	/// <summary>
	/// Combat attributes that can be set.
	/// </summary>
	public enum CombatAttributes
	{
		CanUseCover = 0,
		CanUseVehicles = 1,
		CanDoDrivebys = 2,
		CanLeaveVehicle = 3,
		CanFightArmedPedsWhenNotArmed = 5,
		CanTauntInVehicle = 20,
		AlwaysFight = 46,
		IgnoreTrafficWhenDriving = 52,
		FreezeMovement = 292,
		PlayerCanUseFiringWeapons = 1424
	};
}
