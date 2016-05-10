using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

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
    [Range(5, 70)]
    public int minWispsToWin;
    [Space(10)]
    [Range(0, 1)]
    public float growSpeed;
    [Space(10)]
    [Range(0, 720)]
    public float rotateSpeed;
    [Space(10)]
    public GameObject lightChild;
    [Space(10)]
    public GameObject particleChild;
    [Space(10)]
    public GameObject fog;
    [Space(10)]
    public GameObject ascendedWisps;
    [Space(10)]
    public GameObject dandelionSkin;
    [Space(10)]
    public GameObject particleAscend;
    [Space(10)]
    public Canvas winMenu;


    ParticleSystem.ShapeModule shapeModuleFog;
    ParticleSystem.ShapeModule shapeModuleWisp;

    bool step1Reached = false;
    bool endReached = false;

    float actualAggroRange;
    float newAggroRange;

    float actualParticleRange;
    float newParticleRange;

    float actualLightRange;
    int wispsAscended;
    float growTime;

    // Use this for initialization
    void Start()
    {
        winMenu.enabled = false;
        growTime = 0f;
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

        if (growTime > 0f)
        {
            Grow();
            growTime -= Time.deltaTime;
        }
        else
        {
            growTime = 0f;
        }

        if(Input.GetButtonUp("Submit") && endReached)
        {
            SceneManager.LoadScene(0);

            if (DataManagement.data.topScore[DataManagement.data.levelNumber] < LivesManager.livesManager.actualLives)
            {
                DataManagement.data.topScore[DataManagement.data.levelNumber] = LivesManager.livesManager.actualLives;
                DataManagement.data.Save();
            }
        }
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
        growTime++;
        Instantiate(particleAscend, particleChild.transform.position, Quaternion.identity);
        if (wispsAscended >= step1 && !step1Reached)
        {
            step1Reached = true;
            particleChild.SetActive(true);
        }

        if(wispsAscended >= minWispsToWin && endReached == false)
        {
            LevelEnd();
        }

        if(DataManagement.data.savedDandelions[DataManagement.data.levelNumber] < wispsAscended)
        {
            DataManagement.data.savedDandelions[DataManagement.data.levelNumber] = wispsAscended;
            DataManagement.data.Save();
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

    void Grow()
    {
        Vector3 scale = dandelionSkin.transform.localScale;
        scale += new Vector3(growSpeed, growSpeed, growSpeed) * Time.deltaTime;
        dandelionSkin.transform.localScale = scale;
        dandelionSkin.transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }

    void LevelEnd()
    {
        switch(DataManagement.data.levelNumber)
        {
            case 0:
                DataManagement.data.levelUnlocked[0] = true;
                DataManagement.data.Save();
                break;

            case 1:
                DataManagement.data.levelUnlocked[1] = true;
                DataManagement.data.Save();
                break;

            case 2:
                DataManagement.data.levelUnlocked[2] = true;
                DataManagement.data.Save();
                break;
        }
        winMenu.enabled = true;
        endReached = true;
    }
}
