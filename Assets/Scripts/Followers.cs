using UnityEngine;
using System.Collections;

public class Followers : MonoBehaviour
{
    public GameObject[] wisps;
    public Vector3 spawnPoint;
    public int index;

    CameraDezoom cam;


    // Use this for initialization
    void Start ()
    {
        cam = GameObject.Find("Main Camera").GetComponent<CameraDezoom>();
        spawnPoint = transform.position;
        wisps = new GameObject[GameObject.FindGameObjectsWithTag("SleepingWisp").Length +1];
        wisps[0] = gameObject;
        index = 0;
	}

    public Transform AddFollower(GameObject g)
    {
        // if (index < wisps.Length)
        // {
        index++;
        if (index > 1)
        {
            for (int i = index; i > 1; i--)
            {
                wisps[i] = wisps[i - 1];
                wisps[i].GetComponent<WispIA>().SetIndex(i);
               // Debug.Log("Moved wisp from " + i + " to " + (i + 1));
            }
            wisps[1] = g;
            g.GetComponent<WispIA>().SetIndex(1);
            for (int i = 1; i <= index; i++)
            {
                wisps[i].GetComponent<WispIA>().SetTarget(wisps[i - 1].transform);
            }

            wisps[2].GetComponent<WispIA>().SetTarget(wisps[1].transform);
        }
        wisps[1] = g;
        g.GetComponent<WispIA>().SetIndex(1);
        cam.CameraZooming(1);
        //  }
        return transform;
    }

    public void KillFollower(int i)
    {
        if (i < index)
        {
            wisps[i + 1].GetComponent<WispIA>().SetTarget(wisps[i - 1].transform);

            for (int j = i+1; j <= index; j++)
            {
                wisps[j - 1] = wisps[j];
                wisps[j - 1].GetComponent<WispIA>().SetIndex(j - 1);
            }
            wisps[index] = null;
            index--;
        }
        else
        {
            wisps[index] = null;
            index--;
        }
        cam.CameraZooming(-1);
    }
    public Transform GetFollowing(int i)
    {
        Debug.Log(i);
        return wisps[i - 1].transform;
    }
    public void PlayerKill()
    {
        for(int j = 1; j <= index; j++)
        {
            wisps[j].GetComponent<WispIA>().ClearFollowers();
            cam.CameraZooming(-1);
        }
        index = 0;
        transform.position = spawnPoint;
    }

    public IEnumerator BreakChain(int i)
    {
        while(i <= index)
        {
            wisps[i].GetComponent<WispIA>().Death();
            yield return new WaitForSeconds(0.1f);
        }
    }

}
