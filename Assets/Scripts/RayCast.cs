using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast : MonoBehaviour
{
    void Update()
    {
        // 플레이어의 시점에서 화면 가운데로 레이를 쏩니다.
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        // 레이가 어떤 오브젝트에 충돌했는지 체크합니다.
        if (Physics.Raycast(ray, out hit))
        {
            // 충돌한 오브젝트의 이름을 콘솔에 출력합니다.
            Debug.Log("Object hit: " + hit.transform.name);
        }
    }
}
