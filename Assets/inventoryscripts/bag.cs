using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New BAG", menuName = "Inventory/new bag")]
public class bag :ScriptableObject
{
   public List<item> items = new List<item>();
    public int itemNum;
}
