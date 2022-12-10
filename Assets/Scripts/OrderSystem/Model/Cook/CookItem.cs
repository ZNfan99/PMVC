using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CookState
{
    Leisure=0,//空闲
    Busy,//忙碌
    nor
}

public class CookItem
{
    public int id { get; set; }
    public string name { get; set; }
    public CookState state { get; set; }
    public string cooking { get; set; }
    public Order cookOrder { get; set; }

    public CookItem(int id, string name, CookState state = 0, string cooking = "", Order cookOrder = null)
    {
        this.id = id;
        this.name = name;
        this.state = state;
        this.cooking = cooking;
        this.cookOrder = cookOrder;
    }

    public override string ToString()
    {
        return id + "号厨师\n" + name + "\n" + ResultState();
    }

    private string ResultState()
    {
        if(state.Equals(CookState.Leisure))
        {
            return "休息中";
        }
        return "忙碌中：" + cookOrder.client.id + "做菜中";
    }
}
