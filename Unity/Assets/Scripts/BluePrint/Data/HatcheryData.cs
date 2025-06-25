using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 细胞器孵化所
/// </summary>
public class HatcheryData : ConstructionData 
{
    //正在生产的命令进度
    public ResourceCommand Command = new ResourceCommand();
    
    //生产命令列表
    public List<ResourceCommand> CommandList = new List<ResourceCommand>();
    public HatcheryData(string constructionName, BluePrintData bluePrintData, int rotation) 
        : base(constructionName, bluePrintData, rotation) 
    {
        
    }

}
