using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScene : MonoBehaviour
{
    public float delay = 2f;
    public float fadeTime = 2f;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Load());
        }
    }
    
    IEnumerator Load()
    {
        yield return new WaitForSeconds(delay);

        // 이미지 불러오기
        Image image = GetComponent<Image>();
        RectTransform rectTransform = image.rectTransform;
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height);
        image.color = Color.clear;

        // Fade In 효과
        float startTime = Time.time;
        while (Time.time - startTime <= fadeTime)
        {
            image.color = Color.Lerp(Color.clear, Color.black, (Time.time - startTime) / fadeTime);
            yield return null;
        }
        image.color = Color.black;
        
        yield return new WaitForSeconds(delay);

    }
}
