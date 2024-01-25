using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : MonoBehaviour
{
    #region Field

    // Player가 가지고 있는 모든 상태, 현재 상태
    private Dictionary<E_Player_States, State<PlayerFSM>> _states;
    private StateMachine<PlayerFSM> _stateMachine;
    
    #endregion

    #region Properties
    
    public E_Player_States CurrentState // 현재 상태
    {
        get;
        private set;
    }

    #endregion
    

    public void Awake()
    {
        // Player가 가질 수 없는 상태 개수만큼 메모리 할당, 각 상태에 클래스 메모리 할당
        _states = new Dictionary<E_Player_States, State<PlayerFSM>>
        {
            {E_Player_States.NormallyState, new NormallyState()},
            {E_Player_States.GlobalState, new GlobalState()}
        };
        
        // 상태를 관리하는 StateMachine에 메모리를 할당하고, 첫 상태를 설정
        _stateMachine = new StateMachine<PlayerFSM>();
        _stateMachine.Setup(this, _states[E_Player_States.NormallyState]);
        
        // 전역 상태 설정
        _stateMachine.SetGlobalState(_states[E_Player_States.GlobalState]);
        
        // 위에처럼 나머지 필드 데이터 셋업
        /*

        */
    }

    public void Update()
    {
        _stateMachine.ExecuteUpdate();
    }

    public void FixedUpdate()
    {
        _stateMachine.ExecuteFixedUpdate();
    }

    public void ChangeState(E_Player_States newState)
    {
        CurrentState = newState; // 상태가 바뀌면 새로운 상태가 현재상태로
        
        _stateMachine.ChangeState(_states[newState]);
    }
    
    public void RevertToPreviousState()
    {
        _stateMachine.RevertToPreviousState();
    }
}