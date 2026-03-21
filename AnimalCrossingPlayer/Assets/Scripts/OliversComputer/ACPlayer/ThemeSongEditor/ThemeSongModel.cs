using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace OliversComputer.ACPlayer.ThemeSongEditor
{
    public class ThemeSongModel
    {
        private readonly GameObject m_musicGameObject;
        
        internal readonly List<int> m_noteValues = new(16);

        public ThemeSongModel(GameObject gameObject)
        {
            m_musicGameObject = gameObject;
            
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

        internal IEnumerator PlayThemeSong(List<int> score, Action onFinished, bool isPreview)
        {
            
            double interval = isPreview ? 4.0 / 15.0 :10.0 / 11.0;
            uint eventID = isPreview ? 1929178478u : 1945722105u;
            double startTime = Time.timeAsDouble;
            int noteIndex = 0;
            int activeMidiNote = -1;

            foreach (int note in score)
            {
                double nextTime = startTime + noteIndex * interval;

                while (Time.timeAsDouble < nextTime)
                {
                    yield return null;
                }

                switch (note)
                {
                    case (int)NoteNames.Sustain:
                        //Do Nothing
                        break;
                    
                    case (int)NoteNames.Rest:
                        if (activeMidiNote >= 0)
                        {
                            SendMidiNote(activeMidiNote, false, eventID);
                            activeMidiNote = -1;
                        }
                        break;
                    
                    case (int)NoteNames.Random:
                        if (activeMidiNote >= 0)
                        {
                            SendMidiNote(activeMidiNote, false, eventID);
                        }
                        int randomNote = GetMidiNote(Random.Range(2, 14));
                        SendMidiNote(randomNote, true, eventID);
                        activeMidiNote = randomNote;
                        break;
                    
                    default:
                    {
                        int midiNote = GetMidiNote(note);
                        if (isPreview) midiNote += 12;
                        if (activeMidiNote >= 0)
                        {
                            SendMidiNote(activeMidiNote, false, eventID);
                        }

                        SendMidiNote(midiNote, true, eventID);
                        activeMidiNote = midiNote;
                        break;
                    }
                }
                
                // 口风琴只演奏半拍
                if (isPreview && activeMidiNote >= 0
                              && note != (int)NoteNames.Sustain
                              && note != (int)NoteNames.Rest)
                {
                    double noteOffTime = nextTime + interval * 0.5;
                    while (Time.timeAsDouble < noteOffTime)
                    {
                        yield return null;
                    }
                    SendMidiNote(activeMidiNote, false, eventID);
                    activeMidiNote = -1;
                }

                noteIndex++;

            }
            
            if (activeMidiNote >= 0)
            {
                SendMidiNote(activeMidiNote, false, eventID);
            }

            onFinished?.Invoke();
        }

        private int GetMidiNote(int note)
        {
            return note switch
            {
                2 => 55,
                3 => 57,
                4 => 59,
                5 => 60,
                6 => 62,
                7 => 64,
                8 => 65,
                9 => 67,
                10 => 69,
                11 => 71,
                12 => 72,
                13 => 74,
                14 => 76,
                _ => 69
            };
        }

        public void PlaySingleNote(byte midiNote, byte velocity = 127)
        {
            // 序列化一个 NOTE_ON + NOTE_OFF 消息数组
            AkMIDIPostArray posts = new AkMIDIPostArray(2);

            // NOTE_ON
            AkMIDIPost onPost = new AkMIDIPost();
            onPost.midiEvent.byType = AkMIDIEventTypes.NOTE_ON;
            onPost.midiEvent.byChan = 0;
            onPost.midiEvent.byOnOffNote = midiNote;
            onPost.midiEvent.byVelocity = 127; // velocity 0–127
            onPost.uOffset = 0; // 立即执行
            posts[0] = onPost;

            // NOTE_OFF
            AkMIDIPost offPost = new AkMIDIPost();
            offPost.midiEvent.byType = AkMIDIEventTypes.NOTE_OFF;
            offPost.midiEvent.byChan = 0;
            offPost.midiEvent.byOnOffNote = midiNote;
            offPost.midiEvent.byVelocity = 0;
            offPost.uOffset = 12800; // 立即紧跟
            posts[1] = offPost;

            // 发送到指定 Wwise Event
            // 这会依赖你配置 Event（比如包含一个可响应 MIDI 的 Synth）
            AkUnitySoundEngine.PostMIDIOnEvent(1929178478, m_musicGameObject, posts, (ushort)posts.Count());
        }

        internal void SendMidiNote(int midiNote, bool mode, uint eventId)
        {
            AkMIDIPostArray posts = new AkMIDIPostArray(1);
            AkMIDIPost onPost = new AkMIDIPost();
            onPost.midiEvent.byType = mode ? AkMIDIEventTypes.NOTE_ON : AkMIDIEventTypes.NOTE_OFF;
            onPost.midiEvent.byChan = 0;
            onPost.midiEvent.byOnOffNote = (byte)midiNote;
            onPost.midiEvent.byVelocity = 127;
            onPost.uOffset = 0;
            posts[0] = onPost;
            AkUnitySoundEngine.PostMIDIOnEvent(eventId, m_musicGameObject, posts, (ushort)posts.Count());
            Debug.Log($"Sending Note {midiNote} to {mode}");
        }
    }
}
