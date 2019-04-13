using System;
using GTA;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Set all nearby peds on fire.
	/// </summary>
	public class SetPedsOnFire : Command
	{
		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			Ped player = Game.Player.Character;

			if (player == null)
			{
				mod.ShowText($"{from} tried to put EVERYONE on fire!");
				return;
			}

			var peds = World.GetNearbyPeds(player.Position, 1000f);

			var scriptFires = new List<ScriptFire>();

			foreach (var ped in peds)
			{
				if (ped.IsHuman && !ped.IsOnFire && ped.IsAlive && ped != player)
				{
					ped.Stumble(1000);
					var id = WorldExtension.StartScriptFire(ped.Position, 5, true);
					scriptFires.Add(new ScriptFire(id, ped));
				}
			}

			if (scriptFires.Count == 0)
			{
				mod.ShowText($"{from} tried to put pedestrians on fire but there are none :(");
				return;
			}

			mod.AddTicker(new SetPedsOnFireTicker(scriptFires, 10f));
			mod.ShowText($"{from} set {scriptFires.Count} pedestrians on fire!");
		}

		class ScriptFire
		{
			private int handle;
			private Ped ped;
			private bool removed;

			public ScriptFire(int handle, Ped ped)
			{
				this.handle = handle;
				this.ped = ped;
				this.removed = false;
			}

			/// <summary>
			/// Move the script fire to the pedestrian.
			/// </summary>
			public void MoveToPed()
			{
				if (removed)
				{
					return;
				}

				if (ped.IsOnFire || !ped.IsAlive)
				{
					this.Remove();
					return;
				}

				ped.Stumble(1000);
				WorldExtension.RemoveScriptFire(this.handle);
				this.handle = WorldExtension.StartScriptFire(ped.Position, 5, true);
			}

			/// <summary>
			/// Remove the script fire.
			/// </summary>
			public void Remove()
			{
				if (removed)
				{
					return;
				}

				WorldExtension.RemoveScriptFire(this.handle);
				removed = true;
			}
		}

		class SetPedsOnFireTicker : ITicker
		{
			/// <summary>
			/// Pedestrians on fire.
			/// </summary>
			List<ScriptFire> scriptFires;
			/// <summary>
			/// The timer to tick.
			/// </summary>
			float timer;
			int lastSecond;

			public SetPedsOnFireTicker(List<ScriptFire> scriptFires, float timer)
			{
				this.scriptFires = scriptFires;
				this.timer = timer;
				this.lastSecond = (int)timer;
			}

			public bool Tick()
			{
				timer -= Game.LastFrameTime;

				if (timer > 0)
				{
					if ((int)timer != lastSecond)
					{
						lastSecond = (int)timer;

						foreach (var fire in scriptFires)
						{
							fire.MoveToPed();
						}
					}

					return false;
				}

				foreach (var fire in scriptFires)
				{
					fire.Remove();
				}

				return true;
			}

			public String What()
			{
				return "putting pedestrians on fire";
			}
		}
	}
}
