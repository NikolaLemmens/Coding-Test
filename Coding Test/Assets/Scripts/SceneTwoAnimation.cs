using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SceneTwoAnimation : MonoBehaviour
{
    public GameObject spheresParent;
    [SerializeField] private GameObject[] sceneTwoSpheres;
    [SerializeField] private Vector3 centerPosition;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject camera;

    private int nextScene;

    // Wait a few frames and stay as much as possible out of Awake() and Start() to ensure seamless scene transition.
    void Start()
    {
        StartCoroutine(ShowSceneTwo());
    }

    IEnumerator ShowSceneTwo()
    {
        yield return new WaitForSeconds(1.0f);
        animator.SetTrigger("Fade In Spheres");  
    }
    // Animation Event called at the beginning of the rotation animation.
    // Only start listening when spheres are rotating.
    public void StartListeningAfterSpheresFadeIn()
    {
        UIController.GetInstance().OnSwitchToScene += OnSwitchScene;
        camera.GetComponent<PhysicsRaycaster>().enabled = true;
    }
    
    public void OnSwitchScene(int sceneToLoad)
    {
        StartCoroutine(SwitchScene(sceneTwoSpheres[0], sceneToLoad));
        camera.GetComponent<PhysicsRaycaster>().enabled = false;
    }

    public IEnumerator SwitchScene(GameObject centerSphere, int sceneToLoad)
    {
        nextScene = sceneToLoad;
        // Stop animation.
        animator.enabled = false;
        yield return null;
        for (int i = 0; i < sceneTwoSpheres.Length; i++)
        {
            if(sceneTwoSpheres[i] == centerSphere)
            {
                StartCoroutine(MoveSphere(sceneTwoSpheres[i], centerPosition, 3.0f));
            }
            else
            {
                StartCoroutine(FadeOutSphere(sceneTwoSpheres[i], 0.5f));
            }
        }
        yield return new WaitForSeconds(4.0f);
        DontDestroyOnLoad(spheresParent);
       
        OnFadeComplete();
        
    }

    private void OnFadeComplete()
    {
        StartCoroutine(UIController.GetInstance().ChangeTimelineUI(1, 2));
        UIController.GetInstance().OnSwitchToScene -= OnSwitchScene;
        SceneManager.LoadScene(nextScene);
        StartCoroutine(DisableScript());
    }

    IEnumerator DisableScript()
    {
        while (SceneManager.GetActiveScene().buildIndex == 1)
        {
            yield return null;
        }
        this.enabled = false;
    }

    private IEnumerator MoveSphere(GameObject sphere, Vector3 targetPos, float time)
    {
        Vector3 startPos = sphere.transform.position;

        float timer = 0f;

        while (timer < time)
        {
            timer += Time.deltaTime;
            sphere.transform.position = Vector3.Lerp(startPos, targetPos, timer / time);
            yield return null;
        }
        sphere.transform.position = targetPos;
    }

    private IEnumerator FadeOutSphere(GameObject sphere, float time)
    {
        Vector3 originalScale = sphere.transform.localScale;
        Vector3 destinationScale = new Vector3(0,0,0);

        float timer = 0f;

        while (timer < time)
        {
            timer += Time.deltaTime;
            sphere.transform.localScale = Vector3.Lerp(originalScale, destinationScale, timer / time);
            yield return null;
        }
        sphere.SetActive(false);
    }

  
}
