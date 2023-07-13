using UnityEngine;
using UnityEngine.EventSystems;

public class TowerItemUI : MonoBehaviour, IPointerClickHandler
{
    public Tower towerPrefab;
    public GameObject towerPreviewPrefab;
    public TowerStatsSO towerStatsSO;
    [SerializeField] GameObject itemHighlightImage;

    private void Awake()
    {
        InitiateTowerPreview();
    }

    private void Start()
    {
        HideTowerPreview();
        AddTowerPrefabToPool();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Selected(this);
    }

    public void Selected(TowerItemUI towerItemUI)
    {
        GameObject currentTower = GameManager.Singleton.GetSelectedTowerGO();
        currentTower = towerPrefab.gameObject ? towerPrefab.gameObject : null;
        GameManager.Singleton.SetSelectedTower(towerPrefab.gameObject);
        UIManager.Singleton.SetSelectedTowerItem(towerItemUI);
        itemHighlightImage.SetActive(true);
        ShowTowerPreview();
    }

    public void InitiateTowerPreview()
    {
        towerPreviewPrefab = towerPreviewPrefab ? Instantiate(towerPreviewPrefab) : Instantiate(towerPrefab.gameObject);
    }

    private void AddTowerPrefabToPool()
    {
        PoolSystem.Singleton.AddObjectToPooledObject(towerPrefab.gameObject, 10);
    }

    public void ShowTowerPreview()
    {
        towerPreviewPrefab.SetActive(true);
    }

    public void HideTowerPreview()
    {
        towerPreviewPrefab.SetActive(false);
    }

    public void SetTowerPreviewPosition(Vector3 newPos)
    {
        towerPreviewPrefab.transform.position = newPos;
    }

    public void Deselected()
    {
        itemHighlightImage.SetActive(false);
        HideTowerPreview();
    }
}
