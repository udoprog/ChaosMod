using System;
using GTA;
using GTA.Native;

namespace ChaosMod
{
	public interface IKeyDown
	{
		/// <summary>
		/// Handle key down event.
		///
		/// If it returns true handle will be removed.
		/// </summary>
		bool KeyDown();
	}
}
