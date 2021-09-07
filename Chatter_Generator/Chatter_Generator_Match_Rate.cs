using System;
using System.Numerics;
using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using OpenTabletDriver.Plugin.Output;

namespace Chatter_Generator
{
    [PluginName("Chatter Generator Match Rate")]
    public class Chatter_Generator_Match_Rate : IPositionedPipelineElement<IDeviceReport>
    {
        private float intensity;
        private float intensityOffset;
        private static readonly Random seed = new Random();

        public Vector2 CalculateOffset()
        {
            return new Vector2
            (
                (float)seed.NextDouble() * intensity - intensityOffset,
                (float)seed.NextDouble() * intensity - intensityOffset
            );
        }

        public event Action<IDeviceReport> Emit;

        public void Consume(IDeviceReport value)
        {
            if (value is ITabletReport report)
            {
                report.Position = report.Position + CalculateOffset();
                value = report;
            }

            Emit?.Invoke(value);
        }

        public PipelinePosition Position => PipelinePosition.PostTransform;

        [Property("Chatter Intensity"), DefaultPropertyValue(10f), ToolTip("The amount of chatter to be added.")]
        public float Intensity
        {
            get => intensity;
            set
            {
                intensity = value;
                intensityOffset = value / 2;
            }
        }
    }
}
