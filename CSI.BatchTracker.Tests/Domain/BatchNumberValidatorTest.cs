using NUnit.Framework;
using CSI.BatchTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.Domain
{
    [TestFixture]
    public class BatchNumberValidatorTest
    {
        [Test]
        public void ValidBatchNumber()
        {
            BatchNumberValidator validator = new BatchNumberValidator();
            Assert.True(validator.Validate("872880407301"));
        }
    }
}
