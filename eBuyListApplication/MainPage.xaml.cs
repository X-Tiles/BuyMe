using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using eBuyListApplication.Model;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Shell;

namespace eBuyListApplication
{
    public partial class MainPage
    {

        #region EventHandlers

        public EBuyList ChangeListName { get; set; }

        public static EBuyListsManager Manager = new EBuyListsManager();

        public MainPage()
        {
            InitializeComponent();          
            MainLongListSelector.DataContext = Manager.GetAllLists();           
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            MainLongListSelector.DataContext = null;
            MainLongListSelector.DataContext = Manager.GetAllLists();

        }

        private void MainLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainLongListSelector.SelectedItem == null)
                return;

            NavigationService.Navigate(new Uri("/DetailsPage.xaml?selectedItem=" + (MainLongListSelector.SelectedItem as EBuyList).Id, UriKind.Relative));

            MainLongListSelector.SelectedItem = null;
        }

        private void AddBarIconButton_OnClick(object sender, EventArgs e)
        {
            if (Manager.GetAllLists().Count >= 5)
            {
                MessageBox.Show("Możesz dodać maksymalnie 5 list");
                return;
            }

            else
            {
                AddTextBoxForAddingNewList();
                AddBarButton().IsEnabled = false;
            }

        }

        void ConfirmAddingListAppBarButton_Click(object sender, EventArgs e)
        {
            if (ChangeListName == null)
            {
                //(sender as ApplicationBarIconButton).IsEnabled = true;
                var newListIndex = Manager.AddNewList(AddNewListTextBox.Text);
                NavigationService.Navigate(new Uri("/DetailsPage.xaml?selectedItem=" + newListIndex, UriKind.Relative));
                AddNewListTextBox.Text = "";
                AddNewListTextBox.Visibility = Visibility.Collapsed;
                
                AddBarButton().IsEnabled = true;
            }

            else
            { 
                ChangeListName.Name = AddNewListTextBox.Text;
                MainLongListSelector.DataContext = null;
                MainLongListSelector.DataContext = Manager.GetAllLists();
                AddNewListTextBox.Text = "";
                AddNewListTextBox.Visibility = Visibility.Collapsed;
                AddBarButton().IsEnabled = true;
                ChangeListName = null;
            }

        }

        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            var list = (sender as MenuItem).DataContext;
            ChangeListName = (list as EBuyList);
            AddTextBoxForAddingNewList();
            AddBarButton().IsEnabled = false;

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


        private void AddNewListTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AddNewListTextBox.Text != "")
            {
                ConfirmBarButton().IsEnabled = true;
            }

            else 
            {
                ConfirmBarButton().IsEnabled = false;
            }
        }

        #endregion


        #region HelperMethods

        private void AddTextBoxForAddingNewList()
        {
            AddNewListTextBox.Height = 80;
            AddNewListTextBox.BorderBrush = new SolidColorBrush(Colors.Black);
            AddNewListTextBox.FontSize = 30;
            AddNewListTextBox.Visibility = Visibility.Visible;
            AddNewListTextBox.Focus();
        }

        private ApplicationBarIconButton AddBarButton()
        {
            ApplicationBarIconButton button = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
            return button;
        }

        private ApplicationBarIconButton ConfirmBarButton()
        {
            ApplicationBarIconButton button = (ApplicationBarIconButton)ApplicationBar.Buttons[1];
            return button;
        }

        #endregion
    }
}