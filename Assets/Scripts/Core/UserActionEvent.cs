using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserActionEvent
{
    public enum EventCondition
    {
        first, second, third, fourth, fifth, sixth
    }

    public string Name { get; private set; }
    public float FlamePower { get; private set; }
    public EventCondition Condition { get; private set; }

    public static UserActionEvent RandomEvent
    {
        get
        {
            EventCondition condition = (EventCondition)Random.Range(0, 6);
            return new UserActionEvent(
                System.Enum.GetName(typeof(EventCondition), condition),
                Random.Range(5f, 10f), condition
            );
        }
    }

    public UserActionEvent(string name, float flamePower, EventCondition condition)
    {
        Name = name;
        FlamePower = flamePower;
        Condition = condition;
    }

    public float OnFulfilled()
    {
        return FlamePower;
    }
}
