using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndScene : MonoBehaviour
{
    private Player player;
    
    private float delay = 2f;
    private float fadeTime = 2f;

    private float time;
    

    private void Update()
    {
        time += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        //player.canMove = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject endObj = GameObject.Find("EndScene");
            Image image = endObj.GetComponentInChildren<Image>();
            Color color = image.color;
            
            if (image.color.a < 1f) // 투명도가 1보다 작을 때
            {
                color.a = 0.2f * time;
                image.color = color;
            }
            else
            {
                TextMeshProUGUI endText = endObj.GetComponentInChildren<TextMeshProUGUI>();
                endText.gameObject.SetActive(true);
            }
        }
        
    }
    
}
