using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 溶酶体
/// </summary>
public class RecycleData : ConstructionData
{
    //溶酶体正在生产的资源状态
    public ResourceCommand Command = new ResourceCommand();
    
    //生产命令列表
    public List<ResourceCommand> CommandList = new List<ResourceCommand>();

    public RecycleData(string constructionName, BluePrintData bluePrintData, int rotation) 
        : base(constructionName, bluePrintData, rotation) 
    {
        
    }

}
