using UnityEngine;

[RequireComponent(typeof(StateMachine))]
public class InputComp : MonoBehaviour
{
    StateMachine _stateMachine;

    void Awake()
    {
        _stateMachine = GetComponent<StateMachine>();
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
