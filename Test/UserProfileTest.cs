using System;
using System.Diagnostics;
using System.Security.Cryptography;
using Es.Udc.DotNet.MiniPortal.Model;
using Es.Udc.DotNet.MiniPortal.Model.UserService.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Es.Udc.DotNet.MiniPortal.Test
{
    [TestClass]
    public class UserProfileTest
    {

        // Variables used in several tests are initialized here
        private const String loginName = "loginNameTest";

        private const String clearPassword = "password";
        private const String firstName = "name";
        private const String lastName = "lastName";
        private const String email = "user@udc.es";
        private const String language = "es";
        private const String country = "ES";
        private const long NON_EXISTENT_USER_ID = -1;

        [TestMethod]
        public void EqualsCompareSameUserTest()
        {

            UserProfile userA = new UserProfile();
            userA.usrId = 1;
            userA.loginName = loginName;
            userA.firstName = firstName;
            userA.lastName = lastName;
            userA.language = language;
            userA.country = country;
            userA.email = email;
            userA.enPassword = PasswordEncrypter.Crypt(clearPassword);

            UserProfile userB = new UserProfile();
            userB.usrId = 1;
            userB.loginName = loginName;
            userB.firstName = firstName;
            userB.lastName = lastName;
            userB.language = language;
            userB.country = country;
            userB.email = email;
            userB.enPassword = PasswordEncrypter.Crypt(clearPassword);

            Assert.IsTrue(userA.Equals(userB));

        }

        [TestMethod]
        public void EqualsCompareSameInstanceTest()
        {

            UserProfile userA = new UserProfile();
            userA.usrId = 1;
            userA.loginName = loginName;
            userA.firstName = firstName;
            userA.lastName = lastName;
            userA.language = language;
            userA.country = country;
            userA.email = email;
            userA.enPassword = PasswordEncrypter.Crypt(clearPassword);

            Assert.IsTrue(userA.Equals(userA));

        }

        [TestMethod]
        public void EqualsCompareDifferentUsersTest()
        {

            UserProfile userA = new UserProfile();
            userA.usrId = 1;
            userA.loginName = loginName;
            userA.firstName = firstName;
            userA.lastName = lastName;
            userA.language = language;
            userA.country = country;
            userA.email = email;
            userA.enPassword = PasswordEncrypter.Crypt(clearPassword);

            UserProfile userB = new UserProfile();
            userB.usrId = 2;
            userB.loginName = loginName;
            userB.firstName = firstName;
            userB.lastName = lastName;
            userB.language = language;
            userB.country = country;
            userB.email = email;
            userB.enPassword = PasswordEncrypter.Crypt(clearPassword);

            Assert.IsFalse(userA.Equals(userB));

        }

        [TestMethod]
        public void GetHashCodeTest()
        {

            UserProfile userA = new UserProfile();
            userA.usrId = 1;
            userA.loginName = loginName;
            userA.firstName = firstName;
            userA.lastName = lastName;
            userA.language = language;
            userA.country = country;
            userA.email = email;
            userA.enPassword = PasswordEncrypter.Crypt(clearPassword);

            UserProfile userB = new UserProfile();
            userB.usrId = 1;
            userB.loginName = loginName;
            userB.firstName = firstName;
            userB.lastName = lastName;
            userB.language = language;
            userB.country = country;
            userB.email = email;
            userB.enPassword = PasswordEncrypter.Crypt(clearPassword);

            Assert.AreEqual(userA.GetHashCode(), userB.GetHashCode());

        }

        [TestMethod]
        public void ToStringTest()
        {
            UserProfile userA = new UserProfile();
            userA.usrId = 1;
            userA.loginName = loginName;
            userA.firstName = firstName;
            userA.lastName = lastName;
            userA.language = language;
            userA.country = country;
            userA.email = email;
            userA.enPassword = PasswordEncrypter.Crypt(clearPassword);

            String toStringOutput = userA.ToString();

            Assert.IsNotNull(toStringOutput);

            Assert.IsTrue(toStringOutput.Contains(Convert.ToString(userA.usrId)));
            Assert.IsTrue(toStringOutput.Contains(userA.loginName));
            Assert.IsTrue(toStringOutput.Contains(userA.firstName));
            Assert.IsTrue(toStringOutput.Contains(userA.lastName));
            Assert.IsTrue(toStringOutput.Contains(userA.language));
            Assert.IsTrue(toStringOutput.Contains(userA.country));
            Assert.IsTrue(toStringOutput.Contains(userA.email));
            Assert.IsTrue(toStringOutput.Contains(userA.enPassword));

        }
    }
}
