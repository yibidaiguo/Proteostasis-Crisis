using UnityEngine;


public class PlayerData
{
    //玩家建造数据
    //key：基于蓝图配置的初始化位置的坐标，value：该位置的建筑数据
    public Data<MyDictionary<Vector3Int, ConstructionData>> constructionDictionary = new ();
}
