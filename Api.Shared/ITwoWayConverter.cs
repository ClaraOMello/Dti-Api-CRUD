namespace Api.Shared
{
    public interface ITwoWayConverter<TTYpe, TOtherType>
    {
        TTYpe Convert(TOtherType t);

        TOtherType Convert(TTYpe t);
    }
}
