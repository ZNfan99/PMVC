using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookProxy : Proxy
{
    public new const string NAME = "CookProxy";

    //待处理订单
    public Queue<Order> WaitOrders = new Queue<Order>();
    public IList<CookItem> Cooks
    {
        get { return (IList<CookItem>)base.Data; }
    }

    public CookProxy() : base(NAME, new List<CookItem>())
    {

    }

    public override void OnRegister()
    {
        base.OnRegister();
        AddCook(new CookItem(1, "强尼", 0));
        AddCook(new CookItem(2, "托尼"));
        AddCook(new CookItem(3, "鲍比", 0));
        AddCook(new CookItem(4, "缇米"));
    }
    public void AddCook(CookItem item)
    {
        Cooks.Add(item);
    }
    public void RemoveCook(CookItem item)
    {
        Cooks.Remove(item);
    }

    public void CookCooking(Order order)
    {
        for (int i = 0; i < Cooks.Count; i++)
        {
            if (Cooks[i].state == CookState.Leisure)//找到空闲的厨师，改变其状态
            {
                Cooks[i].state = CookState.Busy;
                Cooks[i].cooking = order.names;//厨师炒的菜
                Cooks[i].cookOrder = order;// 厨师炒菜的菜单
                UnityEngine.Debug.Log(order.names);
                SendNotification(OrderSystemEvent.REFRESHCOOK);//找到空闲厨师去刷新一下厨师显示的状态
                return;
            }
        }
        //TODO 如果没有空闲厨师 做订单缓存
        WaitOrders.Enqueue(order);
    }

    public void ChangeCookState(CookItem cookItem)
    {
        GetCookItem(cookItem.id).state = CookState.Leisure;
        if(WaitOrders.Count != 0)
        {
            CookCooking(WaitOrders.Dequeue());
            return;
        }
        SendNotification(OrderSystemEvent.REFRESHCOOK);//刷新一下厨师显示的状态
    }

    public CookItem GetCookItem(int id)
    {
        for (int i = 0; i < Cooks.Count; i++)
        {
            if (Cooks[i].id == id)
            {
                return Cooks[i];
            }
        }
        return null;
    }
}

