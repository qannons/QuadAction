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
    public Image grenadeImg;

    public TMP_Text[] enemyText;

    public RectTransform bossHPGroup;
    public RectTransform bossHPBar;

    // Start is called before the first frame update
    void Start()
    {
        maxScoretTxt.text = string.Format("{0:n0}", PlayerPrefs.GetInt("MaxScore"));
    }

    // Update is called once per frame
    void Update()
    {
        if (isBattle)
            playTime += Time.deltaTime;
    }

    private void LateUpdate()
    {
        scoreTxt.text = string.Format("{0:n0}", player.score);
        stageTxt.text = "STAGE " + stage;


        int hour = (int)(playTime / 3600);
        playTimeTxt.text = "";

        playerHPTxt.text = player.health + " / " + player.maxHP;
        playerCoinTxt.text = string.Format("{0:n0}", player.coin);

        if (player.equipWeapon == null)
            playerAmmoTxt.text = "- / -" + player.ammo;
        else if (player.equipWeapon.type == Weapon.Type.Melee)
            playerAmmoTxt.text = "- / " + player.ammo;
        else
            playerAmmoTxt.text = player.equipWeapon.curAmmo + " / " + player.ammo;

        //무기는 모두 들고 시작할꺼라서 비활성화했음
        //for(int i = 0; i < weaponImg.Length; i++)
        //    weaponImg[i].color = new Color(1, 1, 1, player.hasWeapons[i] ? 1 : 0);

        //폭탄 이미지
        grenadeImg.color = new Color(1, 1, 1, player.numGrenades > 0 ? 1 : 0);

        //적 숫자
        for(int i = 0; i < enemyText.Length; i ++)
            enemyText[i].text = enemyCnt[i].ToString();


    }
    public void GameStart()
    {
        menuCam.SetActive(false);
        gameCam.SetActive(true);

        menuPanel.SetActive(false);
        gamePanel.SetActive(true);

        //player.gameObject.SetActive(true); 
    }
}
