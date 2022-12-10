using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 通知者负责通知的发放，并持有Facade
/// 继承该接口的类调用SendNotification 会执行到Facade的SendNotification
/// </summary>
public class Notifier : INotifier
{
    //D:\unity\Project\PureMVC\Assets\PureMVC\Patterns\Facade.cs
    //持有Facade
    private IFacade m_facade =  Facade.Instance;

    public void SendNotification(string notificationName)
    {
        this.m_facade.SendNotification(notificationName);
    }

    public void SendNotification(string notificationName, object body)
    {
        this.m_facade.SendNotification(notificationName, body);
    }

    public void SendNotification(string notificationName, object body, string type)
    {
        this.m_facade.SendNotification(notificationName, body, type);
    }

    protected IFacade Facaded
    {
        get
        {
            return this.m_facade;
        }
    }
}
