using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MadPot.Interfaces;
using MadPot.Tutorials;
using TMPro;

namespace MadPot
{
    [CreateAssetMenu(fileName = "New Combination", menuName = "Mad Pot/Combination", order = 50)]
    public class Combination : ScriptableObject
    {
        [field: SerializeField] public TutorialType TutorialBehaviourType { get; set; } = TutorialType.None;
        [field: SerializeField] public FigureType TargetFigureType { get; set; } = FigureType.None;
        [field: SerializeField] public ProductType TargetProductsType { get; set; } = ProductType.Inedible;
        [field: SerializeField] public List<LevelProduct> Products { get; set; } = new List<LevelProduct>();

        private ITutorial _tutorialBehaviour = null;

#if UNITY_EDITOR && !DISABLE_EDITOR_RESTRICTIONS
        public bool IsTutorial => TutorialBehaviourType != TutorialType.None;
#else
        public bool IsTutorial => TutorialBehaviourType != TutorialType.None && !PlayerPrefs.HasKey("tutorial-" + TutorialBehaviourType.ToString());
#endif

        public bool IsProductCorrect(Product product)
        {
            return product.Type == TargetProductsType || (TargetProductsType == ProductType.Edible && product.Type != ProductType.Inedible);
        }

        public IEnumerable<Vector3> GetTargetPoints(Camera camera)
        {
            LevelProduct first = null;

            for (int i = 0; i < Products.Count; i++)
            {
                var levelProduct = Products[i];

                if (levelProduct.Product == null)
                {
                    continue;
                }

                if (!IsProductCorrect(levelProduct.Product))
                {
                    continue;
                }

                if (first == null)
                {
                    first = levelProduct;
                }

                yield return levelProduct.GetTargetPosition(camera);
            }

            if (first != null)
            {
                yield return first.GetTargetPosition(camera);
            }
        }

        public IEnumerable<Vector3> GetFailPoints(Camera camera)
        {
            for (int i = 0; i < Products.Count; i++)
            {
                var levelProduct = Products[i];

                if (levelProduct.Product == null)
                {
                    continue;
                }

                if (IsProductCorrect(levelProduct.Product))
                {
                    continue;
                }

                yield return levelProduct.GetTargetPosition(camera);
            }
        }

        public IEnumerable<Vector3> GetNullPoints(Camera camera)
        {
            for (int i = 0; i < Products.Count; i++)
            {
                var levelProduct = Products[i];

                if (levelProduct.Product != null)
                {
                    continue;
                }

                yield return levelProduct.GetTargetPosition(camera);
            }
        }

        public IEnumerator StartTutorial(LevelInformation level, TutorialHand hand, TutorialViewer viewer, TextMeshProUGUI text, LineRenderer line)
        {
            if (_tutorialBehaviour == null && IsTutorial)
            {
                switch (TutorialBehaviourType)
                {
                    case TutorialType.Line:
                        _tutorialBehaviour = new BaseTutorial()
                        {
                            CurrentLevel = level
                        };
                        break;
                    case TutorialType.Triangle:
                        _tutorialBehaviour = new BaseTutorial()
                        {
                            CurrentLevel = level
                        };
                        break;
                    case TutorialType.Rectangle:
                        _tutorialBehaviour = new BaseTutorial()
                        {
                            CurrentLevel = level
                        };
                        break;
                    case TutorialType.Groups:
                        _tutorialBehaviour = new GroupsTutorial()
                        {
                            CurrentLevel = level
                        };
                        break;
                }
            }

            if (_tutorialBehaviour != null)
            {
                yield return _tutorialBehaviour.StartTutorial(hand, viewer, text, line);
            }

            yield return null;
        }
    }
}
