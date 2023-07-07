using UnityEngine;
using UnityEngine.EventSystems;

public class TowerItemUI : MonoBehaviour, IPointerClickHandler
{
    public GameObject towerPrefab;
    [SerializeField] GameObject itemHighlightImage;

    public void OnPointerClick(PointerEventData eventData)
    {

        Selected(this);
    }

    public void Selected(TowerItemUI towerItemUI)
    {
        GameObject currentTower = GameManager.Singleton.GetSelectedTower();
        currentTower = towerPrefab ? towerPrefab : null;
        GameManager.Singleton.SetSelectedTower(towerPrefab);
        UIManager.Singleton.SetSelectedTowerItem(towerItemUI);
        itemHighlightImage.SetActive(true);

    }

    public void Deselected()
    {
        itemHighlightImage.SetActive(false);
    }
}
