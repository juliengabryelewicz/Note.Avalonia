using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Note.AppCtx;
using Note.DbContexts;
using Note.Entities;
using Note.Repositories;

namespace Note.Services
{
    public class NoteService : INoteService
    {
        private NoteDbContext DbContext { get; init; }

        private INoteRepository NoteRepository { get; init; }

        public NoteService(
            NoteDbContext dbContext,
            INoteRepository noteRepository)
        {
            DbContext = dbContext;
            NoteRepository = noteRepository;
        }

        async public Task<NoteEntity> CreateAsync(string title, string content)
        {
            NoteEntity NoteEntity = new NoteEntity
            {
                Title = title,
                Content = content
            };

            await NoteRepository.CreateAsync(NoteEntity);

            return NoteEntity;
        }

        async public Task<List<NoteEntity>> ReadAsync()
        {
            return await NoteRepository.ReadAsync();
        }

        async public Task<NoteEntity> ReadOneAsync(long entityNoteId)
        {
            return await NoteRepository.ReadOneAsync(entityNoteId);
        }

        async public Task<NoteEntity> UpdateAsync(NoteEntity noteEntity)
        {

            await NoteRepository.UpdateAsync(noteEntity);

            return noteEntity;
        }

        async public Task<bool> DeleteAsync(long note)
        {
            Console.Write(note);
            return await NoteRepository.DeleteAsync(note);
        }
    }
}