using UnityModManagerNet;

namespace Gauge
{
    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        [Draw("Rail gauge (requires restart)")]
        public Gauge gauge = Gauge.ThreeFootNarrow;

        [Draw("Switch Type (requires restart)")]
        public SwitchType switchType = SwitchType.Dynamic;

        [Draw("Adjust Ballast Width (requires restart)")]
        public bool adjustBallastWidth = true;

        public void OnChange()
        {
            // yup
        }

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }

    public enum SwitchType
    {
        Dynamic,
        Modified
    }
}
