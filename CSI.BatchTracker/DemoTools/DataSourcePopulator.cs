using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;

namespace CSI.BatchTracker.DemoTools
{
    internal class DataSourcePopulator
    {
        public IBatchOperatorSource OperatorSource { get; private set; }
        public IActiveInventorySource InventorySource { get; private set; }
        public IReceivedBatchSource ReceivingSource { get; private set; }
        public IImplementedBatchSource ImplementationSource { get; private set; }

        Random random = new Random();
        int nextPoNumber = 50000;
        string batchNumber = string.Empty;
        BatchOperator receivingOperator;
        BatchOperator implementingOperator;
        List<ReceivedBatch> receiveableBatches;

        DateTime receivingDate;
        DateTime implementingDate;

        int totalReceivedVessels;

        List<string> randomToteColor = new List<string>
        {
            "White", "White", "White", "White", "White",
            "Yellow", "Yellow", "Yellow",
            "Black", "Black",
            "Red", "Red"
        };

        List<string> randomDrumColor = new List<string>
        {
            "Blue Red", "Blue Red", "Blue Red", "Blue Red",
            "Deep Blue", "Deep Blue", "Deep Blue",
            "Deep Green", "Deep Green", "Deep Green",
            "Bright Yellow", "Bright Yellow",
            "Bright Red", "Bright Red"
        };

        public DataSourcePopulator(
            IBatchOperatorSource operatorSource,
            IActiveInventorySource inventorySource,
            IReceivedBatchSource receivingSource,
            IImplementedBatchSource implementationSource)
        {
            OperatorSource = operatorSource;
            InventorySource = inventorySource;
            ReceivingSource = receivingSource;
            ImplementationSource = implementationSource;
            receivingDate = DateTime.Today.AddDays(-100);
            implementingDate = receivingDate.AddDays(10);
            receiveableBatches = new List<ReceivedBatch>();
        }

        public void Run()
        {
            PopulateOperatorSource();
            BuildAndReceiveTenTrucks();            
        }

        void PopulateOperatorSource()
        {
            Dictionary<string, string> names = new Dictionary<string, string>
            {
                { "Jason", "Walker" },
                { "Geoff", "Nelson" },
                { "Luis Angel", "Calderon" },
                { "Israel", "Coyomani" },
                { "Rigel", "Salas" },
                { "Benjamin", "Hueramo" },
                { "Miguel", "Garcia" }
            };

            foreach (KeyValuePair<string, string> name in names)
            {
                BatchOperator newOperator = new BatchOperator(name.Key, name.Value);
                OperatorSource.SaveOperator(newOperator);
            }
        }

        void BuildAndReceiveTenTrucks()
        {
            for (int i = 0; i < 10; i++)
            {
                GetNextPoNumber();
                GetNextReceivingDate();
                FillPoWithTotes();
                FillPoWithDrums();
            }

            ReceiveAllBatches();
            ImplementMostBatches();
        }

        void GetNextPoNumber()
        {
            nextPoNumber += random.Next(25, 100);
        }

        void GetNextReceivingDate()
        {
            do
            {
                receivingDate = receivingDate.AddDays(random.Next(5, 11));
            }
            while (receivingDate.DayOfWeek == DayOfWeek.Saturday || receivingDate.DayOfWeek == DayOfWeek.Sunday);
        }

        void FillPoWithTotes()
        {
            int toteCount = random.Next(8, 11);

            while (toteCount > 0)
            {
                ReceivedBatch received = GetRandomTote();

                if (received.Quantity > toteCount)
                {
                    received.Quantity = toteCount;
                }

                receiveableBatches.Add(received);
                toteCount -= received.Quantity;
                totalReceivedVessels += received.Quantity;
            }
        }

        ReceivedBatch GetRandomTote()
        {
            int toteQty = 0;
            string toteColor = randomToteColor[random.Next(0, randomToteColor.Count)];

            switch (toteColor)
            {
                case "White":
                    toteQty = random.Next(3, 6);
                    break;

                case "Yellow":
                    toteQty = random.Next(2, 4);
                    break;

                case "Black":
                case "Red":
                    toteQty = random.Next(1, 3);
                    break;
            }

            receivingOperator = OperatorSource.FindBatchOperator(random.Next(1, 3));
            string batchNumber = GetRandomBatchNumber();

            return new ReceivedBatch(toteColor, batchNumber, receivingDate, toteQty, nextPoNumber, receivingOperator);
        }

        void FillPoWithDrums()
        {
            int drumCount = random.Next(3, 9);

            while (drumCount > 0)
            {
                ReceivedBatch received = GetRandomDrum();

                if (received.Quantity > drumCount)
                {
                    received.Quantity = drumCount;
                }

                receiveableBatches.Add(received);
                drumCount -= received.Quantity;
                totalReceivedVessels += received.Quantity;
            }
        }

        ReceivedBatch GetRandomDrum()
        {
            int drumQty = 0;
            string drumColor = randomDrumColor[random.Next(0, randomDrumColor.Count)];

            switch (drumColor)
            {
                case "Blue Red":
                    drumQty = random.Next(3, 5);
                    break;

                case "Deep Green":
                case "Deep Blue":
                    drumQty = random.Next(2, 4);
                    break;

                case "Bright Red":
                case "Bright Yellow":
                    drumQty = random.Next(1, 3);
                    break;
            }

            receivingOperator = OperatorSource.FindBatchOperator(random.Next(1, 3));
            string batchNumber = GetRandomBatchNumber();

            return new ReceivedBatch(drumColor, batchNumber, receivingDate, drumQty, nextPoNumber, receivingOperator);
        }

        string GetRandomBatchNumber()
        {
            string batchNumber;

            do
            {
                int year = random.Next(0, 2) == 0 ? 8 : 9;
                int week = random.Next(10, 51);
                int batchSuffix = random.Next(0, 10);
                int runPrefix = random.Next(1, 6);
                int runSuffix = random.Next(1, 10);
                batchNumber = string.Format("8728{0}{1}0{2}{3}0{4}", year, week, batchSuffix, runPrefix, runSuffix);
            }
            while (BatchNumberIsNotUnique(batchNumber));

            return batchNumber;
        }

        bool BatchNumberIsNotUnique(string batchNumber)
        {
            foreach (ReceivedBatch batch in receiveableBatches)
            {
                if (batch.BatchNumber == batchNumber)
                {
                    return true;
                }
            }

            return false;
        }

        void ReceiveAllBatches()
        {
            foreach (ReceivedBatch received in receiveableBatches)
            {
                ReceivingSource.SaveReceivedBatch(received);
            }
        }

        void ImplementMostBatches()
        {
            int numberOfImplementations = totalReceivedVessels - 24;
            for (int i = 0; i < numberOfImplementations; i++)
            {
                implementingDate = GetNextImplementingDate(implementingDate);
                string batchNumber = InventorySource.CurrentInventory[random.Next(0, 10)].BatchNumber;
                implementingOperator = OperatorSource.FindBatchOperator(random.Next(1, OperatorSource.OperatorRepository.Count + 1));
                ImplementationSource.AddBatchToImplementationLedger(batchNumber, implementingDate, implementingOperator);
            }
        }

        DateTime GetNextImplementingDate(DateTime currentDate)
        {
            do
            {
                currentDate = currentDate.AddHours(random.Next(8, 16)).AddMinutes(random.Next(10, 60));
            }
            while (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday);

            return currentDate;
        }
    }
}
