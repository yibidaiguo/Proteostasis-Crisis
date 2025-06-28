using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothNetData : ConstructionData
{
    //光面内质网正在生产的资源状态
    public ResourceCommand Command = new ResourceCommand();
    
    //生产命令列表
    public List<ResourceCommand> CommandList = new List<ResourceCommand>();
    public SmoothNetData(string constructionName, BluePrintData bluePrintData, int rotation) 
        : base(constructionName, bluePrintData, rotation) 
    {
        
    }
}
