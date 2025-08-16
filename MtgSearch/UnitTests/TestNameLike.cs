using MtgSearch.Server.Models.Data;
using MtgSearch.Server.Models.Logic.Predicates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class TestNameLike
    {
        ServerCardModel TestCard => new("Tezzeret, The Seeker");
        ServerCardModel TestCard2 => new("Gideon's Hammer");
        ServerCardModel TrapCard => new("Gideon the Hammer the Man");
        [TestMethod, Timeout(500)]
        public void TestSingleWord()
        {
            string query = "tezzret";
            var pred = new NameLikePredicate(query);
            Assert.IsTrue(pred.Apply(TestCard), query);
        }
        [TestMethod, Timeout(500)]
        //[TestMethod]
        public void TestTwoWord()
        {
            string query = "the seaker";
            var pred = new NameLikePredicate(query);
            Assert.IsTrue(pred.Apply(TestCard), query);
        }
        [TestMethod, Timeout(500)]
        public void TestSingleWordSymbol()
        {
            string query = "gideon";
            var pred = new NameLikePredicate(query);
            Assert.IsTrue(pred.Apply(TestCard2), query);
        }
        [TestMethod, Timeout(500)]
        public void TestMultiWordSymbol()
        {
            string query = "gideon hamer";
            var pred = new NameLikePredicate(query);
            Assert.IsTrue(pred.Apply(TestCard2), query);
        }
        //matches the first 'the', can it find the second for the full match?
        [TestMethod, Timeout(500)]
        public void TestFalsePositive()
        {
            string query = "the man";
            var pred = new NameLikePredicate(query);
            Assert.IsTrue(pred.Apply(TrapCard), query);
        }
    }
}
