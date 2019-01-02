using CSI.BatchTracker.Domain;
using CSI.BatchTracker.Domain.NativeModels;
using NUnit.Framework;

namespace CSI.BatchTracker.Tests.Domain
{
    [TestFixture]
    class BatchOperatorValidatorTest
    {
        BatchOperator batchOperator;
        BatchOperatorValidator validator;

        [SetUp]
        public void SetUp()
        {
            validator = new BatchOperatorValidator();
        }

        [Test]
        public void BatchOperatorValidatesSuccessfully()
        {
            batchOperator = new BatchOperator("Jane", "Doe");
            Assert.True(validator.Validate(batchOperator));
        }

        [Test]
        public void BatchOperatorNotValidIfFirstNameEmpty()
        {
            batchOperator = new BatchOperator("", "Doe");
            Assert.False(validator.Validate(batchOperator));
        }

        [Test]
        public void BatchOperatorNotValidIfFirstNameIsWhitespace()
        {
            batchOperator = new BatchOperator(" ", "Doe");
            Assert.False(validator.Validate(batchOperator));
        }

        [Test]
        public void BatchOperatorNotValidIfFirstNameIsNull()
        {
            batchOperator = new BatchOperator(null, "Doe");
            Assert.False(validator.Validate(batchOperator));
        }

        [Test]
        public void BatchOperatorNotValidIfLastNameEmpty()
        {
            batchOperator = new BatchOperator("Jane", "");
            Assert.False(validator.Validate(batchOperator));
        }

        [Test]
        public void BatchOperatorNotValidIfLastNameIsWhitespace()
        {
            batchOperator = new BatchOperator("Jane", " ");
            Assert.False(validator.Validate(batchOperator));
        }

        [Test]
        public void BatchOperatorNotValidIfLastNameIsNull()
        {
            batchOperator = new BatchOperator("Jane", "");
            Assert.False(validator.Validate(batchOperator));
        }
    }
}
