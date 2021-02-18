using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSorting : MonoBehaviour
{

    public string layerName;
    public int order;

    private MeshRenderer rend;

    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        rend.sortingLayerName = layerName;
        rend.sortingOrder = order;
    }

    public void Update()
    {
        if (rend.sortingLayerName != layerName)
            rend.sortingLayerName = layerName;
        if (rend.sortingOrder != order)
            rend.sortingOrder = order;
    }

    //public void OnValidate()
    //{
    //    rend = GetComponent<MeshRenderer>();
    //    rend.sortingLayerName = layerName;
    //    rend.sortingOrder = order;
    //}
}
