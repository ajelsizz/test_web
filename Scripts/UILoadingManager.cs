using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Loading
{
    static public readonly string Title = "TITLE_SCENE_N";
    static public readonly string Battle = "BATTLE_SCENE";
    static public readonly string Lobby = "LOBBY_SCENE_N";
    static public readonly string Shop = "SHOP_SCENE";

    static public void Load(string nextSceneName = null)
    {
        string next = nextSceneName;
        if (string.IsNullOrEmpty(next))
        {
            next = Application.loadedLevelName;
        }
        UILoadingManager.Load(next);
    }
}

public class UILoadingManager : MonoBehaviour 
{
    static private readonly string _loadingSceneName = "LOADING_SCENE_N";
    static private string _nextSceneName = null;
    static private string _prevSceneName = null;


    static public void Load(string nextSceneName)
    {     
        if (string.IsNullOrEmpty(nextSceneName)) return;
        //        else if (nextSceneName == Loading.Init)
        //        {
        //            Application.LoadLevel(nextSceneName);
        //            return;
        //        }

        Debug.Log("a = " + _loadingSceneName);
        _nextSceneName = nextSceneName;
        Application.LoadLevel(_loadingSceneName);
    }

    void Start ()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        StartCoroutine("PreLoading");
    }

    IEnumerator PreLoading()
    {
        yield return new WaitForSeconds(1.5f);
        AsyncOperation sync = Application.LoadLevelAsync(_nextSceneName);
    }

    //IEnumerator SyncLoadScene()
    //{
    //  //  _nextNaviType = GetNavigationType();

    //    GameObject _loading = GameObject.Find("Loading");
    //    if (_loading != null)
    //        DestroyImmediate(_loading);

    //    GameObject loading = Utility.LoadAndInstantiateGameObject("Prefabs/UI/Loading");
    //    loading.transform.parent = root;
    //    loading.transform.localScale = Vector3.one;
    //    loading.transform.parent = null;
    //    UILoading uiLoaindg = loading.GetComponent<UILoading>();
    //    uiLoaindg.Set(_nextNaviType);
    //    uiLoaindg.In();
    //    DontDestroyOnLoad(loading);

    //    yield return new WaitForSeconds(0.5f);

    //    StartCoroutine(ToNext());
    //}
}
