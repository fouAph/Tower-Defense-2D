using UnityEngine;

[CreateAssetMenu(fileName = "LevelSelect", menuName = "Tower Defense/LevelSelectItem", order = 0)]
public class LevelSelectItemData : ScriptableObject
{
    public int levelBuildIndex;
    public Sprite levelPreviewSprite;

}
