using UnityEngine;

namespace SwiftGoogleSheetParser
{
    public abstract class SheetSingleton<T, TM> : SheetParserContainer where T : new() where TM : SheetSingleton<T, TM>
    {
        public static T Main
        {
            get
            {
                return Instance != null ? Instance._localInstance : default(T);
            }
        }

        static TM _instance;

        static TM Instance
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
                        Debug.LogError("Cannot find instance of " +  typeof(T).Name + " under path: " + path);
                    }
                }

                return _instance;
            }
        }

        protected abstract string ResourcesFolder { get; }
        
        [SerializeField] T _localInstance;
        
        protected override void ParseSheet(string[] rows)
        {
            _localInstance = SheetParser.ParseToObject(_localInstance, rows, GetSeparator(), GetCustomParsers());
        }
    }
}

