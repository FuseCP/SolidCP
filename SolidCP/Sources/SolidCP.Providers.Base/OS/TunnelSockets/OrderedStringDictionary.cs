using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.Text;

namespace SolidCP.Providers
{

    public class OrderedStringDictionary : OrderedDictionary, IDictionary<string, string?>
    {
        public class StringCollection<T>: ICollection<T>
        {
            ICollection BaseCollection;
            public StringCollection(ICollection baseCollection) { BaseCollection = baseCollection; }

            public int Count => BaseCollection.Count;

            public bool IsReadOnly => true;

            public void Add(T item) => throw new NotSupportedException("Add not supported on collection");
            public void Clear() => throw new NotSupportedException("Clear not supported on collection");

            public bool Contains(T item) => throw new NotSupportedException("Contains not supported on collection");

            public void CopyTo(T[] array, int arrayIndex) => BaseCollection.CopyTo(array, arrayIndex);

            public IEnumerator<T> GetEnumerator()
            {
                foreach (T item in BaseCollection) yield return item;
            }

            public bool Remove(T item)
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator() => BaseCollection.GetEnumerator();
        }

        public OrderedStringDictionary() : base() { }

        public virtual string? this[string key]
        {
            get => base[key] as string;
            set
            {
                if (value == null) Remove(key);
                else base[key] = value;
            }
        }

        public virtual ICollection<string> Keys => new StringCollection<string>(base.Keys);

        public virtual ICollection<string?> Values => new StringCollection<string?>(base.Values);

        public virtual void Add(string key, string? value) => base.Add(key, value);


        public virtual void Add(KeyValuePair<string, string?> item) => base.Add(item.Key, item.Value);

        public virtual bool Contains(KeyValuePair<string, string?> item) => this[item.Key] == item.Value;

        public virtual bool ContainsKey(string key) => base.Contains(key);

        public virtual void CopyTo(KeyValuePair<string, string?>[] array, int arrayIndex)
        {
            foreach (DictionaryEntry item in ((IEnumerable)this))
            {
                if (arrayIndex >= array.Length) break;
                var keyValuePair = new KeyValuePair<string, string?>((string)item.Key, (string?)item.Value);
                array[arrayIndex++] = keyValuePair;
            }
        }

        public virtual bool Remove(string key)
        {
            var hasKey = base.Contains(key);
            if (hasKey) base.Remove(key);
            return hasKey;
        }
        public virtual bool Remove(KeyValuePair<string, string?> item) => Remove(item.Key);

        public virtual bool TryGetValue(string key, out string? value)
        {
            if (base.Contains(key))
            {
                value = (string?)base[key];
                return true;
            } else
            {
                value = null;
                return false;
            }
        }

        public virtual IEnumerator<KeyValuePair<string, string?>> GetEnumerator()
        {
            foreach (DictionaryEntry item in ((IEnumerable)this)) {
                yield return new KeyValuePair<string, string?>((string)item.Key, (string?)item.Value);
            }
        }
    }
}
