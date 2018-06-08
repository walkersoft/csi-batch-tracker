using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Experimental;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.Experimental
{
    [TestFixture]
    class DataStoreTest
    {
        DataStore store;
        ObservableCollection<BatchOperator> batchOperators;

        [SetUp]
        public void SetUp()
        {
            store = new DataStore();

            batchOperators = new ObservableCollection<BatchOperator>
            {
                new BatchOperator("Jane", "Doe"),
                new BatchOperator("John", "Doe"),
                new BatchOperator("Remy", "Quaid")
            };
        }

        [Test]
        public void BatchOperatorsAreSetAndEqualToThree()
        {
            store.BatchOperators = batchOperators;

            Assert.AreSame(batchOperators, store.BatchOperators);
            Assert.AreEqual(3, store.BatchOperators.Count);
        }
    }
}
