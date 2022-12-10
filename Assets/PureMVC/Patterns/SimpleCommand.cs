using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCommand : Notifier, ICommand, INotifier
{
    //Execute 执行
    public virtual void Execute(INotification notification)
    {
        
    }
}
