using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq.Expressions;

public class Menu_Script : MonoBehaviour
{
    public Button beginButton;
    public GameObject resetButtonObject;
    public Button resetButton;
    public GameObject fadeOut;

    // Start is called before the first frame update
    void Start()
    {
        fadeOut.transform.localScale = new Vector3(2, 2, 2);
        StartCoroutine(FadeIn());

        if (PlayerPrefs.GetInt("SaveState", 0) == 1)
        {
            resetButtonObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Begin()
    {
        StartCoroutine(Proceed(false));
    }

    public void Restart()
    {
        StartCoroutine(Proceed(true));
    }

    IEnumerator FadeIn()
    {   
        fadeOut.SetActive(true);
        fadeOut.GetComponent<Image>().color = new Color(0, 0, 0, 1);

        if (PlayerPrefs.GetInt("SaveState", 0) == 1)
        {
            yield return new WaitForSecondsRealtime(1);
        }
        else
        {
            yield return new WaitForSecondsRealtime(5);
        }

        float x = 1;
        while (x > 0)
        {
            fadeOut.GetComponent<Image>().color = new Color(0, 0, 0, x);
            x -= 0.01f;
            yield return new WaitForSecondsRealtime(0.1f);
        }

        fadeOut.SetActive(false);
    }

    IEnumerator Proceed(bool reset)
    {
        fadeOut.SetActive(true);
        float x = 0;
        while (x < 1)
        {
            fadeOut.GetComponent<Image>().color = new Color(0, 0, 0, x);
            x += 0.01f;
            yield return new WaitForSecondsRealtime(0.02f);
        }
        
        if (reset == true)
            PlayerPrefs.SetInt("SaveState", 0);
        
        SceneManager.LoadScene("Level_1");
    }
}
