using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range};
    public Type type;
    public int damage;
    public float rate;
    public int maxAmmo;
    public int curAmmo;
    public BoxCollider meleeArea;
    public TrailRenderer trailRenderer;
    
    public Transform bulletPos;
    public GameObject bullet;

    public Transform bulletCasePos;
    public GameObject bulletCase;

    public void Awake()
    {
        if(type == Type.Melee)
            meleeArea.enabled = false;
    }
    public void Use()
    {
        if(type == Type.Melee) 
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
        else if(type == Type.Range && curAmmo > 0)
        {
            StartCoroutine("Shot");
            --curAmmo;

        }
    }

    IEnumerator Swing()
    {
        //yield 키워드를 여러 개 사용하여 시간 차 로직 작성 가능

        //1
        yield return new WaitForSeconds(0.4f); //0.1초 대기
        meleeArea.enabled = true;
        trailRenderer.enabled = true;
        //2
        yield return new WaitForSeconds(1f); //0.1초 대기
        meleeArea.enabled = false;
        trailRenderer.enabled = false;

        //3
        //yield return new WaitForSeconds(0.1f); //0.1초 대기

    }
    IEnumerator Shot()
    {
        //1. 총알 발사
        GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bullletRigid = instantBullet.GetComponent<Rigidbody>();
        //인스턴스화 된 총알에 속도 적용하기
        bullletRigid.velocity = bulletPos.forward * 50;
        //bullletRigid.AddForce(Vector3.down * 0.1f);

        yield return null;
        //2. 탄피 배출
        GameObject instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody caseRigid = instantCase.GetComponent<Rigidbody>();
        //탄피에 랜덤화 힘 가하기
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, - 2) + Vector3.up * Random.Range(2, 3);
        caseRigid.AddForce(caseVec, ForceMode.Impulse);
        //회전하면서 나갈거임
        caseRigid.AddTorque(Vector3.up*10, ForceMode.Impulse);

    }
}
