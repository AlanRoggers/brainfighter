using UnityEngine;

public class FighterComponent : MonoBehaviour
{
    FighterData _fighterData;
    Animator _animator;
    bool _constructor;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Initialize the characteristics of Fighter, If it already have initialized,
    /// the method dont do anything
    /// </summary>
    public void Constructor(Fighter fighter)
    {
        if (_constructor) return;

        _constructor = true;

        Debug.Log($"Fighter Constructor with {fighter}");

        _fighterData = Resources.Load<FighterData>($"{fighter}");

        if (_fighterData == null)
        {
            Debug.LogWarning($"Data of {fighter} doesnt exist");
            return;
        }

        if (_animator == null)
        {
            Debug.LogWarning("Animator Component doesnt exist in the GameObject");
            return;
        }

        _animator.runtimeAnimatorController = _fighterData.animatorController;
    }
}
