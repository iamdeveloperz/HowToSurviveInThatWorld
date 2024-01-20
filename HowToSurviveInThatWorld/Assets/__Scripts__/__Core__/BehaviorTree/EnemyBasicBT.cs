using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBasicBT : MonoBehaviour
{
    #region Global Variable
    
    private DataContext _enemyData;
    private BehaviorTreeRunner _btRunner;

    #endregion
    
    

    #region Unity Event Method

    private void Awake()
    {
        InitializeData();
    }

    private void Update()
    {
        throw new NotImplementedException();
    }

    #endregion
    
    

    # region Setting BT
    
    private INode SettingBT()
    {
        return new SelectorNode
        (
            new List<INode>()
            {
                new Inverter
                (
                    new SelectorNode    // ## 적 감지 & 순찰 분기 노드
                    (
                        new List<INode>()
                        {
                            new ActionNode(Test1), // 범위 안에 적이 있는가?
                            new SelectorNode    // ## 순찰 판정 분기 노드
                            (
                                new List<INode>()
                                {
                                    new UntilFail
                                    (
                                        new ActionNode(Test1) // 올바른 목적지가 있는가?
                                    ),
                                    new SequenceNode
                                    (
                                        new List<INode>()
                                        {
                                            new ActionNode(Test1), // 목적지에 도착헸는가?
                                            new ActionNode(Test2) // 목적지에 도착했는가?
                                        }
                                    ),
                                    new ActionNode(Test1) // 이동
                                }
                            )
                        }
                    )
                ),
                new SequenceNode    // ## 공격실행 판정 분기 노드
                (
                    new List<INode>()
                    {
                        new ActionNode(Test1),
                        new ActionNode(Test2),
                        new ActionNode(Test3)
                    }
                ),
                new ActionNode(Test1) // 추적
            }
        );
    }
    
    #endregion
    
    

    # region Action(Leaf) Nodes
    private INode.E_NodeState Test1()
    {
        return INode.E_NodeState.ENS_Running;
    }
    
    private INode.E_NodeState Test2()
    {
        return INode.E_NodeState.ENS_Running;
    }
    
    private INode.E_NodeState Test3()
    {
        return INode.E_NodeState.ENS_Running;
    }
    #endregion



    #region Action Logics

    private void TemporaryMethod()
    {
        
    }

    #endregion
    
    
    
    #region Initializer

    private void InitializeData()
    {
        _enemyData = DataContext.CreatDataContext(this.gameObject);
        _btRunner = new BehaviorTreeRunner(SettingBT());
    }

    #endregion
}