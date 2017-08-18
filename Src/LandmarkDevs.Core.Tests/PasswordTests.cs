using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LandmarkDevs.Core.Security.Crypto;

namespace LandmarkDevs.Core.Tests
{
    [TestClass]
    public class PasswordTests
    {
        [TestMethod]
        public void Test_That_Hash_Is_Created()
        {
            var password = "tms58431";
            var hashedPassword = PasswordManager.HashPassword(password);
            Console.WriteLine(hashedPassword);
            System.Diagnostics.Debug.WriteLine(hashedPassword);
            Assert.IsNotNull(hashedPassword);
            Assert.IsInstanceOfType(hashedPassword, typeof(string));
        }

        [TestMethod]
        public void Test_That_Password_Is_Authenticated_When_Password_Is_Correct()
        {
            var password = "password";
            var hashedPassword = PasswordManager.HashPassword(password);
            var validated = PasswordManager.ValidatePassword(password, hashedPassword);
            Assert.IsTrue(validated);
        }

        [TestMethod]
        public void Test_That_Password_Is_Not_Authenticated_When_Password_Is_Incorrect()
        {
            var password = "password";
            var badPassword1 = "Password";
            var badPassword2 = "PASSWORD";
            var badPassword3 = "P@$$w04d";
            var hashedPassword = PasswordManager.HashPassword(password);
            var validated1 = PasswordManager.ValidatePassword(badPassword1, hashedPassword);
            var validated2 = PasswordManager.ValidatePassword(badPassword2, hashedPassword);
            var validated3 = PasswordManager.ValidatePassword(badPassword3, hashedPassword);
            Assert.IsFalse(validated1);
            Assert.IsFalse(validated2);
            Assert.IsFalse(validated3);
        }

        [TestMethod]
        public void Test_That_Hashes_For_The_Same_Password_Are_Not_Equal()
        {
            var password = "password";
            var hashedPassword = PasswordManager.HashPassword(password);
            System.Threading.Thread.Sleep(1000);
            var hashedPassword2 = PasswordManager.HashPassword(password);
            Assert.AreNotEqual(hashedPassword, hashedPassword2);
        }
    }
}
