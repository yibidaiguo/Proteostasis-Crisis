using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolgiData : ConstructionData 
{
    //正在生产的命令进度
    public Data<ResourceCommand> Command = new Data<ResourceCommand>();

    //正在生产的那个命令的单元的进度
    public Data<float> CommandProgress = new Data<float>(); 
    
    //生产命令列表
    public MyList<ResourceCommand> CommandList = new MyList<ResourceCommand>();


    public GolgiData(string constructionName, BluePrintData bluePrintData, int rotation) 
        : base(constructionName, bluePrintData, rotation) 
    {
        
    }
}
