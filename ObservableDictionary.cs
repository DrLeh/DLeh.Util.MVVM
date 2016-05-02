using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLeh.Util.MVVM
{
    public class ObservableDictionary<T, S> : ViewModelBase, IDictionary<T, S>
    {
        public ObservableDictionary()
        {
            theDict = new Dictionary<T, S>();
        }
        public ObservableDictionary(Dictionary<T, S> dict)
        {
            theDict = dict;
        }

        public static implicit operator ObservableDictionary<T, S>(Dictionary<T, S> dict)
        {
            return new ObservableDictionary<T, S>(dict);
        }
        public static implicit operator Dictionary<T, S>(ObservableDictionary<T, S> dict)
        {
            return dict.GetInternalDictionary();
        }

        public Dictionary<T, S> GetInternalDictionary()
        {
            return theDict;
        }



        #region IDictionary<T,S>

        private Dictionary<T, S> _theDict;
        Dictionary<T, S> theDict { get { return _theDict; } set { _theDict = value; NotifyPropertyChanged(); } }

        public void Add(T key, S value)
        {
            theDict.Add(key, value);
            NotifyPropertyChanged(key.ToString());
            NotifyPropertyChanged(() => theDict);
        }

        public bool ContainsKey(T key)
        {
            return theDict.ContainsKey(key);
        }

        public ICollection<T> Keys
        {
            get { return theDict.Keys; }
        }

        public bool Remove(T key)
        {
            var result = theDict.Remove(key);
            NotifyPropertyChanged(key.ToString());
            NotifyPropertyChanged(() => theDict);
            return result;
        }

        public bool TryGetValue(T key, out S value)
        {
            return theDict.TryGetValue(key, out value);
        }

        public ICollection<S> Values
        {
            get { return theDict.Values; }
        }

        public S this[T key]
        {
            get
            {
                if (this.ContainsKey(key))
                    return theDict[key];
                return default(S);
            }
            set
            {
                theDict[key] = value;
                NotifyPropertyChanged(key.ToString());
                NotifyPropertyChanged(() => theDict);
            }
        }

        public void Add(KeyValuePair<T, S> item)
        {
            theDict[item.Key] = item.Value;
            NotifyPropertyChanged(() => theDict);
        }

        public void Clear()
        {
            theDict.Clear();
            foreach (var key in this.Keys)
                NotifyPropertyChanged(key.ToString());
            NotifyPropertyChanged(() => theDict);
        }

        public bool Contains(KeyValuePair<T, S> item)
        {
            return theDict.Contains(item);
        }

        public void CopyTo(KeyValuePair<T, S>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return theDict.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<T, S> item)
        {
            var res = theDict.Remove(item.Key);
            NotifyPropertyChanged(() => item.Key);
            NotifyPropertyChanged(() => theDict);
            return res;
        }

        public IEnumerator<KeyValuePair<T, S>> GetEnumerator()
        {
            return theDict.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return theDict.GetEnumerator();
        }
        #endregion IDictionary<T,S>
    }
}
