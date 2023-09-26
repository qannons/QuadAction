using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject menuCam;
    public GameObject gameCam;
    public Player player;
    public Boss boss;
    public int stage;
    public float playTime;
    public bool isBattle;
    public int[] enemyCnt;

    public GameObject menuPanel;
    public GameObject gamePanel;
    public TMP_Text maxScoretTxt;
    public TMP_Text scoreTxt;
    public TMP_Text stageTxt;
    public TMP_Text playTimeTxt;
    public TMP_Text playerHPTxt;
    public TMP_Text playerAmmoTxt;
    public TMP_Text playerCoinTxt;

    public Image[] weaponImg;

    public TMP_Text[] enemyText;

    public RectTransform bossHPGroup;
    public RectTransform bossHPBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
