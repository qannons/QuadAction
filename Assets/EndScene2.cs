using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene2 : MonoBehaviour
{
    public float fadeSpeed = 0.5f;
    private CanvasGroup canvasGroup;
    bool flag = true;
    public GameManager gameManager;


    void Start()
    {
        // 해당 GameObject에 CanvasGroup 컴포넌트가 없다면 추가
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // 시작할 때 알파 값을 1로 초기화
        canvasGroup.alpha = 1f;
    }

    void Update()
    {
        if(flag == false ) { return; }
        // 시간에 따라 알파 값을 서서히 감소시켜 이미지가 어두워지도록 함
        
        canvasGroup.alpha = Mathf.Lerp(1f, 0f, Mathf.PingPong(Time.time * fadeSpeed, 1f));
        if (canvasGroup.alpha >= 0.997f ) 
        { 
            flag = false;
            gameManager.FloatEndSceneTxt();
        }
    }
}
