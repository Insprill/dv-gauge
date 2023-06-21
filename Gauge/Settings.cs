using BepInEx.Configuration;

namespace Gauge
{
    public class Settings
    {
        public readonly ConfigEntry<int> gauge;
        public readonly ConfigEntry<int> sleeperSpacing;
        public readonly ConfigEntry<bool> adjustBallastWidth;

        public Settings(ConfigFile config)
        {
            gauge = config.Bind("Gauge", "Width", 1435, "The gauge of the track, in millimeters.");
            sleeperSpacing = config.Bind("Gauge", "SleeperSpacing", 750, "The spacing between sleepers, in millimeters.");
            adjustBallastWidth = config.Bind("General", "AdjustBallastWidth", true, "Adjust the ballast width to match the track. May cause terrain holes around tunnels.");
        }
    }
}
