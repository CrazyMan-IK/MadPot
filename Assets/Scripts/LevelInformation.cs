using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadPot
{
    [CreateAssetMenu(fileName = "New Level", menuName = "Mad Pot/Level", order = 100)]
    public class LevelInformation : ScriptableObject
    {
        public event Action CombinationChanged = null;

        [field: SerializeField] public List<Combination> Combinations { get; set; } = new List<Combination>();
        [field: SerializeField] public LevelInformation NextLevel { get; set; } = null;

        [NonSerialized] private bool _isCompleted = false;
        [NonSerialized] private int _currentCombination = 0;

        public Combination CurrentCombination => Combinations[_currentCombination];
        public bool IsCompleted => _isCompleted;
        
        public bool IsProductCorrect(Product product)
        {
            return CurrentCombination.IsProductCorrect(product);
        }

        public IEnumerable<Vector3> GetTargetPoints(Camera camera)
        {
            return CurrentCombination.GetTargetPoints(camera);
        }

        public IEnumerable<Vector3> GetFailPoints(Camera camera)
        {
            return CurrentCombination.GetFailPoints(camera);
        }

        public IEnumerable<Vector3> GetNullPoints(Camera camera)
        {
            return CurrentCombination.GetNullPoints(camera);
        }

        public void CompleteCombination()
        {
            if (_currentCombination >= Combinations.Count - 1)
            {
                _isCompleted = true;
                return;
            }

            PlayerPrefs.SetInt("tutorial-" + CurrentCombination.TutorialBehaviourType.ToString(), 1);
            PlayerPrefs.Save();

            _currentCombination++;
            CombinationChanged?.Invoke();
        }

        public void Reset()
        {
            _isCompleted = false;
            _currentCombination = 0;
        }
    }
}
