using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using eBuyListApplication.Model;
using Microsoft.Phone.Tasks;

namespace eBuyListApplication
{
    public partial class MainPage
    {
        public static EBuyListsManager Manager = new EBuyListsManager();

        // Constructor
        public MainPage()
        {
            InitializeComponent();          
            MainLongListSelector.DataContext = Manager.GetAllLists();           
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //if (MainLongListSelector.DataContext == null)
            //{
            //    MainLongListSelector.DataContext = _manager.GetAllLists();
            //}

            MainLongListSelector.DataContext = null;
            MainLongListSelector.DataContext = Manager.GetAllLists();

            //TODO Załadowanie danych jak przechodzi się na tę stronę z innej
        }

        // Handle selection changed on LongListSelector
        private void MainLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected item is null (no selection) do nothing
            if (MainLongListSelector.SelectedItem == null)
                return;

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/DetailsPage.xaml?selectedItem=" + (MainLongListSelector.SelectedItem as EBuyList).Id, UriKind.Relative));


            // Reset selected item to null (no selection)
            MainLongListSelector.SelectedItem = null;
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}

        private void AddBarIconButton_OnClick(object sender, EventArgs e)
        {
            AddNewListTextBox.Height = 100;
            AddNewListTextBox.BorderBrush = new SolidColorBrush(Colors.Black);
            AddNewListTextBox.FontSize = 20;
            AddNewListTextBox.Visibility = Visibility.Visible;


            AddNewListButton.Height = 70;
            AddNewListButton.Content = "Zapisz listę";
            AddNewListButton.Background = new SolidColorBrush(Colors.DarkGray);
            AddNewListButton.Visibility = Visibility.Visible;

            AddNewListButton.Click += addNewListButton_Click;      
        }

        void addNewListButton_Click(object sender, RoutedEventArgs e)
        {
            if (Manager.GetAllLists().Count > 5)
            {
                MessageBox.Show("Możesz maksymalnie dodać 5 list"); 
            }

            else
            {
                if (AddNewListTextBox.Text != "")
                {
                    var newListIndex = Manager.AddNewList(AddNewListTextBox.Text);

                    NavigationService.Navigate(new Uri("/DetailsPage.xaml?selectedItem=" + newListIndex, UriKind.Relative));

                    AddNewListTextBox.Text = "";

                    AddNewListTextBox.Visibility = Visibility.Collapsed;
                    AddNewListButton.Visibility = Visibility.Collapsed;
                }
                else
                {
                    return;
                }
            }
        }

        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            var item = (sender as MenuItem).DataContext;
            int listToRemoveId = (item as EBuyList).Id;

            Manager.RemoveListByIndex(listToRemoveId);
            
            //NOTE:Wbrew pozorom to ma sens:) 
            MainLongListSelector.DataContext = null;
            MainLongListSelector.DataContext = Manager.GetAllLists();
        }

        private void Share_OnClickShare_OnClick(object sender, RoutedEventArgs e)
        {
            var sms = new SmsComposeTask();
            var myListToSend = (sender as MenuItem).DataContext;

            var myProductsOnList = (myListToSend as EBuyList).Products;

            var smsListBody = myProductsOnList.Aggregate("Twoja lista skarbie:" + "\n", (current, item) => current + (item + "\n"));

            sms.Body = smsListBody;
            sms.Show();
        }
    }
}