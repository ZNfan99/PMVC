using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClientState
{
    WaitMenu,
    WaitFood,
    Eating,
    Pay
}

public class ClientItem 
{
    public int id { get; set; }
    public int population { get; set; }
    public ClientState state { get; set; }

    public int waiterId { get; set; }

    public ClientItem(int id, int population, ClientState state)
    {
        this.id = id;
        this.population = population;
        this.state = state;
    }
    public override string ToString()
    {
        return id + "号桌" + "\n" + population + "个人" + "\n" + returnState(state);
    }
    private string returnState(ClientState state)
    {
        if (state.Equals(ClientState.WaitMenu))
            return "等待菜单";
        if (state.Equals(ClientState.WaitFood))
            return "等待上菜";
        if (state.Equals(ClientState.Eating))
            return "就餐中";
        return "已经结账";
    }
}
