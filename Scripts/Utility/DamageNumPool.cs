using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumPool : MonoBehaviour
{

	[SerializeField]
	private GameObject _uiPrefab;


    private void Start()
    {
		//foreach (DamageNumUI ui in damageUis)
		//{
		//	_damageNumUiList.Add(ui);
		//	ui.gameObject.SetActive(false);
		//}
	}

	private void OnDestroy()
    {

    }

}
