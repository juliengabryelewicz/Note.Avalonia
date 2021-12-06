using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using DynamicData;
using Mapster;
using ReactiveUI;

using Note.DependencyInjection;
using Note.FormDto;
using Note.Services;
using Note.Entities;

using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using MessageBox.Avalonia.Models;
using MessageBoxAvaloniaEnums = MessageBox.Avalonia.Enums;

namespace Note.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<NoteFormDto> Notes { get; private set; } = new();

        private string _NoteTitle;
        public string NoteTitle
        {
            get => _NoteTitle;
            set => this.RaiseAndSetIfChanged(ref _NoteTitle, value);
        }

        private string _NoteContent;
        public string NoteContent
        {
            get => _NoteContent;
            set => this.RaiseAndSetIfChanged(ref _NoteContent, value);
        }

        private long _NoteId;
        public long NoteId
        {
            get => _NoteId;
            set => this.RaiseAndSetIfChanged(ref _NoteId, value);
        }

        private INoteService NoteService { get; init; }

        public MainWindowViewModel(INoteService noteService)
        {
            this.NoteService = noteService;

            this.NoteId = 0;
            this.NoteTitle="";
            this.NoteContent="";

            this.Notes.AddRange(NoteService.ReadAsync().GetAwaiter().GetResult()
                .Adapt<IEnumerable<NoteFormDto>>());
        }


        public void OnCreateButtonClicked()
        {
            this.NoteTitle = String.Empty;
            this.NoteContent = String.Empty;
            this.NoteId = 0;
        }

        public void OnUpdateButtonClicked()
        {
            if(this.NoteTitle==String.Empty){
                var messageBoxTitleRequired = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow("Warning", "Title is required");
                messageBoxTitleRequired.Show();
            }else{
                if(this.NoteId==0){
                    NoteFormDto note = NoteService
                        .CreateAsync(NoteTitle, NoteContent)
                        .GetAwaiter().GetResult().Adapt<NoteFormDto>();

                    this.Notes.Insert(0, note);
                    this.NoteId = note.NoteEntityId;
                }else{

                    NoteEntity noteEntity = NoteService
                        .ReadOneAsync(NoteId)
                        .GetAwaiter().GetResult();

                    noteEntity.Title = NoteTitle;
                    noteEntity.Content = NoteContent;

                    NoteFormDto note = NoteService
                        .UpdateAsync(noteEntity)
                        .GetAwaiter().GetResult().Adapt<NoteFormDto>();

                    this.cleanNoteList();
                }
            }
        }

        public void OnReadButtonClicked(long note)
        {
            NoteFormDto Note = NoteService
                .ReadOneAsync(note)
                .GetAwaiter().GetResult().Adapt<NoteFormDto>();

            this.NoteId = Note.NoteEntityId;
            this.NoteTitle = Note.Title;
            this.NoteContent = Note.Content;
        }

        public async void OnDeleteButtonClicked()
        {
            var messageBoxDeleteNote = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxCustomWindow(
                new MessageBoxCustomParams
                {
                    ContentTitle = "Warning", ContentMessage = "Are you sure you want to delete this note?",
                    Icon = MessageBoxAvaloniaEnums.Icon.Error, WindowIcon = null,
                    ButtonDefinitions = new[]
                    {
                        new ButtonDefinition { Name = "No", IsCancel = true },
                        new ButtonDefinition { Name = "Yes", Type = ButtonType.Colored, IsDefault = true }
                    },
                });
            var buttonResult = await messageBoxDeleteNote.Show();

            if(buttonResult=="No"){
                return;
            }

            NoteFormDto Note = NoteService
                .ReadOneAsync(this.NoteId)
                .GetAwaiter().GetResult().Adapt<NoteFormDto>();
          
            NoteService.DeleteAsync(this.NoteId);

            this.cleanNoteList();

            this.NoteId = 0;
            this.NoteTitle = String.Empty;
            this.NoteContent = String.Empty;
        }

        private void cleanNoteList(){
            this.Notes.Clear();
            this.Notes.AddRange(NoteService.ReadAsync().GetAwaiter().GetResult().Adapt<IEnumerable<NoteFormDto>>());
        }

    }

}