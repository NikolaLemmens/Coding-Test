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
        switch (currentSceneIndex)
        {
            case 0:
                OnSwitchToScene(1);
                StartCoroutine(ChangeTimelineUI(0,1));
                break;
            case 1:
                OnSwitchToScene(2);
                break;
            case 2:
                OnSwitchToScene(0);
                StartCoroutine(ChangeTimelineUI(2,0));
                break;
        }
    }

    public void SwopTimelineUI()
    {
        StartCoroutine(ChangeTimelineUI(2, 0));
    }

    public IEnumerator ChangeTimelineUI(int currentScene,int nextScene)
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
