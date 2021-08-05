using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MadPot
{
    public enum FigureType
    {
        None,
        Line,
        Triangle,
        Rectangle
    }

    public class FigureVisualizer : MonoBehaviour
    {
        [SerializeField] private Image _figureImage = null;
        [SerializeField] private Sprite _lineSprite = null;
        [SerializeField] private Sprite _triangleSprite = null;
        [SerializeField] private Sprite _rectangleSprite = null;

        private FigureType _type = FigureType.None;

        public FigureType Type 
        {
            get => _type; 
            set
            {
                if (_type != value)
                {
                    _type = value;

                    switch (_type)
                    {
                        case FigureType.None:
                            gameObject.SetActive(false);
                            break;
                        case FigureType.Line:
                            _figureImage.sprite = _lineSprite;
                            break;
                        case FigureType.Triangle:
                            _figureImage.sprite = _triangleSprite;
                            break;
                        case FigureType.Rectangle:
                            _figureImage.sprite = _rectangleSprite;
                            break;
                    }
                }
            }
        }
    }
}
