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
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            TitlePanel.DataContext = MainPage.Manager.GetListByIndex(SelectedListId());
            DetailsLongListSelector.DataContext = MainPage.Manager.GetListByIndex(SelectedListId()).Products;
            
            foreach(var item in DetailsLongListSelector.ItemsSource)
            {
                if ((item as ListProductItem).IsBought == true)
                {
                    
                }
                else
                {
 
                }
            }


        }

        private void ProductEdit_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ProductDelete_OnClick(object sender, RoutedEventArgs e)
        {

            var item = (sender as MenuItem).DataContext;
            var productToRemove = (item as ListProductItem);

            MainPage.Manager.RemoveProduct(SelectedListId(), productToRemove);

            //NOTE:Wbrew pozorom to ma sens:) 
            DetailsLongListSelector.DataContext = null;
            DetailsLongListSelector.DataContext = MainPage.Manager.GetListByIndex(SelectedListId()).Products;
        }

        private void ProductTextBlock_OnTap(object sender, GestureEventArgs e)
        {
            
            var item = (sender as TextBlock).DataContext;
            ListProductItem productToBuy = (item as ListProductItem);

            bool isBought = productToBuy.IsBought;

            if (isBought == true)
            {
                MainPage.Manager.ChangeProductState(SelectedListId(), productToBuy, false);              
            }

            else
            {
                MainPage.Manager.ChangeProductState(SelectedListId(), productToBuy, true);
            }

            DetailsLongListSelector.DataContext = null;
            DetailsLongListSelector.DataContext = MainPage.Manager.GetListByIndex(SelectedListId()).Products;
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

        private void AddProductAppBarIconButton_Click(object sender, EventArgs e)
        {
            SearchAutoCompleteBox.Text = "";

            SearchAutoCompleteBox.Height = 80;
            SearchAutoCompleteBox.BorderBrush = new SolidColorBrush(Colors.Black);
            SearchAutoCompleteBox.FontSize = 30;

            SearchAutoCompleteBox.Focus();
            SearchAutoCompleteBox.Visibility = Visibility.Visible;
            TitlePanel.Visibility = System.Windows.Visibility.Collapsed;
            AddBarButton().IsEnabled = false;
            ContentPanel.Opacity = 0.15;
        }

        private void ConfirmAddingProductAppBarIconButton_Click(object sender, EventArgs e)
        {

            if (SearchAutoCompleteBox.SelectedItem != null)
            {

                MainPage.Manager.AddNewProductToList(SelectedListId(), SearchAutoCompleteBox.SelectedItem.ToString());
            }

            else
            {
                MainPage.Manager.AddNewProductToList(SelectedListId(), SearchAutoCompleteBox.Text);

            }

            DetailsLongListSelector.DataContext = MainPage.Manager.GetListByIndex(SelectedListId()).Products;
            SearchAutoCompleteBox.Visibility = Visibility.Collapsed;
            TitlePanel.Visibility = System.Windows.Visibility.Visible;
            DetailsLongListSelector.Focus();
            ConfirmBarButton().IsEnabled = false;
            AddBarButton().IsEnabled = true;
            ContentPanel.Opacity = 1;
        }

        private void SearchAutoCompleteBox_TextChanged(object sender, RoutedEventArgs e)
        {
            SearchAutoCompleteBox.ItemsSource = Products.GetProductsByNamePattern(SearchAutoCompleteBox.Text);

            if (SearchAutoCompleteBox.Text != "")
            {
                ConfirmBarButton().IsEnabled = true;
            }

            else
            {
                ConfirmBarButton().IsEnabled = false;
            }


        }




        private int SelectedListId()
        {
            string selectedIndex;
            NavigationContext.QueryString.TryGetValue("selectedItem", out selectedIndex);
            int listId = int.Parse(selectedIndex);
            return listId;
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

    }

}