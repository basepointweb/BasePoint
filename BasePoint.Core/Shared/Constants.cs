namespace BasePoint.Core.Shared
{
    public static class Constants
    {
        public static readonly char ErrorMessageSeparator = ';';
        public static readonly int ZeroBasedFirstIndex = 0;
        public static readonly int FirstIndex = 1;
        public static readonly int Zero = 0;
        public static readonly int QuantityZero = 0;
        public static readonly int QuantityOne = 1;
        public static readonly int QuantityMinusOne = -1;
        public static readonly int DaysInAWeek = 7;
        public static readonly string StringSpace = " ";
        public static readonly char CharSpace = ' ';
        public static readonly char CharComma = ',';
        public static readonly char CharFullStop = '.';
        public static readonly char CharEnter = '\n';
        public static readonly char CharTab = '\n';
        public static readonly string StringComma = ",";
        public static readonly string StringEnter = "\n";
        public static readonly TimeSpan DefaultApiTokenExpirationInMinutes = TimeSpan.FromMinutes(5);
        public static readonly decimal HandredBasedAHundredPercent = 100;
        public static readonly decimal AHundredPercent = 1;
        public static readonly int ComparisonEquals = 0;
        public static readonly int ComparisonLessThan = -1;
        public static readonly int ComparisonGreaterThan = 1;

        public static class ErrorCodes
        {
            public static readonly string DefaultErrorCode = "000";
        }

        public static class ErrorMessages
        {
            public static readonly string DefaultErrorMessage = $"{ErrorCodes.DefaultErrorCode};A unexpected error occurrs.";
            public static readonly string PageNumberMustBeLessOrEqualMaxPage = "001;The page number must be less than or equal to the maximum number of pages '{0}'.";
            public static readonly string PropertyIsRequired = "002;The property {0} is required.";
            public static readonly string PropertyIsInvalid = "003;The property {0} is invalid.";
            public static readonly string EntityMappingError = "004;There mapping for entity {0} is invalid.";
            public static readonly string PageNumberMustBeGreaterThanOrEqualToOne = "005;Page number must be greater than or equal to one.";
            public static readonly string ItemsPerPageMustBeGreaterThanOrEqualToOne = "006;Items per page must be greater than or equal to one.";
        }
    }
}