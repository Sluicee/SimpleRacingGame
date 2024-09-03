using System.Collections.Generic;
using UnityEngine;
using YG;

public static class MetricaSender
{
    public static void Send(string id)
    {
        YandexMetrica.Send(id);
    }

    public static void TriggerSend(string name)
    {
        var eventParams = new Dictionary<string, string>
        {
            { "triggers", name }
        };

        YandexMetrica.Send("triggers", eventParams);
    }
}
