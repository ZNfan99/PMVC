using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : IModel
{
    protected static volatile IModel m_instance;
    protected IDictionary<string,IProxy> m_proxyMap = new Dictionary<string,IProxy>();
    protected static readonly object m_staticSyncRoot = new object();
    protected readonly object m_syncRoot = new object();

    protected Model() 
    {
        this.InitializeModel();
    }

    protected virtual void InitializeModel()
    {

    }

    public virtual void RegisterProxy(IProxy proxy)//MenuProxy
    {
        lock (this.m_syncRoot)
        {
            this.m_proxyMap[proxy.ProxyName] = proxy;//key: MenuProxy   value: MenuProxy
        }
        proxy.OnRegister();
    }

    public virtual bool HasProxy(string proxyName)
    {
        lock (this.m_syncRoot)
        {
            return this.m_proxyMap.ContainsKey(proxyName);
        }
    }

    public IProxy RemoveProxy(string proxyName)
    {
        IProxy proxy = null;
        lock (this.m_syncRoot)
        {
            if (this.m_proxyMap.ContainsKey(proxyName))
            {
                proxy = this.RetrieveProxy(proxyName);
                this.m_proxyMap.Remove(proxyName);
            }
        }
        if (proxy != null)
        {
            proxy.OnRemove();
        }
        return proxy;
    }

    public IProxy RetrieveProxy(string proxyName)
    {
        lock (this.m_syncRoot)
        {
            if (!this.m_proxyMap.ContainsKey(proxyName))
            {
                return null;
            }
            return this.m_proxyMap[proxyName];
        }
    }

    public static IModel Instance
    {
        get
        {
            if (m_instance == null)
            {
                lock (m_staticSyncRoot)
                {
                    if (m_instance == null)
                    {
                        m_instance = new Model();
                    }
                }
            }
            return m_instance;
        }
    }
}
