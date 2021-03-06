﻿namespace CSI.BatchTracker.Domain.NativeModels
{
    public sealed class AverageBatch
    {
        public string ColorName { get; private set; }
        public int ProductionDays { get; private set; }
        public int QuantityUsed { get; set; }

        public float AverageUsage
        {
            get { return QuantityUsed / (float)ProductionDays; }
        }

        public string DisplayUsage
        {
            get { return AverageUsage.ToString("0.00"); }
        }

        public AverageBatch(string colorName, int productionDays, int quantityUsed = 0)
        {
            ColorName = colorName;
            ProductionDays = productionDays;
            QuantityUsed = quantityUsed;
        }
    }
}
