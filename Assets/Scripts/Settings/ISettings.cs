namespace Settings
{
    public interface ISettings: IDamagerSettings, IUpdaterSettings
    { 
        
    }

    public interface IUpdaterSettings
    {
        public float DeformationSpeed { get; }
    }

    public interface IDamagerSettings
    {
        public float ImpulseMaximumThreshold { get; }
        public float ImpulseMinimumThreshold { get; }
        public float Radius { get; }
        public float Multiplier { get; }
    }
}