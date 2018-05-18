﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using LiveCharts;
using LiveCharts.Wpf;
using nucs.JsonSettings;
using nucs.JsonSettings.Fluent;

namespace MoalemYar.UserControls
{
    /// <summary>
    /// Interaction logic for Achievement.xaml
    /// </summary>
    public partial class Achievement : UserControl
    {
        private SettingsBag Setting { get; } = JsonSettings.Construct<SettingsBag>(AppVariable.fileName + @"\config.json").EnableAutosave().LoadNow();

        public Achievement()
        {
            InitializeComponent();
           
            getSchool();
        }

        #region Async Query
        public async static Task<List<DataClass.Tables.School>> GetAllSchoolsAsync()
        {
            using (var db = new DataClass.myDbContext())
            {
                var query = db.Schools.Select(x => x);
                return await query.ToListAsync();
            }
        }
        public async static Task<List<DataClass.DataTransferObjects.StudentsDto>> GetAllStudentsAsync(long BaseId)
        {
            using (var db = new DataClass.myDbContext())
            {
                var query = db.Students.OrderBy(x => x.LName).Where(x => x.BaseId == BaseId).Select(x => new DataClass.DataTransferObjects.StudentsDto { Name = x.Name, LName = x.LName, FName = x.FName, BaseId = x.BaseId, Id = x.Id });
                return await query.ToListAsync();
            }
        }
        private void getSchool()
        {
            try
            {
                var query = GetAllSchoolsAsync();
                query.Wait();
                List<DataClass.Tables.School> data = query.Result;
                if (data.Any())
                {
                    cmbEditBase.ItemsSource = data;
                }
            }
            catch (Exception)
            {
            }
        }
        private void getStudent(long BaseId)
        {
            try
            {
                var query = GetAllStudentsAsync(BaseId);
                query.Wait();
                List<DataClass.DataTransferObjects.StudentsDto> data = query.Result;
                if (data.Any())
                {
                    dataGrid.ItemsSource = data;
                }
                else
                {
                    MainWindow.main.ShowNoDataNotification(null);
                }
            }
            catch (Exception)
            {
            }
        }


        #endregion
        private void cmbEditBase_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            getStudent(Convert.ToInt64(cmbEditBase.SelectedValue));
           
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            waterfallFlow.Children.Clear();
            Series series = new ColumnSeries();
            switch (Setting[AppVariable.ChartType])
            {
                case AppVariable.CHART_Column:
                    series = new ColumnSeries { };
                    break;
                case AppVariable.CHART_Column2:
                    series = new StackedColumnSeries { };
                    break;
                case AppVariable.CHART_Line:
                    series = new LineSeries { };
                    break;
                case AppVariable.CHART_Line2:
                    series = new StepLineSeries { };
                    break;
                case AppVariable.CHART_Area:
                    series = new StackedAreaSeries { };
                    break;
            }

            for (int i = 0; i < 7; i++)
            {
                MaterialChart _addUser;
                Control _currentUser;
                _addUser = new MaterialChart("کتاب" + i, "مهدی", new string[] { "ok", "no", "ok", "no", "ok", "no" }, new double[] { 10, 20, 100, 60 }, series, AppVariable.GetBrush(Convert.ToString(Setting[AppVariable.ChartColor] ?? AppVariable.CHART_GREEN)));
                _currentUser = _addUser;
                waterfallFlow.Children.Add(_currentUser);
                waterfallFlow.Refresh();
            }

        }

    }
}