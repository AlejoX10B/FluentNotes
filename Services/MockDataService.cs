using FluentNotes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentNotes.Services
{
    public class MockDataService : IDataService
    {
        private readonly List<Notebook> _notebooks;
        private readonly List<Note> _notes;

        public MockDataService()
        {
            _notebooks = InitializeNotebooks();
            _notes = InitializeNotes();
        }

        private List<Notebook> InitializeNotebooks()
        {
            return new List<Notebook>
            {
                new Notebook("Trabajo", "Notas relacionadas con proyectos laborales", Color.Blue),
                new Notebook("Personal", "Notas personales y recordatorios", Color.Green),
                new Notebook("Estudios", "Apuntes y material de estudio", Color.Orange),
                new Notebook("Ideas", "Lluvia de ideas y conceptos", Color.Purple),
                new Notebook("Proyectos", "Desarrollo y documentación técnica", Color.Accent)
            };
        }

        private List<Note> InitializeNotes()
        {
            var workNotebook = _notebooks.First(n => n.Name == "Trabajo");
            var personalNotebook = _notebooks.First(n => n.Name == "Personal");
            var studyNotebook = _notebooks.First(n => n.Name == "Estudios");
            var projectsNotebook = _notebooks.First(n => n.Name == "Proyectos");

            return new List<Note>
            {
                new Note(workNotebook.Id, "Reunión con el cliente", "Detalles de la reunión y acciones a seguir", FileType.Text),
                new Note(workNotebook.Id, "Planificación del proyecto", "Cronograma y tareas asignadas", FileType.Text),
                new Note(personalNotebook.Id, "Lista de compras", "Productos necesarios para la semana", FileType.Text),
                new Note(personalNotebook.Id, "Recordatorio de cumpleaños", "Comprar regalo para el cumpleaños de Juan", FileType.Text),
                new Note(studyNotebook.Id, "Apuntes de matemáticas", "Conceptos clave y ejercicios resueltos", FileType.Text),
                new Note(studyNotebook.Id, "Resumen de historia", "Eventos importantes del siglo XX", FileType.Text),
                new Note(projectsNotebook.Id, "Documentación técnica del API", "Especificaciones y endpoints del API", FileType.Text),
                new Note(projectsNotebook.Id, "Plan de pruebas del software", "Estrategia y casos de prueba definidos", FileType.Text)
            };
        }


        // Implementación de Notebooks

        public List<Notebook> GetNotebooks() => _notebooks;

        public Notebook? GetNotebook(string id) => _notebooks.FirstOrDefault(n => n.Id == id);

        public void CreateNotebook(Notebook notebook)
        {
            if (string.IsNullOrEmpty(notebook.Id))
                notebook.Id = Guid.NewGuid().ToString();

            notebook.CreatedAt = DateTime.Now;
            notebook.UpdatedAt = DateTime.Now;
            _notebooks.Add(notebook);
        }

        public void UpdateNotebook(Notebook notebook)
        {
            var existing = GetNotebook(notebook.Id);
            if (existing != null)
            {
                existing.Name = notebook.Name;
                existing.Description = notebook.Description;
                existing.Color = notebook.Color;
                existing.UpdatedAt = DateTime.Now;
            }
        }

        public void DeleteNotebook(string id)
        {
            var notebook = GetNotebook(id);
            if (notebook != null)
            {
                _notebooks.Remove(notebook);

                var notesToRemove = _notes.Where(n => n.NotebookId == id).ToList();
                foreach (var note in notesToRemove)
                {
                    _notes.Remove(note);
                }
            }
        }


        // Implementación de Notes
        public List<Note> GetNotesByNotebook(string notebookId) =>
            _notes.Where(n => n.NotebookId == notebookId).ToList();

        public List<Note> GetAllNotes() => _notes;

        public Note? GetNote(string id) => _notes.FirstOrDefault(n => n.Id == id);

        public void CreateNote(Note note)
        {
            if (string.IsNullOrEmpty(note.Id))
                note.Id = Guid.NewGuid().ToString();

            note.CreatedAt = DateTime.Now;
            note.UpdatedAt = DateTime.Now;
            _notes.Add(note);
        }

        public void UpdateNote(Note note)
        {
            var existing = GetNote(note.Id);
            if (existing != null)
            {
                existing.Title = note.Title;
                existing.Content = note.Content;
                existing.FileType = note.FileType;
                existing.UpdatedAt = DateTime.Now;
            }
        }

        public void DeleteNote(string id)
        {
            var note = GetNote(id);
            if (note != null)
            {
                _notes.Remove(note);
            }
        }

    }
}
