using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserActionEvent
{
    public enum EventCondition
    {
        none, moreAir, addCoal, useHammer, reheatMetal, useGlitter, saveTheHeart
    }

    public string Name { get; private set; }
    public float FlamePower { get; private set; }
    public EventCondition Condition { get; private set; }

    public static UserActionEvent RandomEvent {
        get {
            EventCondition condition = (EventCondition)Random.Range(1, 7);
            return new UserActionEvent(EventName(condition), Random.Range(5f, 10f), condition);
        }
    }

    public UserActionEvent(string name, float flamePower, EventCondition condition)
    {
        Name = name;
        FlamePower = flamePower;
        Condition = condition;
    }

    public static string EventName(EventCondition condition) => condition switch
    {
        EventCondition.none => "",
        EventCondition.moreAir => "Pull the Rope",
        EventCondition.addCoal => "Add Coal to Fire",
        EventCondition.useHammer => "Use the Hammer",
        EventCondition.reheatMetal => "Reheat the Metal",
        EventCondition.useGlitter => "Sprinkle the Glitter",
        EventCondition.saveTheHeart => "Save the Heart",
        _ => ""
    };

    public float OnFulfilled()
    {
        return FlamePower;
    }
}
