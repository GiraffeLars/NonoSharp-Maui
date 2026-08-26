# NonoSharp Maui
[![Release Version](https://img.shields.io/github/v/release/GiraffeLars/NonoSharp-Maui?include_prereleases&label=Latest%20Release
)](https://github.com/GiraffeLars/NonoSharp-Maui/releases)

A cross-platform Nonogram game built with C# and .NET MAUI, featuring pre-made and randomly generated puzzles together. Built on top of my NonoSharp API, found [here](https://github.com/GiraffeLars/NonoSharp). 

> Status: Currently in-development. A beta build is available in [Releases](https://github.com/GiraffeLars/NonoSharp-Maui/releases).

<img src="docs/PicrossGame.png" alt="Nonogram Puzzle being solved" width="500"/>


## What is NonoSharp?
NonoSharp is an API for C#, together with this UI consumer. The API allows easy creation and playing of [Nonogram](https://en.wikipedia.org/wiki/Nonogram) (also known as Picross) puzzles.
Nonograms are Japanese puzzles where you fill in a picture based on hints given to you.
The hints, either on the left-side or top-side of the grid, show how many groups there are in a given row/column and show how many cells each group consists of.
By filling the grid one cell at a time, eventually you reach the solution.

## Features of the MAUI project
- **A fully functional Nonogram game**, complete with hint checking, built on top of NonoSharp
- **Cross-platform** UI built with MAUI
- **Saving and loading solutions** to/from custom file format
- **Create** your **own** puzzle with the Puzzle Creator

## Using the API
For information regarding the API, and using it, see the [NonoSharp GitHub page](https://github.com/GiraffeLars/NonoSharp).

## Getting Started
### Playing
To play the game build upon the API, install the beta release in the [Releases](https://github.com/GiraffeLars/NonoSharp-Maui/releases) tab.
Currently, only a build for Windows is available. If you wish to play on a different platform, see the section below.

### Contributing
To contribute, clone the project and open it in your prefered IDE, such as Visual Studio. The project makes use of [.NET 10.0](https://dotnet.microsoft.com/en-us/download/dotnet/10.0), [MAUI](https://dotnet.microsoft.com/en-us/apps/maui) and [sqlite-net-pcl](https://www.nuget.org/packages/sqlite-net-pcl/), and of course [NonoSharp](https://www.nuget.org/packages/NonoSharp/).
> **Note:** I cannot guarantee (full) functionality on operating systems other than Windows or Android. While other MAUI platforms are supported, they may contain unexpected issues. 
> Some features may not be available on all operating systems due to MAUI limitations.

### Installing dependencies
In order to build the project, you will need, as mentioned above, .NET 10.0, .NET MAUI and sqlite-net-pcl. To install the .NET MAUI workload, run the following command in your terminal
```
dotnet workload install maui
```
Alternatively, it is also possible to automatically install the workload when installing Visual Studio by selecting the corresponding option in the installer.

After .NET MAUI has successfully installed, clone the project and open it in your IDE. Before building the project, run the following command. This will install required dependencies, such as sqlite-net-pcl and fix other possible issues. 
```
dotnet restore
```
### Building the project
After setting everything up, you can build the MAUI project with
```
dotnet build src/NonoSharp.Maui/NonoSharp.Maui.csproj
```

Of course, you are also welcome to use your IDE's debugger to build the project and/or play it.

## Roadmap
Features that are currently planned to be added *(in no particular order)*:
- [x] Random puzzle generation that have a guaranteed solution
- [x] Automatically cross the remaining blank cells upon line completion
- [x] Dark mode support
- [x] Support for pre-made puzzles
- [x] Player-created puzzles and puzzle creator
- [ ] Improve player controls on PC
- [ ] Player statistics
- [ ] UI improvements

## Contribution guidelines
This project started as a solo learning project, but contributions are welcome. Please open a PR or an issue if you wish to contribute. 

When submitting a pull request, please make note of the following:
- Keep PRs focussed
- If you make any changes to the logic, ensure that the tests verify
- Make sure the project builds and functions as intended
- Keep code documented
- Try to keep AI generated code at a minimum

## License
This project is licensed under the **MIT License**. See the `LICENSE` file.
