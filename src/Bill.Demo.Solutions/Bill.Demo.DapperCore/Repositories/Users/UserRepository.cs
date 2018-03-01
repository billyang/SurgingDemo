using Bill.Demo.Core.Authorization.Users;
using Dapper;
using Surging.Core.CPlatform.Ioc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Bill.Demo.DapperCore.Repositories.Users
{
    public class UserRepository: BaseRepository, IBaseRepository<User>
    {
        /// <summary>
        /// 创建一个用户
        /// </summary>
        /// <param name="entity">用户</param>
        /// <param name="connectionString">链接字符串</param>
        /// <returns></returns>
        public Task<Boolean> CreateEntity(User entity, String connectionString = null)
        {
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection(connectionString))
            {
                string insertSql = @"INSERT  INTO dbo.auth_User
                                    ( TenantId ,
                                      Name ,
                                      Password ,
                                      SecurityStamp ,
                                      FullName ,
                                      Surname ,
                                      PhoneNumber ,
                                      IsPhoneNumberConfirmed ,
                                      EmailAddress ,
                                      IsEmailConfirmed ,
                                      EmailConfirmationCode ,
                                      IsActive ,
                                      PasswordResetCode ,
                                      LastLoginTime ,
                                      IsLockoutEnabled ,
                                      AccessFailedCount ,
                                      LockoutEndDateUtc
                                    )
                            VALUES  ( @tenantid ,
                                      @name ,
                                      @password ,
                                      @securitystamp ,
                                      @fullname ,
                                      @surname ,
                                      @phonenumber ,
                                      @isphonenumberconfirmed ,
                                      @emailaddress ,
                                      @isemailconfirmed ,
                                      @emailconfirmationcode ,
                                      @isactive ,
                                      @passwordresetcode ,
                                      @lastlogintime ,
                                      @islockoutenabled ,
                                      @accessfailedcount ,
                                      @lockoutenddateutc
                                    );";
                return Task.FromResult<Boolean>(conn.Execute(insertSql, entity) > 0);
            }
        }

        /// <summary>
        /// 根据主键Id删除一个用户
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <param name="connectionString">链接字符串</param>
        /// <returns></returns>
        public Task<Boolean> Delete(Int64 id, String connectionString = null)
        {
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection(connectionString))
            {
                string deleteSql = @"DELETE FROM [dbo].[auth_User]
                                            WHERE Id = @Id";
                return Task.FromResult<Boolean>(conn.Execute(deleteSql, new { Id = id }) > 0);
            }
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <param name="connectionString">链接字符串</param>
        /// <returns></returns>
        public Task<IEnumerable<User>> GetAllEntity(String connectionString = null)
        {
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection(connectionString))
            {
                string querySql = @"SELECT  Id ,
                                    TenantId ,
                                    Name ,
                                    Password ,
                                    SecurityStamp ,
                                    FullName ,
                                    Surname ,
                                    PhoneNumber ,
                                    IsPhoneNumberConfirmed ,
                                    EmailAddress ,
                                    IsEmailConfirmed ,
                                    EmailConfirmationCode ,
                                    IsActive ,
                                    PasswordResetCode ,
                                    LastLoginTime ,
                                    IsLockoutEnabled ,
                                    AccessFailedCount ,
                                    LockoutEndDateUtc
                            FROM    dbo.auth_User;";
                return Task.FromResult<IEnumerable<User>>(conn.Query<User>(querySql));
            }
        }

        /// <summary>
        /// 根据主键Id获取一个用户
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <param name="connectionString">链接字符串</param>
        /// <returns></returns>
        public Task<User> GetEntityById(Int64 id, String connectionString = null)
        {
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection(connectionString))
            {
                string querySql = @"SELECT  Id ,
                                    TenantId ,
                                    Name ,
                                    Password ,
                                    SecurityStamp ,
                                    FullName ,
                                    Surname ,
                                    PhoneNumber ,
                                    IsPhoneNumberConfirmed ,
                                    EmailAddress ,
                                    IsEmailConfirmed ,
                                    EmailConfirmationCode ,
                                    IsActive ,
                                    PasswordResetCode ,
                                    LastLoginTime ,
                                    IsLockoutEnabled ,
                                    AccessFailedCount ,
                                    LockoutEndDateUtc
                            FROM    dbo.auth_User 
                            WHERE Id = @Id";
                return Task.FromResult<User>(conn.QueryFirstOrDefault<User>(querySql, new { Id = id }));
            }
        }

        /// <summary>
        /// 修改一个用户
        /// </summary>
        /// <param name="entity">要修改的用户</param>
        /// <param name="connectionString">链接字符串</param>
        /// <returns></returns>
        public Task<Boolean> Update(User entity, String connectionString = null)
        {
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection(connectionString))
            {
                string updateSql = @"UPDATE dbo.auth_User
                                    SET TenantId = @tenantid,
	                                    Name = @name,
	                                    Password = @password,
	                                    SecurityStamp = @securitystamp,
	                                    FullName = @fullname,
	                                    Surname = @surname,
	                                    PhoneNumber = @phonenumber,
	                                    IsPhoneNumberConfirmed = @isphonenumberconfirmed,
	                                    EmailAddress = @emailaddress,
	                                    IsEmailConfirmed = @isemailconfirmed,
	                                    EmailConfirmationCode = @emailconfirmationcode,
	                                    IsActive = @isactive,
	                                    PasswordResetCode = @passwordresetcode,
	                                    LastLoginTime = @lastlogintime,
	                                    IsLockoutEnabled = @islockoutenabled,
	                                    AccessFailedCount = @accessfailedcount,
	                                    LockoutEndDateUtc = @lockoutenddateutc
                                     WHERE Id = @Id";
                return Task.FromResult<Boolean>(conn.Execute(updateSql, entity) > 0);
            }
        }
    }
}
