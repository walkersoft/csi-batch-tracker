using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.DataSource.MemorySource;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Storage.MemoryStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        DateTime receivingDate;

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
            receivingDate = DateTime.Today.AddDays(-50);
        }

        public void Run()
        {
            PopulateOperatorSource();
            BuildAndReceiveTenTrucks();
            
        }

        void PopulateOperatorSource()
        {
            Dictionary<string, string> names = new Dictionary<string, string>();
            names.Add("Jason", "Walker");
            names.Add("Geoff", "Nelson");
            names.Add("Luis Angel", "Calderon");
            names.Add("Israel", "Coyomani");
            names.Add("Rigel", "Salas");
            names.Add("Benjamin", "Hueramo");
            names.Add("Miguel", "Garcia");

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
                FillPoWithTotes();
            }
        }

        void GetNextPoNumber()
        {
            nextPoNumber += random.Next(25, 200);
        }

        void FillPoWithTotes()
        {
            int toteCount = random.Next(8, 11);

            while (toteCount > 0)
            {
                ReceivedBatch received = GetRandomTote();
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

            receivingOperator = OperatorSource.FindBatchOperator(random.Next(1, OperatorSource.OperatorRepository.Count + 1));
            string batchNumber = GetRandomBatchNumber();

            do
            {
                receivingDate.AddDays(random.Next(3, 7));
            }
            while (receivingDate.DayOfWeek == DayOfWeek.Saturday || receivingDate.DayOfWeek == DayOfWeek.Sunday);

            return new ReceivedBatch(toteColor, batchNumber, receivingDate, toteQty, nextPoNumber, receivingOperator);
        }

        string GetRandomBatchNumber()
        {
            int year = random.Next(0, 2) == 0 ? 8 : 9;
            int week = random.Next(10, 51);
            int batchSuffix = random.Next(0, 10);
            int runPrefix = random.Next(1, 6);
            int runSuffix = random.Next(1, 10);

            return string.Format("8728{0}{1}0{2}{3}0{4}", year, week, batchSuffix, runPrefix, runSuffix);
        }
    }
}
