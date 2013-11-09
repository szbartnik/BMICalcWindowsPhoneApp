using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PanoramaApp1.Infrastructure;
using PanoramaApp1.Resources;

namespace PanoramaApp1.Utilities
{
    public class BMIValidationOutput
    {
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public string Error { get; set; }
    }

    public class BMICalculator
    {
        public decimal Height { get; private set; }
        public decimal Weight { get; private set; }

        private ICollection<BMILimit> _BMILimitsList;

        private static readonly int _minH = 100;
        private static readonly int _maxH = 300;
        private static readonly int _minW = 30;
        private static readonly int _maxW = 300;

        public BMICalculator(BMIValidationOutput input, ICollection<BMILimit> BMILimitsList)
            : this(input.Height, input.Weight, BMILimitsList) { }

        public BMICalculator(decimal height, decimal weight, ICollection<BMILimit> BMILimitsList)
        {
            Height = height;
            Weight = weight;

            _BMILimitsList = BMILimitsList;
            _BMILimitsList.ForEach(v => v.IsSelected = false);
        }

        public decimal Value
        {
            get { return Math.Round(Weight / ((Height * Height) / 10000), 2); }
        }

        public BMILimit ShowLimit
        {
            get { return _BMILimitsList.SingleOrDefault(t => Value.Between(t.LowBoundary, t.UpBoundary, true)); }
        }

        public ICollection<BMILimit> ShowLimitList
        {
            get { return _BMILimitsList; }
        }

        public static BMIValidationOutput ValidateInputText(string height, string weight)
        {
            decimal hResult, wResult;
            var output = new BMIValidationOutput() { Error = null };

            if (!decimal.TryParse(height, out hResult))
            {
                output.Error = AppResources.BMIHeightInvalidInputException;
                return output;
            }
            if (!decimal.TryParse(weight, out wResult))
            {
                output.Error = AppResources.BMIWeightInvalidInputException;
                return output;
            }
            if (!hResult.Between(_minH, _maxH, true))
            {
                output.Error = String.Format("{0} {1} {2} {3} cm",
                    AppResources.BMIHeightInvalidRangeException,
                    _minH,
                    AppResources.AndConjuntion,
                    _maxH);
                return output;
            }
            if (!wResult.Between(_minW, _maxW, true))
            {
                output.Error = String.Format("{0} {1} {2} {3} kg",
                    AppResources.BMIWeightInvalidRangeException,
                    _minW,
                    AppResources.AndConjuntion,
                    _maxW);
                return output;
            }

            output.Height = hResult;
            output.Weight = wResult;
            return output;
        }
    }
}
