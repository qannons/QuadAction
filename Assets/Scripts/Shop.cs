using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public RectTransform uiGroup;
    public Animator animator;
    public GameObject[] itemObjects;
    public int[] itemPrices;
    public Transform[] itemPoses;
    public TMP_Text talkText;
    public string[] talklDatas;
    Player enterPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Enter(Player player)
    {
        enterPlayer = player;
        //È­¸é Á¤Áß¾Ó¿¡ ¿À°Ô²û
        uiGroup.anchoredPosition = Vector3.zero;
    }

    public void Exit()
    {
        animator.SetTrigger("doHello");
        uiGroup.anchoredPosition = Vector3.down * 1000;
    }    

    public void Buy(int index)
    {
        int price = itemPrices[index];
        if(price > enterPlayer.coin)
        {
            StopCoroutine(Talk());
            StartCoroutine(Talk());
            return;
        }
        enterPlayer.coin -= price;
        Vector3 ranVec = Vector3.right * Random.Range(-3, 3) + Vector3.forward * Random.Range(-3, 3);
        Instantiate(itemObjects[index], itemPoses[index].position + ranVec, itemPoses[index].rotation);
    }

    IEnumerator Talk() 
    {
        talkText.text = talklDatas[1];
        yield return new WaitForSeconds(2f);
        talkText.text = talklDatas[0];
    }
}
