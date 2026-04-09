# Changelog


## [0.0.18] - Drill Detection Alpha

### Added
- forward drill detection from `DrillPoint`
- drill safety speed check before target validation
- drill target detection using raycast against world collision
- conversion pipeline for drill targeting: `global hit -> to_local() -> local_to_map()`
- `DrillTargetInfo` target data structure for detected drill hits
- `DrillDetectDistancePx` config value in `ShipConfig`
- central `Debug.cs` debug controller with section-based switches
- drill detection console debug routed through central debug system

### Changed
- ship now evaluates drill targets directly from its own forward position instead of relying on scene-specific references
- drill detection debug output is now cleaner and easier to toggle during development
- debug flow was centralized for future systems instead of using local ad-hoc prints

### Fixed
- drill detection now correctly resolves target cells through the hit `TileMapLayer`
- console spam from repeated drill detection messages is reduced through centralized debug handling

### Removed
- Nothing yet

## [0.0.17] - Flight Collision baseline

### Added
- collision tuning values added to `ShipConfig` for flight impact handling
- hybrid collision response in `Ship.cs` combining slide/scrape damping with controlled head-on rebound
- flight bounce cooldown timer to prevent repeated rebound spam during contact

### Changed
- ship collision feel tuned to reduce pinball-like asteroid impacts
- outer asteroid contact now preserves more tangential movement for smoother scrape behavior
- head-on flight impacts now use a controlled rebound instead of pure stick-to-surface stopping
- `Ship` collision baseline adjusted through inspector-driven tuning for faster iteration

### Fixed
- flight collisions no longer rely on pure stop/stick behavior during frontal asteroid contact
- side contact now behaves more like sliding along the asteroid surface instead of chaotic bounce


## [0.0.16] - Test Asteroid

### Added
- `AsteroidTest.tscn` test asteroid scene
- `AsteroidLayer` using `TileMapLayer`
- `asteroid_test_tileset.tres`
- first asteroid collision tile
- test asteroid placed into main scene

### Changed
- main test scene now includes first asteroid collision slice

### Fixed
- ship can now physically collide with a test asteroid body

## [0.0.15] - Camera and debug HUD

### Added
- debug HUD with speed readout
- debug HUD with flight mode readout
- debug HUD with velocity vector readout
- debug HUD with zoom readout
- mouse wheel camera zoom controls
- min/max zoom limits for test camera

### Changed
- camera setup refined for movement testing
- debug HUD moved under main scene for cleaner structure
- camera smoothing temporarily disabled for clearer movement readout

### Fixed
- missing debug HUD label references
- debug HUD null reference spam during runtime
- camera shake caused by current smoothing setup during acceleration

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