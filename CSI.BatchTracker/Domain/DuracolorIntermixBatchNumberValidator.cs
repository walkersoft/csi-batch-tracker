﻿using CSI.BatchTracker.Domain.Contracts;
using CSI.BatchTracker.Exceptions;
using System.Text.RegularExpressions;

namespace CSI.BatchTracker.Domain
{
    public sealed class DuracolorIntermixBatchNumberValidator : AbstractBatchNumberValidator
    {
        public DuracolorIntermixBatchNumberValidator(int length)
        {
            CheckIfLengthIsPositiveAndNotZero(length);
            BatchNumberLength = length;
        }

        public DuracolorIntermixBatchNumberValidator()
        {
            BatchNumberLength = 12;
        }

        void CheckIfLengthIsPositiveAndNotZero(int length)
        {
            if (length <= 0)
            {
                throw new BatchNumberValidationException("Specified length of a batch number must be greater that zero.");
            }
        }

        public override bool Validate(string batchNumber)
        {
            if (string.IsNullOrEmpty(batchNumber))
            {
                return false;
            }

            return Regex.Match(batchNumber, BuildRegexString()).Success;
        }

        string BuildRegexString()
        {
            return @"^872(2|8)[0-9]{" + (BatchNumberLength - 4) + "}$";
        }
    }
}
