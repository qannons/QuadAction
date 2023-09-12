using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range};
    public Type type;
    public int damage;
    public float rate;
    public BoxCollider meleeArea;
    public TrailRenderer trailRenderer;

    public void Use()
    {
        if(type == Type.Melee) 
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
    }

    IEnumerator Swing()
    {
        //yield 키워드를 여러 개 사용하여 시간 차 로직 작성 가능

        //1
        yield return new WaitForSeconds(0.1f); //0.1초 대기
        meleeArea.enabled = true;
        trailRenderer.enabled = true;
        //2
        yield return new WaitForSeconds(0.3f); //0.1초 대기
        meleeArea.enabled = false;

        //3
        yield return new WaitForSeconds(0.3f); //0.1초 대기
        trailRenderer.enabled = false;

    }
    //Use() 메인루틴 -> Swing() 서브루틴 -> Use() 메인 루틴...
    //코루틴의 경우
    //Use() 메인루틴 + Swing() 코루틴 (Co-Op)
}
