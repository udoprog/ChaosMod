using System;
using GTA;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Set random weather.
	/// </summary>
	public class RandomizeWeather : Command
	{
		static Weather[] ALL_WEATHERS = (Weather[])Enum.GetValues(typeof(Weather));

		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			var hours = mod.Rnd.Next(0, 23);
			var minutes = mod.Rnd.Next(0, 59);
			var seconds = mod.Rnd.Next(0, 59);
			TimeSpan currentDayTime = new TimeSpan(hours, minutes, seconds);
			World.CurrentTimeOfDay = currentDayTime;
			World.Weather = ALL_WEATHERS[mod.Rnd.Next(0, ALL_WEATHERS.Length)];

			mod.ShowText($"{from} scrambled the current weather!");
		}
	}
}
