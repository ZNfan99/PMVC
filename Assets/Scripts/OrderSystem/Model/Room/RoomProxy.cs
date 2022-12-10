using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomProxy : Proxy
{
    public new const string NAME = "RoomProxy";
    public IList<RoomItem> Rooms
    {
        get { return (IList<RoomItem>)base.Data; }
    }

    public RoomProxy() : base(NAME, new List<RoomItem>())
    {
        AddRoom(new RoomItem(1, "VIP666", 0));
        AddRoom(new RoomItem(2, "VIP888", 0));
        AddRoom(new RoomItem(3, "VIP999", 0));
    }

    public void AddRoom(RoomItem item)
    {
        Rooms.Add(item);
    }

    public void RemoveRoom(RoomItem item)
    {
        for (int i = 0; i < Rooms.Count; i++)
        {
            if (item.id == Rooms[i].id)
            {
                Rooms[i].state = RoomState.Free;
                Rooms[i].client = null;
                SendNotification(OrderSystemEvent.REFRESHROOM);
                return;
            }
        }
    }

    public void Reception(ClientItem client)
    {
        for (int i = 0; i < Rooms.Count; i++)
        {
            if (Rooms[i].state == RoomState.Free)
            {
                Rooms[i].state = RoomState.Busy;
                Rooms[i].client = client;
                SendNotification(OrderSystemEvent.REFRESHROOM);
                return;
            }
        }
    }
}
