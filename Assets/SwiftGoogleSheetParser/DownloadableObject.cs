using System;
using UnityEngine;

namespace SwiftGoogleSheetParser
{
    public abstract class DownloadableObject : ScriptableObject, ILoadable
    {
        public bool IsDownloading { get; set; }
        
        public abstract void StartDownloading(Action<bool> downloadCallback);

        public abstract void StartDownloading();

        public void Load(Action onLoad)
        {
            StartDownloading(succ => onLoad());
        }
    }
} 