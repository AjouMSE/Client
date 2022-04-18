using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Action = System.Action;
using UnityEditor;
using Utils;

public partial class TableLoader : MonoSingleton<TableLoader>
{
    protected bool _IsLoaded = false;

    protected int _CurrentStep;
    protected int _CompleteStep;

    protected Action _CompleteEvent;
    protected Action _UpdateEvent;

    protected Dictionary<string, KeyValuePair<string, string>> _tableHashs;

    private string m_Clue = string.Empty;
    private bool m_isLoadFail = false;

    Dictionary<string, UnityEngine.Object> cachedResources = new Dictionary<string, UnityEngine.Object>();

    string localPath = "Assets/Table";

    private void UpdateStep()
    {
        ++_CurrentStep;

        if (_UpdateEvent != null)
        {
            _UpdateEvent();
        }
    }

    public void LoadTableData()
    {
        _CurrentStep = 0;
        _CompleteStep = 0;

        LoadTableDataProcess(null);
    }

    public void LoadTableDataProcess(System.Action complete)
    {
        StartCoroutine(LoadTableDataProcess());
        _IsLoaded = true;

        if (null != complete)
            complete();
    }

    private List<object> GetDataList(string fileName)
    {
        return GetDataListFromFile(fileName);
    }

    private List<object> GetDataListFromFile(string path)
    {
        if (m_isLoadFail)
            return null;

        TextAsset texts = Load<TextAsset>(path);

        if (texts == null)
            return null;

        string jsonString = texts.text;

        Dictionary<string, object> jsonData = MiniJSON.Json.Deserialize(jsonString) as Dictionary<string, object>;

        if (jsonData.ContainsKey("data"))
        {
            return jsonData["data"] as List<object>;
        }
        else
        {
            return null;
        }
    }

    public T Load<T>(string fileName) where T : UnityEngine.Object
    {
        T obj = AssetDatabase.LoadAssetAtPath<T>(string.Format(string.Format("{0}/{1}.bytes", localPath, fileName)));

        if (obj != null)
        {
            cachedResources.Add(fileName, obj);
        }
        else
            Debug.LogError(string.Format("{0} 경로에 {1}파일이 없습니다.", localPath, fileName));

        return obj;
    }
}