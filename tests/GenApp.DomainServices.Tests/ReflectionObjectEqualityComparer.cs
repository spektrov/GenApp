namespace GenApp.DomainServices.Tests;
public class ReflectionObjectEqualityComparer<T> : IEqualityComparer<T>
{
    public bool Equals(T x, T y)
    {
        return typeof(T).GetFields()
            .All(f => object.Equals(f.GetValue(x), f.GetValue(y)));
    }

    public int GetHashCode(T obj)
    {
        return typeof(T).GetFields()
            .Select(f => f.GetValue(obj))
            .Aggregate(0, (a, b) => unchecked(a * 31 + (b?.GetHashCode() ?? 0)));
    }
}