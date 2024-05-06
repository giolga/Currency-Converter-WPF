using CurrencyConverter.Context;
using CurrencyConverter.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CurrencyConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            lblCurrency.Content = "Hello Kumi";
            BindCurrency();

            GetData();
        }


        private void BindCurrency()
        {
            DataTable dtCurrency = new DataTable();
            dtCurrency.Columns.Add("Text");
            dtCurrency.Columns.Add("Values");

            dtCurrency.Rows.Add("--SELECT--", 0);
            dtCurrency.Rows.Add("INR", 1);
            dtCurrency.Rows.Add("USD", 75);
            dtCurrency.Rows.Add("EUR", 85);
            dtCurrency.Rows.Add("SAR", 20);
            dtCurrency.Rows.Add("POUND", 5);
            dtCurrency.Rows.Add("DEM", 43);

            cmbFromCurrency.ItemsSource = dtCurrency.DefaultView;
            cmbFromCurrency.DisplayMemberPath = "Text";
            cmbFromCurrency.SelectedValuePath = "Values";
            cmbFromCurrency.SelectedIndex = 0;

            cmbToCurrency.ItemsSource = dtCurrency.DefaultView;
            cmbToCurrency.DisplayMemberPath = "Text";
            cmbToCurrency.SelectedValuePath = "Values";
            cmbToCurrency.SelectedIndex = 0;//--Select--


        }

        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            double ConvertedValue;

            if (txtCurrency.Text == null || txtCurrency.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter Currency", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                txtCurrency.Focus();
                return;
            }
            else if (cmbFromCurrency.SelectedValue == null || cmbFromCurrency.SelectedIndex == 0)
            {
                MessageBox.Show("Please Select Currency From", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                cmbFromCurrency.Focus();
                return;
            }
            else if (cmbToCurrency.SelectedValue == null || cmbToCurrency.SelectedIndex == 0)
            {
                MessageBox.Show("Please Select Currency To", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                cmbToCurrency.Focus();
                return;
            }

            if (cmbFromCurrency.Text == cmbToCurrency.Text)
            {
                ConvertedValue = double.Parse(txtCurrency.Text);

                lblCurrency.Content = cmbToCurrency.Text + " " + ConvertedValue.ToString("N3");
            }
            else
            {
                ConvertedValue = (double.Parse(cmbFromCurrency.SelectedValue.ToString()) * double.Parse(txtCurrency.Text)) / double.Parse(cmbToCurrency.SelectedValue.ToString());
                lblCurrency.Content = cmbToCurrency.Text + " " + ConvertedValue.ToString("N3");
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ClearControls();
        }

        private void ClearControls()
        {

            txtCurrency.Text = string.Empty;
            if (cmbFromCurrency.Items.Count > 0)
            {
                cmbFromCurrency.SelectedIndex = 0;
            }
            if (cmbToCurrency.Items.Count > 0)
            {
                cmbToCurrency.SelectedIndex = 0;
            }

            lblCurrency.Content = "";
            txtCurrency.Focus();
        }

        private void Clear_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void Clear_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txtAmount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        //!!!
        private CurrencyModel currencyModel;

        private void dgvCurrency_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            try
            {
                DataGrid dataGrid = (DataGrid)sender; //Create Object for DataGrid
                DataRowView row_selected = dataGrid.CurrentItem as DataRowView; // For DataRowView


                using (CurrencyDbContext context = new CurrencyDbContext())
                {

                    CurrencyModel selectedModel = dataGrid.CurrentItem as CurrencyModel;

                    var myDb = context.CurrencyModels.ToList();

                    if (selectedModel != null)
                    {
                        if (dgvCurrency.Items.Count > 0)
                        {
                            var model = myDb.Where(m => m.Id == selectedModel.Id).FirstOrDefault();

                            MessageBox.Show($"Id {model.Id}");

                            if (dataGrid.SelectedCells[0].Column.DisplayIndex == 0)
                            {
                                txtAmount.Text = model.Amount.ToString();
                                txtCurrencyName.Text = model.CurrencyName.ToString();
                                btnSave.Content = "Update";


                                context.Entry(model).State = EntityState.Modified;
                                context.SaveChanges();
                                GetData();

                            }

                            if (dataGrid.SelectedCells[0].Column.DisplayIndex == 1)
                            {
                                context.CurrencyModels.Remove(model);
                                context.SaveChanges();
                                GetData();
                            }
                        }
                    }
                }

                #region commented

                //using (CurrencyDbContext context = new CurrencyDbContext())
                //{
                //    var model = context.CurrencyModels.ToList();

                //    if (row_selected != null)
                //    {
                //        if (dgvCurrency.Items.Count > 0)
                //        {
                //            var selectedModel = model.Where(m =>
                //                m.CurrencyName == row_selected["CurrencyName"].ToString() &&
                //                m.Amount == int.Parse(row_selected["Amount"].ToString())).FirstOrDefault();

                //            MessageBox.Show($"Id {selectedModel.Id}");

                //            if (dataGrid.SelectedCells[0].Column.DisplayIndex == 0)
                //            {
                //                txtAmount.Text = row_selected["Amount"].ToString();
                //                txtCurrencyName.Text = row_selected["CurrencyName"].ToString();
                //                btnSave.Content = "Update";
                //            }

                //            if (dataGrid.SelectedCells[0].Column.DisplayIndex == 1)
                //            {

                //            }
                //        }
                //    }

                //}
                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btnSave.Content != "Update")
                {

                    if (txtAmount.Text == null || txtAmount.Text == "")
                    {
                        MessageBox.Show("Please enter amount", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        txtAmount.Focus();
                        return;
                    }
                    else if (txtCurrencyName.Text == null || txtCurrencyName.Text == "")
                    {
                        MessageBox.Show("Please enter currency amount", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        txtCurrencyName.Focus();
                        return;
                    }
                    else
                    {
                        if (MessageBox.Show("Are you sure you want to delete all info from DB?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            //insert into Database
                            using (CurrencyDbContext currencyDbContext = new CurrencyDbContext())
                            {
                                CurrencyModel currencyModel = new CurrencyModel();

                                currencyModel.Amount = int.Parse(txtAmount.Text);
                                currencyModel.CurrencyName = txtCurrencyName.Text;

                                currencyDbContext.CurrencyModels.Add(currencyModel);
                                currencyDbContext.SaveChanges();

                                GetData();
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally {
                btnSave.Content = "Save";
            }


        }


        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (btnSave.Content == "Save")
            {
                btnSave.Content = "Save";
            }

            try
            {
                if (MessageBox.Show("Are you sure you want to delete all info from DB?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {

                    using (CurrencyDbContext context = new CurrencyDbContext())
                    {
                        context.CurrencyModels.RemoveRange(context.CurrencyModels);
                        context.SaveChanges();
                    }

                    GetData();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public void GetData()
        {
            using (CurrencyDbContext context = new CurrencyDbContext())
            {
                if (context.CurrencyModels != null)
                {
                    var model = context.CurrencyModels.ToList();
                    //MessageBox.Show($"Here is {model.Count} rows");
                    dgvCurrency.ItemsSource = model;
                }
            }
        }


    }
}
