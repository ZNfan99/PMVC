using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WaiterProxy : Proxy
{
    public new const string NAME = "WaiterProxy";

    //待处理订单
    public Queue<Order> WaitOrders = new Queue<Order>();

    public IList<WaiterItem> Waiters
    {
        get
        {
            return (IList<WaiterItem>)base.Data;
        }
    }
    public WaiterProxy() : base(NAME, new List<WaiterItem>())
    {

    }

    public override void OnRegister()
    {
        base.OnRegister();
        AddWaiter(new WaiterItem(1, "小丽", 0));
        AddWaiter(new WaiterItem(2, "小红", 0));
        //AddWaiter(new WaiterItem(3, "小花", 0));
    }

    public void AddWaiter(WaiterItem item)
    {
        Waiters.Add(item);
    }

    public void RemoveWaiter(WaiterItem item)
    {
        Waiters.Remove(item);
    }

    public void BecomeIdle(WaiterItem item)
    {

        for (int i = 0; i < Waiters.Count; i++)
        {
            if (item.id == Waiters[i].id)
            {
                ChangeWaiterState(item);
            }
        }
    }

    /// <summary>
    /// 送完菜判断队列里是否还有菜
    /// </summary>
    /// <param name="item"></param>
    public void ChangeWaiterState(WaiterItem item)
    {
        GetWaiterItem(item.id).state = WaiterState.Idle;
        if(WaitOrders.Count != 0)
        {
            Serving(WaitOrders.Dequeue());
            return;
        }
        SendNotification(OrderSystemEvent.REFRESHWAITER);
    }

    public WaiterItem GetWaiterItem(int id)
    {
        for (int i = 0; i < Waiters.Count; i++)
        {
            if(id == Waiters[i].id)
            {
                return Waiters[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 上菜
    /// </summary>
    /// <param name="order"></param>
    public void Serving(Order order)
    {
        for (int i = 0; i < Waiters.Count; i++)
        {
            if (Waiters[i].state == WaiterState.Idle)//找到空闲的厨师，改变其状态
            {
                Waiters[i].state = WaiterState.Busy;
                Waiters[i].order = order;// 厨师炒菜的菜单
                UnityEngine.Debug.Log(order.names);
                SendNotification(OrderSystemEvent.REFRESHWAITER, Waiters[i]);
                SendNotification(OrderSystemEvent.FOOD_TO_CLIENT, Waiters[i]);//发送炒好的菜单信息
                return;
            }
        }
        //TODO 如果没有空闲厨师 做订单缓存
        WaitOrders.Enqueue(order);
    }
    private WaiterItem GetIdleWaiter()
    {
        foreach (WaiterItem waiter in Waiters)
            if (waiter.state.Equals(WaiterState.Idle))
                return waiter;
        UnityEngine.Debug.LogWarning("暂无空闲服务员请稍等..");
        return null;
    }
}
