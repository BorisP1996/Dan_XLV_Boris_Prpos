using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Zadatak_1.Command;
using Zadatak_1.Model;
using Zadatak_1.View;

namespace Zadatak_1.ViewModel
{
    class MagViewModel : ViewModelBase
    {
        MagView magView;

        //constructor and initialization for needed properties
        public MagViewModel(MagView magViewOpen)
        {
            magView = magViewOpen;
            //getting list and sum from tables
            ListProduct = GetProducts();
            Suma = GetSum();
        }
        #region Properties
        private tblProduct product;
        public tblProduct Product
        {
            get
            {
                return product;
            }
            set
            {
                product = value;
                OnPropertyChanged("Product");
            }
        }
        private List<tblProduct> listProduct;
        public List<tblProduct> ListProduct
        {
            get
            {
                return listProduct;
            }
            set
            {
                listProduct = value;
                OnPropertyChanged("ListProduct");
            }
        }
        //will show capacity in warehouse (from 0 to 100)
        private int suma;
        public int Suma
        {
            get
            {
                return suma;
            }
            set
            {
                suma = value;
                OnPropertyChanged("Suma");
            }
        }
        #endregion
        //Creating command which magacioner will use when he wants to store product
        private ICommand store;
        public ICommand Store
        {
            get
            {
                if (store == null)
                {
                    store = new RelayCommand(param => StoreExecute(), param => CanStoreExecute());
                }
                return store;
            }
        }
        private void StoreExecute()
        {
            Delegate d = new Delegate();
            try
            {
                using (Context context = new Context())
                {
                    //if there is enough space, because capacity is 100
                    if (Suma + Product.Amount <= 100)
                    {
                        tblStorage newStorage = new tblStorage();
                        newStorage.ProductID = Product.ProductID;
                        newStorage.Price = Product.Amount;
                        context.tblStorages.Add(newStorage);
                        tblProduct productToStore = (from r in context.tblProducts where r.ProductID == Product.ProductID select r).First();
                        //changing bool propertie (stored) to true
                        productToStore.Stored = true;
                        context.SaveChanges();
                        //MessageBox.Show("Product is stored in warehouse");

                        //DELEGATE implementation
                        d.ProductStored();
                        //updating list and sum
                        ListProduct = GetProducts();
                        Suma = GetSum();
                    }
                    else
                    {
                        //MessageBox.Show("Warehouse capacity is 100. There is not enough free space");

                        //DELEGATE implementation
                        d.WarehouseFull();

                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        
        private bool CanStoreExecute()
        {
            //Product is selected=return true
            if (Product != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Command for closing window
        /// </summary>
        private ICommand close;
        public ICommand Close
        {
            get
            {
                if (close == null)
                {
                    close = new RelayCommand(param => CloseExecute(), param => CanCloseExecute());
                }
                return close;
            }
        }
        private void CloseExecute()
        {
            magView.Close();
        }
        private bool CanCloseExecute()
        {
            return true;
        }

        /// <summary>
        /// Method gets total amount of products => in order to determine if there is space left in warehouse
        /// </summary>
        /// <returns></returns>
        private int GetSum()
        {
            try
            {
                using (Context context = new Context())
                {
                    int sum = 0;
                    //taking all records to list
                    List<tblStorage> storage = context.tblStorages.ToList();

                    ///extracting amounts into one variable
                    foreach (tblStorage item in storage)
                    {
                        sum += item.Price.GetValueOrDefault();
                    }

                    return sum;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                return 0;
            }
        }
        //taking products from tables and inserting them into list
        private List<tblProduct> GetProducts()
        {
            try
            {
                using (Context context = new Context())
                {
                    List<tblProduct> list = new List<tblProduct>();

                    list = context.tblProducts.ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }
        class Delegate
        {
            public delegate void Notification();

            public event Notification OnNotification;

            //Event that will be raised when warehouse capacity is full
            public void WarehouseFull()
            {
                OnNotification += () =>
                {
                    MessageBox.Show("Warehouse capacity is 100. There is not enough free space");
                };
                OnNotification.Invoke();
            }
            //Event that will be raised when product is stored succsesfuly
            public void ProductStored()
            {
                OnNotification += () =>
                {
                    MessageBox.Show("Product is stored in the warehouse");
                };
                OnNotification.Invoke();
            }
        }
    }
}
