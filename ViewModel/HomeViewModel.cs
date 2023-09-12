using LibraryWPF.Model;
using LibraryWPF.Repositories;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.Defaults;
using LiveChartsCore.Measure;
using System.Collections.ObjectModel;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using LiveChartsCore.SkiaSharpView.WPF;
using System.Windows.Media.TextFormatting;

namespace LibraryWPF.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        private string? _totalIssuedBooks;
        private string? _totalReaders;
        private string? _totalPagesRead;
        private string? _totalDebt;
        private string? _popularBookOne;
        private string? _popularBookTwo;
        private string? _popularBookThree;
        private string[] _arrayPopularBook = new string[3];
        private string? _inputYearCountBook;
        private string? _inputYearCountPage;

        private ObservableCollection<double> _valueCartesianChart;
        private List<string> _valueLabelChartBook;
        private ColumnSeries<double> _columnSeriesBook;

        private ObservableCollection<double> _valueCartesianChartTwo;
        private List<string> _valueLabelChartPage;
        private ColumnSeries<double> _columnSeriesPage;

        IUserRepository _userRepository;

        public string? TotalIssuedBooks
        {
            get => _totalIssuedBooks;
            set
            {
                _totalIssuedBooks = value;
                OnPropertyChanged(nameof(TotalIssuedBooks));
            }
        }
        public string? TotalReaders
        {
            get => _totalReaders;
            set
            {
                _totalReaders = value;
                OnPropertyChanged(nameof(TotalReaders));
            }
        }
        public string? TotalPagesRead
        {
            get => _totalPagesRead;
            set
            {
                _totalPagesRead = value;
                OnPropertyChanged(nameof(TotalPagesRead));
            }
        }
        public string? TotalDebt
        {
            get => _totalDebt;
            set
            {
                _totalDebt = value;
                OnPropertyChanged(nameof(TotalDebt));
            }
        }
        public string? PopularBookOne
        {
            get => _popularBookOne ?? string.Empty;
            set
            {
                _popularBookOne = value;
                OnPropertyChanged(nameof(PopularBookOne));
            }
        }
        public string? PopularBookTwo
        {
            get => _popularBookTwo ?? string.Empty;
            set
            {
                _popularBookTwo = value;
                OnPropertyChanged(nameof(PopularBookTwo));
            }
        }
        public string? PopularBookThree
        {
            get => _popularBookThree ?? string.Empty;
            set
            {
                _popularBookThree = value;
                OnPropertyChanged(nameof(PopularBookThree));
            }
        }
        public string[] ArrayPopularBook
        {
            get => _arrayPopularBook;
            set
            {
                _arrayPopularBook = value;
                OnPropertyChanged(nameof(ArrayPopularBook));
                PopularBookOne = _arrayPopularBook[0];
                PopularBookTwo = _arrayPopularBook[1];
                PopularBookThree = _arrayPopularBook[2];
            }
        }
        public string? InputYearCountBook
        {
            get => _inputYearCountBook;
            set
            {
                _inputYearCountBook = value;
                OnPropertyChanged(nameof(InputYearCountBook));
                try
                {
                    var temp = Int32.Parse(value);
                    int digitCount = (int)Math.Log10(temp) + 1;
                    if (digitCount == 4)
                        ExecuteInitializChartBookYear();
                }
                catch (Exception ex)
                {
                    InputYearCountBook = "0";
                }

            }
        }
        public string? InputYearCountPage
        {
            get => _inputYearCountPage;
            set
            {
                _inputYearCountPage = value;
                OnPropertyChanged(nameof(InputYearCountPage));
                try
                {
                    var temp = Int32.Parse(value);
                    int digitCount = (int)Math.Log10(temp) + 1;
                    if (digitCount == 4)
                        ExecuteInitializChartPageYear();
                }
                catch (Exception ex)
                {
                    InputYearCountPage = "0";
                }
            }
        }

        #region Properties CartesianChart 
        public ObservableCollection<double> ValueCartesianChart
        {
            get => _valueCartesianChart ?? (_valueCartesianChart = new ObservableCollection<double>());
            set
            {
                _valueCartesianChart = value;
                OnPropertyChanged(nameof(ValueCartesianChart));
            }
        }
        public ISeries[] SeriesBook { get; set; }
        public Axis[] XAxes { get; set; }
        public ColumnSeries<double> ColumnSeriesBook
        {
            get => _columnSeriesBook;
            set
            {
                _columnSeriesBook = value;
                OnPropertyChanged(nameof(ColumnSeriesBook));
            }
        }
        public Axis axis { get; set; }
        public Axis[] YAxes { get; set; }
            = new Axis[]
                {
                    new Axis
                    {
                        Name = "Количесвто книг",
                        NamePaint = new SolidColorPaint(SKColors.WhiteSmoke),
                        NameTextSize = 12,
                        LabelsPaint = new SolidColorPaint(SKColors.White),
                        TextSize = 14,
                        SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray)
                        {
                            StrokeThickness = 1,
                            PathEffect = new DashEffect(new float[] { 3, 3 })
                        }
                    }
                };

        public ObservableCollection<double> ValueCartesianChartTwo
        {
            get => _valueCartesianChartTwo ?? (_valueCartesianChartTwo = new ObservableCollection<double>());
            set
            {
                _valueCartesianChartTwo = value;
                OnPropertyChanged(nameof(ValueCartesianChartTwo));
            }
        }
        public ISeries[] SeriesPage { get; set; }
        public Axis[] XAxesTwo { get; set; }
        public ColumnSeries<double> ColumnSeriesPage
        {
            get => _columnSeriesPage;
            set
            {
                _columnSeriesPage = value;
                OnPropertyChanged(nameof(ColumnSeriesPage));
            }
        }
        public Axis axisPage { get; set; }
        public Axis[] YAxesPage { get; set; }
            = new Axis[]
                {
                    new Axis
                    {
                        Name = "Количесвто страниц",
                        NamePaint = new SolidColorPaint(SKColors.WhiteSmoke),
                        NameTextSize = 12,
                        LabelsPaint = new SolidColorPaint(SKColors.White),
                        TextSize = 14,
                        SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray)
                        {
                            StrokeThickness = 1,
                            PathEffect = new DashEffect(new float[] { 3, 3 })
                        }
                    }
                };
        #endregion


        public HomeViewModel()
        {
            _userRepository = new UserRepository();
            ColumnSeriesBook = new ColumnSeries<double>();
            ColumnSeriesPage = new ColumnSeries<double>();
            ValueCartesianChart = new ObservableCollection<double> { 0, 0, 0, 0};
            ValueCartesianChartTwo = new ObservableCollection<double> { 0, 0, 0, 0 };
            axis = new Axis();
            axisPage = new Axis();

            ExecuteShowTotalIssuedBooks();
            ExecuteShowTotalReaders();
            ExecuteShowTotalPagesRead();
            ExecuteShowTotalDebt();
            ExecuteShowMostPopularBook();

            ColumnSeriesBook.Values = ValueCartesianChart;
            ColumnSeriesBook.Padding = 5;
            ColumnSeriesPage.Values = ValueCartesianChartTwo;
            ColumnSeriesPage.Padding = 5;

            axis.Name = "Год издания";
            axis.NamePaint = new SolidColorPaint(SKColors.WhiteSmoke);
            axis.NameTextSize = 12;
            axis.LabelsPaint = new SolidColorPaint(SKColors.White);
            axis.TextSize = 14;
            axis.SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray) { StrokeThickness = 1 };

            axisPage.Name = "Год издания";
            axisPage.NamePaint = new SolidColorPaint(SKColors.WhiteSmoke);
            axisPage.NameTextSize = 12;
            axisPage.LabelsPaint = new SolidColorPaint(SKColors.White);
            axisPage.TextSize = 14;
            axisPage.SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray) { StrokeThickness = 1 };

            SeriesBook = new ISeries[] { ColumnSeriesBook };
            SeriesPage = new ISeries[] { ColumnSeriesPage };
            XAxes = new Axis[] { axis };
            XAxesTwo = new Axis[] { axisPage };
        }

        private void ExecuteShowTotalIssuedBooks()
        {
            TotalIssuedBooks = _userRepository.GetByTotalIssuedBooks().ToString();
        }
        private void ExecuteShowTotalReaders()
        {
            TotalReaders = _userRepository.GetByTotalReaders().ToString();
        }
        private void ExecuteShowTotalPagesRead()
        {
            TotalPagesRead = _userRepository.GetByTotalPagesRead().ToString();
        }
        private void ExecuteShowTotalDebt()
        {
            TotalDebt = _userRepository.GetByTotalDebt().ToString();
        }
        private void ExecuteShowMostPopularBook()
        {
            ArrayPopularBook = _userRepository.GetByMostPopularBook();
        }

        private void ExecuteInitializChartBookYear()
        {
            ValueCartesianChart = _userRepository.InitializChartBookYear(InputYearCountBook);
            _valueLabelChartBook = _userRepository.InitializChartBookYearXAxes(InputYearCountBook);
            ColumnSeriesBook.Values = ValueCartesianChart;
            SeriesBook = new ISeries[] { ColumnSeriesBook };
            axis.Labels = _valueLabelChartBook;
            XAxes = new Axis[] { axis };
        }
        private void ExecuteInitializChartPageYear()
        {
            ValueCartesianChartTwo = _userRepository.InitializChartPageYear(InputYearCountPage);
            _valueLabelChartPage = _userRepository.InitializChartBookYearXAxes(InputYearCountPage);
            ColumnSeriesPage.Values = ValueCartesianChartTwo;
            SeriesPage = new ISeries[] { ColumnSeriesPage };
            axisPage.Labels = _valueLabelChartPage;
            XAxesTwo = new Axis[] { axisPage };
        }


    }
}
