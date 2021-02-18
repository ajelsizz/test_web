using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour
{
	protected Transform my_trans = null;
	protected Transform main_camera_trans = null;
	protected Vector3 eualer_angles = Vector3.zero;

	void Awake()
	{
		my_trans = transform;
	}

	void OnEnable()
	{
		main_camera_trans = Camera.main.transform;
	}

	void OnDisable()
	{
		main_camera_trans = null;
	}

	void Update()
	{
		if (main_camera_trans)
		{
			eualer_angles.x = main_camera_trans.eulerAngles.x;
			eualer_angles.y = main_camera_trans.eulerAngles.y;

			my_trans.eulerAngles = eualer_angles;
		}
	}
}
