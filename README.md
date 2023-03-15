[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![Apache 2.0 License][license-shield]][license-url]




<!-- PROJECT LOGO -->
<div align="center">
  <h1>Gauge</h1>
  <p>
    An experimental <a href="https://store.steampowered.com/app/588030">Derail Valley</a> mod rebuilding the valley in 3ft narrow gauge!
    <br />
    <br />
    <a href="https://github.com/Insprill/dv-gauge/issues">Report Bug</a>
    ·
    <a href="https://github.com/Insprill/dv-gauge/issues">Request Feature</a>
  </p>
</div>




<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li><a href="#about-the-project">About The Project</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#building">Building</a></li>
    <li><a href="#building-the-asset-bundle">Building the Asset Bundle</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
  </ol>
</details>




<!-- ABOUT THE PROJECT -->

## About The Project

Gauge is an experimental Derail Valley mod to allow changing the track gauge.
Currently, it only supports 3ft narrow gauge.




<!-- BUILDING -->

## Building

To build Gauge, you'll need to have [Custom Car Loader](https://www.nexusmods.com/derailvalley/mods/324) 1.8.3 or newer installed.  

You'll also need to create a new [`Directory.Build.targets`](https://learn.microsoft.com/en-us/visualstudio/msbuild/customize-your-build?view=vs-2022) file to specify your reference paths. 
This file should be located in the root of the project, next to this README.
You can use one of the examples below as a template depending on your platform.

<details>
<summary>Windows</summary>

Here's an example file for Windows you can use as a template.
Replace the provided paths with the paths to your Derail Valley installation directory.
Make sure to include the semicolons between each of the paths, but not after the last one!
Note that shortcuts like `%ProgramFiles%` *cannot* be used.
```xml
<Project>
    <PropertyGroup>
        <ReferencePath>
            C:\Program Files (x86)\Steam\steamapps\common\Derail Valley\DerailValley_Data\Managed\;
            C:\Program Files (x86)\Steam\steamapps\common\Derail Valley\DerailValley_Data\Managed\UnityModManager\;
            C:\Program Files (x86)\Steam\steamapps\common\Derail Valley\Mods\DVCustomCarLoader\
        </ReferencePath>
        <AssemblySearchPaths>$(AssemblySearchPaths);$(ReferencePath);</AssemblySearchPaths>
    </PropertyGroup>
</Project>
```
</details>

<details>
<summary>Linux</summary>

Here's an example file for Windows you can use as a template.
Replace the provided paths with the paths to your Derail Valley installation directory.
Make sure to include the semicolons between each of the paths, but not after the last one!
```xml
<Project>
    <PropertyGroup>
        <ReferencePath>
            /home/username/.local/share/Steam/steamapps/common/Derail Valley/DerailValley_Data/Managed/;
            /home/username/.local/share/Steam/steamapps/common/Derail Valley/DerailValley_Data/Managed/UnityModManager/;
            /home/username/.local/share/Steam/steamapps/common/Derail Valley/Mods/DVCustomCarLoader/
        </ReferencePath>
        <AssemblySearchPaths>$(AssemblySearchPaths);$(ReferencePath);</AssemblySearchPaths>
    </PropertyGroup>
</Project>
```
</details>

To test your changes, `Gauge.dll` will need to be copied into the mod's install directory (e.g. `...Derail Valley/Mods/Gauge`) along with `info.json`.
The .dll can be found in `bin/Debug` or `bin/Release` depending on the selected build configuration.
The info.json can be found in the root of this repository.




<!-- BUILDING ASSET BUNDLE -->

## Building The Asset Bundle

To build the AssetBundle for gauge, you'll need to install Unity **2019.4.22f1**.
You can then open up the `GaugeBundleBuilder` project in this repo.

To add the meshes to the project you'll need to export them yourself using something like [AssetStudio][asset-studio-url].
You can find all the meshes you need to export in `GaugeBundleBuilder/Assets/Meshes/meshes.txt`. If you're missing meshes, it'll' warn you when you try to build.




<!-- CONTRIBUTING -->

## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create.  
Any contributions you make are **greatly appreciated**!  
If you're new to contributing to open-source projects, you can follow [this][contributing-quickstart-url] guide.




<!-- LICENSE -->

## License

Code is distributed under the Apache 2.0 license.  
See [LICENSE][license-url] for more information.

AssetBundle assets are owned by Altfuture and are included with permission, for the purpose of having Read/Write protection on them removed, which is necessary for this mod to function.
These assets are not covered by the Apache 2.0 license and have different terms and conditions. Contact [support@altfuture.gg][altfuture-support-email-url] for more information.




<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->

[contributors-shield]: https://img.shields.io/github/contributors/Insprill/dv-gauge.svg?style=for-the-badge
[contributors-url]: https://github.com/Insprill/dv-gauge/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/Insprill/dv-gauge.svg?style=for-the-badge
[forks-url]: https://github.com/Insprill/dv-gauge/network/members
[stars-shield]: https://img.shields.io/github/stars/Insprill/dv-gauge.svg?style=for-the-badge
[stars-url]: https://github.com/Insprill/dv-gauge/stargazers
[issues-shield]: https://img.shields.io/github/issues/Insprill/dv-gauge.svg?style=for-the-badge
[issues-url]: https://github.com/Insprill/dv-gauge/issues
[license-shield]: https://img.shields.io/github/license/Insprill/dv-gauge.svg?style=for-the-badge
[license-url]: https://github.com/Insprill/dv-gauge/blob/master/LICENSE
[altfuture-support-email-url]: mailto:support@altfuture.gg
[contributing-quickstart-url]: https://docs.github.com/en/get-started/quickstart/contributing-to-projects
[asset-studio-url]: https://github.com/Perfare/AssetStudio
