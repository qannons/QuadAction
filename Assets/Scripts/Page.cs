using UnityEngine;
using UnityEngine.UI;

public class Page : MonoBehaviour
{
    public Button leftButton;
    public Button rightButton;
    public Sprite[] imageArray;
    private int index = 0;

    private void Start()
    {
        leftButton = GetComponent<Button>();
        rightButton = GetComponent<Button>();
        leftButton.onClick.AddListener(OnClickLeftButton);
        rightButton.onClick.AddListener(OnClickRightButton);
    }

    public void OnClickLeftButton()
    {
        if(index > 0)
        {
            index--;
            GetComponent<Image>().sprite = imageArray[index];
        }
    }

    public void OnClickRightButton()
    {
        if(index < imageArray.Length - 1)
        {
            index++;
            GetComponent<Image>().sprite = imageArray[index];
        }
    }

    public void OnClickCloseButton()
    {

    }
}