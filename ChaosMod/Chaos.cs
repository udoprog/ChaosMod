using System;
using System.Drawing;
using GTA;
using GTA.Native;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NativeUI;

namespace ChaosMod
{
	public class Chaos : Script
	{
		private const int bufSize = 8 * 1024;
		private State state;
		private EndPoint epFrom = new IPEndPoint(IPAddress.Any, 0);
		private AsyncCallback recv = null;
		/// <summary>
		/// Timers currently running.
		/// </summary>
		private LinkedList<ITicker> tickers = new LinkedList<ITicker>();
		private LinkedList<IKeyDown> keyDowns = new LinkedList<IKeyDown>();
		private Dictionary<TickerId, ITicker> uniqueTickers = new Dictionary<TickerId, ITicker>();

		public static WeaponHash[] ALL_WEAPONS = (WeaponHash[])Enum.GetValues(typeof(WeaponHash));
		public static List<VehicleHash> ALL_VEHICLES = AllVehicles();
		public static Dictionary<String, VehicleHash> ALL_VEHICLES_BY_ID = AllVehiclesById();
		public static Dictionary<String, WeaponHash> ALL_WEAPONS_BY_ID = AllWeaponsById();

		static List<VehicleHash[]> SLOW_CARS = SlowCars();
		static List<VehicleHash[]> NORMAL_CARS = NormalCars();
		static List<VehicleHash[]> FAST_CARS = FastCars();
		static List<VehicleHash[]> BIKES = Bikes();
		static List<VehicleHash[]> PEDAL_BIKES = PedalBikes();
		static Dictionary<String, Command> COMMANDS = Commands();

		/// <summary>
		/// Currently active timers.
		/// </summary>
		private TimerBarPool timers;
		/// <summary>
		/// Menu to manually trigger commands.
		/// </summary>
		private UIMenu menu;

		/// <summary>
		/// Hate group for the current player.
		/// </summary>
		public HateGroup HateGroup
		{
			get;
		}

		/// <summary>
		/// Access random number generator.
		/// </summary>
		public Random Rnd
		{
			get;
		}

		public Chaos()
		{
			timers = new TimerBarPool();
			menu = BuildMenu();

			Tick += (o, e) => timers.Draw();
			Tick += (o, e) => menu.Draw();
			Tick += OnTick;
			KeyDown += OnKeyDown;

			var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
			socket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7291));

			// Note: this is the only way to make sure we close the fricking socket.
			Aborted += (o, e) =>
			{
				socket.Close();
			};

			state = new State();
			Receive(socket);

			Rnd = new Random();
			HateGroup = new HateGroup();

			ShowText("ChaosMod Reloaded");
		}

		static UIMenu BuildMenu()
		{
			var menu = new UIMenu("ChaosMod", "~b~Trigger Commands");

			var items = new List<String>();

			foreach (var pair in COMMANDS)
			{
				items.Add(pair.Key);
			}

			items.Sort();

			foreach (var item in items)
			{
				menu.AddItem(new UIMenuItem(item));
			}

			return menu;
		}

		/// <summary>
		/// Commands to set up.
		/// </summary>
		/// <returns></returns>
		static Dictionary<String, Command> Commands()
		{
			var d = new Dictionary<String, Command>();
			d.Add("kill", new Commands.Kill());
			d.Add("give-weapon", new Commands.GiveWeapon());
			d.Add("wanted", new Commands.Wanted());
			d.Add("randomize-weather", new Commands.RandomizeWeather());
			d.Add("randomize-color", new Commands.RandomizeColor());
			d.Add("randomize-character", new Commands.RandomizeCharacter());
			d.Add("license", new Commands.License());
			d.Add("invincibility", new Commands.Invincibility());
			d.Add("stumble", new Commands.Stumble());
			d.Add("take-health", new Commands.TakeHealth());
			d.Add("spawn-enemy", new Commands.SpawnEnemy());
			d.Add("boost", new Commands.Boost(false));
			d.Add("super-boost", new Commands.Boost(true));
			d.Add("super-speed", new Commands.SuperSpeed());
			d.Add("super-jump", new Commands.SuperJump());
			d.Add("super-swim", new Commands.SuperSwim());
			d.Add("fall", new Commands.Fall());
			d.Add("spawn-vehicle", new Commands.SpawnVehicle());
			d.Add("blow-tires", new Commands.BlowTires());
			d.Add("kill-engine", new Commands.KillEngine());
			d.Add("repair", new Commands.Repair());
			d.Add("brake", new Commands.Brake());
			d.Add("take-weapon", new Commands.TakeWeapon());
			d.Add("take-ammo", new Commands.TakeAmmo());
			d.Add("give-ammo", new Commands.GiveAmmo());
			d.Add("give-armor", new Commands.GiveArmor());
			d.Add("take-all-weapons", new Commands.TakeAllWeapons());
			d.Add("give-health", new Commands.GiveHealth());
			d.Add("exploding-bullets", new Commands.ExplodingBullets());
			d.Add("exploding-punches", new Commands.ExplodingPunches());
			d.Add("drunk", new Commands.Drunk(false));
			d.Add("very-drunk", new Commands.Drunk(true));
			d.Add("set-on-fire", new Commands.SetOnFire());
			d.Add("set-peds-on-fire", new Commands.SetPedsOnFire());
			d.Add("make-fire-proof", new Commands.MakeFireProof());
			d.Add("make-peds-aggressive", new Commands.MakePedsAggressive());
			d.Add("matrix-slam", new Commands.MatrixSlam());
			d.Add("disable-control", new Commands.DisableControl());
			d.Add("close-parachute", new Commands.CloseParachute());
			d.Add("mod-vehicle", new Commands.ModVehicle());
			d.Add("levitate", new Commands.Levitate());
			d.Add("eject", new Commands.Eject());
			d.Add("slow-down-time", new Commands.SlowDownTime());
			d.Add("levitate-entities", new Commands.LevitateEntities());
			d.Add("fire-ammo", new Commands.FireAmmo());
			d.Add("fuel-leakage", new Commands.FuelLeakage());
			d.Add("superman", new Commands.Superman());
			d.Add("change-current-vehicle", new Commands.ChangeCurrentVehicle());
			d.Add("randomize-doors", new Commands.RandomizeDoors());
			d.Add("skyfall", new Commands.Skyfall());
			d.Add("taze", new Commands.Taze());
			d.Add("taze-others", new Commands.TazeOthers());
			d.Add("reduce-gravity", new Commands.ReduceGravity());
			return d;
		}

		/// <summary>
		/// Handle key presses.
		/// </summary>
		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			HandleKeyDowns();

			if (e.KeyCode == Keys.H)
			{
				var selection = menu.MenuItems[menu.CurrentSelection];

				Command command = null;

				if (COMMANDS.TryGetValue(selection.Text, out command))
				{
					var args = new List<String>();
					COMMANDS[selection.Text].Handle(this, "tester", args);
				}

				menu.Visible = false;
			}

			if (e.KeyCode == Keys.J)
			{
				if (!menu.Visible)
				{
					menu.Visible = true;
				}

				menu.GoDownOverflow();
			}

			if (e.KeyCode == Keys.K)
			{
				if (!menu.Visible)
				{
					menu.Visible = true;
				}

				menu.GoUpOverflow();
			}

			if (menu.Visible)
			{
				if (e.KeyCode == Keys.Back)
				{
					menu.Visible = false;
				}
			}
		}

		/// <summary>
		/// Function to call every tick.
		/// </summary>
		private void OnTick(object sender, EventArgs e)
		{
			HandleTickers();
			HandleUniqueTickers();
			CheckQueue();
		}

		void Receive(Socket socket)
		{
			socket.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv = (ar) =>
			{
				State so = (State)ar.AsyncState;
				int bytes = 0;

				try
				{
					bytes = socket.EndReceiveFrom(ar, ref epFrom);
				} catch(ObjectDisposedException)
				{
					return;
				}

				socket.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv, so);
				var s = Encoding.ASCII.GetString(so.buffer, 0, bytes);
				state.queue.Enqueue(s);
			}, state);
		}

		public void ShowText(String text)
		{
			GTA.UI.Notification.Show(text);
		}

		/// <summary>
		/// Show the given text for the given amount of time.
		/// </summary>
		public void ShowText(String text, float timer)
		{
			GTA.UI.Notification.Show(text);
		}

		private void CheckQueue()
		{
			if (state.queue.IsEmpty)
			{
				return;
			}

			string line;
			while (state.queue.TryDequeue(out line))
			{
				var parts = line.Trim().Split(' ');

				if (parts.Length < 3)
				{
					GTA.UI.Notification.Show($"received bad command");
					return;
				}

				var from = parts[0];
				var id = parts[1];
				var name = parts[2];

				Call(from, id, name, parts.Skip(3));
			}
		}

		/// <summary>
		/// Call the given command programmatically.
		/// </summary>
		public void Call(String from, String id, String name, IEnumerable<String> rest)
		{
			Command command = null;

			if (!COMMANDS.TryGetValue(name, out command))
			{
				return;
			}

			try
			{
				command.Handle(this, from, rest);
			}
			catch (Exception e)
			{
#if DEBUG
				GTA.UI.Notification.Show($"something went wrong\nuser: {from}\nid: {id}\nname: {name}\nException: {e}");
#else
				GTA.UI.Notification.Show($"something went wrong\nuser: {from}\nid: {id}\nname: {name}");
#endif
			}
		}

		/// <summary>
		/// Setup a new timer.
		/// </summary>
		public PlayerTimer Timer(String what, float duration)
		{
			BarTimerBar bar = new BarTimerBar(what);
			bar.ForegroundColor = Color.White;
			bar.BackgroundColor = Color.DimGray;
			timers.Add(bar);
			return new PlayerTimer(timers, bar, duration);
		}

		/// <summary>
		/// Setup a new timer.
		/// </summary>
		public Gauge Gauge(String what)
		{
			BarTimerBar bar = new BarTimerBar(what);
			timers.Add(bar);
			return new Gauge(timers, bar);
		}

		/// <summary>
		/// Setup a new timer.
		/// </summary>
		public RandomTimer RandomTimer(float initial, float magnitude)
		{
			return new RandomTimer(Rnd, initial, magnitude);
		}

		/// <summary>
		/// Setup a new timer.
		/// </summary>
		public AnonymousTimer AnonymousTimer(float duration)
		{
			return new AnonymousTimer(timers, 0f, duration);
		}

		/// <summary>
		/// Setup a new timer.
		/// </summary>
		public AnonymousTimer AnonymousTimer(float initial, float duration)
		{
			return new AnonymousTimer(timers, initial, duration);
		}

		/// <summary>
		/// Add a thing that handles ticks every frame.
		/// </summary>
		public void AddTicker(ITicker ticker)
		{
			this.tickers.AddLast(ticker);
		}

		/// <summary>
		/// Add a thing that handles ticks every frame.
		/// </summary>
		public void AddKeyDown(IKeyDown keyDown)
		{
			this.keyDowns.AddLast(keyDown);
		}

		/// <summary>
		/// Check if the given ticker is active.
		/// </summary>
		public bool HasTicker(TickerId id)
		{
			return uniqueTickers.ContainsKey(id);
		}

		/// <summary>
		/// Add a ticker which there can only exist one of.
		/// 
		/// If the ticker exists, replaces it and returns false.
		/// Otherwise returns true.
		/// </summary>
		public bool AddUniqueTicker(TickerId id, ITicker ticker)
		{
			ITicker value = null;
			var added = true;

			if (uniqueTickers.TryGetValue(id, out value))
			{
				value.Stop();
				added = false;
			}

			uniqueTickers[id] = ticker;
			return added;
		}

		/// <summary>
		/// Handle calling tick on all spawned tickers.
		/// </summary>
		private void HandleTickers()
		{
			var current = tickers.First;

			while (current != null)
			{
				if (current.Value.Tick())
				{
					var prev = current;
					current = current.Next;
					prev.Value.Stop();
					tickers.Remove(prev);
					continue;
				}

				current = current.Next;
			}
		}

		/// <summary>
		/// Handle calling tick on all spawned tickers.
		/// </summary>
		private void HandleKeyDowns()
		{
			var current = keyDowns.First;

			while (current != null)
			{
				if (current.Value.KeyDown())
				{
					var prev = current;
					current = current.Next;
					keyDowns.Remove(prev);
					continue;
				}

				current = current.Next;
			}
		}

		/// <summary>
		/// Handle calling tick on all unique tickers.
		/// </summary>
		private void HandleUniqueTickers()
		{
			var removed = new List<KeyValuePair<TickerId, ITicker>>();

			foreach (var pair in uniqueTickers)
			{
				if (!pair.Value.Tick())
				{
					continue;
				}

				removed.Add(pair);
			}

			// remove all keys that have expired.
			foreach (var pair in removed)
			{
				uniqueTickers.Remove(pair.Key);
				pair.Value.Stop();
			}
		}

		/// <summary>
		/// Randomize the appearance of the current character.
		/// </summary>
		void RandomizeVehicle(String from)
		{
			Ped player = Game.Player.Character;

			if (player == null)
			{
				return;
			}

			// TODO
		}

		/// <summary>
		/// Give health.
		/// </summary>
		void GiveHealth()
		{
			Ped player = Game.Player.Character;

			if (player == null)
			{
				return;
			}

			player.Health = 100;
		}

		/// <summary>
		/// Take all health from the character.
		/// </summary>
		public void TakeHealth()
		{
			Ped player = Game.Player.Character;

			if (player == null)
			{
				return;
			}

			if (player.Health > 10)
			{
				player.Health = 10;
			}

			if (player.Armor > 10)
			{
				player.Armor = 10;
			}
		}

		/// <summary>
		/// Pick a vehicle based on an ID.
		/// </summary>
		public VehicleHash? PickVehicle(string vehicle)
		{
			switch (vehicle)
			{
				case "fighter-jet":
					return FIGHTER_JETS[Rnd.Next(0, FIGHTER_JETS.Length)];
				case "slow":
					return PickRandomCar(SLOW_CARS);
				case "normal":
					return PickRandomCar(NORMAL_CARS);
				case "fast":
					return PickRandomCar(FAST_CARS);
				case "bike":
					return PickRandomCar(BIKES);
				case "pedalbike":
					return PickRandomCar(PEDAL_BIKES);
				case "blimp":
					return BLIMPS[Rnd.Next(0, BLIMPS.Length)];
				case "jet-ski":
					return JET_SKIS[Rnd.Next(0, JET_SKIS.Length)];
				case "tank":
					return TANKS[Rnd.Next(0, TANKS.Length)];
				case "sub":
					return SUBMERSIBLE[Rnd.Next(0, SUBMERSIBLE.Length)];
				default:
					VehicleHash byId = VehicleHash.Adder;

					// Match by ID.
					if (ALL_VEHICLES_BY_ID.TryGetValue(vehicle, out byId)) {
						return byId;
					}

					return RandomVehicle();
			}
		}

		/// <summary>
		/// Return a random vehicle.
		/// </summary>
		/// <returns></returns>
		VehicleHash RandomVehicle()
		{
			return ALL_VEHICLES[Rnd.Next(0, ALL_VEHICLES.Count)];
		}

		VehicleHash? PickRandomCar(List<VehicleHash[]> hashes)
		{
			var list = hashes[Rnd.Next(0, hashes.Count)];

			if (list.Length == 0)
			{
				return null;
			}

			return list[Rnd.Next(0, list.Length)];
		}

		/**
		 * Take away current weapon.
		 */
		void TakeWeapon()
		{
			Ped player = Game.Player.Character;
			Weapon weapon = player.Weapons.Current;

			if (weapon != null)
			{
				player.Weapons.Remove(weapon);
			}
			else
			{
				var weapons = PlayerWeapons();

				if (weapons.Count == 0)
				{
					return;
				}

				var take = weapons[Rnd.Next(0, weapons.Count - 1)];
				// take a random weapon.
				player.Weapons.Remove(take);
			}
		}

		public class State
		{
			public byte[] buffer = new byte[bufSize];
			public ConcurrentQueue<string> queue = new ConcurrentQueue<string>();
		}

		/// <summary>
		/// Get a list of all player weapons.
		/// </summary>
		public List<WeaponHash> PlayerWeapons()
		{
			var player = Game.Player.Character;

			List<WeaponHash> list = new List<WeaponHash>();

			if (player == null)
			{
				return list;
			}

			foreach (WeaponHash w in ALL_WEAPONS)
			{
				if (!player.Weapons.HasWeapon(w))
				{
					continue;
				}

				list.Add(w);
			}

			return list;
		}

		static VehicleHash[] BLIMPS = new VehicleHash[]
		{
			VehicleHash.Blimp,
			VehicleHash.Blimp2,
		};

		static VehicleHash[] JET_SKIS = new VehicleHash[]
		{
			VehicleHash.Seashark,
			VehicleHash.Seashark2,
			VehicleHash.Seashark3,
		};

		static VehicleHash[] TANKS = new VehicleHash[]
		{
			VehicleHash.Rhino
		};

		static VehicleHash[] SUBMERSIBLE = new VehicleHash[]
		{
			VehicleHash.Submersible,
			VehicleHash.Submersible2,
		};

		static VehicleHash[] FIGHTER_JETS = new VehicleHash[]
		{
			VehicleHash.Lazer,
		};

		static List<VehicleHash[]> SlowCars()
		{
			List<VehicleHash[]> cars = new List<VehicleHash[]>();
			cars.Add(new VehicleHash[] { VehicleHash.Benson });
			cars.Add(new VehicleHash[] { VehicleHash.Docktug });
			cars.Add(new VehicleHash[] { VehicleHash.UtilityTruck });
			cars.Add(new VehicleHash[] { VehicleHash.Bus });
			cars.Add(new VehicleHash[] { VehicleHash.RentalBus });
			cars.Add(new VehicleHash[] { VehicleHash.PBus });
			cars.Add(new VehicleHash[] { VehicleHash.Cutter });
			cars.Add(new VehicleHash[] { VehicleHash.Bulldozer });
			cars.Add(new VehicleHash[] { VehicleHash.Peyote });
			cars.Add(new VehicleHash[] { VehicleHash.Paradise });
			cars.Add(new VehicleHash[] { VehicleHash.Packer });
			cars.Add(new VehicleHash[] { VehicleHash.FireTruck, });
			return cars;
		}

		static List<VehicleHash[]> NormalCars()
		{
			List<VehicleHash[]> cars = new List<VehicleHash[]>();
			cars.Add(new VehicleHash[] { VehicleHash.Blista, VehicleHash.Blista2, VehicleHash.Blista3, });
			cars.Add(new VehicleHash[] { VehicleHash.Blista, VehicleHash.Blista2, VehicleHash.Blista3, });
			cars.Add(new VehicleHash[] { VehicleHash.Bullet, });
			cars.Add(new VehicleHash[] { VehicleHash.Cavalcade, VehicleHash.Cavalcade2 });
			cars.Add(new VehicleHash[] { VehicleHash.Crusader });
			cars.Add(new VehicleHash[] {
				VehicleHash.Dune,
			});
			cars.Add(new VehicleHash[] {
				VehicleHash.Police,
				VehicleHash.Police2,
				VehicleHash.Police3,
				VehicleHash.Police4,
			});
			cars.Add(new VehicleHash[] {
				VehicleHash.Pigalle,
			});
			cars.Add(new VehicleHash[] {
				VehicleHash.Tornado,
				VehicleHash.Tornado2,
				VehicleHash.Tornado3,
				VehicleHash.Tornado4,
				VehicleHash.Tornado5,
				VehicleHash.Tornado6,
			});
			cars.Add(new VehicleHash[] { VehicleHash.Buffalo, VehicleHash.Buffalo2, VehicleHash.Buffalo3, });
			cars.Add(new VehicleHash[] { VehicleHash.Osiris, });
			cars.Add(new VehicleHash[] { VehicleHash.Gauntlet, });
			return cars;
		}

		static List<VehicleHash[]> FastCars()
		{
			List<VehicleHash[]> cars = new List<VehicleHash[]>();
			cars.Add(new VehicleHash[] { VehicleHash.Infernus, VehicleHash.Infernus2, });
			cars.Add(new VehicleHash[] { VehicleHash.Feltzer2, VehicleHash.Feltzer3 });
			cars.Add(new VehicleHash[] { VehicleHash.Jester, VehicleHash.Jester2, VehicleHash.Jester3, });
			cars.Add(new VehicleHash[] { VehicleHash.Massacro, VehicleHash.Massacro2 });
			cars.Add(new VehicleHash[] { VehicleHash.Ninef, VehicleHash.Ninef2, });
			cars.Add(new VehicleHash[] { VehicleHash.Surano });
			cars.Add(new VehicleHash[] { VehicleHash.Comet2, VehicleHash.Comet3 });
			cars.Add(new VehicleHash[] { VehicleHash.Carbonizzare, });
			cars.Add(new VehicleHash[] { VehicleHash.Banshee, VehicleHash.Banshee2 });
			cars.Add(new VehicleHash[] { VehicleHash.Coquette, VehicleHash.Coquette2, VehicleHash.Coquette3, });
			cars.Add(new VehicleHash[] { VehicleHash.Bullet });
			cars.Add(new VehicleHash[] { VehicleHash.RapidGT, VehicleHash.RapidGT2, VehicleHash.RapidGT3, });
			cars.Add(new VehicleHash[] { VehicleHash.Furoregt });
			cars.Add(new VehicleHash[] {
				VehicleHash.Dominator,
				VehicleHash.Dominator2,
				VehicleHash.Dominator3,
			});

			return cars;
		}

		/// <summary>
		/// Get a list of all vehicles.
		/// </summary>
		static List<VehicleHash> AllVehicles()
		{
			var array = (VehicleHash[])Enum.GetValues(typeof(VehicleHash));
			return array.ToList();
		}

		/// <summary>
		/// Get a dictionary of all vehicles by id.
		/// </summary>
		static Dictionary<String, VehicleHash> AllVehiclesById()
		{
			var array = (VehicleHash[])Enum.GetValues(typeof(VehicleHash));
			var d = new Dictionary<String, VehicleHash>();

			foreach (var v in array)
			{
				d[v.ToString().ToLower()] = v;
			}

			return d;
		}

		/// <summary>
		/// Get a dictionary of all weapons by id.
		/// </summary>
		static Dictionary<String, WeaponHash> AllWeaponsById()
		{
			var array = (WeaponHash[])Enum.GetValues(typeof(WeaponHash));
			var d = new Dictionary<String, WeaponHash>();

			foreach (var v in array)
			{
				d[v.ToString().ToLower()] = v;
			}

			return d;
		}

		static List<VehicleHash[]> Bikes()
		{
			List<VehicleHash[]> bikes = new List<VehicleHash[]>();
			bikes.Add(new VehicleHash[] { VehicleHash.Akuma });
			bikes.Add(new VehicleHash[] { VehicleHash.Bagger });
			bikes.Add(new VehicleHash[] { VehicleHash.Bati, VehicleHash.Bati2 });
			bikes.Add(new VehicleHash[] { VehicleHash.Blazer, VehicleHash.Blazer2, VehicleHash.Blazer3, VehicleHash.Blazer4, VehicleHash.Blazer5, });
			bikes.Add(new VehicleHash[] { VehicleHash.CarbonRS });
			bikes.Add(new VehicleHash[] { VehicleHash.Daemon });
			bikes.Add(new VehicleHash[] { VehicleHash.Double });
			bikes.Add(new VehicleHash[] { VehicleHash.Faggio, VehicleHash.Faggio2, VehicleHash.Faggio3, });
			bikes.Add(new VehicleHash[] { VehicleHash.Hakuchou, VehicleHash.Hakuchou2 });
			bikes.Add(new VehicleHash[] { VehicleHash.Hexer });
			bikes.Add(new VehicleHash[] { VehicleHash.Innovation });
			bikes.Add(new VehicleHash[] { VehicleHash.Nemesis });
			bikes.Add(new VehicleHash[] { VehicleHash.PCJ });
			bikes.Add(new VehicleHash[] { VehicleHash.Ruffian });
			bikes.Add(new VehicleHash[] { VehicleHash.Sanchez, VehicleHash.Sanchez2 });
			bikes.Add(new VehicleHash[] { VehicleHash.Sovereign });
			bikes.Add(new VehicleHash[] { VehicleHash.Thrust });
			bikes.Add(new VehicleHash[] { VehicleHash.Vader });
			bikes.Add(new VehicleHash[] { VehicleHash.Vindicator });
			return bikes;
		}

		static List<VehicleHash[]> PedalBikes()
		{
			List<VehicleHash[]> bikes = new List<VehicleHash[]>();
			bikes.Add(new VehicleHash[] { VehicleHash.TriBike, VehicleHash.TriBike2, VehicleHash.TriBike3, });
			bikes.Add(new VehicleHash[] { VehicleHash.Bmx });
			return bikes;
		}

		/// <summary>
		/// Get a list of all weapons player is missing.
		/// </summary>
		List<WeaponHash> PlayerMissingWeapons()
		{
			var player = Game.Player.Character;

			List<WeaponHash> list = new List<WeaponHash>();

			if (player == null)
			{
				return list;
			}

			foreach (WeaponHash w in ALL_WEAPONS)
			{
				if (w == WeaponHash.Unarmed)
				{
					continue;
				}

				if (!player.Weapons.IsWeaponValid(w))
				{
					continue;
				}

				if (player.Weapons.HasWeapon(w))
				{
					continue;
				}

				list.Add(w);
			}

			return list;
		}

		/// <summary>
		/// Get a weapon by id.
		/// </summary>
		public WeaponHash? GetWeaponById(String id)
		{
			if (id == "random")
			{
				var missingWeapons = PlayerMissingWeapons();

				if (missingWeapons.Count == 0)
				{
					return null;
				}

				return missingWeapons[Rnd.Next(0, missingWeapons.Count)];
			}

			var player = Game.Player.Character;

			if (player == null)
			{
				return null;
			}

			WeaponHash weaponHash = WeaponHash.Unarmed;

			if (ALL_WEAPONS_BY_ID.TryGetValue(id, out weaponHash))
			{
				return weaponHash;
			}

			return null;
		}
	}

	/// <summary>
	/// Keeps track and updates the group that hates the current player.
	/// </summary>
	public class HateGroup
	{
		public RelationshipGroup GroupId
		{
			get;
		}

		public HateGroup()
		{
			GroupId = World.AddRelationshipGroup("hates-player");
			GroupId.SetRelationshipBetweenGroups(Game.GenerateHash("PLAYER"), Relationship.Hate);
		}
	}

	/// <summary>
	/// A timer that can be ticked.
	/// </summary>
	public interface Timer
	{
		/// <summary>
		/// Get the remaining time.
		/// </summary>
		float Remaining
		{
			get;
		}

		/// <summary>
		/// Get the total duration of the timer.
		/// </summary>
		float Duration
		{
			get;
		}

		/// <summary>
		/// Stop the timer, cleaning up any resources it uses.
		/// </summary>
		void Stop();

		/// <summary>
		/// Tick the timer.
		/// </summary>
		bool Tick();
	}

	public class PlayerTimer : ITicker, Timer
	{
		private TimerBarPool pool;
		private BarTimerBar bar;
		private float duration;
		private float elapsed;

		public float Remaining
		{
			get
			{
				return duration - elapsed;
			}
		}

		public float Duration
		{
			get
			{
				return duration;
			}
		}

		public PlayerTimer(TimerBarPool pool, BarTimerBar bar, float duration)
		{
			this.pool = pool;
			this.bar = bar;
			this.duration = duration;
			this.elapsed = 0f;
		}

		/// <summary>
		/// Stop the current timer, removing it from being displayed.
		/// </summary>
		public override void Stop()
		{
			pool.Remove(bar);
		}

		/// <summary>
		/// Tick the current timer, possibly advancing it.
		/// </summary>
		public override bool Tick()
		{
			elapsed += Game.LastFrameTime;

			if (elapsed >= duration)
			{
				return true;
			}

			bar.Percentage = Remaining / duration;
			return false;
		}
	}

	/// <summary>
	/// An anonymous timer that doesn't have a bar associated with it.
	/// </summary>
	public class AnonymousTimer : Timer
	{
		private TimerBarPool pool;

		public float Time;
		public float Duration
		{
			get;
		}

		public float Remaining
		{
			get
			{
				return Duration - Time;
			}
		}

		public AnonymousTimer(TimerBarPool pool, float time, float duration)
		{
			this.pool = pool;
			this.Time = time;
			this.Duration = duration;
		}

		public void Stop()
		{
		}

		/// <summary>
		/// Tick the current timer, possibly advancing it.
		/// </summary>
		public bool Tick()
		{
			Time += Game.LastFrameTime;
			return Time >= Duration;
		}

		/// <summary>
		/// Get the current time as a percentage.
		/// </summary>
		public float Percentage()
		{
			return Time / Duration;
		}
	}

	/// <summary>
	/// A timer that ticks randomly.
	/// </summary>
	public class RandomTimer : Timer
	{
		private Random rnd;
		private float initial;
		private float magnitude;

		public float Time;
		public float Duration
		{
			get;
			set;
		}

		public float Remaining
		{
			get
			{
				return Duration - Time;
			}
		}

		public RandomTimer(Random rnd, float initial, float magnitude)
		{
			this.rnd = rnd;
			this.initial = initial;
			this.magnitude = magnitude;
			this.Time = 0;
			this.Duration = NextDuration();
		}

		public void Stop()
		{
		}

		/// <summary>
		/// Tick the current timer, possibly advancing it.
		/// </summary>
		public bool Tick()
		{
			Time += Game.LastFrameTime;

			if (Time >= Duration)
			{
				Time = 0;
				Duration = NextDuration();
				return true;
			}

			return false;
		}

		/// <summary>
		/// Get the current time as a percentage.
		/// </summary>
		public float Percentage()
		{
			return Time / Duration;
		}

		/// <summary>
		/// Get the next duration.
		/// </summary>
		private float NextDuration()
		{
			return (float)rnd.NextDouble() * magnitude + initial;
		}
	}

	public class Gauge
	{
		private TimerBarPool pool;
		private BarTimerBar bar;
		private bool visible;

		public bool Visible
		{
			get
			{
				return visible;
			}

			set
			{
				if (value == visible)
				{
					return;
				}

				if (value)
				{
					pool.Add(bar);
				}
				else
				{
					pool.Remove(bar);
				}

				visible = value;
			}
		}

		public Gauge(TimerBarPool pool, BarTimerBar bar)
		{
			this.pool = pool;
			this.bar = bar;
			this.visible = true;
		}

		/// <summary>
		/// Stop the gauge, removing it from being displayed.
		/// </summary>
		public void Stop()
		{
			if (visible)
			{
				pool.Remove(bar);
			}
		}

		/// <summary>
		/// Set the percentage of the gauge, as a value between 0-1.
		/// </summary>
		public void Set(float percentage)
		{
			bar.Percentage = percentage;
		}

		/// <summary>
		/// Clear the bar.
		/// </summary>
		public void Clear(String text)
		{
			bar.Label = text;
			bar.Percentage = 1f;
			bar.ForegroundColor = Color.Black;
		}
	}
}
