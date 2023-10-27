using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    //Player player;

    public float raycastDistance; // 레이캐스트 거리 설정

    void Update()
    {
        // 카메라의 위치에서 바라보는 방향으로 레이캐스트를 발사합니다.
        Ray ray = new Ray(transform.position + Vector3.up * 20, transform.forward);
        RaycastHit hit;

        // 레이캐스트가 어떤 오브젝트와 충돌했는지 확인합니다.
        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // 만약 레이캐스트가 특정 오브젝트와 충돌했다면, 해당 오브젝트와의 상호작용을 구현할 수 있습니다.
            if (hit.collider.gameObject.tag == "Door") // "Door"는 상호작용하려는 오브젝트의 태그입니다.
            {
                // 여기에 상호작용 코드를 작성하세요.
                Debug.Log("Interacted with the object!");
            }
            else
                Debug.Log("Not Interacted");
        }
        else
            Debug.Log("Any");
    }
}
