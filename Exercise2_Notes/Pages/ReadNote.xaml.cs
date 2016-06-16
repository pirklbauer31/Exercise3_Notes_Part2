using Exercise2_Notes.ViewModels;
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
using Exercise2_Notes.Models;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Exercise2_Notes.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ReadNote : Page
    {
        public ReadNote()
        {
            this.InitializeComponent();

            /*
            var notes = ViewModel.Notes;
            for (int i = 1; i <= ViewModel.MaxNotes && i <= notes.Count; i++)
            {
                ListViewNotes.Items.Add(notes[notes.Count-i]);
            }
            */
        }

        public MainViewModel ViewModel => DataContext as MainViewModel;
    }
}
