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
using Windows.Phone.Speech.Recognition;
using System.ComponentModel;

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

            PanoramaMain.DataContext = MainPage.Manager.GetListByIndex(SelectedListId());

            DetailsLongListSelector.DataContext = MainPage.Manager.GetListByIndex(SelectedListId()).Products;

            TitlePanel.DataContext = MainPage.Manager.GetListByIndex(SelectedListId()).Name;

            //CategoriesProductsLongListSelector.DataContext = MainPage.Manager.GetListByIndex(SelectedListId()).Products;

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


        private void AddProductAppBarIconButton_Click(object sender, EventArgs e)
        { 
            SearchAutoCompleteBox.Text = "";

            SearchAutoCompleteBox.Height = 80;
            SearchAutoCompleteBox.BorderBrush = new SolidColorBrush(Colors.Black);
            SearchAutoCompleteBox.FontSize = 30;
         
            SearchAutoCompleteBox.Focus();
            TitlePanel.Opacity = 0.15;
            SearchAutoCompleteBox.Visibility = Visibility.Visible;
            AddBarButton().IsEnabled = false;
            ContentPanel.Opacity = 0.15;


        }

        private void ConfirmAddingProductAppBarIconButton_Click(object sender, EventArgs e)
        {

            MainPage.Manager.AddNewProductToList(SelectedListId(), SearchAutoCompleteBox.Text);


            DetailsLongListSelector.DataContext = MainPage.Manager.GetListByIndex(SelectedListId()).Products;
            SearchAutoCompleteBox.Visibility = Visibility.Collapsed;
            TitlePanel.Visibility = System.Windows.Visibility.Visible;
            DetailsLongListSelector.Focus();
            ConfirmBarButton().IsEnabled = false;
            AddBarButton().IsEnabled = true;
            ContentPanel.Opacity = 1;
            TitlePanel.Opacity = 1;

            DetailsLongListSelector.DataContext = null;
            DetailsLongListSelector.DataContext = MainPage.Manager.GetListByIndex(SelectedListId()).Products;
        }

        private void SearchAutoCompleteBox_TextChanged(object sender, RoutedEventArgs e)
        {
            if (SearchAutoCompleteBox.SelectedItem != null)
            {
                return;            
            }

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

        //TODO
        private void PanoramaMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        //TODO
        private void SpeechToTextButton_Click(object sender, RoutedEventArgs e)
        {

            //TODO Speech recognition
            //try
            //{

            //    SpeechRecognizer reco = new SpeechRecognizer();



            //    reco.Settings.InitialSilenceTimeout = TimeSpan.FromSeconds(1);
            //    reco.Settings.EndSilenceTimeout = TimeSpan.FromSeconds(5);

            //    SpeechRecognitionResult recoResult = await reco.RecognizeAsync();

            //    SearchAutoCompleteBox.Text = recoResult.Text;
            //}

            //catch (System.Threading.Tasks.TaskCanceledException)
            //{


            //}

            //catch (Exception err)
            //{
            //    const int privacyPolicyHResult = unchecked((int)0x80045509);

            //    if (err.HResult == privacyPolicyHResult)
            //    {
            //        MessageBox.Show("To run this sample, you must first accept the speech privacy policy. To do so, navigate to Settings -> speech on your phone and check 'Enable Speech Recognition Service' ");
            //    }
            //    else
            //    {
            //        MessageBox.Show(String.Format("An error occurred: {0}", err.Message));
            //    } 
            //}
        }

        //Wynik błędu w kontrolce Auto Complete Box
        private void SearchAutoCompleteBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }



        private void ClearBoughtApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            MainPage.Manager.GetListByIndex(SelectedListId()).RemoveCheckedProducts();

            DetailsLongListSelector.DataContext = null;
            DetailsLongListSelector.DataContext = MainPage.Manager.GetListByIndex(SelectedListId()).Products;
        }

        //TODO
        private void SortApplicationBarMenuItem_Click(object sender, EventArgs e)
        {


        }



        #region HelperMethods

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

        #endregion


        //BackButton override
        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            // put any code you like here
            if (SearchAutoCompleteBox.Visibility != System.Windows.Visibility.Visible)
            {

            }
            else
            {
                SearchAutoCompleteBox.Visibility = System.Windows.Visibility.Collapsed;
                TitlePanel.Opacity = 1;
                ContentPanel.Opacity = 1;
                AddBarButton().IsEnabled = true;
                e.Cancel = true;
            }
        }



    }

}