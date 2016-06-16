using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Exercise2_Notes.ViewModels;
using Windows.UI.Core;
using Windows.UI.Popups;
using Exercise2_Notes.Models;
using GalaSoft.MvvmLight.Views;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Exercise2_Notes.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateNote : Page
    {
        public CreateNote()
        {
            this.InitializeComponent();
            txtDateTime.Text = DateTime.Now.ToString();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ((App)App.Current).OnBackRequested += OnBackRequested;

            base.OnNavigatedTo(e);

            if (e.Parameter != null)
            {
                cmdCreateNote.Content = "Edit";

                Note editNote = (Note) e.Parameter;
                //to store old note for cancelling 
                ViewModel.UpdateNoteDummy = (Note) e.Parameter;
                //ViewModel.Notes.Remove(editNote);
                //ViewModel

                ViewModel.NewNoteContent = editNote.NoteContent;
                ViewModel.NewNoteDateTime = editNote.NoteDateTime;
                ViewModel.UpdateNote = true;
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ((App)App.Current).OnBackRequested -= OnBackRequested;

            base.OnNavigatingFrom(e);
        }


        private async void OnBackRequested(object sender, BackRequestedEventArgs args)
        {
                if (txtNote.Text == String.Empty)
                {
                    //args.handled auf true means it's already handled, so it stops the page from going back
                    args.Handled = true;

                    var dialog = new MessageDialog("Cannot add empty notes!");
                    dialog.Commands.Add(new UICommand { Label = "Ok", Id = 0 });
                    var res = await dialog.ShowAsync();
                }

                else
                {
                    if (ViewModel.UpdateNote)
                        cmdCreateNote.Content = "Add";
                    HelpCreate();

                }

        }
        public MainViewModel ViewModel => DataContext as MainViewModel;


        private async void cmdCancelNote_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new MessageDialog("Cancel Note creation?");
            dialog.Title = "Cancel Note";
            dialog.Commands.Add(new UICommand { Label = "Ok", Id = 0 });
            dialog.Commands.Add(new UICommand { Label = "Cancel", Id = 1 });
            var res = await dialog.ShowAsync();

            if ((int)res.Id == 0)
            {
                if (ViewModel.UpdateNote)
                {
                    //to restore the old note when cancelling during update of note
                    ViewModel.Notes.Add(ViewModel.UpdateNoteDummy);
                    
                    cmdCreateNote.Content = "Add";
                    ViewModel.UpdateNote = false;
                }
                ViewModel.NewNoteContent = string.Empty;
                ViewModel.NewNoteDateTime = DateTime.MinValue;
                ViewModel.NavigateBack();
            }
        }

        private async void cmdCreateNote_Click(object sender, RoutedEventArgs e)
        {
            if (txtNote.Text == String.Empty)
            {
                var dialog = new MessageDialog("Cannot add empty notes!");
                dialog.Commands.Add(new UICommand { Label = "Ok", Id = 0 });
                var res = await dialog.ShowAsync();
            }

            else
            {
                ViewModel.AddNote();
            }
        }

        private void HelpCreate()
        {
            ViewModel.AddNote();
        }

    }
}
