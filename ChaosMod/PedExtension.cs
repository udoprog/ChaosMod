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
	}
}
