using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Zadatak_1.Command;
using Zadatak_1.View;

namespace Zadatak_1.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        MainWindow main;

        public MainWindowViewModel(MainWindow mainOpen)
        {
            main = mainOpen;
        }
        private string username;
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
                OnPropertyChanged("Username");
            }
        }
        private string password;
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }

        private ICommand login;
        public ICommand Login
        {
            get
            {
                if (login == null)
                {
                    login = new RelayCommand(param => LoginExecute(), param => CanLoginExecute());
                }
                return login;
            }
        }
        /// <summary>
        /// Method for determining which user has logged in
        /// </summary>
        private void LoginExecute()
        {
            try
            {
                //iz admin is logged
                if (Username == "Mag2019" && Password == "Mag2019")
                {
                    MagView magView = new MagView();
                    magView.ShowDialog();
                }
                //if user is logged
                else if (Username=="Man2019" && Password=="Man2019")
                {
                    ManView manView = new ManView();
                    manView.ShowDialog();
                }
                //if invalid parametres are inputed
                else
                {
                    MessageBox.Show("Invalid parametres");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());

            }
        }
        //can press button only if both fields are not empty
        private bool CanLoginExecute()
        {
            if (String.IsNullOrEmpty(Password) || String.IsNullOrEmpty(Username))
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
            main.Close();
        }
        private bool CanCloseExecute()
        {
            return true;
        }
    }
}
