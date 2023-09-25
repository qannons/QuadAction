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
        //yield Ű���带 ���� �� ����Ͽ� �ð� �� ���� �ۼ� ����

        //1
        yield return new WaitForSeconds(0.4f); //0.1�� ���
        meleeArea.enabled = true;
        trailRenderer.enabled = true;
        //2
        yield return new WaitForSeconds(1f); //0.1�� ���
        meleeArea.enabled = false;
        trailRenderer.enabled = false;

        //3
        //yield return new WaitForSeconds(0.1f); //0.1�� ���

    }
    IEnumerator Shot()
    {
        //1. �Ѿ� �߻�
        GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bullletRigid = instantBullet.GetComponent<Rigidbody>();
        //�ν��Ͻ�ȭ �� �Ѿ˿� �ӵ� �����ϱ�
        bullletRigid.velocity = bulletPos.forward * 50;
        //bullletRigid.AddForce(Vector3.down * 0.1f);

        yield return null;
        //2. ź�� ����
        GameObject instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody caseRigid = instantCase.GetComponent<Rigidbody>();
        //ź�ǿ� ����ȭ �� ���ϱ�
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, - 2) + Vector3.up * Random.Range(2, 3);
        caseRigid.AddForce(caseVec, ForceMode.Impulse);
        //ȸ���ϸ鼭 ��������
        caseRigid.AddTorque(Vector3.up*10, ForceMode.Impulse);

    }
}
