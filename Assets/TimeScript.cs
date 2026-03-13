using System;
using UnityEngine;
using TMPro;

public class TimeDisplay : MonoBehaviour
{
    public TextMeshPro timeText;
    TimeZoneInfo timezone;

    void Start()
    {
        timezone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
        InvokeRepeating("UpdateTime", 0, 1);
    }

    void UpdateTime()
    {
        DateTime localTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, timezone);
        timeText.text = "Current Time\n\n";
        timeText.text = timeText.text + localTime.ToString("hh:mm:ss tt");
    }
}