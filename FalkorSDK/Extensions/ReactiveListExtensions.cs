namespace FalkorSDK.Extensions
{
    using System;

    using ReactiveUI;

    public static class ReactiveListExtensions
    {
         public static ReactiveList<T> Create<T>(this ReactiveList<T> value)
        {
             foreach (var x in Enum.GetValues(typeof(T)))
             {
                value.Add((T)x);
             }

             return value;
        }
    }
}