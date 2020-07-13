using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Zadatak_1.Command;
using Zadatak_1.Model;
using Zadatak_1.View;

namespace Zadatak_1.ViewModel
{
    class ManViewModel : ViewModelBase
    {
        ManView manView;
        
        public ManViewModel(ManView manViewOpen)
        {
            manView = manViewOpen;
            //initializing list, inserting records from database
            listProduct = GetProducts();
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

        private string name;
        public string Name
        {
            get
            {
                return name;

            }

            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        private string code;
        public string Code
        {
            get
            {
                return code;
            }
            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }
        private int amount;
        public int Amount
        {
            get
            {
                return amount;
            }
            set
            {
                amount = value;
                OnPropertyChanged("Amount");
            }
        }
        private int price;
        public int Price
        {
            get
            {
                return price;
            }
            set
            {
                price = value;
                OnPropertyChanged("Price");
            }
        }
        private bool stored;
        public bool Stored
        {
            get
            {
                return stored;
            }
            set
            {
                stored = value;
                OnPropertyChanged("Stored");
            }
        }
        #endregion

        /// <summary>
        /// Command for saving product into database
        /// </summary>
        private ICommand save;
        public ICommand Save
        {
            get
            {
                if (save == null)
                {
                    save = new RelayCommand(param => SaveExecute(), param => CanSaveExecute());
                }
                return save;
            }
        }
        private void SaveExecute()
        {
            try
            {
                using (Context context = new Context())
                {
                    tblProduct newProduct = new tblProduct();
                    newProduct.ProdName = Name;
                    newProduct.ProdCode = Code;
                    newProduct.Price = Price;
                    newProduct.Amount = Amount;
                    newProduct.Stored = false;
                    context.tblProducts.Add(newProduct);
                    context.SaveChanges();
                    MessageBox.Show("Product is saved");
                    //refreshing list of products
                    ListProduct = GetProducts();
                    //DELEGATE implementation, logging action to file
                    Delegate d = new Delegate();
                    d.MenagerAdded(Name);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }            
        }
        private bool CanSaveExecute()
        {
            if (String.IsNullOrEmpty(Name) || String.IsNullOrEmpty(Code) || Price<=0 || Amount<=0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

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
            manView.Close();
        }
        private bool CanCloseExecute()
        {
            return true;
        }
        /// <summary>
        /// Command for deleting
        /// </summary>
        private ICommand delete;
        public ICommand Delete
        {
            get
            {
                if (delete==null)
                {
                    delete = new RelayCommand(param => DeleteExecute(), param => CanDeleteExecute());
                }
                return delete;
            }
        }
        /// <summary>
        /// Rule: stored product can not be deleted
        /// Fact: if product is stored, he exists in tblStorage
        /// </summary>
        private void DeleteExecute()
        {
            try
            {
                using (Context context = new Context())
                {
                    //collecting foreign keys for all stored products
                    List<int> listOfKeys = new List<int>();
                    List<tblStorage> storageList = context.tblStorages.ToList();
                    foreach (tblStorage item in storageList)
                    {
                        listOfKeys.Add(item.ProductID.GetValueOrDefault());
                    }
                    //only if product is not stored=>does not exist in tblStorage (his key does not exist in that table)
                    if (!listOfKeys.Contains(Product.ProductID))
                    {
                        tblProduct productToRemove = (from x in context.tblProducts where x.ProductID == Product.ProductID select x).First();
                        context.tblProducts.Remove(productToRemove);
                        context.SaveChanges();
                        MessageBox.Show("Product is deleted");
                        ListProduct = GetProducts();
                        //calling delegate to write action to the file
                        Delegate d = new Delegate();
                        d.MenagerDeleting(productToRemove.ProdName);
                    }
                    else
                    {
                        MessageBox.Show("It is not possible to delete stored product.");
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        /// <summary>
        /// Determines when it is possible to press Delete button
        /// </summary>
        /// <returns></returns>
        private bool CanDeleteExecute()
        {
            //only if product is selected
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
        /// Taking records from table and inserting into List
        /// </summary>
        /// <returns></returns>
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
            /// <summary>
            /// Event will be raies when new product is added; writes action to the file
            /// </summary>
            /// <param name="material"></param>
            public void MenagerAdded(string material)
            {
                string path = @"../../Loger.txt";
                OnNotification += () =>
                {
                    StreamWriter sw = new StreamWriter(path, true);

                    sw.WriteLine("[" + DateTime.Now.ToString("dd-MM-yyyy, H:mm:ss") + "] " + "Manager added product: {0}", material);

                    sw.Close();
                };
                OnNotification.Invoke();
            }
            /// <summary>
            /// Event will be raies when new product is deleted; writes action to the file
            /// </summary>
            /// <param name="material"></param>
            public void MenagerDeleting(string material)
            {
                string path = @"../../Loger.txt";
                OnNotification += () =>
                {
                    StreamWriter sw = new StreamWriter(path, true);

                    sw.WriteLine("[" + DateTime.Now.ToString("dd-MM-yyyy, H:mm:ss") + "] " + "Manager deleted product: {0}", material);

                    sw.Close();
                };
                OnNotification.Invoke();
            }
        }
    }
}
