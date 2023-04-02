namespace Settings
{
    public interface ISettings: IDamagerSettings
    { 
        public float ImpulseMaximumThreshold { get; }
        public float ImpulseMinimumThreshold { get; }
    }

    public interface IDamagerSettings
    {
        public float Radius { get; }
        public float Multiplier { get; }
    }
}