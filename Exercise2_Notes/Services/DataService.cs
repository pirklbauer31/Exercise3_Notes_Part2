using System;
using System.Collections.Generic;
using Exercise2_Notes.Models;

namespace Exercise2_Notes.Services
{
    public class DataService : IDataService
    {
        private readonly List<Note> allNotes;

        public DataService()
        {
            allNotes = new List<Note>();
        }

        public IEnumerable<Note> GetAllNotes()
        {
            return allNotes;
        }

        public void AddNote(Note note)
        {
            allNotes.Add(note);
        }

        public void SaveNote(Note note)
        {
            note.NoteDateTime = DateTime.Now;
        }

        public void DeleteNote(Note note)
        {
            allNotes.Remove(note);
        }
    }
}