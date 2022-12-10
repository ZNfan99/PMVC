using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCommand : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        RoomProxy roomProxy = Facaded.RetrieveProxy(RoomProxy.NAME) as RoomProxy; //房间的代理
        ClientItem client = notification.Body as ClientItem;
        roomProxy.Reception(client);
    }
}
