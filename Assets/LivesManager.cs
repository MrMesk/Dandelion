using UnityEngine;
using System.Collections;

public class LivesManager : MonoBehaviour
{
    [Range(0,5)]
    public int lives = 2;
    int actualLives;
    public GameObject[] livesObjects;
	// Use this for initialization
	void Start ()
    {
        actualLives = lives;
        livesObjects[0].SetActive(true);
        livesObjects[1].SetActive(true);
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void LifeCHange(int lifeModif)
    {
        actualLives += lifeModif;
    }

    void LifeCheck()
    {
        switch(actualLives)
        {
            case -1:
                GameOver();
                break;

            case 0:
                livesObjects[0].SetActive(false);
                livesObjects[1].SetActive(false);
                break;

            case 1:
                livesObjects[0].SetActive(true);
                livesObjects[1].SetActive(false);
                livesObjects[2].SetActive(false);
                break;

            case 2:
                livesObjects[1].SetActive(true);
                livesObjects[2].SetActive(false);
                break;

            case 3:
                livesObjects[2].SetActive(true);
                livesObjects[3].SetActive(false);
                break;

            case 4:
                livesObjects[3].SetActive(true);
                livesObjects[4].SetActive(false);
                break;
            case 5:
                livesObjects[5].SetActive(true);
                break;
        }
    }

    void GameOver()
    {

    }
}
