using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : IView
{
    protected static volatile IView m_instance;
    protected IDictionary<string, IMediator> m_mediatorMap = new Dictionary<string, IMediator>();
    protected IDictionary<string, IList<IObserver>> m_observerMap = new Dictionary<string, IList<IObserver>>();
    protected static readonly object m_staticSyncRoot = new object();
    protected readonly object m_syncRoot = new object();

    protected View()
    {
        this.InitializeView();
    }

    public virtual bool HasMediator(string mediatorName)
    {
        lock (this.m_syncRoot)
        {
            return this.m_mediatorMap.ContainsKey(mediatorName);
        }
    }

    protected virtual void InitializeView()
    {
    }

    public virtual void NotifyObservers(INotification notification)//StartUp MainUI
    {
        //一个接口的集合 相当于是观察者模式要实现的方法
        IList<IObserver> list = null;
        lock (this.m_syncRoot)
        {
            if (this.m_observerMap.ContainsKey(notification.Name))
            {
                IList<IObserver> collection = this.m_observerMap[notification.Name];//Controller executeCommand
                list = new List<IObserver>(collection);
            }
        }
        //去遍历这个集合 调用每个实现接口的方法
        if (list != null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].NotifyObserver(notification);//StartUp MainUI
            }
        }
    }

    public virtual void RegisterMediator(IMediator mediator)
    {
        lock (this.m_syncRoot)
        {
            if (this.m_mediatorMap.ContainsKey(mediator.MediatorName))//MenuMediator
            {
                return;
            }
            this.m_mediatorMap[mediator.MediatorName] = mediator;
            //OrderSystemEvent.UPMENU
            //OrderSystemEvent.CANCEL_ORDER
            //OrderSystemEvent.SUBMITMENU
            IList<string> list = mediator.ListNotificationInterests();
            if (list.Count > 0)
            {
                //实例化一个观察者notifyContext= MenuMediator  notifyMethod = "handleNotification"
                IObserver observer = new Observer(mediator, "handleNotification");
                for (int i = 0; i < list.Count; i++)
                {
                    //将方法名注册给观察者
                    this.RegisterObserver(list[i].ToString(), observer);
                }
            }
        }
        mediator.OnRegister();
    }

    public virtual void RegisterObserver(string notificationName, IObserver observer)//StartUp (Controller executeCommand)
    {
        lock (this.m_syncRoot)
        {
            if (!this.m_observerMap.ContainsKey(notificationName))
            {
                this.m_observerMap[notificationName] = new List<IObserver>();
            }
            this.m_observerMap[notificationName].Add(observer);
        }
    }

    public virtual IMediator RemoveMediator(string mediatorName)
    {
        IMediator notifyContext = null;
        lock (this.m_syncRoot)
        {
            if (!this.m_mediatorMap.ContainsKey(mediatorName))
            {
                return null;
            }
            notifyContext = this.m_mediatorMap[mediatorName];
            IList<string> list = notifyContext.ListNotificationInterests();
            for (int i = 0; i < list.Count; i++)
            {
                this.RemoveObserver(list[i], notifyContext);
            }
            this.m_mediatorMap.Remove(mediatorName);
        }
        if (notifyContext != null)
        {
            notifyContext.OnRemove();
        }
        return notifyContext;
    }

    public virtual void RemoveObserver(string notificationName, object notifyContext)
    {
        lock (this.m_syncRoot)
        {
            if (this.m_observerMap.ContainsKey(notificationName))
            {
                IList<IObserver> list = this.m_observerMap[notificationName];
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].CompareNotifyContext(notifyContext))
                    {
                        list.RemoveAt(i);
                        break;
                    }
                }
                if (list.Count == 0)
                {
                    this.m_observerMap.Remove(notificationName);
                }
            }
        }
    }

    public virtual IMediator RetrieveMediator(string mediatorName)
    {
        lock (this.m_syncRoot)
        {
            if (!this.m_mediatorMap.ContainsKey(mediatorName))
            {
                return null;
            }
            return this.m_mediatorMap[mediatorName];
        }
    }

    

    public static IView Instance
    {
        get
        {
            if (m_instance == null)
            {
                lock (m_staticSyncRoot)
                {
                    if (m_instance == null)
                    {
                        m_instance = new View();
                    }
                }
            }
            return m_instance;
        }
    }
}
