using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DriveParts.Entities;

namespace DrivePartsTests
{
    [TestClass]
    public class HashTester
    {
        [TestMethod]
        public void TestValidHash()
        {
            string hash = "ISMvKXpXpadDiUoOSoAfww==";
            string pass = "admin";

            bool expexted = true;
            bool actual = AuthChecker.GetHash(pass) == hash;

            Assert.AreEqual(expexted, actual);
        }

        public void TestWrongHash()
        {
            string hash = "asdFsFASdffasdf23423423sdffsd=";
            string pass = "admin";

            bool expexted = false;
            bool actual = AuthChecker.GetHash(pass) == hash;

            Assert.AreEqual(expexted, actual);
        }
    }
}
