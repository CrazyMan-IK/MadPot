using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadPot.Interfaces
{
    public enum TutorialType
    {
        None,
        Line,
        Triangle,
        Rectangle,
        Groups
    }

    public interface ITutorial
    {
        LevelInformation CurrentLevel { get; set; }

        IEnumerator StartTutorial(TutorialHand hand, TutorialViewer viewer, LineRenderer line);
    }
}
