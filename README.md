# Picross
A cross-platform Picross game built with C# and .NET MAUI, featuring randomly generated puzzles and reusable game logic API.

> Status: Currently in-development. A beta build is available in Releases.

<img src="docs/PicrossGame.png" alt="Picross Puzzle being solved" width="500"/>


## What is Picross?
[Picross](https://en.wikipedia.org/wiki/Nonogram) (also known as Nonogram) is a Japanese puzzle game, where you fill in a picture based on hints given to you.
The hints, either on the left-side or top-side of the grid, show how many groups there are in a given row/column and show how many cells each group consists of.
By filling the grid one cell at a time, eventually you reach the solution.

## Features
- **A fully functional Picross game**, complete with hint checking
- **Randomly generated puzzles** guaranteed to be solvable as verified by a solver
- **Cross-platform** UI built with MAUI
- An **API** allowing for game logic to be reused in other project without building it yourself

## Using the API
The core logic for the puzzles is located in *Picross.Game*, and is independent of the MAUI UI.
This makes it possible to reference in any .NET project.
Currently supported functions include:
- Abstracted grid, making it easy to implement in your projects
- Built-in undo/redo functionality
- Checking whether the puzzle is solved
- A hint system, together with whether a hint is completed by the user.
- Events for cells changing states and the puzzle being solved correctly

### Example usage
```csharp
using Picross.Game;
using Picross.Game.Events;
 
// Creates a new random 10x10 puzzle. Generation is guaranteed to produce a solvable puzzle.
// This method is also available asynchronously via GameAPI.CreateRandomPuzzleAsync
var game = GameAPI.CreateRandomPuzzle(10, 10); // (width x height)
 
// Fill in or cross a cell (coordinates are zero-indexed, (0, 0) is top-left)
game.FillCell(2, 3);
game.CrossCell(0, 0);
 
// Moves can be undone/redone
if (game.CanUndo)
{
    game.Undo();
}
 
// Check individual cell state
bool isFilled = game.IsSquareFilled(2, 3);
 
// Check overall progress
if (game.IsPuzzleSolved())
{
    Console.WriteLine("Solved!");
} 
else 
{
    Console.WriteLine("Not solved :(");
}

 
// The hints shown alongside the grid (e.g. "3 1" for a row) are available for building your own UI
Hints[] columnHints = game.ColumnHints;
Hints[] rowHints = game.RowHints;

// There are also some events provided
game.CellStateChanged =+ (s, e) => {
    Console.WriteLine("A cell has changed states");

    // Include using Picross.Game.Events to gain access to the event args
};
```

## Getting Started
To play a basic Picross game build upon the API, a beta release is available in the release tab. To contribute,
you can clone the project and open it in your prefered IDE. The project makes use of [.NET 10.0](https://dotnet.microsoft.com/en-us/download/dotnet/10.0), [MAUI](https://dotnet.microsoft.com/en-us/apps/maui) and [sqlite-net-pcl](https://www.nuget.org/packages/sqlite-net-pcl/).
You will possibly have to run `dotnet restore` in case your build fails.

> **Note:** I cannot guarantee functionality on operating systems other than Windows or Android, but they should work
considering MAUI is multi-platform. Some features may not be available on all operating systems due to MAUI limitations.

> A more detailed getting started section will be added at a later date.

## Roadmap
Features that are currently planned to be added *(in no particular order)*:
- [x] Random puzzle generation that have a guaranteed solution
- [x] Automatically cross the remaining blank cells upon line completion
- [x] Dark mode support
- [ ] Support for pre-made puzzles
- [ ] Improve player controls on PC
- [ ] Player statistics
- [ ] UI improvements
- [ ] Player-created puzzles and puzzle creator

## Contributing
This project started as a solo learning project, but contributions are welcome. Please open a PR or an issue if you wish to contribute. 

When submitting a pull request, please make note of the following:
- Keep PRs focussed
- If you make any changes to the logic, ensure that the tests verify
- Make sure the project buids and functions as intended
- Keep code documented
- Try to keep AI generated code at a minimum

## License
This project is licensed under the **MIT License**. See the `LICENSE` file.
