using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.Text;

namespace SolidCP.Providers
{

    public class OrderedNameDictionary<T> : OrderedDictionary, IDictionary<string, T> where T: class
    {
        public class TypedCollection<U>: ICollection<U>
        {
            ICollection BaseCollection;
            public TypedCollection(ICollection baseCollection) { BaseCollection = baseCollection; }

            public int Count => BaseCollection.Count;

            public bool IsReadOnly => true;

            public void Add(U item) => throw new NotSupportedException("Add not supported on collection");
            public void Clear() => throw new NotSupportedException("Clear not supported on collection");

            public bool Contains(U item) => throw new NotSupportedException("Contains not supported on collection");

            public void CopyTo(U[] array, int arrayIndex) => BaseCollection.CopyTo(array, arrayIndex);

            public IEnumerator<U> GetEnumerator()
            {
                foreach (U item in BaseCollection) yield return item;
            }

            public bool Remove(U item)
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator() => BaseCollection.GetEnumerator();
        }

        public OrderedNameDictionary() : base() { }
        public OrderedNameDictionary(StringComparer comparer): base(comparer) { }

        public virtual T this[string key]
        {
            get => base[key] as T;
            set
            {
                if (value == null) Remove(key);
                else base[key] = value;
            }
        }

        public new virtual ICollection<string> Keys => new TypedCollection<string>(base.Keys);

        public new virtual ICollection<T> Values => new TypedCollection<T>(base.Values);

        public virtual void Add(string key, T value) => base.Add(key, value);


        public virtual void Add(KeyValuePair<string, T> item) => base.Add(item.Key, item.Value);

        public virtual bool Contains(KeyValuePair<string, T> item) => Contains(item.Key);

        public virtual bool ContainsKey(string key) => base.Contains(key);

        public virtual void CopyTo(KeyValuePair<string, T>[] array, int arrayIndex)
        {
            foreach (DictionaryEntry item in ((IEnumerable)this))
            {
                if (arrayIndex >= array.Length) break;
                var keyValuePair = new KeyValuePair<string, T>((string)item.Key, item.Value as T);
                array[arrayIndex++] = keyValuePair;
            }
        }

        public virtual bool Remove(string key)
        {
            var hasKey = base.Contains(key);
            if (hasKey) base.Remove(key);
            return hasKey;
        }
        public virtual bool Remove(KeyValuePair<string, T> item) => Remove(item.Key);

        public virtual bool TryGetValue(string key, out T value)
        {
            if (base.Contains(key))
            {
                value = base[key] as T;
                return true;
            } else
            {
                value = null;
                return false;
            }
        }

        public new IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            foreach (DictionaryEntry item in ((IEnumerable)this)) {
                yield return new KeyValuePair<string, T>((string)item.Key, item.Value as T);
            }
        }
    }
}
