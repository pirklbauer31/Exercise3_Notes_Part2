﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Exercise2_Notes.Models;
using Exercise2_Notes.Pages;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Exercise2_Notes.Services;

namespace Exercise2_Notes.ViewModels
{
    public class MainViewModel:ViewModelBase
    {
        private readonly NavigationService navigationService;
        private readonly IDataService dataService;
        private readonly IStorageService storageService;

        public MainViewModel(IDataService dataService, IStorageService storageService)
        {
           // Notes = new ObservableCollection<Note> { new Note("TestNote", DateTime.Now)};
            searchedNotes = new ObservableCollection<Note>();
          
            CurrentNote = new Note();
            
            AddNoteCommand = new RelayCommand(AddNote);
            MaxNotes = 5;
            NewSearchString = "";
            OrderAscending = false;
            UpdateNote = false;

            this.dataService = dataService;
            this.storageService = storageService;

            navigationService = new NavigationService();
            navigationService.Configure("CreateNotePage", typeof(CreateNote));
            navigationService.Configure("ReadNotePage", typeof(ReadNote));
            navigationService.Configure("SettingsPage", typeof(SettingsPage));
            navigationService.Configure("SearchPage", typeof(SearchNote));
        }

        public Note CurrentNote { get; set; }
        //public ObservableCollection<Note> Notes { get; set; }
        public string NewNoteContent { get; set; }
        public DateTime NewNoteDateTime { get; set; }
        public int MaxNotes { get; set; }
        public bool? OrderAscending { get; set; }
        public bool UpdateNote { get; set; }
        public Note UpdateNoteDummy { get; set; }

        public string NewSearchString { get; set; }
        public ObservableCollection<Note> searchedNotes { get; set; }

        public ObservableCollection<Note> SearchNotes
        {
            get
            {
                searchedNotes.Clear();

                var temp = dataService.GetAllNotes();
                
                 temp =
                    temp.Where(
                        n =>
                            (n.NoteContent.ToUpper().Contains(NewSearchString.ToUpper()) ||
                             NewSearchString == String.Empty));
                /*
                var temp =
                    Notes.Where(
                        n =>
                            (n.NoteContent.ToUpper().Contains(NewSearchString.ToUpper()) ||
                             NewSearchString == String.Empty));
                             */
                if (OrderAscending==true)
                    temp = temp.OrderBy(n => n.NoteDateTime).Take(MaxNotes);
                else
                    temp = temp.OrderByDescending(n => n.NoteDateTime).Take(MaxNotes);
                
                foreach (var n in temp)
                {
                    searchedNotes.Add(n);
                }
                return searchedNotes;
            }
        }

        public void AddNote()
        {

            if (UpdateNote)
            {
                //Notes.Add(new Note(NewNoteContent, NewNoteDateTime));
                //dataService.AddNote(new Note(NewNoteContent, NewNoteDateTime));
                dataService.UpdateNote(CurrentNote);
                
                //NewNoteContent = string.Empty;
                //NewNoteDateTime = DateTime.MinValue;

                UpdateNote = false;
                navigationService.GoBack();
            }

            else
            {
                //Notes.Add(new Note(NewNoteContent, DateTime.Now));
                //dataService.AddNote(new Note(NewNoteContent, DateTime.Now));
                CurrentNote.NoteDateTime = DateTime.Now;
                dataService.AddNote(CurrentNote);
                //NewNoteContent = string.Empty;
                //NewNoteDateTime = DateTime.MinValue;

            }
           
            CurrentNote = new Note();
        }

        public void DeleteNote(Note noteToDelete, ListView listView)
        {
            //Notes.Remove(noteToDelete);
            dataService.DeleteNote(noteToDelete);
            //listView.ItemsSource = Notes;
            listView.ItemsSource = SearchNotes;

        }

        public void EditNote(Note noteToEdit)
        {
            navigationService.NavigateTo("CreateNotePage", noteToEdit);
        }

        public RelayCommand AddNoteCommand { get; }

        public async void ShowPopupMenu(object sender, ItemClickEventArgs e)
        {
            var listView = (ListView)sender;

            PopupMenu popupMenu = new PopupMenu();
            popupMenu.Commands.Add(new UICommand("Edit Note", command => EditNote((Note)e.ClickedItem)));
            popupMenu.Commands.Add(new UICommand("Remove Note", command => DeleteNote((Note)e.ClickedItem, (ListView)e.OriginalSource)));

            
            await popupMenu.ShowAsync(listView.RenderTransformOrigin);
        }

        public void SavePersist()
        {
            storageService.Write("MaxNotes", MaxNotes);
            storageService.Write("OrderAscending", OrderAscending);

            storageService.Write(nameof(dataService), dataService.GetAllNotes());
        }

        public void LoadPersist()
        {
            MaxNotes = storageService.Read<int>("MaxNotes", 5);
            OrderAscending = storageService.Read<bool?>("OrderAscending", false);

            foreach (var note in storageService.Read<List<Note>>(nameof(dataService), null))
            {
                dataService.AddNote(note);
            }
        }

        public void NavigateToCreateNotePage()
        {
            navigationService.NavigateTo("CreateNotePage");
        }

        public void NavigateToReadNotePage()
        {
            navigationService.NavigateTo("ReadNotePage");
        }

        public void NavigateToSettingsPage()
        {
            navigationService.NavigateTo("SettingsPage");
        }

        public void NavigateToSearchPage()
        {
            navigationService.NavigateTo("SearchPage");
        }

        public void NavigateBack()
        {
            navigationService.GoBack();
        }

    }


}
