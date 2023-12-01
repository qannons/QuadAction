using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast : MonoBehaviour
{
    void Update()
    {
        // �÷��̾��� �������� ȭ�� ����� ���̸� ���ϴ�.
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        // ���̰� � ������Ʈ�� �浹�ߴ��� üũ�մϴ�.
        if (Physics.Raycast(ray, out hit))
        {
            // �浹�� ������Ʈ�� �̸��� �ֿܼ� ����մϴ�.
            Debug.Log("Object hit: " + hit.transform.name);
        }
    }
}
