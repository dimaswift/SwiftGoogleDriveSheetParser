using System;
using System.IO;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace SwiftGoogleSheetParser
{
    public abstract class SheetParserContainer : DownloadableObject
    {
        public enum Type
        {
            TSV, CSV
        }

        [SerializeField] protected Type _type;
        
        [SerializeField] string _url = "";

        string FilePath
        {
            get
            {
                return  Application.persistentDataPath + "/" + name + Extension;
            }
        }
        
        string Extension
        {
            get
            {
                return  _type == Type.CSV ? ".csv" : ".tsv";
            }
        }

        public bool Downloaded { get; set; }
        
        Action<bool> _downloadCallback;

        public override void StartDownloading()
        {
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                try
                {
                    ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
                    var myWebClient = new WebClient();
                    var path = FilePath;
                    myWebClient.DownloadFileAsync(new Uri(_url), path);
                    myWebClient.DownloadFileCompleted += OnDownloaded;
                }
                catch
                {
                    OnDownloaded(null, null);
                }
            }
            else
            {
                
                OnDownloaded(null, null);
            }
        }
        
        bool MyRemoteCertificateValidationCallback(System.Object sender,
            X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            bool isOk = true;
            if (sslPolicyErrors != SslPolicyErrors.None) 
            {
                for (int i=0; i<chain.ChainStatus.Length; i++) 
                {
                    if (chain.ChainStatus[i].Status == X509ChainStatusFlags.RevocationStatusUnknown) 
                    {
                        continue;
                    }
                    chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                    chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                    chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan (0, 1, 0);
                    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                    bool chainIsValid = chain.Build ((X509Certificate2)certificate);
                    if (!chainIsValid)
                    {
                        isOk = false;
                        break;
                    }
                }
            }
            return isOk;
        }

        public override void StartDownloading(Action<bool> downloadCallback)
        {
            _downloadCallback = downloadCallback;
            StartDownloading();
        }

        protected abstract ICustomStringParser[] GetCustomParsers();
        
        protected abstract void ParseSheet(string[] rows);

        protected string GetSeparator()
        {
            return _type == Type.CSV ? "," : "\t";
        }
        
        public void ParseLocalFile()
        {
            if(!File.Exists(FilePath))
                return;
            Debug.Log(File.ReadAllText(FilePath));
            var cvsText = File.ReadAllLines(FilePath).ToArray();
            ParseSheet(cvsText);
            SetFileDirty();
        }

        public void CheckDownload()
        {
            if (Downloaded)
            {
                ParseLocalFile();
                Downloaded = false;
            }
        }
        
        public void OnDownloaded(object sender, AsyncCompletedEventArgs e)
        {
            Downloaded = e != null && e.Error == null;
            if (!Application.isEditor)
            {
                Dispatcher.RunOnMainThread(() => _downloadCallback(Downloaded));
                return;
            }

            if (_downloadCallback != null)
                _downloadCallback.Invoke(Downloaded);
            
        }

        protected void SetFileDirty()
        {
#if UNITY_EDITOR
                
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }
    }
}

