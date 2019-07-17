using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneOneAnimation : MonoBehaviour
{
    public Animator animator;
    private int nextScene;
  
    // Wait a few frames and stay as much as possible out of Awake() and Start() to ensure seamless scene transition.
    void Start()
    {
        StartCoroutine(ShowSceneOne());
    }
    
    IEnumerator ShowSceneOne()
    {
        yield return new WaitForSeconds(1.0f);
        animator.SetTrigger("Fade In Text");
        yield return null;
        while (Utilities.isPlaying(animator, "Fade In Text"))
        {
            yield return null;
        }
        UIController.GetInstance().animationHasFinished = true;

    }

    void OnEnable()
    {
        StartCoroutine(LateEnable());
    }

    IEnumerator LateEnable()
    {
        yield return null;
        UIController.GetInstance().OnSwitchToScene += OnSwitchScene;
    }

    void OnDisable()
    {
        UIController.GetInstance().OnSwitchToScene -= OnSwitchScene;
    }

    void OnSwitchScene(int sceneToLoad)
    {
        StartCoroutine(SwitchScenes(sceneToLoad));
    }

    IEnumerator SwitchScenes(int sceneToLoad)
    {
        nextScene = sceneToLoad;
        animator.SetTrigger("Fade Out Text");
        yield return null;
        while (Utilities.isPlaying(animator, "Fade Out Text"))
        {
            yield return null;
        }
        SceneManager.LoadScene(nextScene);
    }










    

}
