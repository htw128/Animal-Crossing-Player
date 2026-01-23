using System.Collections.Generic;

namespace OliversComputer.ACPlayer.ThemeSongEditor
{
    public class ThemeSongModel
    {
        internal List<int> m_noteValues = new(16);

        public void Initialize()
        {
            if (GlobalService.Instance.CacheReader.Exists("ThemeSong"))
            {
                m_noteValues = GlobalService.Instance.CacheReader.Read<List<int>>("ThemeSong");
            }
            else
            {
                for (int i = 0; i < 16; i++)
                {
                    m_noteValues.Add(0);
                }
            }
        }

        public void UpdateNote(int index, int newNoteValue)
        {
            if (index >= 0 && index < m_noteValues.Count)
            {
                m_noteValues[index] = newNoteValue;
            }
        }

        public void SaveThemeSong()
        {
            GlobalService.Instance.CacheWriter.Write("ThemeSong", m_noteValues).Commit();
        }
        
    }
}
