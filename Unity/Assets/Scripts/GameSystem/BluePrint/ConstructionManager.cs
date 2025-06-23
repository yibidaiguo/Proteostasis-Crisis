using System;
using JKFrame;
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
    
    [SerializeField]
    private BluePrintConfig _bluePrintConfig;
    public BluePrintConfig BluePrintConfig => _bluePrintConfig;
    public Action OnStartBuilding{get;private set;} = null;
    public Action OnBuilding{get;private set;} = null;
    public Action OnFinishBuilding{get;private set;} = null;
    [SerializeField]private PlayerConstructionData _playerConstructionData;
    private BluePrint bluePrint;

    public ConstructionData curData
    {
        get { return _curData; }
        set
        {
            _curData = value;
            if (_curData != null)
                ChangeState(BuildState.Start);
            else
                ChangeState(BuildState.Idle);
        }
    }
    [SerializeField]private ConstructionData _curData;

    void Start()
    {
        bluePrint = GetComponentInChildren<BluePrint>();
        bluePrint.Init();
        //TODO:暂时先在这里生成蓝图，实际上这个应该属于GM的职责
        bluePrint.GenerateBaseBluePrint();
        InitPlayerConstructionData();
    }

    /// <summary>
    /// 建造
    /// </summary>
    /// <param name="construction">建造的建筑</param>
    /// <param name="position">建造的位置</param>
    public void Constructed(Vector3 position,ConstructionData data)
    {
        if(curData == null) return;
        ChangeState(BuildState.Constructeding);
        //TODO:这里只做简单的判断，实际上应该有更复杂的判断，比如建筑是否可以建造，是否有足够的资源等等
        if (bluePrint.Constructed(position, data))
            ChangeState(BuildState.Finished);
        else
            ChangeState(BuildState.Start);
    }

    public void DestoryConstruction(Vector3 position)
    {
        bluePrint.RemoveConstructionData(position);
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
                Debug.Log("开始建造");
                break;
            case BuildState.Constructeding:
                OnBuilding?.Invoke();
                Debug.Log("正在建造");
                break;
            case BuildState.Finished:
                OnFinishBuilding?.Invoke();
                Debug.Log("建造完成");
                curData = null;
                break;
        }
    }

    /// <summary>
    /// 初始化玩家的建造数据
    /// </summary>
    public void InitPlayerConstructionData()
    {
        if (_playerConstructionData == null) return;
        foreach (var constructionData in _playerConstructionData.constructionDictionary)
        {
            Constructed(constructionData.Key, constructionData.Value);
        }
    }
    
    /// <summary>
    /// 注册建造状态的回调
    /// </summary>
    /// <param name="state"></param>
    /// <param name="action"></param>
    public void RegiesterBuildStateAction(BuildState state,Action action)
    {
        switch (state)
        {
            case BuildState.Start:
                OnStartBuilding += action;                
                break;
            case BuildState.Constructeding:
                OnBuilding += action;
                break;
            case BuildState.Finished:
                OnFinishBuilding += action;
                break;
        }
    }
}
