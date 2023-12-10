using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryCanvas : MonoBehaviour
{
    private Player player;
    public GameObject storyCanvas;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if()
    }

    public void OpenStoryCanvas()
    {
        player.canMove = false;
        storyCanvas.SetActive(true);
    }
    
    public void CloseStoryCanvas()
    {
        player.canMove = true;
        storyCanvas.SetActive(true);
    }
    
}
