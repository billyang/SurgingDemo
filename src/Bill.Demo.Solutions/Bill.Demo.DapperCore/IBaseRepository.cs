using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bill.Demo.DapperCore
{
    public interface IBaseRepository<T> where T : class
    {
        /// <summary>
        /// 添加一个实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        Task<Boolean> CreateEntity(T entity, String connectionString = null);

        /// <summary>
        /// 根据主键Id获取一个实体
        /// </summary>
        /// <param name="id"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        Task<T> GetEntityById(Int64 id, String connectionString = null);

        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllEntity(String connectionString = null);

        /// <summary>
        /// 修改一个实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        Task<Boolean> Update(T entity, String connectionString = null);

        /// <summary>
        /// 根据主键Id删除一个实体
        /// </summary>
        /// <param name="id"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        Task<Boolean> Delete(Int64 id, String connectionString = null);
    }
}
