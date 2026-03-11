# Changelog


## [0.0.14] - First Ship Movement

### Added
- `ShipConfig` resource as one source of truth for ship movement values
- realistic and arcade flight mode support
- desired-direction ship movement for `move_left`, `move_right`, `move_up`, and `move_down`
- `full_stop` input support
- ship-follow camera setup for movement testing

### Changed
- ship movement now uses desired heading instead of direct manual thrust-only steering
- realistic flight mode now waits for alignment before enabling main thrust
- arcade flight mode keeps thrust active during direction changes
- full stop behavior tuned to feel closer to auxiliary thrusters
- ship movement feel adjusted to preserve inertia during rotation in realistic mode

### Fixed
- broken script reference on `BackgroundRoot`
- invalid input action usage in `Ship.cs`
- main scene startup issues caused by incorrect C# script attachment

## [0.0.132] - 2026-03-11

### Added
- `Ship.tscn` placeholder scene
- placeholder ship sprite with idle variant
- thrust sprite variant for engine-on visual
- `DrillPoint` marker
- basic `Ship.cs` skeleton
- exported variables for visual, movement, and debug tuning
- ship instance added to main scene

### Changed
- ship collision shape aligned to the main hull body
- ship visual setup prepared for idle/thrust texture switching

## [0.0.131] - 2026-03-11

### Added
- Space background placeholder added
- Far star layer added
- Near star layer added
- Parallax2D background structure prepared for future movement testing

### Changed
- Background scene structure expanded for readable movement feedback


## [0.0.13] - 2026-03-10

### Added
- Main project scene created
- Basic node structure prepared
- Placeholder ship scene created
- Ship scene instanced into GameplayRoot
- Input actions added
- Initial project folders prepared
- C# script skeletons prepared for Main and Shi

### Changed
- Ship root changed to CharacterBody2D for controlled movement foundation

### Fixed
- Nothing yet

### Removed
- GDScript placeholder plan replaced with C# project setup

## [0.0.12] - 2026-03-10

### Added
- ROADMAP.md - Full Roadmap plan

## 0.0.11

### Added
- Initial Discord Setup

### Released
2026-03-08

## 0.0.1

### Added
- Initial AstroMiner C# project foundation
- New Godot 4.6.1 .NET project
- Base folder structure

### Changed
- Set up VS Code + C# workflow

### Fixed
- Correct .NET SDK setup for Godot C#

### Notes
- First clean foundation commit

### Released
2026-03-08