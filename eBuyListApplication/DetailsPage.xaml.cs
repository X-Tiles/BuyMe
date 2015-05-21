using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using eBuyListApplication.Resources;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Automation;
using System.Windows.Data;
using System.Windows.Media;
using Windows.Phone.UI.Input;
using eBuyListApplication.Model;
using Microsoft.Xna.Framework.Content;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace eBuyListApplication
{
    public partial class DetailsPage : PhoneApplicationPage
    {
        // Constructor
        public DetailsPage()
        {
            InitializeComponent();


            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        // When page is navigated to set data context to selected item in list
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //if (DataContext == null)
            //{
            //    string selectedIndex = "";
            //    if (NavigationContext.QueryString.TryGetValue("selectedItem", out selectedIndex))
            //    {
            //        int index = int.Parse(selectedItem);
            //        TitlePanel.DataContext = manager.GetListByIndex(index);

            //    }
            //}

            string selectedIndex = "";

            NavigationContext.QueryString.TryGetValue("selectedItem", out selectedIndex);

            int index = int.Parse(selectedIndex);
            TitlePanel.DataContext = MainPage.Manager.GetListByIndex(index);

            DetailsLongListSelector.DataContext = MainPage.Manager.GetListByIndex(index).Products;

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

        private void AddProductApplicationBarIconButton_OnClick(object sender, EventArgs e)
        {
            AddNewProductTextBox.Text = "";

            AddNewProductTextBox.Height = 100;
            AddNewProductTextBox.BorderBrush = new SolidColorBrush(Colors.Black);
            AddNewProductTextBox.FontSize = 20;

            AddNewProductButton.Height = 70;
            AddNewProductButton.Content = "Dodaj produkt";
            AddNewProductButton.Background = new SolidColorBrush(Colors.DarkGray);

            AddNewProductTextBox.Visibility = Visibility.Visible;
            AddNewProductButton.Visibility = Visibility.Visible;

            //AddNewProductButton.Click += AddNewProductButton_Click;
        }

        void AddNewProductButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedIndex;
            NavigationContext.QueryString.TryGetValue("selectedItem", out selectedIndex);
            int listId = int.Parse(selectedIndex);

            MainPage.Manager.AddNewProductToList(listId, AddNewProductTextBox.Text);
            DetailsLongListSelector.DataContext = MainPage.Manager.GetListByIndex(listId).Products;

            AddNewProductTextBox.Visibility = Visibility.Collapsed;
            AddNewProductButton.Visibility = Visibility.Collapsed;
        }

        private void AddNewProductTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (AddNewProductTextBox.Text == "")
                return;
            
            SearchProductComboBox.Visibility = Visibility.Visible;
            //SearchProductComboBox.Height = 200;

            SearchProductComboBox.DataContext = Products.GetProductsByNamePattern(AddNewProductTextBox.Text);

        }

        private void ProductEdit_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ProductDelete_OnClick(object sender, RoutedEventArgs e)
        {

            var item = (sender as MenuItem).DataContext;

            var productToRemove = (item as ListProductItem);

            string selectedIndex;
            NavigationContext.QueryString.TryGetValue("selectedItem", out selectedIndex);
            int index = int.Parse(selectedIndex);

            MainPage.Manager.RemoveProduct(index, productToRemove);

            //NOTE:Wbrew pozorom to ma sens:) 
            DetailsLongListSelector.DataContext = null;
            DetailsLongListSelector.DataContext = MainPage.Manager.GetListByIndex(index).Products;
        }

        private void ProductTextBlock_OnTap(object sender, GestureEventArgs e)
        {
            
            var item = (sender as TextBlock).DataContext;
            ListProductItem productToBuy = (item as ListProductItem);

            string selectedIndex = "";
            NavigationContext.QueryString.TryGetValue("selectedItem", out selectedIndex);
            int index = int.Parse(selectedIndex);
            bool isBought = productToBuy.IsBought;

            if (isBought == true)
            {
                MainPage.Manager.ChangeProductState(index, productToBuy, false);
                
            }

            else
            {
                MainPage.Manager.ChangeProductState(index, productToBuy, true);
            }

            DetailsLongListSelector.DataContext = null;
            DetailsLongListSelector.DataContext = MainPage.Manager.GetListByIndex(index).Products;
        }

        private void SearchProductComboBox_OnTap(object sender, GestureEventArgs e)
        {
            //AddNewProductTextBox.Text = SearchProductComboBox.SelectedItem.ToString();

        }

        private void SearchProductComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            var product = SearchProductComboBox.SelectedValue as Product;
            if (product == null)
                return;

            SearchProductComboBox.Foreground = new SolidColorBrush(Colors.Green);

            string selectedIndex = "";
            NavigationContext.QueryString.TryGetValue("selectedItem", out selectedIndex);
            int index = int.Parse(selectedIndex);

            MainPage.Manager.AddNewProductToList(index,product);

            SearchProductComboBox.Visibility = System.Windows.Visibility.Collapsed;

            DetailsLongListSelector.DataContext = null;
            DetailsLongListSelector.DataContext = MainPage.Manager.GetListByIndex(index).Products;

            AddNewProductTextBox.Text = "";
            AddNewProductTextBox.Visibility = Visibility.Collapsed;
            AddNewProductButton.Visibility = Visibility.Collapsed;

        }

        private void IsBoughtToogle_OnLoaded(object sender, RoutedEventArgs e)
        {

            if ((sender as ToggleSwitch).IsChecked == true)
            {
                (sender as ToggleSwitch).Content = "Kupione";
            }
            else
            {
                (sender as ToggleSwitch).Content = "Do kupienia";
            }
        }

        private void IsBoughtToogle_OnChecked(object sender, RoutedEventArgs e)
        {
               (sender as ToggleSwitch).Content = "Kupione";

        }


        private void IsBoughtToogle_OnUnchecked(object sender, RoutedEventArgs e)
        {
            (sender as ToggleSwitch).Content = "Do kupienia";
        }
    }

}