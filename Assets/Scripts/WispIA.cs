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
    [Space(10)]
    public LayerMask aggroLayer;
    [Space(10)]
    public Material[] mats;
    [Space(10)]
    public GameObject particleDeath;

    Followers follow;

    bool isFar = false;
    bool isNearby = false;
    bool targetSet = false;
    bool isLighting = false;

    Material[] materials;
    Animator anim;

    Vector3 velocity;
    int followIndex;
    Light l;

    MeshRenderer my_Renderer;

    Transform target;

	// Use this for initialization
	void Start ()
    {
        l = transform.Find("Light").GetComponent<Light>();
        follow = GameObject.FindGameObjectWithTag("Player").GetComponent<Followers>();
        my_Renderer = transform.Find("Model").GetComponent<MeshRenderer>();
        materials = my_Renderer.materials;
        materials[0] = mats[2];
        my_Renderer.materials = materials;
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {   
	    if(!isNearby)
        {
            if(!isFar)
            {
               if(NearbyCheck(farRange, 1))
               {
                    isFar = true;
                    StartCoroutine(Awaking());
               }
            }
            else if(NearbyCheck(nearRange, 0))
            {
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
                if (Vector3.Distance(transform.position, target.position) > 0f)
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
            }
            else
            {
                materials[0] = mats[2];
                my_Renderer.materials = materials;
                isFar = false;
                isNearby = false;
            }
        }
    }
    public void Death()
    {
        if(!isLighting)
        {
            follow.KillFollower(followIndex);
        } 
        Instantiate(particleDeath, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void ClearFollowers()
    {
        Instantiate(particleDeath, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void Ascend(Transform t)
    {
       
        SetTarget(t);
    }

    public void GoToLocation(Transform pos)
    {
        follow.KillFollower(followIndex);
        isLighting = true;
        target = pos;
        StartCoroutine(lightGrow());
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Tree")
        {
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
