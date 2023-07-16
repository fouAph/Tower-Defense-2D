using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerItemUI : MonoBehaviour, IPointerClickHandler
{
    public Tower towerPrefab;
    public GameObject towerPreviewPrefab;
    public GameObject lockObject;
    public Image towerImage;
    public TowerStatsSO towerStatsSO;
    [SerializeField] GameObject itemHighlightImage;

    private void Awake()
    {
        InitiateTowerPreview();
        towerImage = GetComponent<Image>();
    }

    private void Start()
    {
        HideTowerPreview();
        AddTowerPrefabToPool();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.Singleton.CheckIfEnoughCoinForPrice(towerStatsSO.towerPrice))
            Selected(this);
    }

    public void Selected(TowerItemUI towerItemUI)
    {
        Tower currentTower = GameManager.Singleton.GetSelectedTowerGO();
        currentTower = towerPrefab ? towerPrefab : null;
        GameManager.Singleton.SetSelectedTower(towerPrefab);
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

    public void ShowLockObject()
    {
        lockObject.SetActive(true);
    }

    public void HideLockObject()
    {
        lockObject.SetActive(false);
    }

    public void Deselected()
    {
        itemHighlightImage.SetActive(false);
        HideTowerPreview();
    }
}
