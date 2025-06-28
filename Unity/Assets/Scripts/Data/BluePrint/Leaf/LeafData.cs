using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 叶绿体
/// </summary>
public class LeafData : ConstructionData
{
    //叶绿体正在生产的资源的进度
    public Data<ResourceCommand> resourceCommand = new Data<ResourceCommand>();
    
    //正在生产的那个命令的单元的进度
    public Data<float> CommandProgress = new Data<float>(); 

    //叶绿体是否正在工作
    public Data<bool> isWorking = new Data<bool>();

    public LeafData(string constructionName, BluePrintData bluePrintData, int rotation) 
        : base(constructionName, bluePrintData, rotation) 
    {
        
    }

}
