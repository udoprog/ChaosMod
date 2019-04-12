using System;
using GTA;
using System.Collections.Generic;

namespace ChaosMod.Commands
{
	/// <summary>
	/// Freeze the player's camera.
	/// 
	/// EXPERIMENTAL - currently does not work.
	/// </summary>
	public class CameraFreeze : Command
	{
		public bool AlreadyStuck
		{
			get;
			set;
		}

		public void Handle(Chaos mod, String from, IEnumerable<String> rest)
		{
			var player = Game.Player.Character;

			if (AlreadyStuck)
			{
				mod.ShowText($"{from} tried to freeze your camera!");
				return;
			}

			if (player == null)
			{
				return;
			}

			if (!WorldExtension.HasAnimationSetLoaded("move_m@drunk@verydrunk"))
			{
				WorldExtension.RequestAnimationSet("move_m@drunk@verydrunk");
			}

			var current = World.RenderingCamera;
			var cam = World.CreateCamera(current.Position, current.Rotation, current.FieldOfView);
			cam.FarClip = current.FarClip;
			cam.FarDepthOfField = current.FarDepthOfField;
			cam.IsActive = true;
			current.IsActive = false;
			World.RenderingCamera = cam;
			mod.AddTicker(new CameraUnfreezeTicker(10, this, current));
			mod.ShowText($"{from} froze your camera!");
		}
	}

	/// <summary>
	/// Unfreeze the camera after the given duration is over.
	/// </summary>
	class CameraUnfreezeTicker : ITicker
	{
		private float timer;
		private CameraFreeze parent;
		private Camera camera;

		public CameraUnfreezeTicker(float timer, CameraFreeze parent, Camera camera)
		{
			this.timer = timer;
			this.parent = parent;
			this.camera = camera;
		}

		public bool Tick()
		{
			var delta = Game.LastFrameTime;
			timer -= delta;

			if (timer <= 0)
			{
				return true;
			}

			var current = World.RenderingCamera;
			current.IsActive = false;
			this.camera.IsActive = true;
			World.RenderingCamera = this.camera;
			parent.AlreadyStuck = false;
			return false;
		}

		public String What()
		{
			return "Frozen Camera";
		}
	}
}
