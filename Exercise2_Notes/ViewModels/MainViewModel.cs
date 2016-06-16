using System;
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

        public MainViewModel()
        {
            //Notes = new ObservableCollection<Note>(NoteSaver.Notes);
            Notes = new ObservableCollection<Note> { new Note("TestNote", DateTime.Now)};
            searchedNotes = new ObservableCollection<Note>();
            AddNoteCommand = new RelayCommand(AddNote);
            MaxNotes = 5;
            NewSearchString = "";
            OrderAscending = false;
            UpdateNote = false;

            navigationService = new NavigationService();
            navigationService.Configure("CreateNotePage", typeof(CreateNote));
            navigationService.Configure("ReadNotePage", typeof(ReadNote));
            navigationService.Configure("SettingsPage", typeof(SettingsPage));
            navigationService.Configure("SearchPage", typeof(SearchNote));
        }

        public Note Note { get; set; }
        public ObservableCollection<Note> Notes { get; set; }
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
                dataService.SaveNote(new Note(NewNoteContent, NewNoteDateTime));
                NewNoteContent = string.Empty;
                NewNoteDateTime = DateTime.MinValue;

                UpdateNote = false;
                navigationService.GoBack();
            }

            else
            {
                //Notes.Add(new Note(NewNoteContent, DateTime.Now));
                dataService.SaveNote(new Note(NewNoteContent, DateTime.Now));
                NewNoteContent = string.Empty;
                NewNoteDateTime = DateTime.MinValue;
            }
           
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
