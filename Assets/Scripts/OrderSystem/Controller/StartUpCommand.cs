using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class StartUpCommand : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        //菜单代理
        MenuProxy menuProxy = new MenuProxy();
        Facaded.RegisterProxy(menuProxy);

        //客户端代理
        ClientProxy clientProxy = new ClientProxy();    
        Facaded.RegisterProxy(clientProxy);

        //服务员代理
        WaiterProxy waitProxy = new WaiterProxy();
        Facaded.RegisterProxy(waitProxy);

        //厨师端代理
        CookProxy cookProxy = new CookProxy();
        Facaded.RegisterProxy(cookProxy);

        //订单代理
        OrderProxy orderProxy = new OrderProxy();
        Facaded.RegisterProxy(orderProxy);

        RoomProxy roomProxy = new RoomProxy();
        Facaded.RegisterProxy(roomProxy);

        //model ==== 以上

        //view ==== 以下

        MainUI mainUI = notification.Body as MainUI;

        if (null == mainUI)
            throw new Exception("程序启动失败..");
        Facaded.RegisterMediator(new MenuMediator(mainUI.MenuView));
        Facaded.RegisterMediator(new ClientMediator(mainUI.ClientView));
        Facaded.RegisterMediator(new WaiterMediator(mainUI.WaitView));
        Facaded.RegisterMediator(new CookMediator(mainUI.CookView));
        Facaded.RegisterMediator(new RoomMediator(mainUI.RoomView));

        Facaded.RegisterCommand(OrderCommandEvent.GUEST_BE_AWAY, typeof(GuestBeAwayCommand));
        Facaded.RegisterCommand(OrderCommandEvent.GET_ORDER, typeof(GetAndExitOrderCommand));
        Facaded.RegisterCommand(OrderCommandEvent.CookCooking, typeof(CookCommand));
        Facaded.RegisterCommand(OrderCommandEvent.selectWaiter, typeof(WaiterCommand));
        Facaded.RegisterCommand(OrderCommandEvent.ChangeClientState, typeof(ClientChangeStateCommand));
        Facaded.RegisterCommand(OrderCommandEvent.Reception, typeof(RoomCommand));
    }
}
