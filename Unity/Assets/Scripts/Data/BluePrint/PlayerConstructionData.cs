using System.Collections.Generic;
using UnityEngine;

//玩家建造数据
public class PlayerConstructionData : MonoBehaviour
{
    //key：基于蓝图配置的初始化位置的坐标，value：该位置的建筑数据
    public Dictionary<Vector3Int, ConstructionData> constructionDictionary = new (); //建造字典
    
}
