using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuCharacterController : MonoBehaviour
{
    NavMeshAgent nav;

    public Vector3 lvl1Pos;
    public Vector3 lvl2Pos;
    public Vector3 lvl3Pos;

    // Use this for initialization
    void Start ()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    public void Level1()
    {
        SwitchLevel(lvl1Pos);
    }
    public void SwitchLevel(Vector3 newPos)
    {
        nav.SetDestination(newPos);
    }
}
