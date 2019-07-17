using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneThreeAnimation : MonoBehaviour
{
    public GameObject cubesParent;
    [SerializeField] private Animator animator;
    [SerializeField] private Button restartButton;
    private List<GameObject> sceneThreeCubes;
    private int nextScene;
    private GameObject transferredSphere;

    // Wait a few frames and stay as much as possible out of Awake() and Start() to ensure seamless scene transition.
    void Start()
    {
        sceneThreeCubes = Utilities.GetChildren(cubesParent);
        StartCoroutine(ShowSceneThree());
    }

    IEnumerator ShowSceneThree()
    {
        transferredSphere = GameObject.FindGameObjectWithTag("SpheresParent");
        yield return new WaitForSeconds(1.0f);
        animator.SetTrigger("Fade In Cubes");
        yield return null;
        while (Utilities.isPlaying(animator, "Fade In Cubes"))
        {
            yield return null;
        }
        // Only start listening when cubes are rotating.
        restartButton.onClick.AddListener(() => {
            OnRestartButtonClick();
        });
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

    public void OnSwitchScene(int sceneToLoad)
    {
        StartCoroutine(SwitchScenes(sceneToLoad));
    }

    public void OnRestartButtonClick()
    {
        StartCoroutine(SwitchScenes(0));
        UIController.GetInstance().SwopTimeline();
    }

    private IEnumerator SwitchScenes(int sceneToLoad)
    {
      
        nextScene = sceneToLoad;
        // Stop animation.
        animator.enabled = false;
        yield return null;
        for (int i = 0; i < sceneThreeCubes.Count; i++)
        {
           StartCoroutine(Utilities.FadeOutGameobject(sceneThreeCubes[i], 3.0f));
        }
        StartCoroutine(Utilities.FadeOutGameobject(transferredSphere, 3.0f));

        yield return new WaitForSeconds(4.0f);
      
        Destroy(transferredSphere);

        SceneManager.LoadScene(nextScene);
    }
    
}
