using System;
using GTA;
using GTA.Native;

namespace ChaosMod
{
	/// <summary>
	/// Identifier for a unique ticker.
	/// </summary>
	public enum TickerId
	{
		SuppressShocking,
		SetOnFire,
		MakeFireProof,
		SuperSpeed,
		SuperJump,
		Invincibility,
		ExplodingPunches,
		ExplodingBullets,
	}

	public interface ITicker
	{
		bool Tick();

		/// <summary>
		/// What does the ticker do.
		/// </summary>
		String What();
	}

	class SuppressShockingTicker : ITicker
	{
		/// <summary>
		/// Time the vehicle is being boosted for.
		/// </summary>
		float timer;

		public SuppressShockingTicker(float timer)
		{
			this.timer = timer;
		}

		public bool Tick()
		{
			timer -= Game.LastFrameTime;

			if (timer > 0)
			{
				return false;
			}

			WorldExtension.SuppressShockingEventsNextFrame();
			return true;
		}

		public String What()
		{
			return null;
		}
	}
}
