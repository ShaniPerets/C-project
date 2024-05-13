using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PL
{
    internal static class Tools
    {
        //show a messagebox of the error while error occurs
        public static void ErrorOccuredMesssage(string message = "Error Occurred")
        {
            MessageBoxResult mbResult =
                MessageBox.Show("press OK to continue, else press Cancel",
                 message,
                MessageBoxButton.OKCancel,
                MessageBoxImage.Error);

            switch (mbResult)
            {
                case MessageBoxResult.OK:
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        //show a messagebox of a succeed operation
        public static void successMesssage(string message = "success message")
        {
            MessageBox.Show("operation succeed",
             message,
            MessageBoxButton.OK,
            MessageBoxImage.Information);
        }

    }
}
