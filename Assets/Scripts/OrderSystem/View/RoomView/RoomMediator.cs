using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMediator : Mediator
{
    private RoomProxy roomProxy = null;
    public new const string NAME = "RoomMediator";

    public RoomView RoomView
    {
        get
        {
            return (RoomView)base.ViewComponent;
        }
    }
    public RoomMediator(RoomView view) : base(NAME, view)
    {
        RoomView.CallRoom += () => { SendNotification(OrderSystemEvent.CALLROOM); };
        RoomView.CleanRoom += (item) => { SendNotification(OrderSystemEvent.REFRESHROOM, item); };
    }
    public override void OnRegister()
    {
        base.OnRegister();
        roomProxy = Facaded.RetrieveProxy(RoomProxy.NAME) as RoomProxy;
        if (null == roomProxy)
            throw new Exception(CookProxy.NAME + "is null.");
        RoomView.UpdateRoom(roomProxy.Rooms);
    }

    public override IList<string> ListNotificationInterests()
    {
        IList<string> notifications = new List<string>();
        notifications.Add(OrderSystemEvent.CALLROOM);
        notifications.Add(OrderSystemEvent.REFRESHROOM);         
        return notifications;
    }
    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case OrderSystemEvent.CALLROOM:
                ClientItem client = notification.Body as ClientItem;
                if (null == client)
                    throw new Exception("client is null ,please check it.");
                Debug.Log("客人入住");
                SendNotification(OrderCommandEvent.Reception,client);
                //SendNotification(OrderSystemEvent.REFRESHROOM);
                break;
            case OrderSystemEvent.REFRESHROOM:
                roomProxy = Facaded.RetrieveProxy(RoomProxy.NAME) as RoomProxy;
                Debug.Log("刷新房间状态");
                if (null == roomProxy)
                    throw new Exception(RoomProxy.NAME + "is null.");
                
                RoomView.ResfrshRoom(roomProxy.Rooms);
                break; 

        }
    }
}
