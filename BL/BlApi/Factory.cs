namespace BlApi;
/// <summary>
/// use the Config class to load and initialize the correct class with which we would like to implement the data layer.
/// </summary>
public static class Factory
{
    public static IBl Get() => new BlImplementation.Bl();
}
