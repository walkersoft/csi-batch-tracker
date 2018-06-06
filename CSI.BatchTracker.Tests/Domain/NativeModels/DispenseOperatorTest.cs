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
    public class DispenseOperatorTest
    {
        DispenseOperator dispenseOperator;

        [Test]
        public void OperatorIsSetupCorrectly()
        {
            string firstName = "Jane";
            string lastName = "Doe";
            dispenseOperator = new DispenseOperator(firstName, lastName);

            Assert.AreEqual(firstName, dispenseOperator.FirstName);
            Assert.AreEqual(lastName, dispenseOperator.LastName);
        }

        [Test]
        public void GetOperatorInitials()
        {
            string firstName = "Jane";
            string lastName = "Doe";
            string initials = "JD";

            dispenseOperator = new DispenseOperator(firstName, lastName);

            Assert.AreEqual(initials, dispenseOperator.GetInitials());
        }

        [Test]
        public void ExceptionIfOperatorFirstNameIsEmpty()
        {
            Assert.Throws<System.ArgumentException>(() => new DispenseOperator("", "Doe"));
        }

        [Test]
        public void ExceptionIfOperatorLastNameIsEmpty()
        {
            Assert.Throws<System.ArgumentException>(() => new DispenseOperator("Jane", ""));
        }
    }
}
