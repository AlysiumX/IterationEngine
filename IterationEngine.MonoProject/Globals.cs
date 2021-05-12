using System;
using System.Collections.Generic;
using System.Text;

namespace IterationEngine.MonoProject
{
    public static class Globals
    {
        public static GameSettings GameSettings { get; private set; }

        public static void Initialize()
        {
            GameSettings = new GameSettings();
            GameSettings.CreateSettingsFileIfNotExists();
            GameSettings.LoadSettings();
        }
    }
}
