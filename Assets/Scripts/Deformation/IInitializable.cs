public interface IInitializable<in T>
{
    public void Initialize(T value);
}

public interface IInitializable<in T1, in T2>
{
    public void Initialize(T1 value1, T2 value2);
}
