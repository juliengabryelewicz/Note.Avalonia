using System.Collections.Generic;
using System.Threading.Tasks;

using Note.Entities;

namespace Note.Repositories
{
    public interface INoteRepository
    {
        Task<NoteEntity> CreateAsync(NoteEntity Note);
        Task<List<NoteEntity>> ReadAsync();
        Task<NoteEntity> ReadOneAsync(long entityNoteId);
        Task<NoteEntity> UpdateAsync(NoteEntity Note);
        Task<bool> DeleteAsync(long entityNoteId);
    }

}