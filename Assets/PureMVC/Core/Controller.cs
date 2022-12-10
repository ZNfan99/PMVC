using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : IController
{
    protected IDictionary<string, Type> m_commandMap = new Dictionary<string, Type>();
    protected static volatile IController m_instance;
    protected static readonly object m_staticSyncRoot = new object();
    protected readonly object m_syncRoot = new object();
    protected IView m_view;

    protected Controller()
    {
        this.InitializeController();
    }

    private void InitializeController()
    {
        this.m_view = View.Instance;
    }

    public void ExecuteCommand(INotification notification)//StartUp MainUI
    {
        Type type = null;
        lock (this.m_syncRoot)
        {
            if (!this.m_commandMap.ContainsKey(notification.Name))
            {
                return;
            }
            type = this.m_commandMap[notification.Name];//StartCommand
        }
        object obj2 = Activator.CreateInstance(type);
        if (obj2 is ICommand)
        {
            ((ICommand)obj2).Execute(notification);//执行StartCommand中的Execute
        }
    }

    public bool HasCommand(string notificationName)
    {
        lock (this.m_syncRoot)
        {
            return this.m_commandMap.ContainsKey(notificationName);
        }
    }

    public void RegisterCommand(string notificationName, Type commandType)//StartUp StartUpCommand
    {
        lock (this.m_syncRoot)
        {
            //map相当于是一个库
            if (!this.m_commandMap.ContainsKey(notificationName))
            {
                this.m_view.RegisterObserver(notificationName, new Observer(this,"executeCommand"));
            }
            this.m_commandMap[notificationName] = commandType;//StartUp StartUpCommand
        }
    }

    public void RemoveCommand(string notificationName)
    {
        lock (this.m_syncRoot)
        {
            if (this.m_commandMap.ContainsKey(notificationName))
            {
                this.m_view.RemoveObserver(notificationName, this);
                this.m_commandMap.Remove(notificationName);
            }
        }
    }
    public static IController Instance
    {
        get
        {
            if (m_instance == null)
            {
                lock (m_staticSyncRoot)
                {
                    if (m_instance == null)
                    {
                        m_instance = new Controller();
                    }
                }
            }
            return m_instance;
        }
    }
}
