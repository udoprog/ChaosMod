# ChaosMod for GTA V

This is a the GTA V mod for the [`gtav` setmod] module.

It is the mod side that allows viewer interaction like this:
https://www.twitch.tv/setbac/clip/UnusualCaringNewtWOOP

The bot that handles this is [setmod].

[`gtav` setmod]: https://github.com/udoprog/setmod#gtav
[setmod]: https://github.com/udoprog/setmod

## Dependencies

* [ScriptHookV.NET](https://www.gta5-mods.com/tools/scripthookv-net)
* [NativeUI](https://github.com/Guad/NativeUI/releases/)

## Building

Open up the project in Visual Studio 2017 and Build.

You might have to modify `Properties -> Build Events -> Post-build event command line`,
since it is currently setup to copy the mod to the `scripts` folder after it is built.

This is currently `C:\Program Files (x86)\Steam\SteamApps\common\Grand Theft Auto V\scripts`,
and would have to be modified to suit your machine in case you want the automatic copying to work.

## Protocol

The mod currently binds a UDP socket to `127.0.0.1:7291`, which means you have to integrate with it on your local machine.

It expects you to send datagrams which are structure like the following:

```
<user> " " <id> " " <command...>
```

Note the `" "` above which is an ASCII space character.

* `<user>` is used to indicate who did something.
* `<id>` is used to correlate any errors you get on screen with the command that sent it. It can be anything as long as you can compare it.
  - In `setmod` this is an increasing number which is logged with the command in setmod.
* `<command...>` the command to run. Available commands are detailed below.

If you have netcat installed, you can do:

```
nc -u localhost 7291
```

And type out commands to test them.

For example, you could type:

```
setbac 42 blow-tires
```

To blow the tires into your current game.

## Commands

* `blow-tires`
* `boost`
* `brake`
* `fall`
* `give-ammo`
* `give-armor`
* `give-health`
* `give-weapon <id>` - Gives a weapon for the given category.
  - Some available categories includes: `random`, `m4`, `ak47`.
    For a full list, see the source code.
* `invincibility`
* `kill-engine`
* `license <license...>` - Sets the license plate to `<license...>`. It uses the rest of the datagram.
* `randomize-character`
* `randomize-color`
* `randomize-vehicle` - To Be Implemented
* `randomize-weather` - Randomize the current weather.
* `repair` - Repair the current vehicle.
* `spawn-enemy <amount>` - Spawn `<amount>` enemies.
* `spawn-vehicle <id>` - Spawn a vehicle in the category identified by `<id>`.
  - Some available categories includes: `random`, `fast`, `normal`, `slow`.
    For a full list, see the source code.
* `stumble`
* `super-boost`
* `super-jump`
* `super-speed`
* `super-swim`
* `take-all-weapons`
* `take-ammo` - Takes the ammo for the currently equipped weapon.
* `take-health` - Takes all but 10% of your health.
* `take-weapon` - Takes the current weapon.
* `wanted <level>` - Sets the wanted level to `<level>`, `wanted 0` clears your wanted level.
* `exploding-bullets` - Enable exploding bullets for 10 seconds.
* `exploding-punches` - Enable exploding punches for 10 seconds.
* `drunk` - Make the player drunk for 10 seconds.
* `very-drunk` - Make the player very drunk for 10 seconds.
* `set-on-fire` - Set the player on fire.
* `set-peds-on-fire` - Set other pedestrians on fire.
* `make-peds-aggressive` - Make a number of peds aggressive.
* `matrix-slam` - Perform a matrix slam.

## License

ChaosMod is distributed under the terms of both the MIT license and the Apache License (Version 2.0).

See LICENSE-APACHE and LICENSE-MIT for details.