using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New State List", menuName = "Variables/StateList")]
public class StateList : ScriptableObject
{
    public List<StateType> States;
}