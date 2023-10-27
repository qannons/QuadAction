using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    //Player player;

    public float raycastDistance; // ����ĳ��Ʈ �Ÿ� ����

    void Update()
    {
        // ī�޶��� ��ġ���� �ٶ󺸴� �������� ����ĳ��Ʈ�� �߻��մϴ�.
        Ray ray = new Ray(transform.position + Vector3.up * 20, transform.forward);
        RaycastHit hit;

        // ����ĳ��Ʈ�� � ������Ʈ�� �浹�ߴ��� Ȯ���մϴ�.
        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // ���� ����ĳ��Ʈ�� Ư�� ������Ʈ�� �浹�ߴٸ�, �ش� ������Ʈ���� ��ȣ�ۿ��� ������ �� �ֽ��ϴ�.
            if (hit.collider.gameObject.tag == "Door") // "Door"�� ��ȣ�ۿ��Ϸ��� ������Ʈ�� �±��Դϴ�.
            {
                // ���⿡ ��ȣ�ۿ� �ڵ带 �ۼ��ϼ���.
                Debug.Log("Interacted with the object!");
            }
            else
                Debug.Log("Not Interacted");
        }
        else
            Debug.Log("Any");
    }
}
