// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using NUnit.Framework;
using System.Collections.ObjectModel;

namespace CableSector.ViewModels
{
    /// <summary>
    /// Класс-тест "Массив значений длин".
    /// </summary>
    [TestFixture]
    public class ArrayRecordTests
    {
        /// <summary>
        /// Setup private fields.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            TestContext.WriteLine(@"--------------------------------------------------------------------------------");
            TestContext.WriteLine($@"{nameof(Setup)} start.");
            TestContext.WriteLine($@"{nameof(Setup)} complete.");
        }

        /// <summary>
        /// Reset private fields to default state.
        /// </summary>
        [TearDown]
        public void Teardown()
        {
            TestContext.WriteLine(@"--------------------------------------------------------------------------------");
            TestContext.WriteLine($@"{nameof(Teardown)} start.");
            TestContext.WriteLine($@"{nameof(Teardown)} complete.");
            TestContext.WriteLine(@"--------------------------------------------------------------------------------");
        }

        [Test]
        public void Sum_AreEqual()
        {
            TestContext.WriteLine(@"--------------------------------------------------------------------------------");
            TestContext.WriteLine($@"{nameof(Sum_AreEqual)} start.");

            var _records = new ObservableCollection<string> { "21", "22", "23", "24", "25", "26", "27", "28", "29", "20" };
            ArrayRecord _arrayRecord = new ArrayRecord("1", _records);
            var actual = _arrayRecord.CalcSum;
            TestContext.WriteLine($"actual: {actual}");
            var expected = 245;
            TestContext.WriteLine($"expected: {expected}");
            Assert.AreEqual(expected, actual);

            _arrayRecord = new ArrayRecord("1");
            actual = _arrayRecord.CalcSum;
            TestContext.WriteLine($"actual: {actual}");
            expected = 0;
            TestContext.WriteLine($"expected: {expected}");
            Assert.AreEqual(expected, actual);

            TestContext.WriteLine($@"{nameof(Sum_AreEqual)} complete.");
        }

        [Test]
        public void UpdateItem_AreEqual()
        {
            TestContext.WriteLine(@"--------------------------------------------------------------------------------");
            TestContext.WriteLine($@"{nameof(UpdateItem_AreEqual)} start.");

            var _records = new ObservableCollection<string> { "21", "22", "23", "24", "25", "26", "27", "28", "29", "20" };
            ArrayRecord _arrayRecord = new ArrayRecord("1", _records);
            _arrayRecord.UpdateItem(0, "99");
            var actual = _arrayRecord.Items[0];
            TestContext.WriteLine($"actual: {actual}");
            var expected = "99";
            TestContext.WriteLine($"expected: {expected}");
            Assert.AreEqual(expected, actual);

            _arrayRecord = new ArrayRecord("1");
            _arrayRecord.UpdateItem(0, "100");
            _arrayRecord.UpdateItem(3, "10");
            actual = _arrayRecord.CalcSum.ToString();
            TestContext.WriteLine($"actual: {actual}");
            expected = "110";
            TestContext.WriteLine($"expected: {expected}");
            Assert.AreEqual(expected, actual);

            TestContext.WriteLine($@"{nameof(UpdateItem_AreEqual)} complete.");
        }
    }
}
