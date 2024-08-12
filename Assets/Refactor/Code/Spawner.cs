using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] StageData _stageData;
    /// <summary>
    /// Initialize two fighters on the Scene
    /// </summary>
    void Start()
    {
        Instantiate(Resources.Load<GameObject>("Fighter")).TryGetComponent<FighterComponent>(out var figther);
        figther.Constructor(_stageData.BluePlayer);
        Instantiate(Resources.Load<GameObject>("Fighter")).TryGetComponent<FighterComponent>(out figther);
        figther.Constructor(_stageData.OrangePlayer);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _stageData.BlueCurrentHealth -= 10;
    }
}