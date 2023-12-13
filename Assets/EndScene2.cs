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
        // �ش� GameObject�� CanvasGroup ������Ʈ�� ���ٸ� �߰�
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // ������ �� ���� ���� 1�� �ʱ�ȭ
        canvasGroup.alpha = 1f;
    }

    void Update()
    {
        if(flag == false ) { return; }
        // �ð��� ���� ���� ���� ������ ���ҽ��� �̹����� ��ο������� ��
        
        canvasGroup.alpha = Mathf.Lerp(1f, 0f, Mathf.PingPong(Time.time * fadeSpeed, 1f));
        if (canvasGroup.alpha >= 0.997f ) 
        { 
            flag = false;
            gameManager.FloatEndSceneTxt();
        }
    }
}
