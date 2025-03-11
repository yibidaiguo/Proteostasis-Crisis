using System;
using JKFrame;
using Sirenix.OdinInspector;
using UnityEngine;

public class ConstructionManager : SingletonMono<ConstructionManager>
{
    public enum BuildState
    {
        None,
        Idle,
        Start,
        Constructeding,
        Finished
    }
    
    public BuildState CurrentState { get; private set; } = BuildState.Idle;
    [ShowInInspector]
     public BluePrintConfig BluePrintConfig{get;private set;}
     
    public Action OnStartBuilding{get;private set;} = null;
    public Action OnBuilding{get;private set;} = null;
    public Action OnFinishBuilding{get;private set;} = null;
    private BluePrintConstruction _bluePrintConstruction;

    void Start()
    {
        _bluePrintConstruction = GetComponentInChildren<BluePrintConstruction>();
        _bluePrintConstruction.Init();
        //TODO:暂时先在这里生成蓝图，实际上这个应该属于GM的职责
        _bluePrintConstruction.GenerateBaseBluePrint();
    }

    /// <summary>
    /// 建造
    /// </summary>
    /// <param name="construction">建造的建筑</param>
    /// <param name="position">建造的位置</param>
    public void Constructed(ConstructedTileBase construction,Vector3 position)
    {
        ChangeState(BuildState.Start);
        //TODO:这里只做简单的判断，实际上应该有更复杂的判断，比如建筑是否可以建造，是否有足够的资源等等
        if(!_bluePrintConstruction.CheckCanConstructed(position,construction)) return;
    }
    
    /// <summary>
    /// 切换建造状态
    /// </summary>
    /// <param name="state"></param>
    private void ChangeState(BuildState state)
    {
        CurrentState = state;
        switch (state)
        {
            case BuildState.Start:
                OnStartBuilding?.Invoke();                
                break;
            case BuildState.Constructeding:
                OnBuilding?.Invoke();
                break;
            case BuildState.Finished:
                OnFinishBuilding?.Invoke();
                break;
        }
    }
    
}
