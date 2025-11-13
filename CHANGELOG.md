# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).


## [1.4.0] - Unreleased

### Added
- Support for B99.7.
- Added support for the DM1U and Microshunter.
- An option to change rail quality (how squiggly they are) ([#5] by [@WhistleWiz]).
- An option to change rail material (how rusty they are) ([#6] by [@WhistleWiz]).
- Support for [CCL](https://github.com/derail-valley-modding/custom-car-loader) ([#7] by [@WhistleWiz]).

### Fixed
- Fixed some mesh issues with the S060 and S282.
- Fixed the DE2's traction motors and brake shoes not being regauged.
- Fixed rail anchors not being regauged;
- Fixed DH4 wheels clipping through the center of the bogie on narrower gauges.


## [1.3.0] - 2023-07-22

### Changed
- Switched back to UMM.

### Added
- Added support for the S060.


## [1.2.1] - 2023-07-03

### Fixed
- Fixed turntable rails not being regauged.


## [1.2.0] - 2023-07-02

### Changed
- Switched to BepInEx.

### Added
- Added support for Simulator.

### Fixed
- Fixed buffer stop sleepers being regauged.
- Fixed the inside of the S282 firebox being visible outside the locomotive on narrow gauges.


## [1.1.2] - 2023-04-11

### Fixed
- Fixed wheelslip particles not being regauged.
- Fixed an error if the AssetBundle isn't present.


## [1.1.1] - 2023-04-07

### Fixed
- Fixed the actual track gauge being 70mm too narrow.
- Fixed a stretched vertex on switches.


## [1.1.0] - 2023-03-24

### Added
- Added support for broad gauges.
- Added an option to bypass settings restrictions.

### Fixed
- Fixed roundhouse rails not being regauged.
- Fixed service station markers not being regauged.
- Fixed switch stands floating on narrow gauges.
- Fixed some minor switch deformations.
- Fixed non-standard gauge custom cars not being regauged when standard gauge is selected.
- Fixed non-standard gauge custom cars with one default bogie not being regauged properly.
- Fixed train wheels clipping through roundhouse rails (vanilla issue, fix only applies to non-standard gauge).
- Fixed the settings menu showing two different units for custom gauge inputs.
- Fixed the mod trying to load with Unity Mod Manager versions older than 0.25.0.


## [1.0.0] - 2023-02-19
- Initial release.

<!-- Users -->
[@WhistleWiz]: https://github.com/WhistleWiz

<!-- Pull Requests -->
[#7]: https://github.com/Insprill/dv-gauge/pull/7
[#6]: https://github.com/Insprill/dv-gauge/pull/6
[#5]: https://github.com/Insprill/dv-gauge/pull/5

<!-- Diffs -->
[1.4.0]: https://github.com/Insprill/dv-gauge/compare/v1.3.0...HEAD
[1.3.0]: https://github.com/Insprill/dv-gauge/compare/v1.2.1...v1.3.0
[1.2.1]: https://github.com/Insprill/dv-gauge/compare/v1.2.0...v1.2.1
[1.2.0]: https://github.com/Insprill/dv-gauge/compare/v1.1.2...v1.2.0
[1.1.2]: https://github.com/Insprill/dv-gauge/compare/v1.1.1...v1.1.2
[1.1.1]: https://github.com/Insprill/dv-gauge/compare/v1.1.0...v1.1.1
[1.1.0]: https://github.com/Insprill/dv-gauge/compare/v1.0.0...v1.1.0
[1.0.0]: https://github.com/Insprill/dv-gauge/releases/tag/v0.1.0
