using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public Canvas level1Menu;
    public Canvas level2Menu;
    public Canvas level3Menu;
    public Canvas backToMenu;

    public Text[] amountSaved;

    public Text[] biggestChain;

    public Text[] soulsRemaining;
    // Use this for initialization
    void Start ()
    {
        level1Menu = level1Menu.GetComponent<Canvas>();
        level2Menu = level2Menu.GetComponent<Canvas>();
        level3Menu = level3Menu.GetComponent<Canvas>();
        backToMenu = backToMenu.GetComponent<Canvas>();
        level1Menu.enabled = false;
        level2Menu.enabled = false;
        level3Menu.enabled = false;
        backToMenu.enabled = false;

        amountSaved[0].text = DataManagement.data.savedDandelions[0].ToString() + " / 73";
        amountSaved[1].text = DataManagement.data.savedDandelions[1].ToString() + " / 80";
        amountSaved[2].text = DataManagement.data.savedDandelions[2].ToString() + " / 65";

        for(int i = 0; i< 3; i++)
        {
            biggestChain[i].text = DataManagement.data.biggestFollowerAmount[i].ToString();
            soulsRemaining[i].text = DataManagement.data.topScore[i].ToString();
        }
    }

    public void Lvl1()
    {
        level1Menu.enabled = true;
        level2Menu.enabled = false;
        level3Menu.enabled = false;
        backToMenu.enabled = true;
    }

    public void Lvl2()
    {
        level1Menu.enabled = false;
        level2Menu.enabled = true;
        level3Menu.enabled = false;
        backToMenu.enabled = true;
    }

    public void Lvl3()
    {
        level1Menu.enabled = false;
        level2Menu.enabled = false;
        level3Menu.enabled = true;
        backToMenu.enabled = true;
    }

    public void Lvl1Play()
    {
        SceneManager.LoadScene(1);
    }
    public void Lvl2Play()
    {
        SceneManager.LoadScene(2);
    }
    public void Lvl3Play()
    {
        SceneManager.LoadScene(3);
    }
    public void BackToMenu()
    {
        level1Menu.enabled = false;
        level2Menu.enabled = false;
        level3Menu.enabled = false;
        backToMenu.enabled = false;
    }
}
