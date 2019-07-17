using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneThreeAnimation : MonoBehaviour
{
    public GameObject cubesParent;
    [SerializeField] private GameObject[] sceneThreeCubes;
    [SerializeField] private Animator animator;
    [SerializeField] private Button restartButton;
    private int nextScene;
    private GameObject transferredSphere;

    // Wait a few frames and stay as much as possible out of Awake() and Start() to ensure seamless scene transition.
    void Start()
    {
        StartCoroutine(ShowSceneThree());
    }

    IEnumerator ShowSceneThree()
    {
        yield return new WaitForSeconds(1.0f);
        animator.SetTrigger("Fade In Cubes");
        transferredSphere = GameObject.FindGameObjectWithTag("SpheresParent");
    }
    // Animation Event called at the beginning of the rotation animation.
    // Only start listening when cubes are rotating.
    public void StartListeningAfterCubesFadeIn()
    {
        UIController.GetInstance().OnSwitchToScene += OnSwitchScene;
        restartButton.onClick.AddListener(() => {
            OnRestartButtonClick();
        });
        //restartButton.onClick.RemoveAllListeners();
    }   

    public void OnSwitchScene(int sceneToLoad)
    {
        StartCoroutine(SwitchScene(sceneToLoad));
    }

    public void OnRestartButtonClick()
    {
        StartCoroutine(SwitchScene(0));
        UIController.GetInstance().SwopTimeline();
    }

    private IEnumerator SwitchScene(int sceneToLoad)
    {
      
        nextScene = sceneToLoad;
        // Stop animation.
        animator.enabled = false;
        yield return null;
        for (int i = 0; i < sceneThreeCubes.Length; i++)
        {
           StartCoroutine(FadeOutGameobject(sceneThreeCubes[i], 3.0f));
        }
        StartCoroutine(FadeOutGameobject(transferredSphere, 3.0f));

        yield return new WaitForSeconds(4.0f);
       
        
        Destroy(transferredSphere);
       
        OnFadeComplete();

    }

    private void OnFadeComplete()
    {
        UIController.GetInstance().OnSwitchToScene -= OnSwitchScene;
        SceneManager.LoadScene(nextScene);
    }

    private IEnumerator FadeOutGameobject(GameObject cube, float time)
    {
        Vector3 originalScale = cube.transform.localScale;
        Vector3 destinationScale = new Vector3(0, 0, 0);

        float timer = 0f;

        while (timer < time)
        {
            timer += Time.deltaTime;
            cube.transform.localScale = Vector3.Lerp(originalScale, destinationScale, timer / time);
            yield return null;
        }
        cube.SetActive(false);
    }
}
