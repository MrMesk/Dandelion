using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
public class LightFade : MonoBehaviour
{

    public float waverSpeed = 0.5f;
    public float minRimPower = 5f;
    public float maxRimPower = 7f;
    public int matNb = 0;
    Material[] material;

    void Update()
    {
        material = GetComponent<Renderer>().materials;

        float lightCenter = (maxRimPower + minRimPower) * 0.5f;
        float lightDeviation = (maxRimPower - minRimPower) * 0.5f;
        float rimPower = Mathf.Sin(2.0f * Mathf.PI * waverSpeed * Time.time) * lightDeviation + lightCenter;

        material[matNb].SetFloat("_RimPower", rimPower);
        GetComponent<Renderer>().materials = material;
    }
}
