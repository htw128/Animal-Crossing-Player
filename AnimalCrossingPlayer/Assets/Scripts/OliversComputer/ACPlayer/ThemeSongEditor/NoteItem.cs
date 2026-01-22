using System;
using UnityEngine;
using UnityEngine.UI;

namespace OliversComputer.ACPlayer.ThemeSongEditor
{
    public class NoteItem : MonoBehaviour
    {
        private Text m_noteName;
        private Slider m_slider;
        private int m_index;
        
        public Action<int, int> OnNoteValueChanged;
        
        private void Awake()
        {
            m_slider = GetComponent<Slider>();
            m_noteName = transform.Find("NoteName").GetComponent<Text>();
            
            m_slider.onValueChanged.AddListener(HandleSliderChanged);
        }

        public void Bind(int index, int initialValue)
        {
            m_index = index;
            m_slider.value = initialValue;
        }

        private void HandleSliderChanged(float value)
        {
            int intValue = (int)value;
            m_slider.value = intValue;

            if (m_noteName)
            {
                m_noteName.text = GetNoteDisplayName((NoteNames)intValue);
            }
            
            OnNoteValueChanged?.Invoke(m_index, intValue);
        }

        private void OnDestroy()
        {
            if (m_slider)
            {
                m_slider.onValueChanged.RemoveListener(HandleSliderChanged);
            }
        }

        private string GetNoteDisplayName(NoteNames note)
        {
            return note switch
            {
                NoteNames.Rest => "休止",
                NoteNames.Sustain => "延音",
                NoteNames.SolLow => "Sol",
                NoteNames.LaLow => "La",
                NoteNames.SiLow => "Si",
                NoteNames.Do => "Do",
                NoteNames.Re => "Re",
                NoteNames.Mi => "Mi",
                NoteNames.Fa => "Fa",
                NoteNames.Sol => "Sol",
                NoteNames.La => "La",
                NoteNames.Si => "Si",
                NoteNames.DoHigh => "Do",
                NoteNames.ReHigh => "Re",
                NoteNames.MiHigh => "Mi",
                NoteNames.Random => "随机",
                _ => string.Empty
            };
        }
    }
    
    
}
