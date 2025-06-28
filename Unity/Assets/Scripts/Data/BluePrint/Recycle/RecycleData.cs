using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 溶酶体
/// </summary>
public class RecycleData : ConstructionData
{
    //溶酶体正在生产的资源状态
    public Data<ResourceCommand> Command = new Data<ResourceCommand>();
    
    //线粒体正在生产的资源状态
    public Data<ResourceCommand> resourceCommand = new Data<ResourceCommand>();
    
    //生产命令列表
    public MyList<ResourceCommand> CommandList = new MyList<ResourceCommand>();

    public RecycleData(string constructionName, BluePrintData bluePrintData, int rotation) 
        : base(constructionName, bluePrintData, rotation) 
    {
        
    }

}
