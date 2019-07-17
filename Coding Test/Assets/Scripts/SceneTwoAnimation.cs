using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class SceneTwoAnimation : MonoBehaviour
{
    public GameObject spheresParent;
    [SerializeField] private Animator animator;
    [SerializeField] private Vector3 centerPosition;
    [SerializeField] private GameObject mainCamera;
    private List<GameObject> sceneTwoSpheres;
    private int nextScene;

    private static SceneTwoAnimation instance;
    public static SceneTwoAnimation GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    // Wait a few frames and stay as much as possible out of Awake() and Start() to ensure seamless scene transition.
    void Start()
    {
        sceneTwoSpheres = Utilities.GetChildren(spheresParent);
        StartCoroutine(ShowSceneTwo());
    }

    IEnumerator ShowSceneTwo()
    {
        yield return new WaitForSeconds(1.0f);
        animator.SetTrigger("Fade In Spheres");
        yield return null;
        while (Utilities.isPlaying(animator, "Fade In Spheres"))
        {
            yield return null;
        }
        // Only start listening when spheres are rotating.
        mainCamera.GetComponent<PhysicsRaycaster>().enabled = true;
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
        StartCoroutine(SwitchScenes(sceneTwoSpheres[0], sceneToLoad));
    }

    public IEnumerator SwitchScenes(GameObject centerSphere, int sceneToLoad)
    {
        mainCamera.GetComponent<PhysicsRaycaster>().enabled = false;
        nextScene = sceneToLoad;
        // Stop animation.
        animator.enabled = false;
        yield return null;
        for (int i = 0; i < sceneTwoSpheres.Count; i++)
        {
            if(sceneTwoSpheres[i] == centerSphere)
            {
                StartCoroutine(Utilities.MoveSphere(sceneTwoSpheres[i], centerPosition, 3.0f));
            }
            else
            {
                StartCoroutine(Utilities.FadeOutGameobject(sceneTwoSpheres[i], 0.5f));
            }
        }
        yield return new WaitForSeconds(4.0f);
        DontDestroyOnLoad(spheresParent);
        // If coroutine is called by clicking on a Sphere, make sure Timeline swops too.
        UIController.GetInstance().SwopTimeline();
        yield return null;
        SceneManager.LoadScene(nextScene);
    }
}
