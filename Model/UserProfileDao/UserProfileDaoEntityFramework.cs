using Es.Udc.DotNet.ModelUtil.Dao;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using System;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;

namespace Es.Udc.DotNet.MiniPortal.Model.UserProfileDao
{
    /// <summary>
    /// Specific Operations for UserProfile
    /// </summary>
    public class UserProfileDaoEntityFramework :
        GenericDaoEntityFramework<UserProfile, Int64>, IUserProfileDao
    {
        #region Public Constructors

        /// <summary>
        /// Public Constructor
        /// </summary>
        public UserProfileDaoEntityFramework()
        {
        }

        #endregion Public Constructors

        #region IUserProfileDao Members. Specific Operations

        /// <summary>
        /// Finds a UserProfile by his loginName
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        /// <exception cref="InstanceNotFoundException"></exception>
        public UserProfile FindByLoginName(string loginName)
        {
            UserProfile userProfile = null;

            #region Option 1: Using Linq.

            DbSet<UserProfile> userProfiles = Context.Set<UserProfile>();

            var result =
                (from u in userProfiles
                 where u.loginName == loginName
                 select u);

            userProfile = result.FirstOrDefault();

            #endregion Option 1: Using Linq.

            #region Option 2: Using eSQL over dbSet

            //string sqlQuery = "Select * FROM UserProfile where loginName=@loginName";
            //DbParameter loginNameParameter =
            //    new System.Data.SqlClient.SqlParameter("loginName", loginName);

            //userProfile = Context.Database.SqlQuery<UserProfile>(sqlQuery, loginNameParameter).FirstOrDefault<UserProfile>();

            #endregion Option 2: Using eSQL over dbSet

            #region Option 3: Using Entity SQL and Object Services provided by old ObjectContext.

            //String sqlQuery =
            //    "SELECT VALUE u FROM MiniPortalEntities.UserProfiles AS u " +
            //    "WHERE u.loginName=@loginName";

            //ObjectParameter param = new ObjectParameter("loginName", loginName);

            //ObjectQuery<UserProfile> query =
            //  ((System.Data.Entity.Infrastructure.IObjectContextAdapter)Context).ObjectContext.CreateQuery<UserProfile>(sqlQuery, param);

            //var result = query.Execute(MergeOption.AppendOnly);

            //try
            //{
            //    userProfile = result.First<UserProfile>();
            //}
            //catch (Exception)
            //{
            //    userProfile = null;
            //}

            #endregion Option 3: Using Entity SQL and Object Services provided by old ObjectContext.

            if (userProfile == null)
                throw new InstanceNotFoundException(loginName,
                    typeof(UserProfile).FullName);

            return userProfile;
        }

        #endregion IUserProfileDao Members
    }

}