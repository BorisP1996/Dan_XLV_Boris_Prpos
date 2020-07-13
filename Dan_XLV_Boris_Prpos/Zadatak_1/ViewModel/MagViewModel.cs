using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public MagViewModel(MagView magViewOpen)
        {
            magView = magViewOpen;
            ListProduct = GetProducts();
        }

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
        private ICommand store;
        public ICommand Store
        {
            get
            {
                if (store==null)
                {
                    store = new RelayCommand(param => StoreExecute(), param => CanStoreExecute());
                }
                return store;
            }
        }
        private void StoreExecute()
        {
            try
            {
                using (Context context = new Context())
                {
                    int sum = 0;
                    List<tblStorage> storage = context.tblStorages.ToList();

                    foreach (tblStorage item in storage)
                    {
                        sum += item.Price.GetValueOrDefault();
                    }

                    if (sum + Product.Amount<100)
                    {
                        tblStorage newStorage = new tblStorage();
                        newStorage.ProductID = Product.ProductID;
                        newStorage.Price = Product.Amount;
                        context.tblStorages.Add(newStorage);
                        tblProduct productToStore = (from r in context.tblProducts where r.ProductID == Product.ProductID select r).First();
                        productToStore.Stored = true;
                        context.SaveChanges();
                        MessageBox.Show("Product is stored in warehouse");
                    }
                    else
                    {
                        MessageBox.Show("Warehouse capacity is 100. There is not enough free space");
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
            if (Product !=null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
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
    }
}
