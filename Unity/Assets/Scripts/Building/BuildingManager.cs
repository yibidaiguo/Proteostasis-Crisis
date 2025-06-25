using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TODO
/// </summary>

public class BuildingManager : MonoBehaviour
{
    //单例模式
    public static BuildingManager Instance { get; private set; }


    ///
    /// 记录所有游戏逻辑和表现要用到的字段
    ///

    public string buildingName;             //建筑名
    
    public string buildingDescription;      //建筑描述文本

    ///
    /// 建筑预制体
    ///

    [Tooltip("存储所有建筑预制体,需手动添加，只用来创建字典")] 
    public List <GameObject> buildingPrefList = new List<GameObject>();    
    //存储所有建筑预制体,key为建筑名，value为建筑预制体    
    public Dictionary<string, GameObject> buildingPrefDict = new Dictionary<string, GameObject>();   

    ///
    ///
    ///




}
