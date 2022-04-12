namespace api.iapetus11.me.Extensions;

public static class DictionaryExtensions
{
    public static void UpdateWith<TKey, TValue>(this IDictionary<TKey, TValue> dictA, IDictionary<TKey, TValue> dictB)
    {
        foreach (var (key, value) in dictB)
        {
            dictA[key] = value;
        }
    }
}