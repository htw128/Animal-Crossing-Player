using System;
using UnityEngine;

namespace OliversComputer.ACPlayer.ThemeSongEditor
{
    public class PanelController : MonoBehaviour
    {
        private ThemeSongModel m_model;
        private PanelView m_view;

        private void Awake()
        {
            m_model = new ThemeSongModel();
            m_view = gameObject.GetComponent<PanelView>();
            
            m_model.Initialize();
            m_view.InitView(m_model.m_noteValues);
            
            m_view.OnAnyNoteChanged = (index, newValue) =>
            {
                m_model.UpdateNote(index, newValue);
            };
        }

        private void OnEnable()
        {
            m_view.SaveButton.onClick.AddListener(m_model.SaveThemeSong);
            m_view.PreviewButton.onClick.AddListener(PreviewThemeSong);
        }

        private void OnDestroy()
        {
            m_view.SaveButton.onClick.RemoveListener(m_model.SaveThemeSong);
            m_view.PreviewButton.onClick.RemoveListener(PreviewThemeSong);
        }

        private void PreviewThemeSong()
        {
            GlobalService.Instance.Music.PlayThemeSong(true);
        }

    }
}