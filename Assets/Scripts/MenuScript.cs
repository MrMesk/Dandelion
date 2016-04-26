using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuScript : MonoBehaviour
{
    public Canvas quitMenu;
    public Canvas optionsMenu;
    public Button startText;
    public Button exitText;
    public Button optionsText;
    public float fadeSpeed;

    public Text[] StartTexts;
	// Use this for initialization
	void Start ()
    {
        quitMenu = quitMenu.GetComponent<Canvas>();
        optionsMenu = optionsMenu.GetComponent<Canvas>();
        startText = startText.GetComponent<Button>();
        exitText = exitText.GetComponent<Button>();
        quitMenu.enabled = false;
        optionsMenu.enabled = false;
    }
    
    public void ExitPress()
    {
        quitMenu.enabled = true;
        startText.enabled = false;
        optionsText.enabled = false;
        exitText.enabled = false;
    }

    public void OptionsPress()
    {
        optionsMenu.enabled = true;
        startText.enabled = false;
        optionsText.enabled = false;
        exitText.enabled = false;
    }

    public void NoPress()
    {
        quitMenu.enabled = false;
        optionsMenu.enabled = false;
        startText.enabled = true;
        optionsText.enabled = true;
        exitText.enabled = true;
    }

    public void BackToMenu()
    {
        startText.enabled = true;
        optionsText.enabled = true;
        exitText.enabled = true;
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartTextFadeOutPress()
    {
        startText.enabled = false;
        optionsText.enabled = false;
        exitText.enabled = false;

        for (int i = 0; i< StartTexts.Length; i++)
        {
            StartCoroutine(TextFadeOut(StartTexts[i]));
        }
    }

    public void StartTextFadeInPress()
    {
        for (int i = 0; i < StartTexts.Length; i++)
        {
            StartCoroutine(TextFadeIn(StartTexts[i]));
        }
    }

    IEnumerator TextFadeOut(Text text)
    {
  
        Color col = text.color;
   
        while (text.color.a > 0.05f)
        {   
            //col.a -= fadeSpeed * Time.deltaTime;
            col.a = Mathf.Lerp(col.a, 0, fadeSpeed * 2 * Time.deltaTime);
            text.color = col;
            yield return null;
        }
        col.a = 0f;
        text.color = col;

    }

    IEnumerator TextFadeIn(Text text)
    {
        Color col = text.color;
        while (text.color.a < 0.95f)
        {
            col.a = Mathf.Lerp(col.a, 1, fadeSpeed * Time.deltaTime);
            text.color = col;
            yield return null;
        }
        col.a = 1f;
        text.color = col;
    }
}
