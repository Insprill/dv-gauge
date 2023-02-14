using UnityEngine;
using UnityModManagerNet;

namespace Gauge
{
    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        [Draw("Rail gauge (requires restart)")]
        public Gauge gauge = Gauge.ThreeFootNarrow;

        [Draw("Switch Type (requires restart)")]
        [Tooltip("Dynamic switches are smoother but don't have points or frogs.")]
        public SwitchType switchType = SwitchType.Modified;

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
        Modified,
    }
}
