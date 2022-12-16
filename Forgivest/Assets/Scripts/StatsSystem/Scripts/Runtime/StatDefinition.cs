﻿using Core;
using UnityEngine;

namespace StatSystem
{
    [CreateAssetMenu(fileName = "StatDefinition", menuName = "StatSystem/StatDefinition", order = 0)]
    public class StatDefinition : ScriptableObject
    {
        [SerializeField] private int m_BaseValue;
        [SerializeField] private int m_Cap = -1;
        
#if UNITY_EDITOR   

        [SerializeField] private NodeGraph m_Formula;
        public NodeGraph Formula => m_Formula;
#endif        
        public int BaseValue => m_BaseValue;
        public int Cap => m_Cap;

    }
}