using UnityEngine;
using System.Collections;

public class DamageTextPool : MonoBehaviour
{
	// for singleton - begin
	static DamageTextPool _instance = null;

	public static DamageTextPool Instance
	{
		get
		{
			if (_instance == null)
				CreateInstance();

			return _instance;
		}
	}

	public static void CreateInstance()
	{
		if (_instance == null)
		{
			GameObject go = new GameObject("DamageTextPool");
			//DontDestroyOnLoad(go);
			_instance = go.AddComponent<DamageTextPool>();
		}
	}

	void Awake()
	{
		if (_instance == null)
			_instance = this;
	}

	// for singleton - end

	public GameObject prefab = null;
	public int pool_size = 32;

	DamageText[] damage_text_pool = null;

	int index = 0;


	// Use this for initialization
	void Start()
	{
		if (prefab != null)
		{
			damage_text_pool = new DamageText[pool_size];
		
			GameObject go = null;
			for (int i = 0; i < pool_size; ++i)
			{
				go = GameObject.Instantiate(prefab) as GameObject;
				go.transform.parent = transform;

				damage_text_pool[i] = go.GetComponent<DamageText>();
			}
		}
	}

	public DamageText GetDamageText()
	{
		if (damage_text_pool != null)
		{
			index %= pool_size;
			return damage_text_pool[index++];
		}

		return null;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Z))
		{
			DamageText dt = DamageTextPool.Instance.GetDamageText();

			Vector3 pos = this.transform.position;
			dt.SetDamageText(pos, 0, false, false, Color.red);


			//pos.y += 2f;
			//dt.SetDamageText(pos, (int)Random.Range(1,99999), false, false, Color.red);
			//dt.SetDamageText(pos, 1, false, false, Color.red);

		}
		if (Input.GetKeyDown(KeyCode.C))
		{
			DamageText dt = DamageTextPool.Instance.GetDamageText();

			Vector3 pos = this.transform.position;
			dt.SetDamageText(pos, Random.Range(1, 10000), false, false, Color.red);


			//pos.y += 2f;
			//dt.SetDamageText(pos, (int)Random.Range(1,99999), false, false, Color.red);
			//dt.SetDamageText(pos, 1, false, false, Color.red);

		}

		if (Input.GetKeyDown(KeyCode.Z))
		{
			DamageText dt = DamageTextPool.Instance.GetDamageText();

			Vector3 pos = this.transform.position;
			dt.SetDamageText(pos, 0, false, true, Color.red);

		}
	}
}
