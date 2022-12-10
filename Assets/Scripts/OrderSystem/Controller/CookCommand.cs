using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;

public class CookCommand : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        CookProxy cookProxy = Facaded.RetrieveProxy(CookProxy.NAME) as CookProxy; //厨师的代理
        

        if (notification.Type.Equals("Busy"))
        {
            Order order = notification.Body as Order;
            cookProxy.CookCooking(order);
        }
        if(notification.Type.Equals("Rester"))
        {
            CookItem cookItem = notification.Body as CookItem;
            cookProxy.ChangeCookState(cookItem);
        }
    }
}
