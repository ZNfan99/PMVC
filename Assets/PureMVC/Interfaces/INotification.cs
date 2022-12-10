using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//接口可以包含属性、方法、索引指示器和事件
/// <summary>
/// 通知  
/// </summary>
public interface INotification
{
    //重写通知用于调试输出(方法)
    string ToString();

    //通知事件(属性)
    object Body { get; set; }

    //通知名称(只读)
    string Name { get; }

    //通知类型
    string Type { get; set; }
}
