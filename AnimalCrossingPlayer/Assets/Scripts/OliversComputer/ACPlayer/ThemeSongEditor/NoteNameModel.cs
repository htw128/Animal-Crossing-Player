using System;
using UnityEngine;
using UnityEngine.UI;

namespace OliversComputer.ACPlayer.ThemeSongEditor
{
    public class NoteNameModel : MonoBehaviour
    {
        private Text m_noteName;
        private Slider m_slider;

        private void Awake()
        {
            m_slider = GetComponent<Slider>();
            m_noteName = transform.Find("NoteName").GetComponent<Text>();
        }

        public void OnSliderValueChanged()
        {
            NoteList note = (NoteList)(int)m_slider.value;
            m_noteName.text = note.ToString();
        }
    }
}
