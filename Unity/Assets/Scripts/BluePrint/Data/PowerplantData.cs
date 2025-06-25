using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 线粒体
/// </summary>
public class PowerplantData : ConstructionData
{
    //线粒体正在生产的资源状态
    public ResourceCommand resourceCommand = new ResourceCommand();
    //线粒体是否在工作
    public bool isWorking = true;
    public PowerplantData(string constructionName, BluePrintData bluePrintData, int rotation) 
        : base(constructionName, bluePrintData, rotation) 
    {
        
    }

}
