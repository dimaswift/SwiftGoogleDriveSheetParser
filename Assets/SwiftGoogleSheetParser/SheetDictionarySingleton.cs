using UnityEngine;

namespace SwiftGoogleSheetParser
{
    public abstract class SheetDictionarySingleton<T, TM> : SheetDictionary<T> where T : class, IUniqueItem, new() where TM : SheetDictionarySingleton<T, TM>
    {
        static TM _instance;

        public static TM Instance
        {
            get
            {
                if (_instance == null)
                {
                    var i = CreateInstance<TM>();
                    i.name = typeof(TM).Name;
                    var path = i.ResourcesFolder;
                    
                    if(Application.isEditor)
                        DestroyImmediate(i);
                    else  Destroy(i);
                    
                    _instance = Resources.Load<TM>(path);
                    
                    if (_instance == null)
                    {
                        Debug.LogError("Cannot find instance of " + typeof(T).Name + " under path: {path}!");
                    }
                }

                return _instance;
            }
        }
        
        protected abstract string ResourcesFolder { get; }

    }
}

