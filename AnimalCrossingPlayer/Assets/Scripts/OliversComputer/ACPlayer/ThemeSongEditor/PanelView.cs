using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OliversComputer.ACPlayer.ThemeSongEditor
{
    public class PanelView : MonoBehaviour
    {
        [SerializeField] private GameObject m_notePrefab;

        private GameObject m_1stLine;
        private GameObject m_2ndLine;

        public Action<int, int> OnAnyNoteChanged;
        public Button SaveButton { get; private set; }
        public Button PreviewButton { get; private set; }

        private void Awake()
        {
            SaveButton = transform.Find("Save").GetComponent<Button>();
            PreviewButton = transform.Find("Play").GetComponent<Button>();
            m_1stLine = transform.Find("1Line").gameObject;
            m_2ndLine = transform.Find("2Line").gameObject;
        }

        public void InitView(List<int> noteValues)
        {
            ClearLines();

            for (int i = 0; i < noteValues.Count; i++)
            {
                Transform parent = (i < 8) ? m_1stLine.transform : m_2ndLine.transform;
                CreateNotes(i, noteValues[i], parent);
            }
        }

        private void ClearLines()
        {
            foreach (Transform child in m_1stLine.transform)
            {
                Destroy(child.gameObject);
            }
            
            foreach (Transform child in m_2ndLine.transform)
            {
                Destroy(child.gameObject);
            }
        }


        private void CreateNotes(int index, int value,Transform lineRoot)
        {
            GameObject noteGameObject = Instantiate(m_notePrefab, lineRoot);
            noteGameObject.name = $"Note_{index}";

            NoteItem item = noteGameObject.GetComponent<NoteItem>();
            if (!item) return;
            item.Bind(index, value);

            item.OnNoteValueChanged = (idx, val) =>
            {
                OnAnyNoteChanged?.Invoke(idx, val);
            };
        }
    }
}