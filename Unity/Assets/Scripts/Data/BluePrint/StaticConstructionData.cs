using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 细胞器的静态构造数据,
/// </summary>

public class StaticConstructionData
{
    public Data<string> buildingName = new Data<string>();             //建筑名称

    public Data<string> buildingDescription = new Data<string>();      //建筑描述文本

    ///
    /// 细胞器可以下达的生产命令
    /// TODO: 生产命令需要在初始化时添加
    /// 
    public MyList<ResourceCommand> resourceCommands = new MyList<ResourceCommand>();

    //增加生产命令
    public AddResourceCommand(ResourceCommand command)
    {
        resourceCommands.Add(command);
    }

}
