using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class eventsystem : baseNomonoManager<eventsystem>
{
    Dictionary<string, UnityAction> events = new Dictionary<string, UnityAction>();
    //����¼��ĺ���
    public void setUp(string name,UnityAction action)
    {
        //�ж��Ƿ���ڸ�key
        if(events.ContainsKey(name))
        {
            events[name] += action;
        }
        else
        {
            events.Add(name, action);
        }
    }
    //���õĺ���
    public void EventInvoke(string name)
    {
        if(events.ContainsKey(name))
        {
            events[name].Invoke();
        }
    }
    //ɾ���¼�
    public void deleteEvent(string name,UnityAction action)
    {
        if(events.ContainsKey(name))
        {
            events[name] -= action;
        }
    }
    public void clearKey(string name)
    {
        if(events.ContainsKey(name))
        {
            events.Remove(name);
        }
    }
    public void clearAll()
    {
        events.Clear();
    }
}
