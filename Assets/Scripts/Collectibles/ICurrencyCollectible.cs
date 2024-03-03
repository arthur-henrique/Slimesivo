public interface ICurrencyCollectible
{
    int Value { get; }
    string CurrencyId { get; }

    void Collect();
}