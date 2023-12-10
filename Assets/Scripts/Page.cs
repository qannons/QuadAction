using UnityEngine;
using UnityEngine.UI;

public class Page : MonoBehaviour
{
    public Button leftButton;
    public Button rightButton;
    public Sprite[] imageArray;
    private int index;

    private void Start()
    {
        leftButton.onClick.AddListener(OnClickLeftButton);
        rightButton.onClick.AddListener(OnClickRightButton);
    }

    private void OnClickLeftButton()
    {
        if(index > 0)
        {
            index--;
            GetComponent<Image>().sprite = imageArray[index];
        }
    }

    private void OnClickRightButton()
    {
        if(index < imageArray.Length - 1)
        {
            index++;
            GetComponent<Image>().sprite = imageArray[index];
        }
    }
}