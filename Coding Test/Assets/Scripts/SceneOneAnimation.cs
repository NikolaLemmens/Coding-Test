using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneOneAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private int nextScene;
    /*
    private UIController _UIControl;
    private UIController UIControl
    {
        get
        {
            if (_UIControl == null)
            {
                _UIControl = FindObjectOfType<UIController>();
            }
            return _UIControl;
        }
    }
    */

    // Wait a few frames and stay as much as possible out of Awake() and Start() to ensure seamless scene transition.
    void Start()
    {
        StartCoroutine(ShowSceneOne());
    }
    
    IEnumerator ShowSceneOne()
    {
        yield return new WaitForSeconds(1.0f);
        animator.SetTrigger("Fade In Text");
    }

    // Animation Event called at the end of the Fade In animation.
    // Only start listening when Fade In animation is done
    public void StartListeningToSceneSwitch()
    {
        UIController.GetInstance().OnSwitchToScene += OnSwitchScene;
    }

    void OnDisable()
    {
        UIController.GetInstance().OnSwitchToScene -= OnSwitchScene;
    }


    private void OnSwitchScene(int sceneToLoad)
    {
        nextScene = sceneToLoad;
        animator.SetTrigger("Fade Out Text");
    }
    //Animation Event called at the end of the Fade Out animation
    public void OnFadeComplete()
    {
        SceneManager.LoadScene(nextScene);
    }








}
