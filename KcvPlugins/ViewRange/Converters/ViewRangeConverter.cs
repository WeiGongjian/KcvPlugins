﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using Grabacr07.KanColleViewer.ViewModels.Contents.Fleets;
using Grabacr07.KanColleWrapper.Models;
using AMing.Plugins.Core.Extensions;
using Grabacr07.KanColleWrapper;
using AMing.ViewRange.Extensions;

namespace AMing.ViewRange.Converters
{
    public class ViewRangeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = value as FleetViewModel;
            if (data == null)
                return value;

            //return Helper.CalcFleetViewRangeHelper.CalcFleetViewRange(data, Data.Settings.Current.Type);
            var msg = data.Ships.Select(s => s.Ship).Select(s => string.Format("{0}\n{1}\n", s.ToString(), s.SlotItems.Select(si => si.ToString()).ToString(","))).ToString("\n");
            AMing.Plugins.Core.GenericMessager.Current.SendToLogs(msg);

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
