using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using GameAnalyticsSDK;
public enum TypeOfEvent
{
    Start,
    Complete,
    Restart,
    Fail
}
public class EventsAnalitycs : MonoBehaviour
{
    private void Awake()
    {
        //GameAnalytics.Initialize();
        DontDestroyOnLoad(this);
    }

    public static void GameEvent(TypeOfEvent eventType, int levelName)
    {
        switch (eventType)
        {
            case TypeOfEvent.Start:
                 //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, levelName.ToString());
                //SayKit.trackLevelStarted(levelName);

                break;
            case TypeOfEvent.Complete:
                // GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, levelName.ToString());
                //SayKit.trackLevelCompleted(levelName, 0);


                break;
            case TypeOfEvent.Fail:
                //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, levelName.ToString());
                //SayKit.trackLevelFailed(levelName, 0);

                break;
        }
    }
}
