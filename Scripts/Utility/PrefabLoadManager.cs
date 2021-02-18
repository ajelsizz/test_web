using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
//using ToolKit.Patcher;


public class PrefabLoadManager : MonoBehaviour
{

    static PrefabLoadManager _instance;
    public static PrefabLoadManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject();
                go.name = "PrefabLoadManager";
                go.AddComponent<PrefabLoadManager>();
                _instance = go.GetComponent<PrefabLoadManager>();
            }
            return _instance;
        }
    }

    public static PrefabLoadManager GetPrefabLoadManager()
    {
        return _instance;
    }    

    const string sdSpinePathBase = "Spine/SD_Spine/";
    const string spSpinePathBase = "Spine/SP_Spine/";

    //const string idleMapPathBase = "IdleMap/";
    //const string skillEffectPathBase = "Effect/SkillEffect/";
    //const string weatherEffectPathBase = "Effect/Weather/";

    void OnDestroy()
    {
        _instance = null;
    }

	private bool _isUseAssetBundle = false;

    public GameObject GetObject(string path)
    {
        GameObject retrunObject = null;
       // string pathBase = "Prefabs/";

        if (_isUseAssetBundle)
        {
            return retrunObject;
        }

        try
        {
            //retrunObject = Instantiate(Resources.Load(pathBase + path, typeof(GameObject))) as GameObject;
            retrunObject = Instantiate(Resources.Load(path, typeof(GameObject))) as GameObject;

        }

        catch
        {
            UnityEngine.Debug.LogError("Not Found Object. Object Name : " + path);
            return null;
        }
        return retrunObject;
    }

    //public GameObject GetObject(string path, Vector3 pos)
    //{
    //    GameObject retrunObject = null;
    //    string pathBase = "Prefabs/";

    //    if (_isUseAssetBundle)
    //    {
    //        return retrunObject;
    //    }

    //    try
    //    {
    //        retrunObject = Instantiate(Resources.Load(pathBase + path, typeof(GameObject)), pos, Quaternion.identity) as GameObject;
    //    }
    //    catch
    //    {
    //        UnityEngine.Debug.LogError("Not Found Object. Object Name : " + pathBase + path);
    //        return null;
    //    }
    //    return retrunObject;
    //}




	public GameObject ReturnOnlyObject(string path)
	{
		GameObject retrunObject = null;
		string pathBase = "Prefabs/";

		if (_isUseAssetBundle)
		{
			return retrunObject;
		}

		try
		{
			retrunObject = Resources.Load(pathBase + path, typeof(GameObject)) as GameObject;
        }
		catch
		{
			UnityEngine.Debug.LogError("Not Found Object. Object Name : " + pathBase + path);
			return null;
		}
		return retrunObject;
	}

    public GameObject GetSPSpineObject(string spineName)
    {
        GameObject go = GetObjectAssetBundle(spineName);
        if (go == null)
        {
            //    go = GetObject(spSpinePathBase + spineName + "/" + spineName);
            go = GetObject(spineName);
        }

        return go;
    }

    public SkeletonAnimation GetSPSpineAnimation(string spineName)
    { 
        GameObject go = GetSPSpineObject(spineName);
        
        SkeletonAnimation sa = null;
        if (go != null)
        {
            //  UISetter.SetActive(go, true);
            go.SetActive(true);
            sa = go.GetComponent<SkeletonAnimation>();
        }
            

        return sa;
    }



    GameObject GetObjectAssetBundle(string spineName)
    {
#if ASSETBUNDLE_DOWN
        AssetBundle assetBundle = AssetBundleManager.LoadFromFile(spineName, type);
        if (assetBundle != null)
        {
            GameObject go = assetBundle.LoadAsset<GameObject>(spineName + ".prefab");
#if UNITY_EDITOR
            foreach (Renderer smr in go.GetComponentsInChildren<Renderer>(true))
            {
                if (smr.sharedMaterial == null)
                    continue;

                smr.sharedMaterial.shader = Shader.Find(smr.sharedMaterial.shader.name);
            }
#endif
            return Instantiate(go);
        }
#endif

        return null;        
    }


    public GameObject GetSDSpineOnlyObject(string spineName)
    {
        GameObject go = null;

//        AssetBundle assetBundle = AssetBundleManager.LoadFromFile(spineName, ECacheType.Spine);
//        if (assetBundle != null)
//        {
//            go = assetBundle.LoadAsset<GameObject>(spineName + ".prefab");
//#if UNITY_EDITOR
//            foreach (Renderer smr in go.GetComponentsInChildren<Renderer>(true))
//            {
//                if (smr.sharedMaterial == null)
//                    continue;

//                smr.sharedMaterial.shader = Shader.Find(smr.sharedMaterial.shader.name);
//            }
//#endif
//        }

        if(go == null)        
        {
            string path = string.Format("Prefabs/{0}{1}/{2}", sdSpinePathBase, spineName, spineName);
            Debug.Log(path);
            go = Resources.Load(path, typeof(GameObject)) as GameObject;            
            if (!go.activeSelf)
                go.SetActive(true);
        }

        return go;
    }

    //    public GameObject GetTileEffectOnlyObject(string effectName)
    //    {
    //        GameObject go = null;

    //        AssetBundle assetBundle = AssetBundleManager.LoadFromFile(effectName, ECacheType.TileEffect);
    //        if (assetBundle != null)
    //        {
    //            go = assetBundle.LoadAsset<GameObject>(effectName + ".prefab");
    //#if UNITY_EDITOR
    //            foreach (Renderer smr in go.GetComponentsInChildren<Renderer>(true))
    //            {
    //                if (smr.sharedMaterial == null)
    //                    continue;

    //                smr.sharedMaterial.shader = Shader.Find(smr.sharedMaterial.shader.name);
    //            }
    //#endif
    //        }

    //        if (go == null)
    //        {
    //            string path = string.Format("Prefabs/{0}{1}", skillEffectPathBase, effectName);
    //            go = Resources.Load(path, typeof(GameObject)) as GameObject;

    //            if (go != null)
    //            {
    //                if (!go.activeSelf)
    //                    UISetter.SetActive(go, true);
    //            }
    //        }
    //        return go;
    //    }

    //    public GameObject GetSkillEffectOnlyObject(string effectName)
    //    {
    //        GameObject go = null;

    //        AssetBundle assetBundle = AssetBundleManager.LoadFromFile(effectName, ECacheType.SKillEffect);
    //        if (assetBundle != null)
    //        {
    //            go = assetBundle.LoadAsset<GameObject>(effectName + ".prefab");
    //#if UNITY_EDITOR
    //            foreach (Renderer smr in go.GetComponentsInChildren<Renderer>(true))
    //            {
    //                if (smr.sharedMaterial == null)
    //                    continue;

    //                smr.sharedMaterial.shader = Shader.Find(smr.sharedMaterial.shader.name);
    //            }
    //#endif
    //        }

    //        if(go == null)        
    //        {
    //            string path = string.Format("Prefabs/{0}{1}", skillEffectPathBase, effectName);
    //            go = Resources.Load(path, typeof(GameObject)) as GameObject;

    //            if(go != null)
    //            {
    //                if (!go.activeSelf)
    //                    UISetter.SetActive(go, true);
    //            }
    //        }
    //        return go;
    //    }

    public GameObject GetSDSpineObject(string spineName)
    {
        GameObject go = GetObjectAssetBundle(spineName);
        if (go == null)
        {
            go = GetObject(sdSpinePathBase + spineName + "/" + spineName);
        }

        return go;
    }

    //public GameObject GetIdleMapObject(string mapName)
    //{
    //    GameObject go = GetObjectAssetBundle(mapName, ECacheType.IdleMap);
    //    if (go == null)
    //    {
    //        go = GetObject(idleMapPathBase + mapName);
    //    }

    //    return go;
    //}

    //public GameObject GetIdleWeatherObject(string weatherName)
    //{
    //    GameObject go = GetObjectAssetBundle(weatherName, ECacheType.IdleMap);
    //    if(go == null)
    //    {
    //        go = GetObject(weatherEffectPathBase + weatherName);
    //    }

    //    return go;
    //}

    public SkeletonAnimation GetSDSpineAnimation(string spineName)
    {
        GameObject go = GetSDSpineObject(spineName);

        SkeletonAnimation sa = null;
        if (go != null)
            sa = go.GetComponent<SkeletonAnimation>();

        return sa;
    }

    public void GetSDSpineAnimation(string spineName,System.Action<SkeletonAnimation> result)
    {
        GameObject spineObject = PrefabLoadManager.Instance.GetSDSpineObject(spineName);
        SkeletonAnimation sa = null;
        if (spineObject != null)
            sa = spineObject.GetComponent<SkeletonAnimation>();

        if (result != null)
            result(sa);
    }


}
