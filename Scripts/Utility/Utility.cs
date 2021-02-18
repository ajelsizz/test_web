using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//using DarkTonic.MasterAudio;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

#if UNITY_EDITOR
using UnityEditor;
#endif

//스태틱 메소드를 마구 몰아넣는다...
public static class Utility {

    static public readonly string AtlasPath = "Atlas/";

    public static void GoToTilte(bool isCachingClear = false)
    {   // 타이틀로
        if (isCachingClear == true) // 임시
        {
            CachingClear();
        }

        System.GC.Collect();
        Loading.Load(Loading.Title);
    }

    public static void CachingClear()
    {   // 캐싱데이터 지우기.
        bool success = Caching.ClearCache();

        if (success == false)
        {
            Debug.Log("Unable to clear cache");
        }
    }

    public static string Base64Decode(string data)
    {
        try
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();

            byte[] todecode_byte = Convert.FromBase64String(data);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }
        catch (Exception e)
        {
            //throw new Exception("Error in Base64Decode: " + e.Message);
            return data;
        }
    }

    public static string URLAntiCacheRandomizer(string url)
    {
#if UNITY_WEBGL
        return url;
#endif

        string r = "";
        r += UnityEngine.Random.Range(
                      1000000, 8000000).ToString();
        r += UnityEngine.Random.Range(
                      1000000, 8000000).ToString();

        return url + "?p=" + r;
    }    

    static public string GetGameObjectPath(GameObject go)
    {
        string path = "/" + go.name;
        while (go.transform.parent != null)
        {
            go = go.transform.parent.gameObject;
            path = "/" + go.name + path;
        }
        return path;
    }
    
    static public string ReadTextAsset(string textAssetPath)
    {
        TextAsset asset = Resources.Load<TextAsset>(textAssetPath);
        if (asset == null)
        {
            Debug.LogError("not found asset : " + textAssetPath);
            return string.Empty;
        }
        return asset.text;
    }
    
    //static public T DeserializeJson<T>(string textAssetPath)
    //{
    //    //return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(
    //    //    ReadTextAsset(textAssetPath)
    //    //    );
    //}

    static public string[][] ReadCSV(string textAssetPath)
    {
        return ReadCSV( Resources.Load<TextAsset>(textAssetPath) );
    }
    static public string[][] ReadCSV(TextAsset asset)
    {
        if (asset == null) return null;
        try
        {
            string[] line = asset.text.Trim().Split('\n');
            string[][] csv = new string[line.Length][];
            for (int idx = 0; idx < line.Length; ++idx)
            {
                csv[idx] = line[idx].Split(',');
            }
            return csv;

        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
            return null;
        }
    }
    
    static public bool WriteString(string path, string str)
    {
        try
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(path, false, System.Text.Encoding.UTF8))
            {
                sw.Write(str);
                sw.Flush();
                sw.Close();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("WriteString : " + path);
            Debug.LogError(e.Message);
            return false;
        }
        return true;
    }

    static public Texture LoadTexture(string path)
    {
        return Resources.Load<Texture>(path);
    } 

    static public GameObject LoadAndInstantiateGameObject(string path, Transform parent = null)
    {
        GameObject prefab = Resources.Load<GameObject>(path);        
        if (prefab == null) {
            Debug.LogError("not found prefab : " + path);
            return null;
        }
        GameObject go = GameObject.Instantiate(prefab) as GameObject;        
        go.name = prefab.name;
        Transform prefabT = prefab.transform;
        Transform t = go.transform;
        t.parent = parent;
        t.localScale = prefabT.localScale;
        t.localPosition = prefabT.localPosition;
        t.localRotation = prefabT.localRotation;
        if (parent != null) {
            SetLayer(go, parent.gameObject.layer);
        }
        return go;
    }
    static public T LoadAndInstantiateGameObject<T>(string path, Transform parent = null) where T : MonoBehaviour
    {
        GameObject go = LoadAndInstantiateGameObject(path, parent);
        if (go == null)
            return default(T);
        return go.GetComponent<T>();
    }
    static public T FindOrCreateGameObject<T>(string path, Transform parent = null) where T : MonoBehaviour
    {
        T inst = GameObject.FindObjectOfType<T>();
        if (inst != null)
            return inst;
        return LoadAndInstantiateGameObject<T>(path, parent);
    }

	static public void SetLayer (GameObject go, int layer)
	{
		go.layer = layer;

		Transform t = go.transform;
		
		for (int idx = 0, cnt = t.childCount; idx < cnt; ++idx)
		{
			Transform child = t.GetChild(idx);
			SetLayer(child.gameObject, layer);
		}
	}

    private static System.Text.StringBuilder _sb = 
        new System.Text.StringBuilder();
    static public string GetTimeString(double seconds, bool summary = false)
    {
		if ( seconds <= 0.0 ) {
			return string.Empty;
		}
        System.TimeSpan ts = System.TimeSpan.FromSeconds(seconds);
        _sb.Length = 0;
		if ( summary ) {
			if ( ts.Days > 0 ) {
				_sb.AppendFormat( "{0}일", ts.Days );
			} else if ( ts.Hours > 1 ) {
				_sb.AppendFormat( "{0}시간", ts.Hours );
			} else if ( ts.Minutes > 0 ) {
				_sb.AppendFormat( "{0}분{1}초", ts.Minutes, ts.Seconds );
			} else if ( ts.Seconds > 0 ) {
				_sb.AppendFormat( "{0}초", ts.Seconds );
			}
		} else {
			if ( ts.Days > 0 ) {
				_sb.Append(Localization.Format("5768", ts.Days));
			}
			if ( (int)ts.TotalHours > 0.0f ) {
				_sb.Append(Localization.Format("5769", ts.Hours));
			}
			if ( (int)ts.TotalMinutes > 0.0f ) {
				_sb.Append(Localization.Format("5770", ts.Minutes));
			}
			if ( ts.Days <= 0 && ts.TotalSeconds > 0.0 ) {
				_sb.Append(Localization.Format("5771", ts.Seconds));
			}
		}
		return _sb.ToString();
    }
	static public string GetTimeSimpleString(double seconds, bool summary = false)
	{
		if ( seconds <= 0.0 ) {
			return string.Empty;
		}
		System.TimeSpan ts = System.TimeSpan.FromSeconds(seconds);
		_sb.Length = 0;
		if ( summary ) {
			if ( ts.Days > 0 ) {
				_sb.AppendFormat( "{0}일", ts.Days );
			} else if ( ts.Hours > 1 ) {
				_sb.AppendFormat( "{0}시간", ts.Hours );
			} else if ( ts.Minutes > 0 ) {
				_sb.AppendFormat( "{0}분{1}초", ts.Minutes, ts.Seconds );
			} else if ( ts.Minutes <= 0 ) {
				_sb.Append( "1분 미만" );
			}
		} else {
			if ( ts.Days > 0 ) {
				_sb.Append(Localization.Format("5768", ts.Days));
			}
			if ( (int)ts.TotalHours > 0.0f ) {
				_sb.Append(Localization.Format("5769", ts.Hours));
			}
			if ( (int)ts.TotalMinutes > 0.0f ) {
				_sb.Append(Localization.Format("5770", ts.Minutes));
			}
			if ( ts.Days <= 0 && (int)ts.TotalMinutes <= 0.0f ) {
				_sb.Append(Localization.Format("5771", ts.Seconds));
			}
		}
		return _sb.ToString();
	}
	static public string GetTimeHM_MSString(double seconds, bool summary = false)
	{
		if ( seconds <= 0.0 ) {
			return string.Empty;
		}
		System.TimeSpan ts = System.TimeSpan.FromSeconds(seconds);
		_sb.Length = 0;
		if ( summary ) {
			if ( ts.Days > 0 ) {
				_sb.AppendFormat( "{0}일", ts.Days );
			} else if ( ts.Hours > 1 ) {
				_sb.AppendFormat( "{0}시간", ts.Hours );
			} else if ( ts.Minutes > 0 ) {
				_sb.AppendFormat( "{0}분{1}초", ts.Minutes, ts.Seconds );
			} else if ( ts.Minutes <= 0 ) {
				_sb.Append( "1분 미만" );
			}
		} else {
			if ( ts.Days > 0 ) {
				_sb.AppendFormat("{0}D:", ts.Days);
			}
			if ( (int)ts.TotalHours > 0.0f ) {
				_sb.AppendFormat("{0}h:", ts.Hours);
			}
			if ( (int)ts.TotalMinutes > 0.0f ) {
				_sb.AppendFormat("{0}m:", ts.Minutes);
            }
            if((int)ts.TotalSeconds > 0.0f) {
                _sb.AppendFormat("{0}s",ts.Seconds);
            }
		}
		return _sb.ToString();
	}
	static public string GetTimeStringColonFormat(double seconds)
	{		
		if ( seconds <= 0.0 ) {
			return "00:00";
		}
		System.TimeSpan ts = System.TimeSpan.FromSeconds(seconds);
		_sb.Length = 0;
		if (ts.TotalDays >= 1.0) {
			_sb.AppendFormat(Localization.Format("1006070", (int)ts.TotalDays));

        }
        else
		    _sb.AppendFormat ("{0:00}:{1:00}:{2:00}",ts.Hours, ts.Minutes, ts.Seconds);
		return _sb.ToString ();
	}
	static public string GetTimeStringSimpleColonFormat(double seconds)
	{		
		if ( seconds <= 0.0 ) {
			return "00";
		}
		System.TimeSpan ts = System.TimeSpan.FromSeconds(seconds);
		_sb.Length = 0;

		if ( ts.Days > 0 ) {
			_sb.Append(Localization.Format("5768", ts.Days));
			_sb.Append(Localization.Format("5769", ts.Hours));
		} else {
			if (ts.TotalMinutes >= 1.0) {
				_sb.AppendFormat ("{0:00}:{1:00}", ts.Hours, ts.Minutes);
			}
			else {
                _sb.AppendFormat("{0:00}", ts.Seconds);
            }
		}
		return _sb.ToString ();
	}
	static public string GetTimeStringSimpleStringColonFormat(double seconds)
	{		
		if ( seconds <= 0.0 ) {
			return "00";
		}
		System.TimeSpan ts = System.TimeSpan.FromSeconds(seconds);
		_sb.Length = 0;
		if (ts.TotalMinutes >= 1.0) {
			_sb.AppendFormat ("{0:00}:{1}", ts.Hours, Localization.Format("5770", string.Format("{0:00}", ts.Minutes) ));
		}
		else {
			_sb.Append(Localization.Format("5771", ts.Seconds));
		}
		return _sb.ToString ();
	}

    static public string GetTimeStringShopFormat(double seconds)
    {
        if (seconds <= 0.0)
        {
            return "0";
        }
        System.TimeSpan ts = System.TimeSpan.FromSeconds(seconds);
        _sb.Length = 0;
        if (ts.TotalDays >= 1.0)
        {
            _sb.AppendFormat(Localization.Format("1006070", (int)ts.TotalDays));
        }

        if(ts.TotalHours >= 1.0)
            _sb.AppendFormat("{0:00}:", ts.Hours);

        if (ts.TotalMinutes >= 1.0)
            _sb.AppendFormat("{0:00}:", ts.Minutes);


        _sb.AppendFormat("{0:00}", ts.Seconds);
        return _sb.ToString();
    }

    static public string GetTimeStringResourceFormat(double seconds)
    {
        if (seconds <= 0.0)
        {
            return "0";
        }
        System.TimeSpan ts = System.TimeSpan.FromSeconds(seconds);
        _sb.Length = 0;
        if (ts.TotalDays >= 1.0)
        {
            _sb.AppendFormat(string.Format("{0}{1}", (int)ts.TotalDays, Localization.Get("909005")));
        }
        else
        {
            if (ts.TotalHours >= 1.0)
                _sb.AppendFormat("{0:00}:", ts.Hours);
                
            _sb.AppendFormat("{0:00}:", ts.Minutes);

            if (ts.TotalHours < 1.0)
                _sb.AppendFormat("{0:00}", ts.Seconds);
        }

        return _sb.ToString();
    }

    static public string GetTimeStringItemBuffFormat(double seconds)
    {
        if (seconds <= 0.0)
        {
            return "0";
        }
        System.TimeSpan ts = System.TimeSpan.FromSeconds(seconds);
        _sb.Length = 0;
        if (ts.TotalDays >= 1.0)
        {
            _sb.AppendFormat(string.Format("{0}{1}", (int)ts.TotalDays, Localization.Get("909005")));
        }
        else
        {
            if (ts.TotalHours >= 1.0)
                _sb.AppendFormat(string.Format("{0}{1}:", (int)ts.Hours, Localization.Get("909006")));

            if (ts.TotalMinutes >= 1.0)
                _sb.AppendFormat(string.Format("{0}{1}", (int)ts.Minutes, Localization.Get("909007")));

            if (ts.TotalHours < 1.0 && ts.TotalMinutes >= 1.0)
                _sb.AppendFormat(":");

            if (ts.TotalHours < 1.0)
                _sb.AppendFormat(string.Format("{0}{1}", (int)ts.Seconds, Localization.Get("909008")));
        }

        return _sb.ToString();
    }

    static public string GetTimeStringDayFormat(double seconds)
    {
        if (seconds <= 0.0)
        {
            return "0";
        }
        System.TimeSpan ts = System.TimeSpan.FromSeconds(seconds);
        _sb.Length = 0;
        if (ts.TotalDays >= 1.0)
        {
            _sb.AppendFormat(string.Format("{0}{1}", (int)ts.TotalDays, Localization.Get("909005")));
        }
        else
        {
            if (ts.TotalHours >= 1.0)
                _sb.AppendFormat(string.Format("{0}{1} ", (int)ts.Hours, Localization.Get("909006")));

            if (ts.TotalMinutes >= 1.0)
                _sb.AppendFormat(string.Format("{0}{1}", (int)ts.Minutes, Localization.Get("909007")));

            if (ts.TotalMinutes < 1.0)
                _sb.AppendFormat(string.Format("{0}{1}", (int)ts.Seconds, Localization.Get("909008")));
        }

        return _sb.ToString();
    }

    static public string GetTimeStringFromMS(int ms, bool summary = false)
    {
        return GetTimeString((double)ms / 1000.0, summary);
    }

    static public double ToSeconds(double days, double hours, double minutes, double seconds)
    {
        return days * 86400.0 + hours * 3600.0 + minutes * 60.0 + seconds;
    }

	static public int GetRandomNumberByRate(List<int> rateList)
	{
		if (rateList == null || rateList.Count <= 0)
			return 0;
		List<int> list = new List<int> (rateList);
		int max = 0;
		for (int idx = 0; idx < list.Count; ++idx) {
			max += list[idx];
			list[idx] = max;
		}
		int rand = UnityEngine.Random.Range (0, max);
		for(int idx = 0; idx < list.Count; ++idx){
			if(rand < list[idx]){
				return idx;
			}
		}
		Debug.LogError("invalid state");
		rateList.ForEach (x => Debug.LogError (x.ToString ()));
		return -1;
	}

    //묻지도 따지지도 않고 프로퍼티 Get을 호출해 ReturnType형으로 리턴.
    //어떤 체크도 하지 않음.
    static public ReturnType GetPropertyValue<ReturnType>(
        object obj, string propName, object[] index
        )
    {
        System.Type type = obj.GetType();
        System.Reflection.PropertyInfo pi = type.GetProperty(propName);
        if (pi == null) {
            Debug.LogError("invalid state : " + propName);
            return default(ReturnType);
        }
		object value = pi.GetGetMethod().Invoke(obj, index);
        return (ReturnType)value;
    }

    static public void SetPropertyValue(
        object obj, string propName, object value, object[] index
        )
    {
        System.Type type = obj.GetType();
        System.Reflection.PropertyInfo pi = type.GetProperty(propName);
        if (pi == null || pi.GetSetMethod(true) == null) {
            Debug.LogError("invalid state : " + propName);
            return;
        }
        pi.SetValue(obj, value, index);
    }
    
    static public Vector3 ConvertPosWorldToUI(Vector3 worldPos)
    {
        Camera mainCam = Camera.main;
        Camera uiCam = UICamera.mainCamera;
        Vector3 pos = mainCam.WorldToViewportPoint(worldPos);
        pos.z = 0.0f;
        pos = uiCam.ViewportToWorldPoint(pos);
        return pos;
    }

    /// <summary>
    /// src가 가진 프로퍼티이름과 같은 프로퍼티를 dest에서 찾아 값을 복사해준다.
    /// 단순 복사이므로 주의. 찾는 이름이 없으면 무시된다.
    /// 필드가 아닌 프로퍼티만을 사용한다.
    /// </summary>
    /// <param name="src">원본 오브젝트</param>
    /// <param name="dest">복사 대상 오브젝트</param>
    static public void CopyToSameNameProperties(object src, object dest)
    {
        if (src == null || dest == null)
            return;
        System.Reflection.PropertyInfo[] srcPiList =
            src.GetType().GetProperties();
        System.Type destType = dest.GetType();
        for (int idx = 0; idx < srcPiList.Length; ++idx) {
            System.Reflection.PropertyInfo srcProp = srcPiList[idx];
            System.Reflection.PropertyInfo destProp = 
                destType.GetProperty(srcProp.Name);

            if(destProp == null || srcProp.PropertyType != destProp.PropertyType){
                Debug.LogWarning("not found prop : " + srcProp.Name);
                continue;
            }
			destProp.SetValue(dest, srcProp.GetGetMethod().Invoke(src, null), null);
        }
    }

    static public System.DateTime ConvertToDateTime(string yyyymmddFormatString)
    {
        if (string.IsNullOrEmpty(yyyymmddFormatString) ||
            yyyymmddFormatString.Length != 8) {
            Debug.LogError("invalid state");
            return new System.DateTime();
        }
        string yearPart = yyyymmddFormatString.Substring(0, 4);
        string monthPart = yyyymmddFormatString.Substring(4, 2);
        string dayPart = yyyymmddFormatString.Substring(6, 2);

        int year = 0;
        if (!int.TryParse(yearPart, out year)) {
            Debug.LogError("invalid year : " + yearPart);
            return new System.DateTime();
        }
        int month = 0;
        if (!int.TryParse(monthPart, out month)) {
            Debug.LogError("invalid month : " + monthPart);
            return new System.DateTime();
        }
        int day = 0;
        if (!int.TryParse(dayPart, out day)) {
            Debug.LogError("invalid day : " + dayPart);
            return new System.DateTime();
        }
        return new System.DateTime(year, month, day);
    }

//	public static class EnumUtil<T>
//	{
//		public static T Parse(string s)		
//		{
//			return (T)Enum.Parse(typeof(T), s);	
//		}
//	}

    static public void SampleAnimationEnd(Animation ani, string clipName)
    {
		ani.Play(clipName);
		ani.Stop();
		var clip = ani.GetClip(clipName);
		clip.SampleAnimation(ani.gameObject, clip.length);
//		//Ngui 버그
//		UISetter.SetActive(ani.gameObject, false);
//		UISetter.SetActive(ani.gameObject, true);
    }

    static public void SampleAnimationStart(Animation ani, string clipName)
    {
        ani.Play(clipName);
        ani.Stop();
        var clip = ani.GetClip(clipName);
        clip.SampleAnimation(ani.gameObject, 0);
    }

    static public T Find<T>(string path)
    {
        GameObject obj = GameObject.Find(path);
        if (obj == null)
        {
            return default(T);
        }

        return obj.GetComponent<T>();
    }

	static public string GetPureId(string itemId, string itemPrefix)
	{
		if (string.IsNullOrEmpty(itemId))
			return null;
		if (!itemId.StartsWith(itemPrefix)) {
			return itemId;
		}
		return itemId.Substring(itemPrefix.Length);
	}

    static public readonly HashSet<System.Type> _TimedObjectTypeSet = new HashSet<System.Type> {
        typeof(Animation),
        typeof(Animator),
        typeof(ParticleSystem),
        typeof(Rigidbody),
        typeof(Rigidbody2D),
        typeof(AudioSource),
    };

    static public UnityEngine.Object[] FindTimedObjects(Transform tf)
    {
        var dst = new List<UnityEngine.Object>();
        _FindTimedObjects(tf, dst);
        return dst.ToArray();
    }

    static public void _FindTimedObjects(Transform t, List<UnityEngine.Object> dst)
    {
        var components = t.GetComponents<Component>();
        foreach (var comp in components)
        {
            if (_TimedObjectTypeSet.Contains(comp.GetType()))
            {
                dst.Add(comp);
            }
        }
        for (var n = 0; n < t.childCount; ++n)
        {
            _FindTimedObjects(t.GetChild(n), dst);
        }
    }
    
	static public void SetBlindNotice(int idx)
	{
		string blindString = PlayerPrefs.GetString("Notice");
		if(!ContainsBlineNoticeIdx(idx.ToString()))
		{
			blindString = string.Format("{0}/{1}", blindString, idx);
			PlayerPrefs.SetString("Notice", blindString);
		}
	}

	static public void SetBlindNoticeResetCheck()
	{
		string blindString = PlayerPrefs.GetString("Notice");
		if(string.IsNullOrEmpty(blindString))
		{
			blindString = string.Format("{0}-", System.DateTime.Now.Day);
			PlayerPrefs.SetString("Notice", blindString);
		}
		else
		{
			int now = System.DateTime.Now.Day;
			string[] list = blindString.Split('-');
			int before = int.Parse(list[0].ToString());
			if(now != before)
			{
				PlayerPrefs.DeleteKey("Notice");
				SetBlindNoticeResetCheck();
			}
		}
	}

	static public string[] GetBlineNoticeIdxList()
	{
		string blindString = PlayerPrefs.GetString("Notice");
		string[] list = blindString.Split('-');
		string[] idxList = list[1].Split('/');
		return idxList;
	}

	static bool ContainsBlineNoticeIdx(string idx)
	{
		bool state = false;
		string[] blindList = GetBlineNoticeIdxList();

		for(int i = 0; i < blindList.Length; i++)
		{
			if(string.IsNullOrEmpty(blindList[i]))
				continue;

			if(blindList[i] == idx)
				state = true;
		}

		return state;
	}

	static public bool PossibleChar(string str)
	{
		char[] values = str.ToCharArray();
		for (int i = 0; i < values.Length; ++i)
		{
			bool state = false;
			int value = System.Convert.ToInt32(values[i]);
			if (value >= 0x0030 && value <= 0x0039) //숫자
			{
				state = true;
			}
			else if (value >= 0x0041 && value <= 0x005A) 
			{
				//				Debug.Log(values[i]+" : english"); //대문자
				state = true;
			}
			else if (value >= 0x0061 && value <= 0x007A) 
			{
				//				Debug.Log(values[i]+" : english"); //소문자
				state = true;
			}
			else if (value >= 0xAC00 && value <= 0xD7AF)
			{
				//				Debug.Log(values[i]+" : hangle"); //한글
				state = true;
			}
			else if (value >= 0x4E00 && value <= 0x9FFF) //중국어
			{
				//				Debug.Log(values[i]+" : china");
				state = true;
			}
			else if (value >= 0x3040 && value <= 0x309F) //히라가나
			{
				//				Debug.Log(values[i]+" : japan");
				state = true;
			}
			else if (value >= 0x30A0 && value <= 0x30FF) //가타카나
			{
				//				Debug.Log(values[i]+" : japan");
				state = true;
			}
            else if (value >= 0xFF00 && value <= 0xFFEF) //일본어 반각
            {
                //              Debug.Log(values[i]+" : japan");
                state = true;
            }
            if (!state)
				return false;
		}
		return true;
	}

	static public string GetStringToDay(double time)
	{
		if(time == 0)
			return "";
		
		System.DateTime dt = new System.DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(time).ToLocalTime();
//		System.DateTime dt = new System.DateTime((long)(time * 1000.0f)).ToLocalTime();

		string language = Localization.language;
		System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
		stringBuilder.Length = 0;
        if (language == "S_Kr" || language == "S_Beon" || language == "S_Gan")
		{
			stringBuilder.Append(Localization.Format("5134", dt.Year));
			stringBuilder.Append("/");
			stringBuilder.Append(Localization.Format("5135", dt.Month));
			stringBuilder.Append("/");
			stringBuilder.Append(Localization.Format("5768", dt.Day));
		}
		else if(language == "S_En")
		{
			stringBuilder.Append(Localization.Format("5135", dt.Month));
			stringBuilder.Append("/");
			stringBuilder.Append(Localization.Format("5768", dt.Day));
			stringBuilder.Append("/");
			stringBuilder.Append(Localization.Format("5134", dt.Year));
		}
		else
		{
			stringBuilder.Append(Localization.Format("5134", dt.Year));
			stringBuilder.Append("/");
			stringBuilder.Append(Localization.Format("5135", dt.Month));
			stringBuilder.Append("/");
			stringBuilder.Append(Localization.Format("5768", dt.Day));
		}
		stringBuilder.Append(" ");
//		stringBuilder.Append(Localization.Format("5769", dt.Hour));
		stringBuilder.Append(dt.Hour);
		stringBuilder.Append(":");
//		stringBuilder.Append(Localization.Format("5770", dt.Minute));
		stringBuilder.Append(dt.Minute.ToString("D2"));
		stringBuilder.Append(":");
//		stringBuilder.Append(Localization.Format("5771", dt.Second));
		stringBuilder.Append(dt.Second.ToString("D2"));
		return stringBuilder.ToString();
//		return dt.ToLocalTime().ToString("yyyy-MM-dd   HH:mm:ss");
	}

	static public string GetStringToDay(System.DateTime dt)
	{
		string language = Localization.language;
		System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
		stringBuilder.Length = 0;
        if (language == "S_Kr" || language == "S_Beon" || language == "S_Gan")
		{
			stringBuilder.Append(Localization.Format("5134", dt.Year));
			stringBuilder.Append("/");
			stringBuilder.Append(Localization.Format("5135", dt.Month));
			stringBuilder.Append("/");
			stringBuilder.Append(Localization.Format("5768", dt.Day));
		}
		else if(language == "S_En")
		{
			stringBuilder.Append(Localization.Format("5135", dt.Month));
			stringBuilder.Append("/");
			stringBuilder.Append(Localization.Format("5768", dt.Day));
			stringBuilder.Append("/");
			stringBuilder.Append(Localization.Format("5134", dt.Year));
		}
		else
		{
			stringBuilder.Append(Localization.Format("5134", dt.Year));
			stringBuilder.Append("/");
			stringBuilder.Append(Localization.Format("5135", dt.Month));
			stringBuilder.Append("/");
			stringBuilder.Append(Localization.Format("5768", dt.Day));
		}
		stringBuilder.Append(" ");
		//		stringBuilder.Append(Localization.Format("5769", dt.Hour));
		stringBuilder.Append(dt.Hour);
		stringBuilder.Append(":");
		//		stringBuilder.Append(Localization.Format("5770", dt.Minute));
		stringBuilder.Append(dt.Minute.ToString("D2"));
		stringBuilder.Append(":");
		//		stringBuilder.Append(Localization.Format("5771", dt.Second));
		stringBuilder.Append(dt.Second.ToString("D2"));
		return stringBuilder.ToString();
		//		return dt.ToLocalTime().ToString("yyyy-MM-dd   HH:mm:ss");
	}

    static public string ReadableFileSize(long byteCount)
    {
        string size = "0 Bytes";
        if (byteCount >= 1073741824.0)
            size = string.Format("{0:##.##}", byteCount / 1073741824.0) + " GB";
        else if (byteCount >= 1048576.0)
            size = string.Format("{0:##.##}", byteCount / 1048576.0) + " MB";
        else if (byteCount >= 1024.0)
            size = string.Format("{0:##.##}", byteCount / 1024.0) + " KB";
        else if (byteCount > 0 && byteCount < 1024.0)
            size = byteCount.ToString() + " Bytes";

        return size;

    }

    public static void DestroyLoadingWindow()
    {
        GameObject loading = GameObject.Find("Loading") as GameObject;

        if (loading != null)
        {
            loading.transform.parent = (GameObject.Find("UI Root") as GameObject).transform;
            loading.transform.localPosition = Vector3.zero;
        }

        UIFade.In(1.3f, false);

        //if (loading != null)
        //    loading.GetComponent<UILoading>().Out();
    }    

    //public static int ConvertToServerArea(ETileType tileType)
    //{
    //    switch (tileType)
    //    {
    //        case ETileType.Wall:
    //        case ETileType.Wall_Bottom:
    //            return 1;
    //        case ETileType.Ground:
    //            return 2;
    //    }

    //    return 0;
    //}

    public static void GcCleaner()
    {
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
        System.GC.WaitForPendingFinalizers();
    }


    public static string TrimStringWithLengthOfByte(string sVal, int iByteLength)
    {
        string sTemp = "";
        string sRet = "";
        int iByteLen = 0;

        for (int i = 0; i < sVal.Length; i++)
        {
            string sStrOfCurIndex = sVal.Substring(i, 1);
            sTemp = sTemp + sStrOfCurIndex;
            iByteLen += Mathf.Min(System.Text.Encoding.UTF8.GetByteCount(sStrOfCurIndex), 2);

            if (iByteLen > iByteLength)
            {
                sRet = sTemp.Substring(0, sTemp.Length - 1);                
                break;
            }
            else sRet = sTemp;
        }

        return sRet;
    }

    //static public void SortPosCharacterList(List<RoCharacter> list)
    //{
    //    for(int i = 0; i < list.Count; i++)
    //    {
    //        RoCharacter character = list[i];
    //        character.slotNum = i;
    //        Debug.Log("character.slotNum character.slotNum character.slotNum character.slotNum character.slotNum character.slotNum  : " + character.slotNum +"        "+ character.idx);
    //    }

    //    list.Sort(
    //        delegate (RoCharacter A, RoCharacter B)
    //        {
    //            if ((int)A.characterParamDataRow.Pos > (int)B.characterParamDataRow.Pos)
    //                return -1;
    //            else if ((int)A.characterParamDataRow.Pos < (int)B.characterParamDataRow.Pos)
    //                return 1;
    //            else
    //            {
    //                if (A.slotNum > B.slotNum)
    //                    return -1;
    //                else if (A.slotNum < B.slotNum)
    //                    return 1;
    //                else
    //                    return 0;
    //            }
    //        });
    //}

    //static public List<RoCharacter> CloneCharacterList(List<RoCharacter> characterList)
    //{
    //    List<RoCharacter> list = new List<RoCharacter>();

    //    for (int i = 0; i < characterList.Count; i++)
    //    {
    //        RoCharacter localCharacter = characterList[i];
    //        RoCharacter character = RoCharacter.Clone(localCharacter);
    //        list.Add(character);
    //    }

    //    return list;
    //}

#if UNITY_EDITOR
    /**
     * 에디트용 함수
     */
    public static bool HasSprite(string atlasName, string spriteName)
    {
        UIAtlas atlas = AssetDatabase.LoadAssetAtPath(string.Format("Assets/Materials/Atlas/{0}.prefab", atlasName), typeof(UIAtlas)) as UIAtlas;
        if (atlas == null)
        {
            Debug.LogError("Not Found Atlas " + atlasName);
            return false;
        }

        UISpriteData spriteData = atlas.GetSprite(spriteName);
        if (spriteData == null)
            return false;

        return true;
    }

    [MenuItem("Tools/UnloadUnusedAssets")]
    public static void UnloadUnusedAssets()
    {
        GcCleaner();     
    }

    [MenuItem("Tools/PlayerPrefsDeletell")]
    public static void PlayerPrefsDeletell()
    {     
        PlayerPrefs.DeleteAll();
    }

#endif

//    public static void StartBgm(string bgmName)
//    {
//#if !UNITY_WEBGL || UNITY_EDITOR
//        if (!string.IsNullOrEmpty(bgmName))
//        {
//            if (MasterAudio.GrabPlaylist(bgmName) == null)
//                CreateBGM(bgmName);

//            if(!MasterAudio.CurrentPlaylist(bgmName))
//                MasterAudio.StartPlaylist(bgmName);
//        }
//        else
//            MasterAudio.StopPlaylist();
//#else
//        if (!string.IsNullOrEmpty(bgmName))
//            WebGLStreamingAudio.instance.Play(bgmName, true);
//        else
//            WebGLStreamingAudio.instance.Pause();
//#endif
//    }


    //public static void CreateBGM(string bgmName)
    //{
    //    string path = string.Format("Sounds/BGM/{0}", bgmName);
    //    AudioClip clip = null;
    //    AssetBundle assetBundle = AssetBundleManager.LoadFromFile(bgmName, ECacheType.Bgm);
    //    if (assetBundle != null)
    //        clip = assetBundle.LoadAsset<AudioClip>(bgmName + ".ogg");

    //    if (clip == null)
    //        clip = Resources.Load<AudioClip>(path);

    //    if (clip == null)
    //        return;

    //    MasterAudio.Playlist playListItem = new DarkTonic.MasterAudio.MasterAudio.Playlist();

    //    playListItem.crossFadeTime = 0.0f;
    //    playListItem.playlistName = bgmName;
    //    MusicSetting musicSet = new DarkTonic.MasterAudio.MusicSetting();
    //    musicSet.clip = clip;
    //    musicSet.isLoop = true;
    //    musicSet.songName = "soundName";
    //    playListItem.MusicSettings.Add(musicSet);
    //    MasterAudio.CreatePlaylist(playListItem, true);
    //}
}


//public static class FileIO
//{
//    public static void Save(byte[] data, string path, bool encrypt = false)
//    {
//#if UNITY_IOS
//            UnityEngine.iOS.Device.SetNoBackupFlag(path);
//#endif

//        using (FileStream fs = new FileStream(path, FileMode.Create))
//        {
//            if (encrypt)
//            {
//                Encryptor.data.Encrypt(fs, data, DataIO.Write);
//            }
//            else
//            {
//                DataIO.Write(fs, data);
//            }
//        }
//    }

//    public static void Save(string data, string path, bool encrypt = false)
//    {
//#if UNITY_IOS
//            UnityEngine.iOS.Device.SetNoBackupFlag(path);
//#endif

//        using (FileStream fs = new FileStream(path, FileMode.Create))
//        {
//            if (encrypt)
//            {
//                Encryptor.data.Encrypt(fs, data, DataIO.Write);
//            }
//            else
//            {
//                DataIO.Write(fs, data);
//            }
//        }
//    }

//    public static void Save(object data, string path, bool encrypt = false)
//    {
//#if UNITY_IOS
//            UnityEngine.iOS.Device.SetNoBackupFlag(path);
//#endif

//        using (FileStream fs = new FileStream(path, FileMode.Create))
//        {
//            if (encrypt)
//            {
//                Encryptor.data.Encrypt(fs, data, DataIO.Write);
//            }
//            else
//            {
//                DataIO.Write(fs, data);
//            }
//        }
//    }

//    public static byte[] ReadBytes(string path, bool encrypt = false)
//    {
//        byte[] result;
//        using (FileStream fs = new FileStream(path, FileMode.Open))
//        {
//            if (encrypt)
//            {
//                result = Encryptor.data.Decrypt(fs, DataIO.ReadBytes);
//            }
//            else
//            {
//                result = DataIO.ReadBytes(fs, fs.Length);
//            }
//        }

//        return result;
//    }

//    public static string ReadString(string path, bool encrypt = false)
//    {
//        string result;
//        using (FileStream fs = new FileStream(path, FileMode.Open))
//        {
//            if (encrypt)
//            {
//                result = Encryptor.data.Decrypt(fs, DataIO.ReadString);
//            }
//            else
//            {
//                result = DataIO.ReadString(fs);
//            }
//        }

//        return result;
//    }

//    public static object ReadObject(string path, bool encrypt = false)
//    {
//        object result;
//        using (FileStream fs = new FileStream(path, FileMode.Open))
//        {
//            if (encrypt)
//            {
//                result = Encryptor.data.Decrypt(fs, DataIO.ReadObject);
//            }
//            else
//            {
//                result = DataIO.ReadObject(fs);
//            }
//        }

//        return result;
//    }
//}

//public static class DataIO
//{
//    public static void Write(Stream stream, string data)
//    {
//        using (StreamWriter sw = new StreamWriter(stream))
//        {
//            sw.Write(data);
//        }
//    }

//    public static void Write(Stream stream, byte[] data)
//    {
//        using (BinaryWriter bw = new BinaryWriter(stream))
//        {
//            bw.Write(data);
//        }
//    }

//    public static void Write(Stream stream, object data)
//    {
//        BinaryFormatter bf = new BinaryFormatter();
//        bf.Serialize(stream, data);
//    }

//    public static byte[] ReadBytes(Stream stream, long length)
//    {
//        byte[] result;

//        using (BinaryReader br = new BinaryReader(stream))
//        {
//            if (length <= int.MaxValue)
//            {
//                result = br.ReadBytes((int)length);
//            }
//            else
//            {
//                Debug.LogWarning("[Encryptor] limited data size");

//                using (MemoryStream dest = new MemoryStream())
//                {
//                    byte[] buffer = new byte[2048];
//                    int bytesRead;
//                    while ((bytesRead = br.Read(buffer, 0, buffer.Length)) > 0)
//                    {
//                        dest.Write(buffer, 0, bytesRead);
//                    }

//                    result = dest.ToArray();
//                }
//            }
//        }

//        return result;
//    }

//    public static string ReadString(Stream stream)
//    {
//        string result;

//        using (StreamReader sr = new StreamReader(stream))
//        {
//            result = sr.ReadToEnd();
//        }

//        return result;
//    }

//    public static object ReadObject(Stream stream)
//    {
//        object result;
//        try
//        {
//            BinaryFormatter formatter = new BinaryFormatter();
//            result = formatter.Deserialize(stream);
//        }
//        catch (System.Exception e)
//        {
//            Debug.LogError(e);
//            result = null;
//        }

//        return result;
//    }

//    public static byte[] ReadBytes(byte[] data, bool descrypt = false)
//    {
//        if (descrypt)
//        {
//            return Encryptor.data.Decrypt(data);
//        }

//        return data;
//    }

//    public static string ReadString(byte[] data, bool descrypt = false)
//    {
//        string result;
//        using (MemoryStream ms = new MemoryStream(data))
//        {
//            if (descrypt)
//            {
//                result = Encryptor.data.Decrypt(ms, ReadString);
//            }
//            else
//            {
//                result = ReadString(ms);
//            }
//        }

//        return result;
//    }

//    public static object ReadObject(byte[] data, bool descrypt = false)
//    {
//        object result;
//        using (MemoryStream ms = new MemoryStream(data))
//        {
//            if (descrypt)
//            {
//                result = Encryptor.data.Decrypt(ms, ReadObject);
//            }
//            else
//            {
//                result = ReadObject(ms);
//            }
//        }

//        return result;
//    }
//}