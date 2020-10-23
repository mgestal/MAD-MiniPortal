using Es.Udc.DotNet.MiniPortal.Model;
using Es.Udc.DotNet.MiniPortal.Model.UserProfileDao;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net.Sockets;
using System.Transactions;
using Es.Udc.DotNet.ModelUtil.Dao;
using Ninject.Activation;

namespace Es.Udc.DotNet.MiniPortal.Test
{
    /// <summary>
    ///This is a test class for IGenericDaoTest and is intended
    ///to contain all IGenericDaoTest Unit Tests
    ///</summary>
    [TestClass()]
    public class IGenericDaoTest
    {
        private static IKernel kernel;

        private TestContext testContextInstance;
        private UserProfile userProfile;
        private static IUserProfileDao userProfileDao;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        #region Additional test attributes

        //
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            kernel = TestManager.ConfigureNInjectKernel();

            userProfileDao = kernel.Get<IUserProfileDao>();
        }

        //
        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            TestManager.ClearNInjectKernel(kernel);
        }

        //
        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
            userProfile = new UserProfile();
            userProfile.loginName = "jsmith";
            userProfile.enPassword = "password";
            userProfile.firstName = "John";
            userProfile.lastName = "Smith";
            userProfile.email = "jsmith@acme.com";
            userProfile.language = "en";
            userProfile.country = "US";

            userProfileDao.Create(userProfile);
        }

        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            try
            {
                userProfileDao.Remove(userProfile.usrId);
            }
            catch (Exception)
            {
            }
        }

        #endregion Additional test attributes

        [TestMethod()]
        public void DAO_FindTest()
        {
            try
            {
                UserProfile actual = userProfileDao.Find(userProfile.usrId);

                Assert.AreEqual(userProfile, actual, "User found does not correspond with the original one.");
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod()]
        public void DAO_ExistsTest()
        {
            try
            {
                bool userExists = userProfileDao.Exists(userProfile.usrId);

                Assert.IsTrue(userExists);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod()]
        public void DAO_NotExistsTest()
        {
            try
            {
                bool userNotExists = userProfileDao.Exists(-1);

                Assert.IsFalse(userNotExists);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        public void DAO_UpdateTest()
        {
            try
            {
                userProfile.firstName = "Juan";
                userProfile.lastName = "González";
                userProfile.email = "jgonzalez@acme.es";
                userProfile.language = "es";
                userProfile.country = "ES";
                userProfile.enPassword = "contraseña";

                userProfileDao.Update(userProfile);

                UserProfile actual = userProfileDao.Find(userProfile.usrId);

                Assert.AreEqual(userProfile, actual);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        /// <summary>
        ///A test for Remove
        [TestMethod()]
        public void DAO_RemoveTest()
        {
            try
            {
                userProfileDao.Remove(userProfile.usrId);

                bool userExists = userProfileDao.Exists(userProfile.usrId);

                Assert.IsFalse(userExists);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        /// <summary>
        ///A test for Create
        ///</summary> 
        [TestMethod()]
        public void DAO_CreateTest()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                UserProfile newUserProfile = new UserProfile();
                newUserProfile.loginName = "login";
                newUserProfile.enPassword = "password";
                newUserProfile.firstName = "John";
                newUserProfile.lastName = "Smith";
                newUserProfile.email = "john.smith@acme.com";
                newUserProfile.language = "en";
                newUserProfile.country = "US";

                userProfileDao.Create(newUserProfile);

                bool userExists = userProfileDao.Exists(newUserProfile.usrId);

                Assert.IsTrue(userExists);

                // transaction.Complete() is not called, so Rollback is executed.
            }
        }

        /// <summary>
        ///A test for Attach
        ///</summary>
        [TestMethod()]
        public void DAO_AttachTest()
        {

            #region Long term context. It is stored in GenericDAO.Context property

            // First we get CommonContext from GenericDAO to check the entity states...
            DbContext dbContext = ((GenericDaoEntityFramework<UserProfile, Int64>)userProfileDao).Context;


            // 1) We look for an userProfile (it was previously created in MyTestInitialize();
            UserProfile user = userProfileDao.Find(userProfile.usrId);

            // Check the user is in the context (from Generic DAO) now after the Find() method execution
            Assert.AreEqual(dbContext.Entry(user).State, EntityState.Unchanged);

            // 2) We are going to delete the previously recovered userProfile
            userProfileDao.Remove(user.usrId);

            // Check the user is not in the context now (EntityState.Detached notes that entity is not tracked by the context)
            Assert.AreEqual(dbContext.Entry(user).State, EntityState.Detached);

            // The user was deleted from database
            Assert.IsFalse(userProfileDao.Exists(user.usrId));

            // 3) So, the user entity is already in memory, but it does not exist neither in the context nor the database.
            // Now, if we attach the entity it will be tracked again by the context...
            userProfileDao.Attach(user);
            
            // EntityState.Unchanged = entity exists in context and in DataBase with the same values 
            Assert.AreEqual(dbContext.Entry(user).State, EntityState.Unchanged);

            // ... and stored in database too
            Assert.IsTrue(userProfileDao.Exists(user.usrId));


            #endregion


            #region Short Term Context (context is disposed after using{} block)

            user = null;

            // If we retrieve the user within a short term context it will only be tracked by the local-context
            // inside the block. It will not be within the generic context

            using (MiniPortalEntities shortTermContext = new MiniPortalEntities())
            {
                DbSet<UserProfile> userProfiles = shortTermContext.UserProfiles;

                user =
                    (from u in userProfiles
                        where u.usrId == userProfile.usrId
                        select u).FirstOrDefault();

                Assert.AreEqual(
                    shortTermContext.ChangeTracker.Entries<UserProfile>().FirstOrDefault().State, 
                    EntityState.Unchanged);

            }

            // EntityState.Detached notes that entity is not tracked by the context
            Assert.AreEqual(dbContext.Entry(user).State, EntityState.Detached);
            
            #endregion


        }
    }

}