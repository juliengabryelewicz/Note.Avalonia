using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Note.Entities;

namespace Note.Services
{
    public interface INoteService
    {
        Task<NoteEntity> CreateAsync(string title, string content);

        Task<List<NoteEntity>> ReadAsync();

        Task<NoteEntity> ReadOneAsync(long entityNoteId);

        Task<NoteEntity> UpdateAsync(NoteEntity noteEntity);

        Task<bool> DeleteAsync(long note);
    }
}