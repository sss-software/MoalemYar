﻿/****************************** ghost1372.github.io ******************************\
*	Module Name:	FindElement.cs
*	Project:		MoalemYar
*	Copyright (C) 2017 Mahdi Hosseini, All rights reserved.
*	This software may be modified and distributed under the terms of the MIT license.  See LICENSE file for details.
*
*	Written by Mahdi Hosseini <Mahdidvb72@gmail.com>,  2018, 5, 25, 11:45 ب.ظ
*
***********************************************************************************/

using nucs.JsonSettings;
using nucs.JsonSettings.Autosave;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MoalemYar
{
    public class FindElement
    {
        public static ISettings Settings = JsonSettings.Load<ISettings>(AppVariable.fileName + @"\config.json").EnableAutosave();

        public static T FindVisualChildByName<T>(DependencyObject parent, string name) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                string controlName = child.GetValue(Control.NameProperty) as string;
                if (controlName == name)
                {
                    return child as T;
                }
                else
                {
                    T result = FindVisualChildByName<T>(child, name);

                    if (result != null)

                        return result;
                }
            }
            return null;
        }
    }
}