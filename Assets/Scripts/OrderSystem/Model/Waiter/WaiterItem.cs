using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WaiterState
{
    Idle = 0,
    Busy,
    nor
}

public class WaiterItem
{
    public WaiterItem(int id, string name, WaiterState state = 0, Order order = null)
    {
        this.id = id;
        this.name = name;
        this.state = state;
    }

    public int id { get; set; }
    public string name { get; set; }
    public WaiterState state { get; set; }
    public Order order {get;set;}

    public override string ToString()
    {
        return id + "号服务员\n" + name + "\n" + ResultState();
    }

    private string ResultState()
    {
        if(state.Equals(WaiterState.Idle))
        {
            return "休息中";
        }
        //if(order == null)
        //{
        //    return "忙碌中"+ "送菜中";
        //}
        return "忙碌中" + order.client.id + "送菜中";
    }
}
