using System;
using System.Numerics;
using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Output;
using OpenTabletDriver.Plugin.Tablet;

namespace Chatter_Generator
{
    [PluginName("Chatter Generator Variable Rate")]
    public class Chatter_Generator_Variable_Rate : AsyncPositionedPipelineElement<IDeviceReport>
    {
        private float intensity;
        private float intensityOffset;
        private Vector2 originalPos;
        private static readonly Random seed = new();

        public Vector2 CalculateOffset()
        {
            return new Vector2
            (
                (float)seed.NextDouble() * intensity - intensityOffset,
                (float)seed.NextDouble() * intensity - intensityOffset
            );
        }

        protected override void ConsumeState()
        {
            if (State is ITabletReport report)
            {
                originalPos = report.Position;
            }
            OnEmit();
        }

        protected override void UpdateState()
        {
            if (State is ITabletReport report)
            {
                report.Position = originalPos + CalculateOffset();
            }

            if (PenIsInRange() || State is not ITabletReport)
            {
                OnEmit();
            }
        }

        public override PipelinePosition Position => PipelinePosition.PostTransform;

        [Property("Chatter Intensity"), DefaultPropertyValue(10f), ToolTip 
        ("Chatter Generator Variable Rate:\n\n" + 
        "The amount of chatter to be added.")]
        public float Intensity
        {
            get => intensity;
            set
            {
                intensity = value;
                intensityOffset = value / 2;
            }
        }
        public float TimerInterval
        {
            get => 1000 / Frequency;
        }
    }
}