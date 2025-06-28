using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothNetData : ConstructionData
{
    //光面内质网正在生产的资源状态
    public Data<ResourceCommand> Command = new Data<ResourceCommand>();
    
    //线粒体正在生产的资源状态
    public Data<ResourceCommand> resourceCommand = new Data<ResourceCommand>();
    
    //生产命令列表
    public MyList<ResourceCommand> CommandList = new MyList<ResourceCommand>();
    public SmoothNetData(string constructionName, BluePrintData bluePrintData, int rotation) 
        : base(constructionName, bluePrintData, rotation) 
    {
        
    }
}
