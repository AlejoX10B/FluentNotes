using FluentNotes.Models;
using System.Collections.Generic;

namespace FluentNotes.Services
{
    public interface IDataService
    {
        // Notebooks
        List<Notebook> GetNotebooks();
        Notebook? GetNotebook(string id);
        void CreateNotebook(Notebook notebook);
        void UpdateNotebook(Notebook notebook);
        void DeleteNotebook(string id);

        // Notes
        List<Note> GetNotesByNotebook(string notebookId);
        List<Note> GetAllNotes();
        Note? GetNote(string id);
        void CreateNote(Note note);
        void UpdateNote(Note note);
        void DeleteNote(string id);
    }
}
