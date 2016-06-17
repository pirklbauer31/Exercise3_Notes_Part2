using System;
using System.Collections.Generic;
using System.Linq;
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

        public void UpdateNote(Note noteToUpdate)
        {
            //Delete note
            //var query = allNotes.FirstOrDefault(n => String.Equals(n.NoteContent, note.NoteContent) && n.NoteDateTime == note.NoteDateTime);
            //var query =
            //    allNotes.Where(n => String.Equals(n.NoteContent, noteContent)).First(n => n.NoteDateTime.Ticks == noteDateTime.Ticks);
            var query = allNotes.First(n => String.Equals(n.NoteId, noteToUpdate.NoteId));

            allNotes.Remove(query);

            //Add updated note
            allNotes.Add(noteToUpdate);
        }
    }
}