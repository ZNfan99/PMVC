using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WaiterView : MonoBehaviour
{
    public UnityAction CallWaiter = null;
    public UnityAction<Order> Order = null;
    public UnityAction Pay = null;
    public UnityAction CallCook = null;
    public UnityAction<WaiterItem> ServerFood = null;

    private ObjectPool<WaiterItemView> objectPool= null;
    private List<WaiterItemView> waiters = new List<WaiterItemView>();
    private Transform parent = null;

    private void Awake()
    {
        parent = transform.Find("Content");
        var prefab = Resources.Load<GameObject>("Prefabs/UI/WaiterItem");
        objectPool = new ObjectPool<WaiterItemView>(prefab,"WaiterPool");
    }

    public void UpdateWaiter(IList<WaiterItem> waiters)
    {
        for (int i = 0; i < this.waiters.Count; i++)
            objectPool.Push(this.waiters[i]);

        this.waiters.AddRange(objectPool.Pop(waiters.Count));
        Move(waiters);
    }

    public void Move(IList<WaiterItem> waiters)
    {
        for (int i = 0; i < this.waiters.Count; i++)
        {
            this.waiters[i].transform.SetParent(parent);
            var item = waiters[i];
            this.waiters[i].transform.Find("Id").GetComponent<Text>().text = item.ToString();
            Color color = Color.white;
            if (item.state.Equals(WaiterState.Idle))
                color = Color.green;
            else if (item.state.Equals(WaiterState.Busy))
            {
                color = Color.yellow;
                StartCoroutine(WaiterServing(item));
            }
            else if(item.state.Equals(WaiterState.nor))
            {
                color = Color.yellow;
            }
            this.waiters[i].transform.Find("Image").GetComponent<Image>().color = color;
        }
    }
    IEnumerator WaiterServing(WaiterItem item, float time = 4)
    {
        item.state = WaiterState.nor;
        yield return new WaitForSeconds(time);
        ServerFood.Invoke(item);
    }
}
