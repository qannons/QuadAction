using UnityEngine;

public class book : MonoBehaviour
{
    public Light pulsatingLight;
    public float maxIntensity = 0.4f;
    public float pulseSpeed = 0.5f; // 주기를 조절하는 값

    void Start()
    {
        if (pulsatingLight == null)
        {
            pulsatingLight = GetComponent<Light>();
        }

        if (pulsatingLight != null)
        {
            // 시작 시 빛이 나지 않도록 초기 Intensity를 0으로 설정
            pulsatingLight.intensity = 0f;

            // InvokeRepeating에서 주기를 조절합니다.
            InvokeRepeating("ToggleLight", 0f, 1f / pulseSpeed);
        }
        else
        {
            Debug.LogError("Light component is missing!");
        }
    }

    void ToggleLight()
    {
        // 현재 intensity 값을 가져옵니다.
        float currentIntensity = pulsatingLight.intensity;

        // 빛이 꺼져 있는 경우
        if (currentIntensity == 0f)
        {
            // 빛을 켭니다.
            pulsatingLight.intensity = maxIntensity;
        }
        else
        {
            // 빛을 끕니다.
            pulsatingLight.intensity = 0f;
        }
    }
}