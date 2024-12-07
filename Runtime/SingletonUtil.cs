using UnityEngine;

namespace XO.Sequencer.Runtime
{
    public class SingletonUtil<T> : MonoBehaviour where T : SingletonUtil<T>
    {
        private static volatile T _instance = null;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType(typeof(T)) as T;
                    if (_instance == null)
                    {
                        var go = new GameObject();
                        go.AddComponent(typeof(T));
                        _instance = go.GetComponent<T>();
                        go.name = _instance.GetType().Name + " (Singleton)";
                    }
                }

                return _instance;
            }
            protected set => _instance = value;
        }
    }
}