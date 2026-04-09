using System;
using UnityEngine;
using UnityEngine.UI;

namespace OCES.ACPlayer.UI
{
    public class PaddingConfig : MonoBehaviour
    {
        public float MobilePadding;
        public float StandalonePadding;
        
        private float m_leftPadding;
        private LayoutElement m_layoutElement;
        private float m_currentPadding;

        private void Awake()
        {
            m_layoutElement = GetComponent<LayoutElement>();

#if UNITY_ANDROID || UNITY_IOS
            m_leftPadding = MobilePadding;
#endif

#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
            m_leftPadding = StandalonePadding;
            
#endif
            
            m_currentPadding = m_layoutElement.preferredWidth = m_leftPadding;
        }

        private void Update()
        {
            if(Mathf.Abs(m_currentPadding - m_leftPadding) > 0.01f)
                m_layoutElement.preferredWidth = m_leftPadding;
        }
    }
}
