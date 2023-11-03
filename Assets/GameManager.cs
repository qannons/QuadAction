using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject menuCam;
    public GameObject gameCam;
    public Player player;
    public Boss boss;
    public GameObject itemShop;
    public GameObject startZone;

    public Transform[] enemyZones;
    public GameObject[] enemies;
    public List<int> enemyList;

    public int stage;
    public float playTime;
    public bool isBattle;
    public int[] enemyCnt = new int[4] {0, 0, 0, 0};

    public GameObject menuPanel;
    public GameObject gamePanel;
    public GameObject overPanel;
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

    public TMP_Text curScoreText;
    public TMP_Text bestScoreText;

    public RectTransform bossHPGroup;
    public RectTransform bossHPBar;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.U
        //enemyList = new List<int>();
        //maxScoretTxt.text = string.Format("{0:n0}", PlayerPrefs.GetInt("MaxScore"));

        //if(PlayerPrefs.HasKey("MaxScore"))
        //{
        //    PlayerPrefs.SetInt("MaxScore", 0);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("d");
        if (isBattle)
            playTime += Time.deltaTime;
    }

    private void LateUpdate()
    {
        //Player UI
        //playerHPTxt.text = player.health + " / " + player.maxHP;
        //playerCoinTxt.text = string.Format("{0:n0}", player.coin);

        //if (player.equipWeapon == null)
        //    playerAmmoTxt.text = "- / -" + player.ammo;
        //else if (player.equipWeapon.type == Weapon.Type.Melee)
        //    playerAmmoTxt.text = "- / " + player.ammo;
        //else
        //    playerAmmoTxt.text = player.equipWeapon.curAmmo + " / " + player.ammo;

        //무기 UI
        //무기는 모두 들고 시작할꺼라서 비활성화했음
        //for(int i = 0; i < weaponImg.Length; i++)
        //    weaponImg[i].color = new Color(1, 1, 1, player.hasWeapons[i] ? 1 : 0);



          
        //보스 UI
        //if(boss != null)
        //{
        //    bossHPGroup.anchoredPosition = Vector3.down * 30;
        //    bossHPBar.localScale = new Vector3((float)boss.curHealth / boss.maxHealth, 1, 1);
        //}
        //else
        //    bossHPGroup.anchoredPosition = Vector3.up * 200;
    }
    public void GameStart()
    {
        menuCam.SetActive(false);
        gameCam.SetActive(true);

        menuPanel.SetActive(false);
        gamePanel.SetActive(true);

        //player.gameObject.SetActive(true); 
    }

    public void StageStart()
    {
        itemShop.SetActive(false);
        startZone.SetActive(false);

        foreach(Transform zone in enemyZones) 
            zone.gameObject.SetActive(true);

        isBattle = true;
        StartCoroutine(InBattle());
    }

    public void StageEnd()
    {
        player.transform.position = new Vector3(0, 0, 0);
        itemShop.SetActive(true);
        startZone.SetActive(true);

        foreach (Transform zone in enemyZones)
            zone.gameObject.SetActive(true);

        isBattle = false;
        stage++;
    }

    IEnumerator InBattle()
    {
        if(stage % 5 == 0)
        {
            enemyCnt[3]++;
            GameObject instantEnemy = Instantiate(enemies[3], enemyZones[0].position, enemyZones[0].rotation);
            Enemy enemy = instantEnemy.GetComponent<Enemy>();
            enemy.target = player.transform;
            enemy.gameManager = this;
            boss = instantEnemy.GetComponent<Boss>();
        }
        else
        {
            for(int i = 0; i < stage; i++)
            {
                int ran = Random.Range(0, 3);
                enemyList.Add(ran);

                enemyCnt[ran]++;
            }
            while(enemyList.Count > 0)
            {
                int ranZone = Random.Range(0, 4);
                GameObject instantEnemy = Instantiate(enemies[enemyList[0]], enemyZones[ranZone].position, enemyZones[ranZone].rotation);
                Enemy enemy = instantEnemy.GetComponent<Enemy>();
                enemy.target = player.transform;
                enemy.gameManager = this;
                enemyList.RemoveAt(0);

                yield return new WaitForSeconds(4f);
            }
        }

        while(enemyCnt[0] + enemyCnt[1] + enemyCnt[2] + enemyCnt[3] > 0)
        {
            yield return null;
        }

        yield return new WaitForSeconds(4f);
        boss = null;
        StageEnd(); 
    }

    public void GameOver()
    {
        overPanel.SetActive(true);
        gamePanel.SetActive(false);

        curScoreText.text = scoreTxt.text;

        int maxScore = PlayerPrefs.GetInt("MaxScore");
        if (player.score > maxScore)
         {
            bestScoreText.gameObject.SetActive(true);
            PlayerPrefs.SetInt("MaxScore", player.score);
        }  
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

}
