using UnityEngine;

[RequireComponent(typeof(StateMachineComp))]
public class InputComp : MonoBehaviour
{
    StateMachineComp _stateMachine;

    void Awake()
    {
        _stateMachine = GetComponent<StateMachineComp>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            _stateMachine.ChangeState(StateType.Idle);
        if (Input.GetKeyDown(KeyCode.S))
            _stateMachine.ChangeState(StateType.Movement);
        if (Input.GetKeyDown(KeyCode.D))
            _stateMachine.ChangeState(StateType.Jump);
    }

}
