using System;
using System.Collections.Generic;
using System.Text.Json;

namespace NonoSharp.Maui
{
    internal static class PuzzleLibrary
    {
        private static Dictionary<int, string>? libraryJson = null;
        private static readonly SemaphoreSlim _loadSemaphore = new(1, 1);

        /// <summary>
        /// Gets the filename of the puzzle with id <paramref name="id"/>
        /// </summary>
        /// <param name="id">ID of the puzzle</param>
        /// <returns>The filename, usually as XXXX_TITLE.ns</returns>
        /// <exception cref="InvalidDataException">Thrown when the json library failed to load</exception>
        /// <exception cref="ArgumentException">Thrown when there is no puzzle with id <paramref name="id"/> in the library</exception>
        public static async Task<string> GetPuzzleFilenameAsync(int id)
        {
            await LoadLibraryAsync();

            var validID = libraryJson!.TryGetValue(id, out string? filename);
            if (!validID || filename == null)
            {
                throw new ArgumentException($"There is no puzzle with ID {id}!");
            }
            return filename;
        }

        /// <summary>
        /// Gets the total number of available puzzles
        /// </summary>
        /// <returns>Total number of puzzles according to the library</returns>
        /// <exception cref="InvalidDataException">Thrown when the json library failed to load</exception>
        public static async Task<int> GetPuzzleTotalAsync()
        {
            await LoadLibraryAsync();
            return libraryJson!.Count;
        }

        /// <summary>
        /// Loads the library json located in Resources/Raw/Puzzles.
        /// </summary>
        /// <exception cref="InvalidDataException">Thrown when the json library failed to load</exception>
        private async static Task LoadLibraryAsync()
        {
            await _loadSemaphore.WaitAsync();

            try
            {
                if (libraryJson != null) return;

                try
                {
                    using var stream = await FileSystem.OpenAppPackageFileAsync("Puzzles/library.json");

                    libraryJson = await JsonSerializer.DeserializeAsync<Dictionary<int, string>>(stream);
                }
                catch (Exception e)
                {
                    throw new InvalidDataException("Failed to load the library JSON!", e);
                }

                if (libraryJson == null) { throw new InvalidDataException("Failed to load the library JSON!"); }
            } 
            finally
            {
                _loadSemaphore.Release(); 
            }

        }
    }
}
