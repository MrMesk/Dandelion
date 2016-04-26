using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public Canvas level1Menu;
    public Canvas level2Menu;
    public Canvas level3Menu;
    // Use this for initialization
    void Start ()
    {
        level1Menu = level1Menu.GetComponent<Canvas>();
        level2Menu = level2Menu.GetComponent<Canvas>();
        level3Menu = level3Menu.GetComponent<Canvas>();
        level1Menu.enabled = false;
        level2Menu.enabled = false;
        level3Menu.enabled = false;
    }

    public void Lvl1()
    {
        level1Menu.enabled = true;
        level2Menu.enabled = false;
        level3Menu.enabled = false;
    }

    public void Lvl2()
    {
        level1Menu.enabled = false;
        level2Menu.enabled = true;
        level3Menu.enabled = false;
    }
}
