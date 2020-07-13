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
    class ManViewModel : ViewModelBase
    {
        ManView manView;
        
        public ManViewModel(ManView manViewOpen)
        {
            manView = manViewOpen;
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
    }
}
