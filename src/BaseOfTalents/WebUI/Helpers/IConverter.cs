namespace WebUI.Helpers
{
    public interface IConverter<in TIn, out TOut>
    {
        TOut Convert(TIn data);
    }
}
