# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

[Unreleased]: https://github.com/udoprog/setmod/compare/1.2.0...HEAD

## [1.2.0]

### Changed
- `boost`, and `superboost` have been nerfed a bit to give the player more control.

### Added
- A menu that can be opened to run commands (`J` or `K`).
- Progress bars for active effects.
- Added commands:
  - `eject` to eject out of the current vehicle.
  - `slow-down-time` to slow down the game time.
  - `levitate` to cause the player and its vehicle to levitate.
  - `levitate-entities` to cause other entities and their vehicles to levitate.
  - `fire-ammo` to give the player fire ammo.
  - `fuel-leakage` to cause the current vehicle to slowly but surely run out of fuel.
  - `superman` (internal only) enables a couple of superman-like effects.

[1.2.0]: https://github.com/udoprog/setmod/compare/1.1.1...1.2.0

## [1.1.1]

### Changed
- A couple of bug fixes related to crashes and vehicle modifications.

[1.1.1]: https://github.com/udoprog/setmod/compare/1.1.0...1.1.1

## [1.1.0]

### Changed
- `superboost` boosted for too long (10 seconds) by mistake. Now it's more like a much more powerful boost.
- `spawn-enemy` now spawns a more diverse group of enemies.

### Added

- Added commands:
  - `exploding-bullets` - Enable exploding bullets for 10 seconds.
  - `exploding-punches` - Enable exploding punches for 10 seconds.
  - `drunk` - Make the player drunk for 10 seconds.
  - `very-drunk` - Make the player very drunk for 10 seconds.
  - `set-on-fire` - Set the player on fire.
  - `set-peds-on-fire` - Set other pedestrians on fire.
  - `make-peds-aggressive` - Make a number of peds aggressive.
  - `matrix-slam` - Perform a matrix slam.
  - `disable-control <id>` - Display the specified control.
  - `mod-vehicle <id>` - Modify the current vehicle.

[1.1.0]: https://github.com/udoprog/setmod/compare/1.0.0...1.1.0