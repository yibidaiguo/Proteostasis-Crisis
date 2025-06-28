using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 叶绿体
/// </summary>
public class LeafData : ConstructionData
{
    //叶绿体正在生产的资源的进度
    public ResourceCommand resourceCommand = new ResourceCommand();
    //叶绿体是否正在工作
    public bool isWorking = true;

    public LeafData(string constructionName, BluePrintData bluePrintData, int rotation) 
        : base(constructionName, bluePrintData, rotation) 
    {
        
    }

}
