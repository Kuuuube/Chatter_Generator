using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using OpenTabletDriver.Plugin.Tablet.Interpolator;
using OpenTabletDriver.Plugin.Timers;
using System;
using System.Numerics;

namespace Parkinsons
{
    [PluginName("Parkinsons")]
    public class Parkinsons : Interpolator
    {

        public Parkinsons(ITimer scheduler) : base(scheduler) { }

        private static readonly Random seed = new Random();
        private Vector2 targetPos, calcTarget;
        private SyntheticTabletReport report;


        public override void UpdateState(SyntheticTabletReport report)
        {
            this.targetPos = report.Position;
            calcTarget = targetPos;

            this.report = report;
        }

        public override SyntheticTabletReport Interpolate()
        {
            this.report.Position = Filter(this.calcTarget);
            return this.report;
        }

        public Vector2 Filter(Vector2 point)
        {
            point.X += seed.Next(intensity*100) - offset;
            point.Y += seed.Next(intensity*100) - offset;
            return point;
        }

        public FilterStage FilterStage => FilterStage.PostTranspose;

        private int intensity;
        private int offset;
        [Property("Intensity")]
        public int Intensity
        {
            get => intensity;
            set
            {
                intensity = value;
                offset = value / 2;
            }
        }
        public float TimerInterval
        {
            get => 1000 / Hertz;
        }
    }
}