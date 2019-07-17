using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ClickableSphere : MonoBehaviour, IPointerClickHandler
{
    #region IPointerClickHandler implementation
    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(SceneTwoAnimation.GetInstance().SwitchScenes(this.gameObject,2));
    }
    #endregion
}
