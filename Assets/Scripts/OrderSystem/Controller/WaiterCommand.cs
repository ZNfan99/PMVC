using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaiterCommand : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        WaiterProxy waiterProxy = Facaded.RetrieveProxy(WaiterProxy.NAME) as WaiterProxy;
        
        if (notification.Type == "SERVING")
        {
            Order order = notification.Body as Order;
            Debug.Log("寻找服务员上菜");
            
            waiterProxy.Serving(order);
        }
        //else if (notification.Type == "Rester")
        //{
        //    WaiterItem item = notification.Body as WaiterItem;
        //    waiterProxy.ChangeWaiterState(item);
        //}
        else if(notification.Type == "ServerFood")
        {
            waiterProxy.BecomeIdle(notification.Body as WaiterItem);
        }
    }
}
