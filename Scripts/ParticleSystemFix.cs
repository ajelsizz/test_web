using UnityEngine;
using System.Collections;

public class ParticleSystemFix : MonoBehaviour
{

    public string sortingLayerName;
    public int sortingOrder;

    /**
     */
    void Start()
    {
        this.GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = sortingLayerName;
        this.GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingOrder = sortingOrder;
        enabled = false;
    }
}

