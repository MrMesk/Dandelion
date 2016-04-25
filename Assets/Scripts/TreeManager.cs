using UnityEngine;
using System.Collections;

public class TreeManager : MonoBehaviour
{
    [Space(10)]
    public LayerMask aggroLayer;
    [Space(10)]
    [Range(5f, 30f)]
    public float aggroRange;
    [Range(0f, 5f)]
    public float lightIncreasePerWisp;
    [Range(0f, 5f)]
    public float LightIncreaseSpeed;
    [Space(10)]
    [Range(10, 20)]
    public int step1;
    [Space(10)]
    public GameObject lightChild;
    [Space(10)]
    public GameObject particleChild;
    [Space(10)]
    public GameObject fog;
    [Space(10)]
    public GameObject ascendedWisps;

    ParticleSystem.ShapeModule shapeModuleFog;
    ParticleSystem.ShapeModule shapeModuleWisp;

    bool step2Reached = false;

    float actualAggroRange;
    float newAggroRange;

    float actualParticleRange;
    float newParticleRange;

    float actualLightRange;
    int wispsAscended;

    // Use this for initialization
    void Start()
    {
        actualLightRange = lightChild.GetComponent<Light>().range;
        wispsAscended = 0;

        shapeModuleFog = fog.GetComponent<ParticleSystem>().shape;
        shapeModuleWisp = ascendedWisps.GetComponent<ParticleSystem>().shape;


        actualAggroRange = aggroRange;
        newAggroRange = actualAggroRange;

        actualParticleRange = shapeModuleFog.radius;
        newParticleRange = actualParticleRange;
    }

    // Update is called once per frame
    void Update()
    {
        NearCheck(actualAggroRange);
        UpdateLight();
    }

    void NearCheck(float range)
    {
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, range, aggroLayer);
        foreach (Collider c in nearbyObjects)
        {
            if (c.transform.tag == "Wisp")
            {
                c.GetComponent<WispIA>().Ascend(transform);
            }
            else if (c.transform.tag == "SleepingWisp")
            {
                c.GetComponent<WispIA>().Ascend(transform);
            }
            else if (c.transform.tag == "Wraith")
            {
                c.GetComponent<WraithIA>().Death();
            }
        }
    }

    public void AscendWisp()
    {
        wispsAscended++;
        SmoothLightIncrease();
        if (wispsAscended >= step1 && !step2Reached)
        {
            step2Reached = true;
            particleChild.SetActive(true);
        }

    }

    public void SmoothLightIncrease()
    {
        actualLightRange += lightIncreasePerWisp;
        newAggroRange += lightIncreasePerWisp;
        newParticleRange += lightIncreasePerWisp;
    }
    void UpdateLight()
    {
        lightChild.GetComponent<Light>().range = Mathf.Lerp(lightChild.GetComponent<Light>().range, actualLightRange, LightIncreaseSpeed * Time.deltaTime);
        actualAggroRange = Mathf.Lerp(actualAggroRange, newAggroRange, LightIncreaseSpeed * Time.deltaTime);
        actualParticleRange = Mathf.Lerp(actualParticleRange, newParticleRange, LightIncreaseSpeed * Time.deltaTime);
        shapeModuleFog.radius = actualParticleRange;
        shapeModuleWisp.radius = actualParticleRange;
    }
}
