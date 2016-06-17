using Exercise2_Notes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise2_Notes.Services
{
    public interface IDataService
    {
        IEnumerable<Note> GetAllNotes();

        void AddNote(Note note);

        void SaveNote(Note note);

        void DeleteNote(Note note);

        void UpdateNote(Note note);
    }
}
