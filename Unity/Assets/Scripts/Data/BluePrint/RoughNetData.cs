using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 粗面内质网
/// </summary>
public class RoughNetData : ConstructionData
{
    //粗面内质网正在生产的资源状态
    public ResourceCommand Command = new ResourceCommand();
    
    //生产命令列表
    public List<ResourceCommand> CommandList = new List<ResourceCommand>();
    public RoughNetData(string constructionName, BluePrintData bluePrintData, int rotation) 
        : base(constructionName, bluePrintData, rotation) 
    {
        
    }
}
