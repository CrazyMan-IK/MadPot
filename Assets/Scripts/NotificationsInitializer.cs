using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;

namespace MadPot
{
    public static class NotificationsInitializer
    {
        private const int NotificationID = 23451;

        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();

            if (notificationIntentData != null)
            {
                Amplitude.Instance.logEvent("notification_clicked");
            }

            var status = AndroidNotificationCenter.CheckScheduledNotificationStatus(NotificationID);

            if (status == NotificationStatus.Unknown)
            {
                var now = DateTime.Now;
                var notification = new AndroidNotification();
                notification.Title = "Mad Pot";
                notification.Text = "A juicy burger is waiting for you! \ud83c\udf54";
                notification.FireTime = new DateTime(now.Year, now.Month, now.Day, 20, 0, 0, DateTimeKind.Local);
                notification.RepeatInterval = new TimeSpan(24, 0, 0);

                AndroidNotificationCenter.SendNotificationWithExplicitID(notification, "Main", NotificationID);
            }
        }
    }
}
