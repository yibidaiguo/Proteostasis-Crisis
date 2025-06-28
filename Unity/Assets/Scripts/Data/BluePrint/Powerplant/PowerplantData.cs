using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 线粒体
/// </summary>
public class PowerplantData : ConstructionData
{
    //线粒体正在生产的资源状态
    public Data<ResourceCommand> resourceCommand = new Data<ResourceCommand>();

    //正在生产的那个命令的单元的进度
    public Data<float> CommandProgress = new Data<float>(); 

    //线粒体是否在工作
    public Data<bool> isWorking = new Data<bool>();
    public PowerplantData(string constructionName, BluePrintData bluePrintData, int rotation) 
        : base(constructionName, bluePrintData, rotation) 
    {
        
    }

}
