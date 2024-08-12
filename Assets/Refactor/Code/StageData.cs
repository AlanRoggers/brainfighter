using UnityEngine;

[CreateAssetMenu(fileName = "Stage Data", menuName = "Data/Stage")]
public class StageData : ScriptableObject
{
    public Fighter BluePlayer;
    public Fighter OrangePlayer;
    public Vector2 BluePosition;
    public Vector2 OrangePosition;
    public int BlueCurrentHealth;
    public int OrangeCurrentHealth;

    void OnEnable()
    {
        BlueCurrentHealth = 100;
        OrangeCurrentHealth = 100;
    }

}