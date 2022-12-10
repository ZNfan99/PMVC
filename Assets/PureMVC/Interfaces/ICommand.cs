using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 命令
/// </summary>
public interface ICommand
{
    //执行通知事件
    void Execute(INotification notification);
}