using UnityEngine;

public class TowerUpgrade : MonoBehaviour
{
    public static TowerUpgrade Singleton;
    private void Awake()
    {
        Singleton = this;
    }

    [SerializeField] GameObject sensorOverlay;
    [SerializeField] private Tower tower;


    public void UpdateSensorOverlay(TowerStatsSO towerStatsSO)
    {
        sensorOverlay.transform.localScale = Vector2.one * towerStatsSO.sensorRadius * 2;
    }

    public void ShowSensorOverlay(Tower _tower)
    {
        tower = _tower;
        UpdateSensorOverlay(_tower.towerStatsSO);
        transform.localPosition = _tower.transform.position;
        sensorOverlay.gameObject.SetActive(true);
    }

    public void ShowSensorOverlay(TowerItemUI towerItemUI)
    {
        TowerStatsSO towerStatsSO = towerItemUI.towerStatsSO;
        UpdateSensorOverlay(towerStatsSO);
        transform.localPosition = towerItemUI.towerPreviewPrefab.transform.position;
        sensorOverlay.gameObject.SetActive(true);

    }

    public void HideSensorOverlay()
    {
        sensorOverlay.gameObject.SetActive(false);
    }
}

