﻿
/****************************** ghost1372.github.io ******************************\
*	Module Name:	AzmonResult.xaml.cs
*	Project:		MoalemYar
*	Copyright (C) 2017 Mahdi Hosseini, All rights reserved.
*	This software may be modified and distributed under the terms of the MIT license.  See LICENSE file for details.
*
*	Written by Mahdi Hosseini <Mahdidvb72@gmail.com>,  2018, 6, 1, 10:33 ب.ظ
*	
***********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MoalemYar.UserControls
{
    /// <summary>
    /// Interaction logic for AzmonResult.xaml
    /// </summary>
    public partial class AzmonResult : UserControl
    {
        public static int _True;
        public static int _False;
        public static int _None;
        public static string _GroupName = string.Empty;
        public static long _UserId;
        public AzmonResult()
        {
            InitializeComponent();
            txtTrue.Text = string.Format(txtTrue.Text, _True);
            txtFalse.Text = string.Format(txtFalse.Text,_False);
            txtNon.Text = string.Format(txtNon.Text, _None);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Azmon.main.exContent.Content = new StartAzmon();

        }
    }
}