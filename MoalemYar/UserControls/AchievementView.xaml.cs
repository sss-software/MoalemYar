﻿using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace MoalemYar.UserControls
{
    /// <summary>
    /// Interaction logic for Achievement.xaml
    /// </summary>
    public partial class AchievementView : UserControl
    {
        private List<DataClass.Tables.Score> _initialCollection;

        public AchievementView()
        {
            InitializeComponent();
            getSchool();
        }

        #region Query

        public void getSchool()
        {
            try
            {
                using (var db = new DataClass.myDbContext())
                {
                    var query = db.Schools.ToList();
                    if (query.Any())
                    {
                        cmbEditBase.ItemsSource = query;
                    }
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
                using (var db = new DataClass.myDbContext())
                {
                    var query = db.Students.OrderBy(x => x.LName).Where(x => x.BaseId == BaseId).Select(x => new DataClass.DataTransferObjects.StudentsDto { Name = x.Name, LName = x.LName, FName = x.FName, BaseId = x.BaseId, Id = x.Id });
                    if (query.Any())
                    {
                        dataGrid.ItemsSource = query.ToList();
                    }
                    else
                    {
                        MainWindow.main.ShowNoDataNotification(null);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void getStudentScore(long StudentId)
        {
            try
            {
                using (var db = new DataClass.myDbContext())
                {
                    var query = db.Scores.Where(x => x.StudentId == StudentId).ToList();
                    if (query.Any())
                        _initialCollection = query;
                    else
                        _initialCollection = null;
                }
            }
            catch (NullReferenceException)
            {
            }
        }

        #endregion Query

        private void cmbEditBase_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            getStudent(Convert.ToInt64(cmbEditBase.SelectedValue));
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                dynamic selectedItem = dataGrid.SelectedItems[0];

                waterfallFlow.Children.Clear();
                Series series = new ColumnSeries();

                switch (FindElement.Settings.ChartType ?? 0)
                {
                    case 0:
                        series = new ColumnSeries { };
                        break;

                    case 1:
                        series = new StackedColumnSeries { };
                        break;

                    case 2:
                        series = new LineSeries { };
                        break;

                    case 3:
                        series = new StepLineSeries { };
                        break;

                    case 4:
                        series = new StackedAreaSeries { };
                        break;
                }

                getStudentScore(selectedItem.Id); // get Student scores

                //get scores merge duplicates and replace string to int
                var score = _initialCollection.GroupBy(x => new { x.Book, x.Date, x.StudentId })
                            .Select(x => new
                            {
                                x.Key.StudentId,
                                x.Key.Book,
                                x.Key.Date,
                                Sum = x.Sum(y => AppVariable.EnumToNumber(y.Scores))
                            }).ToArray();

                //get Book Count for generate chart
                var bookCount = score.GroupBy(x => new { x.Book })
                     .Select(g => new
                     {
                         g.Key.Book
                     }).ToList();

                MaterialChart _addUser;
                Control _currentUser;

                //generate chart based on count of books
                foreach (var item in bookCount)
                {
                    _addUser = new MaterialChart(item.Book, selectedItem.Name + " " + selectedItem.LName, getDateArray(item.Book), getScoreArray(item.Book), getAverage(item.Book), getAverageStatus(item.Book), series, AppVariable.GetBrush(FindElement.Settings.ChartColor ?? AppVariable.CHART_GREEN));
                    _currentUser = _addUser;
                    waterfallFlow.Children.Add(_currentUser);
                }

                #region Create Chart Grid with Shadow
                //Grid grid;
                //grid = new Grid
                //{
                //    Width = 300,
                //    Height = 330,
                //    Effect = new DropShadowEffect
                //    {
                //        BlurRadius = 15,
                //        Direction = -90,
                //        Opacity = .2,
                //        RenderingBias = RenderingBias.Quality,
                //        ShadowDepth = 1
                //    },
                //};

                //var border = new Border()
                //{
                //    Background = new SolidColorBrush(Colors.White),
                //    CornerRadius = new CornerRadius(5)
                //};
                //var border2 = new Border()
                //{
                //    Background = new SolidColorBrush(Colors.White)
                //};
                //Grid.SetRowSpan(border, 4);
                //Grid.SetRowSpan(border2, 3);

                //grid.Children.Add(border);
                //grid.Children.Add(border2);
                /////////////////////////////////

                //var stackFill = new StackPanel
                //{
                //    Orientation = Orientation.Vertical
                //};


                //var txtBook = new TextBlock()
                //{
                //    FontSize = 18,
                //    TextAlignment = TextAlignment.Center,
                //    Text ="Book"
                //};

                //var txtName = new TextBlock()
                //{

                //    FontSize = 18,
                //    TextAlignment = TextAlignment.Center,
                //    Text = "Name"
                //};

                //var chart = new CartesianChart
                //{
                //    Margin = new Thickness(10, 0, 10, 20),
                //    DataTooltip = null,
                //    Hoverable = false,

                //};

                //var stack = new StackPanel
                //{
                //    Margin = new Thickness(20, 0, 20, 0),
                //    VerticalAlignment = VerticalAlignment.Center
                //};
                //var txt = new TextBlock()
                //{
                //    Opacity = .4,
                //    FontSize = 13,
                //    Text = "میانگین نمرات این درس برابر است با:"
                //};

                //var stack2 = new StackPanel
                //{
                //    Orientation = Orientation.Horizontal
                //};

                //var txtAverageDouble = new TextBlock()
                //{

                //    FontSize = 30,
                //    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#303030")),
                //    TextAlignment = TextAlignment.Center,
                //    Text = "20.45"
                //};
                //var txtAverage = new TextBlock()
                //{
                //    VerticalAlignment = VerticalAlignment.Bottom,
                //    Margin = new Thickness(8, 6, 8, 6),
                //    FontSize = 16,
                //    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#303030")),
                //    Text = "نیاز به تلاش بیشتر"
                //};

                //stack2.Children.Add(txtAverageDouble);
                //stack2.Children.Add(txtAverage);

                //stack.Children.Add(txt);
                //stack.Children.Add(stack2);

                //stackFill.Children.Add(txtBook);
                //stackFill.Children.Add(txtName);
                //stackFill.Children.Add(chart);
                //stackFill.Children.Add(stack);

                //grid.Children.Add(stackFill);
                //waterfallFlow.Children.Add(grid);

                #endregion
            }
            catch (ArgumentNullException) { }
            catch (NullReferenceException)
            {
            }
        }

        //get Score Average to string
        private string getAverage(string Book)
        {
            var score = _initialCollection.GroupBy(x => new { x.Book, x.Date, x.StudentId })
                           .Select(x => new
                           {
                               x.Key.StudentId,
                               x.Key.Book,
                               x.Key.Date,
                               Sum = x.Sum(y => AppVariable.EnumToNumber(y.Scores))
                           }).Where(x => x.Book == Book).ToArray();
            var dCount = score.Select(x => x.Date).Count();
            var sum = score.Sum(x => x.Sum);

            var one = decimal.Divide(sum, 1);
            var sec = decimal.Divide(sum, 2);
            var thi = decimal.Divide(sum, 3);
            var forth = decimal.Divide(sum, 4);

            return decimal.Divide(sum, dCount).ToString("0.00");
        }

        private string getAverageStatus(string Book)
        {
            var score = _initialCollection.GroupBy(x => new { x.Book, x.Date, x.StudentId })
                           .Select(x => new
                           {
                               x.Key.StudentId,
                               x.Key.Book,
                               x.Key.Date,
                               Sum = x.Sum(y => AppVariable.EnumToNumber(y.Scores))
                           }).Where(x => x.Book == Book).ToArray();

            var sum = score.Sum(x => x.Sum);

            var dCount = score.Select(x => x.Date).Count();

            var Avg = decimal.Divide(sum, dCount).ToString("0.00");

            var one = decimal.Divide(dCount * 4, 1).ToString("0.00");
            var sec = decimal.Divide(dCount * 4, 2).ToString("0.00");
            var thi = decimal.Divide(dCount * 4, 3).ToString("0.00");
            var forth = decimal.Divide(dCount * 4, 4).ToString("0.00");

            string status = string.Empty;

            if (Convert.ToDecimal(Avg) >= Convert.ToDecimal(sec))
                status = "خیلی خوب";
            else if (Convert.ToDecimal(Avg) < Convert.ToDecimal(one) && Convert.ToDecimal(Avg) >= Convert.ToDecimal(thi))
                status = "خوب";
            else if (Convert.ToDecimal(Avg) < Convert.ToDecimal(sec) && Convert.ToDecimal(Avg) >= Convert.ToDecimal(forth))
                status = "قابل قبول";
            else if (Convert.ToDecimal(Avg) < Convert.ToDecimal(forth))
                status = "نیاز به تلاش بیشتر";

            return status;
        }

        //get Dates to string[]
        private string[] getDateArray(string Book)
        {
            var score = _initialCollection.GroupBy(x => new { x.Book, x.Date, x.StudentId })
                           .Select(x => new
                           {
                               x.Key.StudentId,
                               x.Key.Book,
                               x.Key.Date,
                               Sum = x.Sum(y => AppVariable.EnumToNumber(y.Scores))
                           }).Where(x => x.Book == Book).ToArray();
            return score.Select(x => x.Date).ToArray();
        }

        //get Scores to double[]
        private double[] getScoreArray(string Book)
        {
            var score = _initialCollection.GroupBy(x => new { x.Book, x.Date, x.StudentId })
                           .Select(x => new
                           {
                               x.Key.StudentId,
                               x.Key.Book,
                               x.Key.Date,
                               Sum = x.Sum(y => AppVariable.EnumToNumber(y.Scores))
                           }).Where(x => x.Book == Book).ToArray();
            return score.Select(x => Convert.ToDouble(x.Sum)).ToArray();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            cmbEditBase.SelectedIndex = Convert.ToInt32(FindElement.Settings.DefaultSchool);
        }
    }
}