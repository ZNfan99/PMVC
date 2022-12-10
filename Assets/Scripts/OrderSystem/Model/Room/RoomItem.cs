using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomState
{
    Free,
    Busy,
    Nor
}

public class RoomItem 
{
    public RoomItem(int id, string name, RoomState state, ClientItem client = null)
    {
        this.id = id;
        this.name = name;
        this.state = state;
        this.client = client;
    }

    public int id { get; set; }
    public string name { get; set; }
    public RoomState state { get; set;}

    public ClientItem client { get; set; }

    public override string ToString()
    {
        return id + "号房间\n" + name +"\n" + returnState(state);
    }

    private string returnState(RoomState state)
    {
        if (state.Equals(RoomState.Free))
                return "空闲";
        return "有人";
    }
}
