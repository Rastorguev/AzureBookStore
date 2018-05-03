using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace BookStore.Utils
{
    public class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
    {
        private readonly IEnumerable<TElement> _values;

        public Grouping(TKey key, [NotNull] IEnumerable<TElement> values)
        {
            Key = key;
            this._values = values;
        }

        public TKey Key { get; }

        public IEnumerator<TElement> GetEnumerator() => _values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}