using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fighter Data", menuName = "Data/Fighter")]
public class FighterData : ScriptableObject
{
    public Fighter Name;
    public AnimatorController animatorController;
}