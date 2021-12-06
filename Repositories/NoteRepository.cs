using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Note.DbContexts;
using Note.Entities;

namespace Note.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private NoteDbContext DbContext { get; init; }

        public NoteRepository(NoteDbContext dbContext)
        {
            DbContext = dbContext;
        }

        async public Task<NoteEntity> CreateAsync(NoteEntity Note)
        {
            await DbContext.Notes.AddAsync(Note);

            await DbContext.SaveChangesAsync();

            return Note;
        }

        async public Task<List<NoteEntity>> ReadAsync()
        {
            return await DbContext.Notes
                .OrderByDescending(v => v.NoteEntityId)
                .ToListAsync();
        }

        async public Task<NoteEntity> ReadOneAsync(long NoteEntityId)
        {
            return await DbContext.Notes
                .Where(v => v.NoteEntityId == NoteEntityId)
                .SingleAsync();
        }

        async public Task<NoteEntity> UpdateAsync(NoteEntity Note)
        {
            DbContext.Notes.Attach(Note);
            DbContext.Entry(Note).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();

            return Note;
        }

        async public Task<bool> DeleteAsync(long id)
        {
            NoteEntity Note = await DbContext.Notes
                .SingleAsync(n => n.NoteEntityId == id);

            if (Note != null)
            {
                DbContext.Remove(Note);
                await DbContext.SaveChangesAsync();

                return true;
            } else {
                return false;
            }
        }
    }
}