namespace Visualizer.Converters
{
    interface IConverter<TFrom, TTo>
    {
        TTo Convert(TFrom bitmap);
    }
}
