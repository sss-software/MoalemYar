﻿/****************************** ghost1372.github.io ******************************\
*	Module Name:	AzmonHistory.xaml.cs
*	Project:		MoalemYar
*	Copyright (C) 2017 Mahdi Hosseini, All rights reserved.
*	This software may be modified and distributed under the terms of the MIT license.  See LICENSE file for details.
*
*	Written by Mahdi Hosseini <Mahdidvb72@gmail.com>,  2018, 6, 2, 07:58 ب.ظ
*
***********************************************************************************/

using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MoalemYar.UserControls
{
    /// <summary>
    /// Interaction logic for AzmonHistory.xaml
    /// </summary>
    public partial class AzmonHistoryView : UserControl
    {
        private List<DataClass.DataTransferObjects.myChartTemplate> list = new List<DataClass.DataTransferObjects.myChartTemplate>();
        public ChartValues<DataClass.DataTransferObjects.myChartTemplate> Results { get; set; }
        public ObservableCollection<string> Labels { get; set; }
        public object Mapper { get; set; }

        public AzmonHistoryView()
        {
            InitializeComponent();
        }

        private void loadData()
        {
            try
            {
                using (var db = new DataClass.myDbContext())
                {
                    var qschool = db.Schools.ToList();
                    cmbEditBase.ItemsSource = qschool.Any() ? qschool : null;

                    var qGroupName = db.Groups.ToList();
                    cmbGroups.ItemsSource = qGroupName.Any() ? qGroupName : null;
                }
            }
            catch (Exception)
            {
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            loadData();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbGroups_SelectionChanged(null, null);
        }

        private void cmbEditBase_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (var db = new DataClass.myDbContext())
            {
                long id = Convert.ToInt64(cmbEditBase.SelectedValue);
                var qUser = db.Students.Where(x => x.BaseId == id).ToList();
                dataGrid.ItemsSource = qUser.OrderBy(x => x.LName).Any() ? qUser : null;
            }
        }

        private void cmbGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                dynamic getGroupName = cmbGroups.SelectedItem;
                string gpName = getGroupName.GroupName;
                using (var db = new DataClass.myDbContext())
                {
                    dynamic selectedItem = dataGrid.SelectedItems[0];
                    long uId = selectedItem.Id;
                    var qDates = db.AHistories.Where(x => x.UserId == uId && x.GroupName.Equals(gpName)).Select(x => new { x.DatePass, x.Id }).OrderByDescending(x => x.DatePass).ToList();
                    cmbAzmon.ItemsSource = qDates.Any() ? qDates : null;
                }
            }
            catch (Exception)
            {
            }
        }

        private void cmbAzmon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gpChart.Visibility = Visibility.Visible;
            double[] values = new double[] { };

            try
            {
                dynamic getGroupName = cmbGroups.SelectedItem;
                string gpName = getGroupName.GroupName;
                dynamic getDateText = cmbAzmon.SelectedItem;
                string dPass = getDateText.DatePass;
                using (var db = new DataClass.myDbContext())
                {
                    dynamic selectedItem = dataGrid.SelectedItems[0];
                    long uId = selectedItem.Id;
                    var data = db.AHistories.Where(x => x.UserId == uId && x.DatePass == dPass && x.GroupName.Equals(gpName)).Select(x => x).OrderByDescending(x => x.DatePass).ToList();
                    values = new double[] { data.FirstOrDefault().TrueItem, data.FirstOrDefault().FalseItem, data.FirstOrDefault().NoneItem };

                    list.Add(new DataClass.DataTransferObjects.myChartTemplate { Caption = "پاسخ صحیح", Scores = data.FirstOrDefault().TrueItem });
                    list.Add(new DataClass.DataTransferObjects.myChartTemplate { Caption = "پاسخ غلط", Scores = data.FirstOrDefault().FalseItem });
                    list.Add(new DataClass.DataTransferObjects.myChartTemplate { Caption = "بدون پاسخ", Scores = data.FirstOrDefault().NoneItem });

                    Mapper = Mappers.Xy<DataClass.DataTransferObjects.myChartTemplate>()
                    .X((myData, index) => index)
                    .Y(myData => myData.Scores);

                    //lets take the first 15 records by default;
                    var records = list.OrderBy(x => x.Scores).ToArray();

                    Results = records.AsChartValues();
                    Labels = new ObservableCollection<string>(records.Select(x => x.Caption));
                    DataContext = this;
                }
            }
            catch (Exception)
            {
            }
        }
    }
}