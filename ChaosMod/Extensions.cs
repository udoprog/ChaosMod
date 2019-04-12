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
	}
}
