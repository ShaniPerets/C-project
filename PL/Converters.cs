using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace PL;

//convert the id to string. if id = 0 so the state is adding, otherwise it updating
class ConvertIdToContent : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? "Add" : "Update";
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
//converts the row text color accrding to the status word 
class ConvertWordToColor : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((string)value == "Scheduled")
            return new SolidColorBrush(Colors.Transparent);
        else if ((string)value == "Unscheduled")
            return new SolidColorBrush(Colors.Transparent);
        else if ((string)value == "OnTrack")
            return new SolidColorBrush(Colors.Transparent);
        else if ((string)value == "InJeopardy")
            return new SolidColorBrush(Colors.Transparent);
        else if ((string)value == "Done")
            return new SolidColorBrush(Colors.Transparent);
        else if ((string)value == "None")
            return new SolidColorBrush(Colors.Transparent);
        return new SolidColorBrush(Colors.Black);// Default color
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
//choose the row background color according to the task status
class ConvertStatusToColor : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (string)value switch
        {
            "Scheduled" =>  new SolidColorBrush(Colors.Pink),
            "OnTrack" => new SolidColorBrush(Colors.Bisque),
            "InJeopardy" => new SolidColorBrush(Colors.AliceBlue),
            "Done" => new SolidColorBrush(Colors.Aquamarine),
            "Unscheduled" => new SolidColorBrush(Colors.Plum),

            _ => new SolidColorBrush(Colors.White)//defult color
        };
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

//decide if a control is enabled according to id. return true or false according to this
//return true to add state. is-enabled is true
class ConvertIdToBool : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? true : false;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

//convert bool to its opposite
class BooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
       
        return (bool)value? "false" : "true";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

//show or hide according to true /false
public class ConvertBoolToVisibilityReverse : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
       
        return (bool)value ? Visibility.Collapsed : Visibility.Visible;
        
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

//show or hide according to true /false
public class ConvertBoolToVisibility : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {

        return (bool)value ? Visibility.Visible : Visibility.Collapsed;

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

//convert id to visible or collapse a textbox
class ConvertIdToVisibility : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? Visibility.Collapsed : Visibility.Visible;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
