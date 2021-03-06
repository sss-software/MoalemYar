﻿/****************************** ghost1372.github.io ******************************\
*	Module Name:	AzmonResult.xaml.cs
*	Project:		MoalemYar
*	Copyright (C) 2017 Mahdi Hosseini, All rights reserved.
*	This software may be modified and distributed under the terms of the MIT license.  See LICENSE file for details.
*
*	Written by Mahdi Hosseini <Mahdidvb72@gmail.com>,  2018, 6, 1, 10:33 ب.ظ
*
***********************************************************************************/

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace MoalemYar.UserControls
{
    /// <summary>
    /// Interaction logic for AzmonResult.xaml
    /// </summary>
    public partial class AzmonResultView : UserControl
    {
        public static int _True;
        public static int _False;
        public static int _None;
        public static string _GroupName = string.Empty;
        public static long _UserId;

        private PersianCalendar pc = new PersianCalendar();
        private static string strDate;

        public AzmonResultView()
        {
            InitializeComponent();
            txtTrue.Content = string.Format(txtTrue.Content.ToString(), _True);
            txtFalse.Content = string.Format(txtFalse.Content.ToString(), _False);
            txtNon.Content = string.Format(txtNon.Content.ToString(), _None);
            strDate = pc.GetYear(DateTime.Now).ToString("0000") + "/" + pc.GetMonth(DateTime.Now).ToString("00") + "/" + pc.GetDayOfMonth(DateTime.Now).ToString("00");
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AzmonView.main.exContent.Content = new StartAzmonView();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new DataClass.myDbContext())
            {
                var data = new DataClass.Tables.AHistory()
                {
                    UserId = _UserId,
                    FalseItem = _False,
                    TrueItem = _True,
                    NoneItem = _None,
                    GroupName = _GroupName,
                    DatePass = strDate
                };
                db.AHistories.Add(data);
                db.SaveChanges();
            }
        }
    }
}