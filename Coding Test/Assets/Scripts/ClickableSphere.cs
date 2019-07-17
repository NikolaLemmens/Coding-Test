using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ClickableSphere : MonoBehaviour, IPointerClickHandler
{
    
    #region IPointerClickHandler implementation
    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(this.gameObject.GetComponentInParent<SceneTwoAnimation>().SwitchScenes(this.gameObject,2));
    }
    #endregion
}
