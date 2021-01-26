using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShopManager : MonoBehaviour
{

	public GameObject goShop;
	public GameObject goEquip;
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnShopOpen()
    {
		goShop.SetActive(true);

	}

	public void OnBtnLobbyExit()
    {
		Debug.Log("Lobby Exit");
		Loading.Load(Loading.Lobby);

	}
}
