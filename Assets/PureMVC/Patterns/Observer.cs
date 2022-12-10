using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System;

public class Observer : IObserver
{
    private object m_notifyContext;
    private string m_notifyMethod;
    protected readonly object m_syncRoot = new object();

    public Observer(object notifyContext, string notifyMethod)//Controller executeCommand
    {
        m_notifyContext = notifyContext;
        m_notifyMethod = notifyMethod;
    }

    public object NotifyContext
    { 
        private get
        {
            return m_notifyContext;
        }
        set
        {
            m_notifyContext = value;
        }
    }
    public string NotifyMethod 
    {
        private get 
        { 
            return m_notifyMethod; 
        }
        set
        {
            m_notifyMethod = value;
        }
    }

    public bool CompareNotifyContext(object obj)
    {
        lock (this.m_syncRoot)
        {
            return this.NotifyContext.Equals(obj);
        }
    }

    public void NotifyObserver(INotification notification)//StartUp MainUI
    {
        object notifyContext;
        lock (this.m_syncRoot)
        {
            notifyContext = this.NotifyContext;
            //string notifyMethod = this.NotifyMethod;
        }
        //利用反射获取方法然后执行
        Type type = notifyContext.GetType();
        //这里设置忽略字母的大小写
        BindingFlags bindingAttr = BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase;
        //根据设置的中介者的名字或者是命令的名字执行对应的方法，具体方法的执行在中介者和命令中已经重写对应的方法实现
        MethodInfo method = type.GetMethod(this.NotifyMethod, bindingAttr);
        method.Invoke(notifyContext, new object[] { notification });
    }
}
