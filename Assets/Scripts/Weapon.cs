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
        //yield Ű���带 ���� �� ����Ͽ� �ð� �� ���� �ۼ� ����

        //1
        yield return new WaitForSeconds(0.1f); //0.1�� ���
        meleeArea.enabled = true;
        trailRenderer.enabled = true;
        //2
        yield return new WaitForSeconds(0.3f); //0.1�� ���
        meleeArea.enabled = false;

        //3
        yield return new WaitForSeconds(0.3f); //0.1�� ���
        trailRenderer.enabled = false;

    }
    //Use() ���η�ƾ -> Swing() �����ƾ -> Use() ���� ��ƾ...
    //�ڷ�ƾ�� ���
    //Use() ���η�ƾ + Swing() �ڷ�ƾ (Co-Op)
}
