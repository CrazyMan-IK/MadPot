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
        private const int _notificationID = 23451;

        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();

            if (notificationIntentData != null)
            {
                Amplitude.Instance.logEvent("notification_clicked");
            }

            var status = AndroidNotificationCenter.CheckScheduledNotificationStatus(_notificationID);

            if (status == NotificationStatus.Unknown)
            {
                var notification = new AndroidNotification();
                notification.Title = "Lorem";
                notification.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed sit amet enim ac magna imperdiet dignissim. Vestibulum a lectus a nisi tincidunt congue.";
                notification.FireTime = DateTime.Now.AddDays(1);
                notification.RepeatInterval = new TimeSpan(24, 0, 0);

                AndroidNotificationCenter.SendNotificationWithExplicitID(notification, "Main", _notificationID);
            }
        }
    }
}
