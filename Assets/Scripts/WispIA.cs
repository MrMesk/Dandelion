using UnityEngine;
using System.Collections;

public class WispIA : MonoBehaviour
{
    [Space(10)]
    [Range(5f,15f)]
    public float nearRange;
    [Range(5f,15f)]
    public float farRange;
    [Range(1f, 3f)]
    public float spacing;
    [Range(0.5f, 2f)]
    public float speed;
    [Range(0.5f, 2f)]
    public float wakingTime;
    [Range(2f, 10f)]
    public float spotTime;
    [Range(5f, 10f)]
    public float endGlow;
    [Range(0f, 10f)]
    public float maxLightRange;
    [Space(10)]
    public LayerMask aggroLayer;
    [Space(10)]
    public Material[] mats;
    [Space(10)]
    public GameObject particleDeath;

    [FMODUnity.EventRef]
    public string askSound = "event:/Wisps/Ask";
    [FMODUnity.EventRef]
    public string joinSound = "event:/Wisps/Join";
    [FMODUnity.EventRef]
    public string ascendSound = "event:/Wisps/Ascend";
    [FMODUnity.EventRef]
    public string deathSound = "event:/Wisps/Death";

    Followers follow;

    bool isFar = false;
    bool isNearby = false;
    bool targetSet = false;
    bool isLighting = false;
    bool isAscending = false;

    Material[] materials;
    Animator anim;
    int followIndex;
    Light l;

    SkinnedMeshRenderer my_Renderer;

    Transform target;
    Vector3 lookDir;

    float lightIncr;

    // Use this for initialization
    void Start ()
    {
        lightIncr = 0f;
        l = transform.Find("Light").GetComponent<Light>();
        follow = GameObject.FindGameObjectWithTag("Player").GetComponent<Followers>();
        my_Renderer = transform.Find("Model").GetComponent<SkinnedMeshRenderer>();
        materials = my_Renderer.materials;
        materials[0] = mats[2];
        materials[3] = mats[2];
        my_Renderer.materials = materials;
        // anim = transform.Find("Model").GetComponent<Animator>();
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (isFar && !isNearby)
        {
            LightAwake();
        }
        else if (!isFar && !isNearby)
        {
            LightSleep();
        }
        if (!isNearby)
        {
            if(!isFar)
            {
               if(NearbyCheck(farRange, 1))
               {
                    isFar = true;
                    FMODUnity.RuntimeManager.PlayOneShot(askSound, transform.position);
                    StartCoroutine(Awaking());
               }
            }
            else if(NearbyCheck(nearRange, 0))
            {
                FMODUnity.RuntimeManager.PlayOneShot(joinSound, transform.position);
                SetTarget(follow.AddFollower(gameObject));
                isNearby = true;
            }   
        }
        if(targetSet)
        {
            MoveToTarget();
        }
	}

    bool NearbyCheck(float range, int matID)
    {
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, range, aggroLayer);
        foreach(Collider c in nearbyObjects)
        {
            if(c.transform.tag == "Player")
            {
                materials[0] = mats[matID];
                materials[3] = mats[matID];
                my_Renderer.materials = materials;
                return true;
            }
        }
        return false;
    }

    public void SetTarget(Transform t)
    {
       
        target = t;
        targetSet = true;
        l.enabled = true;
        tag = "Wisp";
    }

    public void SetIndex(int i)
    {
        followIndex = i;
    }

    void MoveToTarget()
    {
        if(target)
        {
            lookDir = target.position - transform.position;
            lookDir.y = 0f;
            transform.right = lookDir.normalized;
            if (!isLighting)
            {
                if (Vector3.Distance(transform.position, target.position) > spacing)
                {
                    transform.position = Vector3.Lerp(transform.position, target.position, speed * Time.deltaTime);
                    if(anim.GetBool("Moving") == false)
                    {
                        anim.SetBool("Moving", true);
                    }
                    
                }
                else if (anim.GetBool("Moving") == true)
                {
                    anim.SetTrigger("Stop");
                    anim.SetBool("Moving", false);
                }
            }
            else
            {
                if (Vector3.Distance(transform.position, target.position) > 0.1f)
                {
                    transform.position = Vector3.Lerp(transform.position, target.position, speed * Time.deltaTime);
                    if (anim.GetBool("Moving") == false)
                    {
                        anim.SetBool("Moving", true);
                    }
                }
                else
                {
                    if (anim.GetBool("Moving") == true)
                    {
                        anim.SetTrigger("Stop");
                        anim.SetBool("Moving", false);
                    }
                }
            }
        }
        else if(!isLighting)
        {
            target = follow.GetFollowing(followIndex);
        }
    }
    public void LightAwake()
    {

        if (l.range < maxLightRange)
        {
            lightIncr += Time.deltaTime / (wakingTime);
            l.range = Mathf.Lerp(2f, maxLightRange, lightIncr);
        }
        else
        {
            lightIncr = 0f;
            l.range = maxLightRange;
        }
    }

    public void LightSleep()
    {

        if (l.range > 0f)
        {
            lightIncr += Time.deltaTime / (wakingTime);
            l.range = Mathf.Lerp(maxLightRange, 0f, lightIncr);
        }
        else
        {
            l.range = 0f;
            lightIncr = 0f;
        }
    }
    IEnumerator Awaking()
    {
        yield return new WaitForSeconds(wakingTime);
        if (isNearby == false)
        {
            if (NearbyCheck(farRange, 0))
            {
                isNearby = true;
                isFar = true;
                SetTarget(follow.AddFollower(gameObject));
                FMODUnity.RuntimeManager.PlayOneShot(joinSound, transform.position);
            }
            else
            {
                materials[0] = mats[2];
                materials[3] = mats[2];
                my_Renderer.materials = materials;
                isFar = false;
                isNearby = false;
            }
        }
    }
    public void Death()
    {   
        if (isLighting)
        {
            Instantiate(particleDeath, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else
        {
            follow.KillFollower(followIndex);
            if (!isAscending)
            {
                FMODUnity.RuntimeManager.PlayOneShot(deathSound, transform.position);
                Instantiate(particleDeath, transform.position, Quaternion.identity);
            }
            
            Destroy(gameObject);
        }
    }
    public void BreakFollowers()
    {
        if(isLighting)
        {
            Instantiate(particleDeath, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else
        {
            follow.StartCoroutine(follow.BreakChain(followIndex));
        }
    }
    public void ClearFollowers()
    {
        Instantiate(particleDeath, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void Ascend(Transform t)
    {
        if(!isAscending)
        {
            SetTarget(t);
            FMODUnity.RuntimeManager.PlayOneShot(joinSound, transform.position);
            if (!isNearby && !isFar)
            {
                isNearby = true;
                isFar = true;
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraDezoom>().CameraZooming(1);
            }
            isAscending = true;
        }
        else
        {
            isNearby = true;
            isFar = true;
            SetTarget(t);
        }
       

    }

    public void GoToLocation(Transform pos)
    {
        FMODUnity.RuntimeManager.PlayOneShot(joinSound, transform.position);
        follow.KillFollower(followIndex);
        isLighting = true;
        target = pos;
        StartCoroutine(lightGrow());
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Tree")
        {
            FMODUnity.RuntimeManager.PlayOneShot(ascendSound, transform.position);
            other.GetComponent<TreeManager>().AscendWisp();
            Death();
        }
    }

    IEnumerator lightGrow()
    {
        while(l.range < endGlow - 0.5f)
        {
            l.range = Mathf.Lerp(l.range, endGlow, speed * Time.deltaTime);
            yield return null;
        }
        if (anim.GetBool("Moving") == true)
        {
            anim.SetTrigger("Stop");
            anim.SetBool("Moving", false);
        }
        // Destroy(target.gameObject);
        yield return new WaitForSeconds(spotTime);
        while (l.range > 1.5f)
        {
            l.range = Mathf.Lerp(l.range, 0f, speed * Time.deltaTime);
            yield return null;
        }
        Destroy(gameObject);
    }
}
