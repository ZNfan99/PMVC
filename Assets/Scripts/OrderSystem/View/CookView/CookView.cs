using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CookView : MonoBehaviour
{
    public UnityAction CallCook = null;
    public UnityAction<CookItem> ServerFood = null;

    private ObjectPool<CookItemView> objectPool = null;
    private List<CookItemView> cooks = new List<CookItemView>();
    private Transform parent = null;

    private void Awake()
    {
        parent = transform.Find("Content");
        var prefab = Resources.Load<GameObject>("Prefabs/UI/CookItem");
        objectPool = new ObjectPool<CookItemView>(prefab, "CookPool");
    }

    public void UpdateCook(IList<CookItem> cooks)
    {
        for (int i = 0; i < this.cooks.Count; i++)
        {
            objectPool.Push(this.cooks[i]);
        }
            

        this.cooks.AddRange(objectPool.Pop(cooks.Count));
        ResfrshCook(cooks);
    }

    public void ResfrshCook(IList<CookItem> cooks)
    {
        for (int i = 0; i < this.cooks.Count; i++)
        {
            this.cooks[i].transform.SetParent(parent);
            var item = cooks[i];
            this.cooks[i].transform.Find("Id").GetComponent<Text>().text = item.ToString();
            Color color = Color.white;
            if (item.state.Equals(CookState.Leisure))
            {
                color = Color.green;
            }
            else if (item.state.Equals(CookState.Busy))
            {
                color = Color.yellow;
                StartCoroutine(Cooking(item));
            }
            else if(item.state.Equals(CookState.nor))
            {
                color = Color.yellow;
            }
            
            this.cooks[i].transform.Find("Image").GetComponent<Image>().color = color;
        }
    }
    public void RefreshCook(CookItem item)
    {
        Color color = Color.green;
        if (item.state.Equals(CookState.Leisure))
        {
            color = Color.green;
        }
        else if (item.state.Equals(CookState.Busy))
        {
            color = Color.yellow;
            StartCoroutine(Cooking(item));
        }
        this.cooks[item.id].transform.Find("Image").GetComponent<Image>().color = color;
    }
    /// <summary>
    /// 炒菜完成
    /// </summary>
    /// <param name="item"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator Cooking(CookItem item, float time = 4)
    {
        item.state = CookState.nor;
        yield return new WaitForSeconds(time);
        //item.state = CookState.Leisure;
        //Debug.Log(item.cookOrder);
        ServerFood.Invoke(item);
    }
}
