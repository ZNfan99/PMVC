using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    public int id { get; set; }
    public ClientItem client { get; set; }
    public IList<MenuItem> menus { get; set; }
    public float pay
    {
        get
        {
            var money = 0.0f;
            foreach (var item in menus)
            {
                money += item.price;
            }
            return money;
        }
    }

    public string names
    {
        get
        {
            string name = "";
            foreach (var item in menus)
            {
                name += item.name + ",";
            }
            return name;
        }
    }
    public Order(ClientItem client, IList<MenuItem> menus)
    {
        this.client = client;
        this.menus = menus;
    }
    public override string ToString()
    {
        return client.id + "号桌 " + client.population + "位顾客点餐" + menus.Count + "道,共消费" + pay + "元";
    }
}
