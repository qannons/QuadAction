using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public Player player;
    public GameObject itemShop;
    public GameObject startZone;
    public GameObject MoveSceneTxt;
    public GameObject BookTxt;

    public GameObject accountbookPanel;
    public GameObject accountbookPanel2;
    public GameObject diaryPanel;

    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(this);
        //playerCamera = GetComponentInChildren<Camera>();
    }
    void Start()
    {
        SceneManager.LoadScene("shop");
        //SceneManager.LoadScene("NaeBu1");
    }


    public void FloatMoveSceneTxt()
    {
        MoveSceneTxt.SetActive(false);
        MoveSceneTxt.SetActive(true);
    }
    public void CloseMoveSceneTxt()
    {
        MoveSceneTxt.SetActive(false);
    }

    public void FloatBookTxt()
    {
        BookTxt.SetActive(false);
        BookTxt.SetActive(true);
    }
    public void CloseBookTxt()
    {
        BookTxt.SetActive(false);
    }
    
    public void FloatAccountStoryPanel()
    {
        player.canMove = false;
        accountbookPanel.SetActive(true);
    }
    public void CloseAccountStoryPanel()
    {
        player.canMove = true;
        accountbookPanel.SetActive(false);
    }
    public void FloatAccountStoryPanel2()
    {
        player.canMove = false;
        accountbookPanel2.SetActive(true);
    }
    public void CloseAccountStoryPanel2()
    {
        player.canMove = true;
        accountbookPanel2.SetActive(false);
    }
    
    public void FloatDiaryStoryPanel()
    {
        player.canMove = false;
        diaryPanel.SetActive(true);
    }
    public void CloseDiaryStoryPanel()
    {
        player.canMove = true;
        diaryPanel.SetActive(false);
    }
}
