using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIController : MonoBehaviour
{
    [SerializeField] private Image[] timelineButtonImages;

    public delegate void SceneSwitch(int sceneToLoad);
    public event SceneSwitch OnSwitchToScene;

    private static UIController instance;
    public static UIController GetInstance()
    {
        return instance;
    }

    public bool animationHasFinished;
    private bool sceneSwitch;

    private IEnumerator lastTimelineSwop;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void OnTimelineButtonClick()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("currentSceneIndex: " + currentSceneIndex);

        // Make sure the current scene animation is finished. 
        if (!animationHasFinished)
            return;
        // Make sure the scene switch is called once.
        if (sceneSwitch)
            return;
        sceneSwitch = true;
        // Extra check to make sure only one coroutine at a time is called.
        if (lastTimelineSwop != null)
            return;

        switch (currentSceneIndex)
        {
            // Scene 1
            case 0:
                OnSwitchToScene(1);
                break;
            // Scene 2
            case 1:
                OnSwitchToScene(2);
                break;
            // Scene 3
            case 2:
                OnSwitchToScene(0);
                break;
        }
    }
    
    // The 'TimelineSwop' coroutine is called on the 'OnSwitchScene' event via the respective Scene Manager. 
    public void StartTimelineSwop(int currentScene, int nextScene)
    {
        lastTimelineSwop = TimelineSwop(currentScene, nextScene);
        StartCoroutine(lastTimelineSwop);
    }

    public IEnumerator TimelineSwop(int currentScene,int nextScene)
    {
        while(SceneManager.GetActiveScene().buildIndex == currentScene)
        {
            yield return null;
        }

        for (int i = 0; i < timelineButtonImages.Length; i++)
        {
            if (timelineButtonImages[i] == timelineButtonImages[nextScene])
            {
                timelineButtonImages[i].gameObject.GetComponent<Animation>().enabled = true;
            }
            else
            {
                timelineButtonImages[i].gameObject.GetComponent<Animation>().enabled = false;
            }
        }

        animationHasFinished = false;
        sceneSwitch = false;

        lastTimelineSwop = null;
    }

}


/*
    // Simulating a finger touch on the timeline (one clickable zone). 
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            switch (currentSceneIndex)
            {
                case 0:
                    OnSwitchToScene(1);
                    ChangeTimelineUI(1);
                    break;
                case 1:
                    OnSwitchToScene(2);
                    ChangeTimelineUI(2);
                    break;
                case 2:
                    OnSwitchToScene(0);
                    ChangeTimelineUI(0);
                    break;
            }
        }
    }
*/
