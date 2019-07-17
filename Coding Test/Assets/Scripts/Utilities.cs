using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{

    public static bool isPlaying(Animator anim, string stateName)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return true;
        else
            return false;
    }

    public static List<GameObject> GetChildren(this GameObject go)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform tran in go.transform)
        {
            children.Add(tran.gameObject);
        }
        return children;
    }

    public static IEnumerator MoveSphere(GameObject sphere, Vector3 targetPos, float time)
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

    public static IEnumerator FadeOutSphere(GameObject sphere, float time)
    {
        Vector3 originalScale = sphere.transform.localScale;
        Vector3 destinationScale = new Vector3(0, 0, 0);

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
