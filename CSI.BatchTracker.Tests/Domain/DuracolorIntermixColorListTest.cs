using CSI.BatchTracker.Domain;
using CSI.BatchTracker.Domain.Contracts;
using NUnit.Framework;

namespace CSI.BatchTracker.Tests.Domain
{
    [TestFixture]
    class DuracolorIntermixColorListTest
    {
        IColorList colors;

        [Test]
        public void DuracolorListIsNineColors()
        {
            int expectedCount = 9;
            colors = new DuracolorIntermixColorList();

            Assert.AreEqual(expectedCount, colors.Count);
        }

        [Test]
        public void DuracolorListColorNamesAreCorrect()
        {
            colors = new DuracolorIntermixColorList();
            string[] colorNames = new string[9]
            {
                "White", "Black", "Yellow", "Red", "Blue Red", "Deep Blue", "Deep Green", "Bright Red", "Bright Yellow"
            };

            for (int i = 0; i < colorNames.Length; i++)
            {
                Assert.AreEqual(colorNames[i], colors.Colors[i]);
            }
        }
    }
}
