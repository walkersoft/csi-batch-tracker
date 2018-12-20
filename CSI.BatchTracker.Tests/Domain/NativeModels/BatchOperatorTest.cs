using CSI.BatchTracker.Domain.NativeModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.Domain.NativeModels
{
    [TestFixture]
    public class BatchOperatorTest
    {
        BatchOperator dispenseOperator;
        readonly string firstName = "Jane";
        readonly string lastName = "Doe";

        [SetUp]
        public void SetUp()
        {
            dispenseOperator = new BatchOperator(firstName, lastName);
        }

        [Test]
        public void OperatorIsSetupCorrectly()
        {
            Assert.AreEqual(firstName, dispenseOperator.FirstName);
            Assert.AreEqual(lastName, dispenseOperator.LastName);
        }

        [Test]
        public void GetOperatorInitials()
        {
            string initials = "JD";
            dispenseOperator = new BatchOperator(firstName, lastName);

            Assert.AreEqual(initials, dispenseOperator.GetInitials());
        }

        [Test]
        public void FullNameInProperty()
        {
            string expected = "Jane Doe";
            Assert.AreEqual(expected, dispenseOperator.FullName);
        }
    }
}
