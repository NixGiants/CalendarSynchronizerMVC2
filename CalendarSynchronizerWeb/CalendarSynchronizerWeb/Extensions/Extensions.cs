

namespace CalendarSynchronizerWeb.Extensions
{
    public static class Extensions
    {
        public static T GetService<T>(this IServiceProvider serviceProvider)
            where T : class
        {
            return serviceProvider.GetService(typeof(T)) as T;
        }

        public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> source, int size = 500)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException("size");

            var section = new List<T>(size);
            foreach (var item in source)
            {
                section.Add(item);

                if (section.Count == size)
                {
                    yield return section.AsReadOnly();
                    section = new List<T>(size);
                }
            }

            if (section.Count > 0)
                yield return section.AsReadOnly();
        }

        public static async Task<IEnumerable<TResult>> SelectAsync<TSource, TResult>(
            this IEnumerable<TSource> source, Func<TSource, Task<TResult>> method,
            int concurrency = int.MaxValue)
        {
            var semaphore = new SemaphoreSlim(concurrency);
            try
            {
                return await Task.WhenAll(source.Select(async s =>
                {
                    try
                    {
                        await semaphore.WaitAsync();
                        return await method(s);
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }));
            }
            finally
            {
                semaphore.Dispose();
            }
        }

        public static IEnumerable<TResult> ZipEqual<TFirst, TResult>(this IEnumerable<TFirst> first, IEnumerable<TFirst> second, IComparer<TFirst> comparer, Func<TFirst, TFirst, TResult> resultSelector)
        {
            var enumerator1 = first.GetEnumerator();
            var enumerator2 = second.GetEnumerator();

            var enumerator1HasElement = enumerator1.MoveNext();
            var enumerator2HasElement = enumerator2.MoveNext();

            while (enumerator1HasElement || enumerator2HasElement)
            {
                if (!enumerator2HasElement)
                {
                    yield return resultSelector(enumerator1.Current, default(TFirst));
                    enumerator1HasElement = enumerator1.MoveNext();
                }
                else if (!enumerator1HasElement)
                {
                    yield return resultSelector(default(TFirst), enumerator2.Current);
                    enumerator2HasElement = enumerator2.MoveNext();
                }
                else
                {
                    var compareResult = comparer.Compare(enumerator1.Current, enumerator2.Current);
                    if (compareResult == 0)
                    {
                        yield return resultSelector(enumerator1.Current, enumerator2.Current);
                        enumerator1HasElement = enumerator1.MoveNext();
                        enumerator2HasElement = enumerator2.MoveNext();
                    }
                    else if (compareResult < 0)
                    {
                        yield return resultSelector(enumerator1.Current, default(TFirst));
                        enumerator1HasElement = enumerator1.MoveNext();
                    }
                    else
                    {
                        yield return resultSelector(default(TFirst), enumerator2.Current);
                        enumerator2HasElement = enumerator2.MoveNext();
                    }
                }
            }
        }
        public static IEnumerable<TResult> ZipEqual<TSource, TTarget, TResult>(this IEnumerable<TSource> first, IEnumerable<TTarget> second, IComparer<object> comparer, Func<TSource, TTarget, TResult> resultSelector)
        {
            var enumerator1 = first.GetEnumerator();
            var enumerator2 = second.GetEnumerator();

            var enumerator1HasElement = enumerator1.MoveNext();
            var enumerator2HasElement = enumerator2.MoveNext();

            while (enumerator1HasElement || enumerator2HasElement)
            {
                if (!enumerator2HasElement)
                {
                    yield return resultSelector(enumerator1.Current, default(TTarget));
                    enumerator1HasElement = enumerator1.MoveNext();
                }
                else if (!enumerator1HasElement)
                {
                    yield return resultSelector(default(TSource), enumerator2.Current);
                    enumerator2HasElement = enumerator2.MoveNext();
                }
                else
                {
                    var compareResult = comparer.Compare(enumerator1.Current, enumerator2.Current);
                    if (compareResult == 0)
                    {
                        yield return resultSelector(enumerator1.Current, enumerator2.Current);
                        enumerator1HasElement = enumerator1.MoveNext();
                        enumerator2HasElement = enumerator2.MoveNext();
                    }
                    else if (compareResult < 0)
                    {
                        yield return resultSelector(enumerator1.Current, default(TTarget));
                        enumerator1HasElement = enumerator1.MoveNext();
                    }
                    else
                    {
                        yield return resultSelector(default(TSource), enumerator2.Current);
                        enumerator2HasElement = enumerator2.MoveNext();
                    }
                }
            }
        }
        public static string ToFullString(this Exception source)
        {
            if (source == null) throw new ArgumentNullException("Source");
            string result = source.ToString();
            return result.Replace(" --->", Environment.NewLine + " --->");
        }

        public static TResult GetValueOrDefaultLazy<TKey, TResult>(this IDictionary<TKey, TResult> container, TKey key, Func<TKey, TResult> def, bool add = false)
        {
            if (container == null || !container.ContainsKey(key))
            {
                var defV = def(key);
                if (add)
                {
                    container[key] = defV;
                }
                return defV;
            }

            return container[key];
        }

        public static TResult GetValueOrDefault<TKey, TResult>(this IDictionary<TKey, TResult> container, TKey key, TResult def = default(TResult), bool add = false)
        {
            if (container == null || !container.ContainsKey(key))
            {
                var defV = def;
                if (add)
                {
                    container[key] = defV;
                }
                return defV;
            }

            return container[key];
        }
    }
}
