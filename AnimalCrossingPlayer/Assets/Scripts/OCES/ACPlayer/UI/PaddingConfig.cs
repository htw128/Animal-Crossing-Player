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

        private void Awake()
        {
            m_layoutElement = GetComponent<LayoutElement>();

#if UNITY_ANDROID || UNITY_IOS
            m_leftPadding = MobilePadding;
#endif

#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
            m_leftPadding = StandalonePadding;
            
#endif
        }

        private void Update()
        {
            m_layoutElement.preferredWidth = m_leftPadding;
        }
    }
}
