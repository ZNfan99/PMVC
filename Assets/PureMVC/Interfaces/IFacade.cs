using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IFacade : INotifier
{
    //负责注册、注销、恢复、和判断是否存在Command、Proxy、Mediator
    //Command 命令  Proxy 代理  Mediator 中介
    bool HasCommand(string notificationName);
    bool HasMediator(string mediatorName);
    bool HasProxy(string proxyName);
    void RegisterCommand(string notificationName, Type commandType); 
    void RegisterMediator(IMediator mediator);
    void RegisterProxy(IProxy proxy);
    void RemoveCommand(string notificationName);
    IMediator RemoveMediator(string mediatorName);
    IProxy RemoveProxy(string proxyName);
    //应该是重新找回移除的中介 移除的中介并没有删除 应该是加到了某个库中
    IMediator RetrieveMediator(string mediatorName);
    IProxy RetrieveProxy(string proxyName);
    //通知观察者
    void NotifyObservers(INotification note);
}
