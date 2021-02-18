using UnityEngine;
using System.Collections;

public class DamageText : MonoBehaviour
{
	public Mesh[] meshs = new Mesh[10];
	public Mesh empty_mesh = null;
	public Material damage_material = null;

	public GameObject immunity_obj = null;

	public float critical_size = 1.0f;
	public float VisibleTime = 1.0f;
	public float RemoveTime = 1.0f;


	const int NUMBER_SIZE = 9;
	MeshFilter[] meshfilters = new MeshFilter[NUMBER_SIZE];	
	CombineInstance[] combine = new CombineInstance[NUMBER_SIZE];
	Material damage_mat = null;

	Vector3 scale = Vector3.one;
	Color text_color = Color.white;
	bool visible = false;
	float timer = 0.0f;

	Transform my_transform = null;
	MeshFilter my_meshfilter = null;

	// Use this for initialization
	void Start()
	{
		my_transform = transform;

		damage_mat = GameObject.Instantiate(damage_material) as Material;

		Transform trans = null;
		for (int i = 0; i < NUMBER_SIZE; ++i)
		{
			trans = my_transform.Find("number_" + i.ToString());
			if (trans != null)
			{
				meshfilters[i] = trans.GetComponent<MeshFilter>();
				trans.gameObject.SetActive(false);
			}
			else
				Debug.LogError("Cannot find \"number_" + i.ToString() + "\".");
		}

		Renderer mesh_renderer = gameObject.AddComponent<MeshRenderer>();
		mesh_renderer.material = damage_mat;
		my_meshfilter = gameObject.AddComponent<MeshFilter>();
		my_meshfilter.mesh = new Mesh();

		gameObject.SetActive(false);

	}

	// Update is called once per frame
	void Update()
	{
		if (visible)
		{
			// dummy sizz
			scale = new Vector3(150, 150, 1);

			if (timer > VisibleTime)
			{
				text_color.a = 1.0f - (timer - VisibleTime) / RemoveTime;
				damage_mat.color = text_color;

				//my_transform.localScale = scale;// *text_color.a;
				my_transform.localScale = scale;

				my_transform.position += Vector3.up * Time.deltaTime;

				if (timer >= VisibleTime + RemoveTime)
				{
					visible = false;
					gameObject.SetActive(false);
				}
			}
			else
            {
				my_transform.localScale = scale * (3.0f - 2.0f * timer / VisibleTime);
			}

			timer += Time.deltaTime;
		}
	}

	public void SetDamageText(Vector3 pos, int damage, bool critical, bool immunity, Color color)
	{
		// select mesh
		int idx = GetStartIndex(damage);

		
		for (int i = 0; i < NUMBER_SIZE; ++i)
		{
			if (i >= idx && damage > 0)
			{
				meshfilters[i].sharedMesh = meshs[damage % 10];
				damage /= 10;

			}
			else if( i == idx && damage == 0)
            {
				meshfilters[i].sharedMesh = meshs[damage % 10];
				damage /= 10;

			}
			else
				meshfilters[i].sharedMesh = empty_mesh;
		}


		// combine mesh
		my_transform.position = Vector3.zero;
		my_transform.rotation = Quaternion.identity;
		my_transform.localScale = Vector3.one;

		for (int i = 0; i < NUMBER_SIZE; ++i)
		{
			combine[i].mesh = meshfilters[i].sharedMesh;
			combine[i].transform = meshfilters[i].transform.localToWorldMatrix;
		}
		my_meshfilter.mesh.CombineMeshes(combine);

		// immunity
		immunity_obj.SetActive(immunity);

		// set transform
		my_transform.position = pos;
		my_transform.localScale = Vector3.zero;
		scale = Vector3.one * (critical ? critical_size : 1.0f);

		// set color
		text_color = color;
		damage_mat.color = text_color;

		// start
		timer = 0.0f;
		visible = true;
		gameObject.SetActive(true);
	}

	int GetStartIndex(int damage)
	{
		int start = 4;

		if (damage >= 10000000)
			start = 0;
		else if (damage >= 100000)
			start = 1;
		else if (damage >= 1000)
			start = 2;
		else if (damage >= 10)
			start = 3;
		else
			start = 4;

		return start;
	}
}
