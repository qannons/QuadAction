using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : Enemy 
{
    // Start is called before the first frame update
    public GameObject missile;
    public Transform missilePortA;
    public Transform missilePortB;

    Vector3 lookVec;
    Vector3 tauntVec;
    bool isLook = true;
    
    protected override void Start()
    {
        base.Start();
        StartCoroutine(Think());
        nav.isStopped = true;
    }

    // Update is called once per frame
   public override void Update()
    {
        if (isAlive == false)
        {
            StopAllCoroutines();
            return;
        }

        if (isLook)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            lookVec = new Vector3(h, 0, v) * 5f;
            transform.LookAt(target.position + lookVec);
        }
        else
            nav.SetDestination(tauntVec);
    }

    IEnumerator Think() 
    {
        yield return new WaitForSeconds(0.1f);

        //int ranAction = Random.Range(0, 5);
        int ranAction = 4;
        switch (ranAction) 
        {
            case 0:
            case 1:
                StartCoroutine(MissileShot());
                break;
            case 2:
            case 3:
                StartCoroutine (RockShot());
                break;
            case 4:
                StartCoroutine(Taunt());
                break;
        }
    }

    IEnumerator MissileShot()
    {
        animator.SetTrigger("doShot");
        yield return new WaitForSeconds(0.2f);
        GameObject instantMissileA = Instantiate(missile, missilePortA.position, missilePortA.rotation);
        BossMissile bossMissileA = instantMissileA .GetComponent<BossMissile>();
        bossMissileA.target = target;
        

        yield return new WaitForSeconds(0.3f);
        GameObject instantMissileB = Instantiate(missile, missilePortB.position, missilePortB.rotation);
        BossMissile bossMissileB = instantMissileB.GetComponent<BossMissile>();
        bossMissileB.target = target;

        yield return new WaitForSeconds(2f);

        StartCoroutine(Think());
    }

    IEnumerator RockShot()
    {
        animator.SetTrigger("doBigShot");

        Instantiate(bullet, transform.position, transform.rotation);

        yield return new WaitForSeconds(2f);

        StartCoroutine(Think());
    }

    IEnumerator Taunt()
    {
        tauntVec = target.position;

        nav.isStopped = false;
        isLook = false;
        boxCollider.enabled = false;
        animator.SetTrigger("doTaunt");

        yield return new WaitForSeconds(1.125f);
        meleeArea.enabled = true;
        yield return new WaitForSeconds(0.375f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.75f);

        nav.isStopped = true;
        isLook = true;
        boxCollider.enabled = true;
        StartCoroutine(Think());
    }
}
