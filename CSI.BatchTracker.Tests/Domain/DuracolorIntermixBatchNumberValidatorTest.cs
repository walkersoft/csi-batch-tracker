﻿using CSI.BatchTracker.Domain;
using CSI.BatchTracker.Domain.Contracts;
using CSI.BatchTracker.Exceptions;
using NUnit.Framework;

namespace CSI.BatchTracker.Tests.Domain
{
    [TestFixture]
    class DuracolorIntermixBatchNumberValidatorTest
    {
        readonly int standardLength = 12;
        IBatchNumberValidator validator;

        [SetUp]
        public void SetUp()
        {
            validator = new DuracolorIntermixBatchNumberValidator(standardLength);
        }

        [Test]
        public void DefaultBatchLengthSetup()
        {
            int expected = 12;
            DuracolorIntermixBatchNumberValidator plainValidator = new DuracolorIntermixBatchNumberValidator();

            Assert.AreEqual(expected, plainValidator.BatchNumberLength);
        }

        [Test]
        public void BatchNumberLengthIsSet()
        {
            Assert.AreEqual(standardLength, validator.GetBatchNumberLength());
        }

        [Test]
        public void BatchNumberValidatedAsCorrect()
        {
            string batch = "872880508201";
            Assert.True(validator.Validate(batch));
        }

        [Test]
        public void ExceptionWhenBatchNumberLengthIsZero()
        {
            Assert.Throws<BatchNumberValidationException>(() => new DuracolorIntermixBatchNumberValidator(0));
        }

        [Test]
        public void ExceptionWhenBatchNumberLengthIsLessThanZero()
        {
            Assert.Throws<BatchNumberValidationException>(() => new DuracolorIntermixBatchNumberValidator(-1));
        }

        [Test]
        public void BatchIsFromLouisville()
        {
            //Batches from Lousiville start with 8728
            string batch = "872880508201";
            Assert.True(validator.Validate(batch));
        }

        [Test]
        public void BatchIsFromHuron()
        {
            //Batches from Huron start with 8728
            string batch = "872280402305";
            Assert.True(validator.Validate(batch));
        }

        [Test]
        public void FailureIfBatchNumberNotFromKnownFacility()
        {
            //Known is Louisville or Huron, so anything that doesn't start with 8728 or 8722 is a fail
            string batch = "872581302501";
            Assert.False(validator.Validate(batch));
        }

        [Test]
        public void FalseIfBatchIsNotFormedCorrectly()
        {
            string batch = "foobar";
            Assert.False(validator.Validate(batch));
        }
    }
}
