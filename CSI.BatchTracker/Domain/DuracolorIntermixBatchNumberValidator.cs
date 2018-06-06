﻿using CSI.BatchTracker.Contracts;
using CSI.BatchTracker.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Domain
{
    public class DuracolorIntermixBatchNumberValidator : AbstractBatchNumberValidator
    {
        public DuracolorIntermixBatchNumberValidator(int length)
        {
            CheckIfLengthIsPositiveAndNotZero(length);
            BatchNumberLength = length;
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
            Match match = Regex.Match(batchNumber, BuildRegexString());
            return match.Success;
        }

        string BuildRegexString()
        {
            return @"^872(2|8)[0-9]{" + (BatchNumberLength - 4) + "}$";
        }
    }
}
