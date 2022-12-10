using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RoomView : MonoBehaviour
{
    public UnityAction CallRoom = null;
    public UnityAction<RoomItem> CleanRoom = null;

    private ObjectPool<RoomItemView> objectPool = null;
    private List<RoomItemView> rooms = new List<RoomItemView>();
    private Transform parent = null;

    private void Awake()
    {
        parent = this.transform.Find("Content");
        var prefab = Resources.Load<GameObject>("Prefabs/UI/RoomItem");
        objectPool = new ObjectPool<RoomItemView>(prefab, "RoomPool");
    }

    public void UpdateRoom(IList<RoomItem> rooms)
    {
        for (int i = 0; i < this.rooms.Count; i++)
            objectPool.Push(this.rooms[i]);

        this.rooms.AddRange(objectPool.Pop(rooms.Count));
        ResfrshRoom(rooms);
    }

    public void ResfrshRoom(IList<RoomItem> rooms)
    {
        for (int i = 0; i < this.rooms.Count; i++)
        {
            this.rooms[i].transform.SetParent(parent);
            var item = rooms[i];
            this.rooms[i].transform.Find("Id").GetComponent<Text>().text = item.ToString();
            Color color = this.rooms[i].GetComponent<Image>().color;

            if (item.state.Equals(RoomState.Free))
            {
                color = Color.green;
            }
            else if (item.state.Equals(RoomState.Busy))
            {
                color = Color.yellow;
                StartCoroutine(RoomClean(rooms[i]));
            }
            else if(item.state.Equals(RoomState.Nor))
            {
                color = Color.yellow;
            }
            this.rooms[i].transform.Find("Image").GetComponent<Image>().color = color;
        }
    }
    IEnumerator RoomClean(RoomItem item,float time = 4)
    {
        item.state = RoomState.Nor;
        yield return new WaitForSeconds(time);
        item.state = RoomState.Free;
        //CleanRoom(item)
        CleanRoom.Invoke(item);
    }
}
