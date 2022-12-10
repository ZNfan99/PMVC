using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMediator : Mediator
{
    private MenuProxy menuProxy = null;
    public new const string NAME = "MenuMediator";

    public MenuView MenuView
    {
        get { return (MenuView)ViewComponent; }
    }

    /// <summary>
    /// new的时候将view中的定义的事件绑定一下 相当于消息中心的Broadcast
    /// </summary> 
    /// <param name="view"></param>
    /// m_mediatorName = MenuMediator  viewComponent = MenuView
    public MenuMediator(MenuView view) : base(NAME,view)
    {
        MenuView.Submit += order => { SendNotification(OrderSystemEvent.SUBMITMENU, order); };
        MenuView.Cancel += () => { SendNotification(OrderSystemEvent.CANCEL_ORDER); };
    }

    public override void OnRegister()
    {
        base.OnRegister();
        //去持有自己的代理
        menuProxy = Facaded.RetrieveProxy(MenuProxy.NAME) as MenuProxy;
        if(menuProxy == null)
        {
            throw new System.Exception(MenuProxy.NAME + "is null!");
        }
        //去刷新一下自己的页面
        MenuView.UpdateMenu(menuProxy.Menus);
    }

    /// <summary>
    /// 对什么事件感兴趣，相当于消息中心的AddListener
    /// </summary>
    /// <returns></returns>
    public override IList<string> ListNotificationInterests()
    {
        IList<string> notifications = new List<string>();
        notifications.Add(OrderSystemEvent.UPMENU);
        notifications.Add(OrderSystemEvent.CANCEL_ORDER);
        notifications.Add(OrderSystemEvent.SUBMITMENU);
        return notifications;
    }

    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case OrderSystemEvent.UPMENU:
                Order order = notification.Body as Order;
                if(order == null)
                {
                    throw new System.Exception("order is null, please check it!");
                }
                MenuView.UpMenu(order);
                break;
            case OrderSystemEvent.CANCEL_ORDER:
                Order order1 = notification.Body as Order;
                if (order1 == null)
                {
                    throw new Exception("order is null ,plase check it!");
                }
                MenuView.CancelMenu();
                SendNotification(OrderCommandEvent.GET_ORDER, order1, "Exit");//取消菜单
                break;
            case OrderSystemEvent.SUBMITMENU:
                Order selectedOrder = notification.Body as Order;
                MenuView.SubmitMenu(selectedOrder);
                SendNotification(OrderSystemEvent.ORDER, selectedOrder);
                break;
            default:
                break;
        }
    }
}
