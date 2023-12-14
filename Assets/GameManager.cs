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

    public GameObject accountBookPanel;
    public GameObject accountBookPanel2;
    public GameObject medicalBookPanel;
    public GameObject diaryPanel;
    public GameObject noteBookPanel;
    public GameObject endScenePanel;
    
    public TextMeshProUGUI endSceneTxt;
    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(this);
        //playerCamera = GetComponentInChildren<Camera>();
    }
    void Start()
    {
        SceneManager.LoadScene("shop");
        //SceneManager.LoadScene("VillageScene");
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
        accountBookPanel.SetActive(true);
    }
    public void CloseAccountStoryPanel()
    {
        player.canMove = true;
        accountBookPanel.SetActive(false);
    }
    public void FloatAccountStoryPanel2()
    {
        player.canMove = false;
        accountBookPanel2.SetActive(true);
    }
    public void CloseAccountStoryPanel2()
    {
        player.canMove = true;
        accountBookPanel2.SetActive(false);
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

    public void FloatMedicalBookPanel()
    {
        player.canMove = false;
        medicalBookPanel.SetActive(true);
    }
    public void CloseMedicalBookPanel()
    {
        player.canMove = true;
        medicalBookPanel.SetActive(false);
    }

    public void FloatNoteBookPanel()
    {
        player.canMove = false;
        noteBookPanel.SetActive(true);
    }
    public void CloseNoteBookPanel()
    {
        player.canMove = true;
        noteBookPanel.SetActive(false);
    }
    public void FloatEndScenePanel()
    {
        player.canMove = false;
        endScenePanel.SetActive(true);
    }
    public void FloatEndSceneTxt()
    {
        //player.canMove = false;
        endSceneTxt.gameObject.SetActive(true);
    }
}
