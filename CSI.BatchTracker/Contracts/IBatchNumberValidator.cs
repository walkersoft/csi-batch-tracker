﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Contracts
{
    public interface IBatchNumberValidator
    {
        bool Validate(string batchNumber);
        int GetBatchNumberLength();
    }
}
