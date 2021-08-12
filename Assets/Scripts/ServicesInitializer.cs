using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadPot
{
    public static class ServicesInitializer
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            Amplitude amplitude = Amplitude.Instance;
            //amplitude.setServerUrl("https://api2.amplitude.com");
            amplitude.logging = true;
            //amplitude.trackSessionEvents(true);
            amplitude.init("b568c487765e41b7b56fb0d556c6c085");

            amplitude.setOnceUserProperty("reg_day", DateTime.Now.ToString("dd.mm.yy"));
        }
    }
}
