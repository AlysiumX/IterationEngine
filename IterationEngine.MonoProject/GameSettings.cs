using IterationEngine.MonoProject.Engine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IterationEngine.MonoProject
{
    public class GameSettings
    {
        private readonly string _gameSettingsFileLocation = $"{AppDomain.CurrentDomain.BaseDirectory}GameSettings.json";

        public int GameWidth { get; set; }
        public int GameHeight { get; set; }
        public int TileSize { get; set; }
        public bool IsFullScreen { get; set; }

        public GameSettings() { }

        public void CreateSettingsFileIfNotExists()
        {
            if( !File.Exists( _gameSettingsFileLocation ) )
            {
                GameWidth = 1600;
                GameHeight = 900;
                TileSize = TileSizes.Small;
                IsFullScreen = false;
                SaveSettingsFile();
            }
        }

        public void LoadSettings()
        {
            var settingsFile = File.ReadAllText( _gameSettingsFileLocation );
            var settings = JsonConvert.DeserializeObject<GameSettings>( settingsFile );

            ValidateAndSetSettings( settings );
        }

        public void SaveSettingsFile()
        {
            var jsonToWrite = JsonConvert.SerializeObject( this, Formatting.Indented );
            File.WriteAllText( _gameSettingsFileLocation, jsonToWrite );
        }

        private void ValidateAndSetSettings( GameSettings settings )
        {
            GameWidth = settings.GameWidth;
            GameHeight = settings.GameHeight;

            var validatedTileSize = ValidateAndChangeTileSizeIfRequire( settings.TileSize );
            TileSize = validatedTileSize.TileSize;

            IsFullScreen = settings.IsFullScreen;

            if( validatedTileSize.TileSizeHadToChange )
                SaveSettingsFile();
        }

        private (bool TileSizeHadToChange, int TileSize) ValidateAndChangeTileSizeIfRequire( int tileSize )
        {
            var validTileSizes = new List<int>
            {
                TileSizes.Small,
                TileSizes.Medium,
                TileSizes.Large
            };

            if( validTileSizes.Contains( tileSize ) )
                return (TileSizeHadToChange: false, TileSize: tileSize);

            if( tileSize < TileSizes.Small )
                return (TileSizeHadToChange: true, TileSizes.Small);

            if( tileSize > TileSizes.Large )
                return (TileSizeHadToChange: true, TileSizes.Large);

            return (TileSizeHadToChange: true, TileSizes.Medium);
        }
    }
}
