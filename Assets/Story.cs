using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.AI;

public class Story : MonoBehaviour
{
    public GameObject player;
    public Camera playerCam;
    public GameObject storyPanel;
    public GameObject the_End;
    public GameObject cam_Pos;
    public GameObject invisWall;
    public GameObject pauseMenu;
    public TMP_Text storyText;
    public TMP_Text endText;
    public BoxCollider2D storyContinueTrigger;
    public BoxCollider2D storyContinueTrigger2;
    public BoxCollider2D storyContinueTrigger3;
    public BoxCollider2D storyContinueTrigger4;
    public BoxCollider2D dialogueTrigger;
    public BoxCollider2D dialogueTrigger2;
    public BoxCollider2D dialogueTrigger3;
    public BoxCollider2D dialogueTrigger4;
    public BoxCollider2D dialogueTrigger5;
    public BoxCollider2D tape_End;
    private int storyProgress;

    void Start()
    {
        int saveState = PlayerPrefs.GetInt("SaveState", 0);

        if (saveState == 0)
            storyProgress = 0;
        else if (saveState == 1)
            storyProgress = 4;
        
        StartCoroutine(StoryBegin());
    }

    void Update()
    {
        if (storyProgress == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                storyProgress = 1;
            }
        }
        else if (storyProgress == 1)
        {
            if (storyContinueTrigger.IsTouching(tape_End))
            {
                storyProgress = 2;
            }
        }
        else if (storyProgress == 2)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                storyProgress = 3;
            }
        }
        else if (storyProgress == 3)
        {
            if (storyContinueTrigger2.IsTouching(tape_End))
            {
                PlayerPrefs.SetInt("SaveState", 1);
                PlayerPrefs.Save();
                storyProgress = 4;
            }
        }
        else if (storyProgress == 4)
        {
            if (storyContinueTrigger3.IsTouching(player.GetComponent<CircleCollider2D>()))
            {
                storyProgress = 5;
            }
        }
        else if (storyProgress == 5)
        {
            if (storyContinueTrigger4.IsTouching(player.GetComponent<CircleCollider2D>()))
            {
                storyProgress = 6;
            }
        }

        if (dialogueTrigger.IsTouching(player.GetComponent<CircleCollider2D>()))
        {
            dialogueTrigger.enabled = false;
            StartCoroutine(BeginDialogue("Why am I here? Why am i alive..?", 5));
        }

        if (dialogueTrigger2.IsTouching(player.GetComponent<CircleCollider2D>()))
        {
            dialogueTrigger2.enabled = false;
            StartCoroutine(BeginDialogue("...", 3));
        }

        if (dialogueTrigger3.IsTouching(player.GetComponent<CircleCollider2D>()))
        {
            dialogueTrigger3.enabled = false;
            StartCoroutine(BeginDialogue("Where am I going? Why must I scale?", 10));
        }

        if (dialogueTrigger4.IsTouching(player.GetComponent<CircleCollider2D>()))
        {
            dialogueTrigger4.enabled = false;
            StartCoroutine(BeginDialogue("I..", 5));
        }

        if (dialogueTrigger5.IsTouching(player.GetComponent<CircleCollider2D>()))
        {   
            dialogueTrigger5.enabled = false;
            StartCoroutine(BeginDialogue("I want to explore the whole, scale and see the wonders of the world..", 5));
        }

        if (Input.GetKeyDown(KeyCode.Escape) && pauseMenu.activeSelf == false)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void Continue()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void Restart()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene("Level_1");
    }

    public void Full_Screen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void Exit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    IEnumerator BeginDialogue(string dialogue, float seconds)
    {   
        storyPanel.SetActive(true);

        storyText.text = dialogue;
        yield return new WaitForSecondsRealtime(seconds);

        storyPanel.SetActive(false);
    }

    IEnumerator StoryBegin()
    {   
        storyPanel.SetActive(false);
        Time.timeScale = 0;
        the_End.SetActive(true);
        float x = 1;
        while (x > 0)
        {
            the_End.GetComponent<Image>().color = new Color(0, 0, 0, x);
            x -= 0.01f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        Time.timeScale = 1;
        the_End.SetActive(false);

        storyText.text = "CLICK TO TOSS YOUR MEASURING TAPE";
        storyPanel.SetActive(true);

        while (storyProgress == 0)
            yield return new WaitForSecondsRealtime(1);
        
        storyText.text = "PRESS SPACE TO LOCK YOUR POSITION";

        while (storyProgress == 1)
            yield return new WaitForSecondsRealtime(1);

        storyText.text = "PRESS W TO DECREASE LENGTH OF TAPE";

        while (storyProgress == 2)
            yield return new WaitForSecondsRealtime(1);
        
        storyPanel.SetActive(false);

        while (storyProgress == 3)
            yield return new WaitForSecondsRealtime(1);

        player.GetComponent<PlayerScript>().followCam = false;
        
        while (storyProgress == 4)
            yield return new WaitForSecondsRealtime(1);
        
        invisWall.SetActive(true);
        player.GetComponent<PlayerScript>().followCam = false;
        player.GetComponent<PlayerScript>().lockedCam = true;
        player.GetComponent<PlayerScript>().posY = cam_Pos.transform.position.y;
        storyText.text = "Almost there.. This is the final... step.";
        storyPanel.SetActive(true);
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(5);
        storyPanel.SetActive(false);
        Time.timeScale = 1;

        while (storyProgress == 5)
            yield return new WaitForSecondsRealtime(1);
        
        x = 0f;
        the_End.SetActive(true);
        while (x < 1)
        {
            the_End.GetComponent<Image>().color = new Color(0, 0, 0, x);
            x += 0.01f;
            yield return new WaitForSecondsRealtime(0.01f);
        }

        yield return new WaitForSecondsRealtime(3);

        x = 0f;
        while (x < 1)
        {
            endText.color = new Color(1, 1, 1, x);
            x += 0.01f;
            yield return new WaitForSecondsRealtime(0.02f);
        }

        yield return new WaitForSecondsRealtime(3);

        x = 1f;
        while (x > 0)
        {
            endText.color = new Color(1, 1, 1, x);
            x -= 0.01f;
            yield return new WaitForSecondsRealtime(0.02f);
        }

        yield return new WaitForSecondsRealtime(3);
        
        PlayerPrefs.SetInt("SaveState", 0);
        SceneManager.LoadScene("Menu");
    }
}
