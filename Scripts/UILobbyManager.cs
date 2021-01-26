using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILobbyManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnBtnBattleScene()
    {
		Loading.Load(Loading.Battle);
		Debug.Log("OnBtnBattleScene");
    }

	public void OnBtnShopScene()
    {
		Loading.Load(Loading.Shop);
		Debug.Log("OnBtnShopScene");
	}
}
