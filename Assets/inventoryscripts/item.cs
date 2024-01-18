using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Tiem", menuName = "Inventory/new item")]
public class item : ScriptableObject
{
    //道具描述？
    public string Name;
   public string describe;
    //道具数目
    public int numItem;
   public bool isGet;//判断此道具是否被获取
}
