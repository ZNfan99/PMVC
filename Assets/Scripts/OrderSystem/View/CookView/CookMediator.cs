using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookMediator : Mediator
{
    private CookProxy cookProxy = null;
    public new const string NAME = "CookMediator";

    public CookView CookView
    {
        get
        {
            return (CookView)base.ViewComponent;
        }
    }

    public CookMediator(CookView view) : base(NAME,view)
    {
        CookView.CallCook += () => { SendNotification(OrderSystemEvent.CALL_COOK); };
        CookView.ServerFood += item => { SendNotification(OrderSystemEvent.SERVER_FOOD, item); };
    }

    public override void OnRegister()
    {
        base.OnRegister();
        cookProxy = Facaded.RetrieveProxy(CookProxy.NAME) as CookProxy; 
        if(null == cookProxy)
        {
            throw new System.Exception(CookProxy.NAME + "is null.");
        }
        CookView.UpdateCook(cookProxy.Cooks);
    }
    public override IList<string> ListNotificationInterests()
    {
        IList<string> notifications = new List<string>();
        notifications.Add(OrderSystemEvent.CALL_COOK);
        notifications.Add(OrderSystemEvent.SERVER_FOOD);
        notifications.Add(OrderSystemEvent.REFRESHCOOK);
        return notifications;
    }

    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case OrderSystemEvent.CALL_COOK:
                Order order = notification.Body as Order;
                if (null == order)
                    throw new Exception("order is null ,please check it.");
                //todo 分配一个厨师开始做菜
                Debug.Log("厨师接收到前台的订单,开始炒菜:" + order.names);
                SendNotification(OrderCommandEvent.CookCooking, order,"Busy");
                break;
            case OrderSystemEvent.SERVER_FOOD:
                Debug.Log("厨师通知服务员上菜");
                CookItem cook = notification.Body as CookItem;
                //SendNotification(OrderSystemEvent.REFRESHCOOK);//刷新一下厨师界面
                //TODO 先修改厨师状态 然后由厨师的代理Proxy发送刷新消息

                SendNotification(OrderCommandEvent.selectWaiter, cook.cookOrder, "SERVING");
                cook.cookOrder = null;
                SendNotification(OrderCommandEvent.CookCooking,cook,"Rester");
                break;
            case OrderSystemEvent.REFRESHCOOK:
                cookProxy = Facaded.RetrieveProxy(CookProxy.NAME) as CookProxy;
                Debug.Log("刷新厨师状态");
                if (null == cookProxy)
                    throw new Exception(CookProxy.NAME + "is null.");
                //CookItem cookitem = notification.Body as CookItem;
                CookView.ResfrshCook(cookProxy.Cooks);
                //CookView.RefreshCook(cookitem);
                break; ;

        }
    }
}
