using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using eBuyListApplication.Model;
using eBuyListApplication.Resources;

namespace eBuyListApplication.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> Items { get; private set; }

        private string _sampleProperty = "Sample Runtime Property Value";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

        /// <summary>
        /// Sample property that returns a localized string
        /// </summary>
        public string LocalizedSampleProperty
        {
            get
            {
                return AppResources.SampleProperty;
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {

            //EBuyListsManager manager = new EBuyListsManager();

            //var lists = manager.GetAllLists();
            //var count = lists.Count;
            // Sample data; replace with real data
            this.Items.Add(new ItemViewModel() { ID = "0", LineOne = "Zakupy w drogerii", LineTwo = "3", LineThree = "4" });
            this.Items.Add(new ItemViewModel() { ID = "1", LineOne = "Sklep zielarski", LineTwo = "5", LineThree = "1" });
            this.Items.Add(new ItemViewModel() { ID = "2", LineOne = "Castorama", LineTwo = "12", LineThree = "14" });
            this.Items.Add(new ItemViewModel() { ID = "3", LineOne = "Plan na tydzień", LineTwo = "2", LineThree = "8" });
            this.Items.Add(new ItemViewModel() { ID = "4", LineOne = "Zakupy - wtorek", LineTwo = "0", LineThree = "6" });

            this.IsDataLoaded = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}