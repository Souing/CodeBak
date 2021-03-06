﻿using Raven.Client;
using Raven.Client.Linq;
using SD.Infrastructure.EntityBase;
using SD.Infrastructure.Repository.RavenDB.Base;
using SD.Infrastructure.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SD.Infrastructure.Repository.RavenDB
{
    /// <summary>
    /// RavenDB单元事务Provider
    /// </summary>
    public abstract class RavenUnitOfWorkProvider : IUnitOfWork
    {
        #region # 创建RavenDB会话对象

        /// <summary>
        /// RavenDB会话对象
        /// </summary>
        private readonly IDocumentSession _dbContext;

        /// <summary>
        /// 构造器
        /// </summary>
        protected RavenUnitOfWorkProvider()
        {
            this._dbContext = RavenDbSession.CommandInstance;
        }

        /// <summary>
        /// 析构器
        /// </summary>
        ~RavenUnitOfWorkProvider()
        {
            this.Dispose();
        }

        #endregion


        //Public

        #region # 注册添加单个实体对象 —— void RegisterAdd<T>(T entity)
        /// <summary>
        /// 注册添加单个实体对象
        /// </summary>
        /// <typeparam name="T">聚合根类型</typeparam>
        /// <param name="entity">新实体对象</param>
        /// <exception cref="ArgumentNullException">新实体对象为空</exception>
        public void RegisterAdd<T>(T entity) where T : AggregateRootEntity
        {
            #region # 验证参数

            if (entity == null)
            {
                throw new ArgumentNullException("entity", string.Format(@"要添加的{0}实体对象不可为空！", typeof(T).Name));
            }
            if (entity.Id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", string.Format(@"要添加的{0}实体对象Id不可为空！", typeof(T).Name));
            }
            if (this.Exists<T>(x => x.Id == entity.Id))
            {
                throw new ArgumentOutOfRangeException("Id", string.Format("Id为\"{0}\"的实体已存在！", entity.Id));
            }

            #endregion

            this._dbContext.Store(entity);
        }
        #endregion

        #region # 注册添加实体对象集合 —— void RegisterAddRange<T>(IEnumerable<T> entities)
        /// <summary>
        /// 注册添加实体对象集合
        /// </summary>
        /// <typeparam name="T">聚合根类型</typeparam>
        /// <param name="entities">实体对象集合</param>
        /// <exception cref="ArgumentNullException">实体对象集合为null或长度为0</exception>
        public void RegisterAddRange<T>(IEnumerable<T> entities) where T : AggregateRootEntity
        {
            #region # 验证参数

            if (entities == null || !entities.Any())
            {
                throw new ArgumentNullException("entities", string.Format("要添加的{0}实体对象集合不可为空！", typeof(T).Name));
            }

            #endregion

            foreach (T entity in entities)
            {
                this.RegisterAdd(entity);
            }
        }
        #endregion

        #region # 注册保存单个实体对象 —— void RegisterSave<T>(T entity)
        /// <summary>
        /// 注册保存单个实体对象
        /// </summary>
        /// <typeparam name="T">聚合根类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <exception cref="ArgumentNullException">实体对象为空</exception>
        /// <exception cref="NullReferenceException">要保存的对象不存在</exception>
        public void RegisterSave<T>(T entity) where T : AggregateRootEntity
        {
            #region # 验证参数

            if (entity == null)
            {
                throw new ArgumentNullException("entity", string.Format("要保存的{0}实体对象不可为空！", typeof(T).Name));
            }

            if (entity.Id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", string.Format(@"要保存的{0}实体对象Id不可为空！", typeof(T).Name));
            }

            if (!this.Exists<T>(entity.Id))
            {
                throw new NullReferenceException(string.Format("不存在Id为{0}的{1}实体对象，请尝试添加操作！", entity.Id, typeof(T).Name));
            }

            #endregion
        }
        #endregion

        #region # 注册保存实体对象集合 —— void RegisterSaveRange<T>(IEnumerable<T> entities)
        /// <summary>
        /// 注册保存实体对象集合
        /// </summary>
        /// <typeparam name="T">聚合根类型</typeparam>
        /// <param name="entities">实体对象集合</param>
        /// <exception cref="ArgumentNullException">实体对象集合</exception>
        /// <exception cref="NullReferenceException">要保存的对象不存在</exception>
        public void RegisterSaveRange<T>(IEnumerable<T> entities) where T : AggregateRootEntity
        {
            #region # 验证参数

            if (entities == null || !entities.Any())
            {
                throw new ArgumentNullException("entities", string.Format("要保存的{0}实体对象集合不可为空！", typeof(T).Name));
            }

            #endregion

            foreach (T entity in entities)
            {
                this.RegisterSave(entity);
            }
        }
        #endregion

        #region # 注册删除单个实体对象（物理删除） —— void RegisterPhysicsRemove<T>(Guid id)
        /// <summary>
        /// 注册删除单个实体对象（物理删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">标识Id</param>
        /// <exception cref="ArgumentNullException">id为空</exception>
        /// <exception cref="NullReferenceException">要删除的对象不存在</exception>
        public void RegisterPhysicsRemove<T>(Guid id) where T : AggregateRootEntity
        {
            #region # 验证参数

            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("id", string.Format("要删除的{0}实体对象id不可为空！", typeof(T).Name));
            }

            if (!this.Exists<T>(id))
            {
                throw new NullReferenceException(string.Format("Id为{0}的{1}实体对象不存在，请重试！", id, typeof(T).Name));
            }

            #endregion

            T entity = this._dbContext.Query<T>().Single(x => x.Id == id);
            this._dbContext.Delete(entity);
        }
        #endregion

        #region # 注册删除单个实体对象（物理删除） —— void RegisterPhysicsRemove<T>(string number)
        /// <summary>
        /// 注册删除单个实体对象（物理删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="number">编号</param>
        /// <exception cref="ArgumentNullException">编号为空</exception>
        /// <exception cref="NullReferenceException">要删除的对象不存在</exception>
        public void RegisterPhysicsRemove<T>(string number) where T : AggregateRootEntity
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentNullException("number", string.Format("要删除的{0}实体对象编号不可为空！", typeof(T).Name));
            }

            if (!this.Exists<T>(number))
            {
                throw new NullReferenceException(string.Format("编号为{0}的{1}实体对象不存在，请重试！", number, typeof(T).Name));
            }

            #endregion

            T entity = this._dbContext.Query<T>().Single(x => x.Number == number);
            this._dbContext.Delete(entity);
        }
        #endregion

        #region # 注册删除单个实体对象（物理删除） —— void RegisterPhysicsRemove<T>(T entity)
        /// <summary>
        /// 注册删除单个实体对象（物理删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        public void RegisterPhysicsRemove<T>(T entity) where T : AggregateRootEntity
        {
            #region # 验证参数

            if (entity == null)
            {
                throw new ArgumentNullException("entity", string.Format(@"要删除的{0}实体对象不可为空！", typeof(T).Name));
            }

            #endregion

            this._dbContext.Delete(entity);
        }
        #endregion

        #region # 注册删除多个实体对象（物理删除） —— void RegisterPhysicsRemoveRange<T>(IEnumerable<Guid> ids)
        /// <summary>
        /// 注册删除多个实体对象（物理删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="ids">标识Id集合</param>
        /// <exception cref="ArgumentNullException">ids为null或长度为0</exception>
        /// <exception cref="NullReferenceException">要删除的对象不存在</exception>
        public void RegisterPhysicsRemoveRange<T>(IEnumerable<Guid> ids) where T : AggregateRootEntity
        {
            #region # 验证参数

            if (ids == null || !ids.Any())
            {
                throw new ArgumentNullException("ids", string.Format("要删除的{0}的id集合不可为空！", typeof(T).Name));
            }

            #endregion

            foreach (Guid id in ids)
            {
                this.RegisterPhysicsRemove<T>(id);
            }
        }
        #endregion

        #region # 注册删除全部（物理删除） —— void RegisterPhysicsRemoveAll<T>()
        /// <summary>
        /// 注册删除全部（物理删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <exception cref="NullReferenceException">要删除的对象不存在</exception>
        public void RegisterPhysicsRemoveAll<T>() where T : AggregateRootEntity
        {
            IRavenQueryable<T> queryable = this.FindAll<T>();

            foreach (T entity in queryable)
            {
                this._dbContext.Delete(entity);
            }
        }
        #endregion

        #region # 注册删除单个实体对象（逻辑删除） —— void RegisterRemove<T>(Guid id)
        /// <summary>
        /// 注册删除单个实体对象（逻辑删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">标识Id</param>
        /// <exception cref="ArgumentNullException">id为空</exception>
        /// <exception cref="NullReferenceException">要删除的对象不存在</exception>
        public void RegisterRemove<T>(Guid id) where T : AggregateRootEntity
        {
            #region # 验证参数

            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("id", string.Format("要删除的{0}实体对象id不可为空！", typeof(T).Name));
            }
            if (!this.Exists<T>(id))
            {
                throw new NullReferenceException(string.Format("Id为{0}的{1}实体对象不存在，请重试！", id, typeof(T).Name));
            }

            #endregion

            T entity = this._dbContext.Load<T>(id);
            entity.Deleted = true;
            entity.DeletedTime = DateTime.Now;
        }
        #endregion

        #region # 注册删除单个实体对象（逻辑删除） —— void RegisterRemove<T>(string number)
        /// <summary>
        /// 注册删除单个实体对象（逻辑删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="number">编号</param>
        /// <exception cref="ArgumentNullException">编号为空</exception>
        /// <exception cref="NullReferenceException">要删除的对象不存在</exception>
        public void RegisterRemove<T>(string number) where T : AggregateRootEntity
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentNullException("number", string.Format(@"要删除的{0}实体对象编号不可为空！", typeof(T).Name));
            }

            if (!this.Exists<T>(number))
            {
                throw new NullReferenceException(string.Format("编号为{0}的{1}实体对象不存在，请重试！", number, typeof(T).Name));
            }

            #endregion

            T entity = this.Single<T>(x => x.Number == number);
            entity.Deleted = true;
            entity.DeletedTime = DateTime.Now;
        }
        #endregion

        #region # 注册删除单个实体对象（逻辑删除） —— void RegisterRemove<T>(T entity)
        /// <summary>
        /// 注册删除单个实体对象（逻辑删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        public void RegisterRemove<T>(T entity) where T : AggregateRootEntity
        {
            #region # 验证参数

            if (entity == null)
            {
                throw new ArgumentNullException("entity", string.Format(@"要删除的{0}实体对象不可为空！", typeof(T).Name));
            }

            #endregion

            entity.Deleted = true;
            entity.DeletedTime = DateTime.Now;
        }
        #endregion

        #region # 注册删除多个实体对象（逻辑删除） —— void RegisterRemoveRange<T>(IEnumerable<Guid> ids)
        /// <summary>
        /// 注册删除多个实体对象（逻辑删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="ids">标识Id集合</param>
        /// <exception cref="ArgumentNullException">ids为null或长度为0</exception>
        public void RegisterRemoveRange<T>(IEnumerable<Guid> ids) where T : AggregateRootEntity
        {
            #region # 验证参数

            if (ids == null || !ids.Any())
            {
                throw new ArgumentNullException("ids", string.Format("要删除的{0}的id集合不可为空！", typeof(T).Name));
            }

            #endregion

            foreach (Guid id in ids)
            {
                this.RegisterRemove<T>(id);
            }
        }
        #endregion

        #region # 注册删除全部（逻辑删除） —— void RegisterRemoveAll<T>()
        /// <summary>
        /// 注册删除全部（逻辑删除）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        public void RegisterRemoveAll<T>() where T : AggregateRootEntity
        {
            IRavenQueryable<T> queryable = this.FindAll<T>();

            foreach (T entity in queryable)
            {
                entity.Deleted = true;
                entity.DeletedTime = DateTime.Now; ;
            }
        }
        #endregion

        #region # 根据Id获取唯一实体对象（修改时用） —— T Resolve<T>(Guid id)
        /// <summary>
        /// 根据Id获取唯一实体对象（修改时用）
        /// </summary>
        /// <typeparam name="T">聚合根类型</typeparam>
        /// <param name="id">Id</param>
        /// <returns>唯一实体对象</returns>
        /// <exception cref="ArgumentNullException">id为空</exception>
        /// <exception cref="NullReferenceException">查询不到任何实体对象</exception>
        /// <exception cref="NotSupportedException">无法将表达式转换SQL语句</exception>
        /// <exception cref="InvalidOperationException">查询到1个以上的实体对象、查询到的实体对象已被删除</exception>
        public T Resolve<T>(Guid id) where T : AggregateRootEntity
        {
            #region # 验证参数

            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("id", string.Format("{0}的id不可为空！", typeof(T).Name));
            }
            if (!this.Exists<T>(id))
            {
                throw new NullReferenceException(string.Format("Id为{0}的{1}实体对象不存在，请重试！", id, typeof(T).Name));
            }

            #endregion

            return this._dbContext.Load<T>(id);
        }
        #endregion

        #region # 根据Id集获取实体对象集合（修改时用） —— IEnumerable<T> ResolveRange<T>(...
        /// <summary>
        /// 根据Id集获取实体对象集合（修改时用）
        /// </summary>
        /// <typeparam name="T">聚合根类型</typeparam>
        /// <param name="ids">Id集</param>
        /// <returns>实体对象集合</returns>
        public IEnumerable<T> ResolveRange<T>(IEnumerable<Guid> ids) where T : AggregateRootEntity
        {
            return this.ResolveRange<T>(x => ids.Contains(x.Id)).AsEnumerable();
        }
        #endregion

        #region # 根据编号获取唯一实体对象（修改时用） —— T Resolve<T>(string number)
        /// <summary>
        /// 根据编号获取唯一实体对象（修改时用）
        /// </summary>
        /// <typeparam name="T">聚合根类型</typeparam>
        /// <param name="number">编号</param>
        /// <returns>单个实体对象</returns>
        /// <exception cref="ArgumentNullException">编号为空</exception>
        public T Resolve<T>(string number) where T : AggregateRootEntity
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentNullException("number", string.Format("{0}的编号不可为空！", typeof(T).Name));
            }
            if (!this.Exists<T>(number))
            {
                throw new NullReferenceException(string.Format("编号为{0}的{1}实体对象不存在，请重试！", number, typeof(T).Name));
            }

            #endregion

            return this.Single<T>(x => x.Number == number);
        }
        #endregion

        #region # 根据编号集获取实体对象集合（修改时用） —— IEnumerable<T> ResolveRange<T>(...
        /// <summary>
        /// 根据编号集获取实体对象集合（修改时用）
        /// </summary>
        /// <typeparam name="T">聚合根类型</typeparam>
        /// <param name="numbers">编号集</param>
        /// <returns>实体对象集合</returns>
        public IEnumerable<T> ResolveRange<T>(IEnumerable<string> numbers) where T : AggregateRootEntity
        {
            return this.ResolveRange<T>(x => numbers.Contains(x.Number)).AsEnumerable();
        }
        #endregion

        #region # 获取全部实体对象（修改时用） —— IEnumerable<T> ResolveAll<T>()
        /// <summary>
        /// 获取全部实体对象（修改时用）
        /// </summary>
        /// <typeparam name="T">聚合根类型</typeparam>
        /// <returns>实体对象集合</returns>
        public IEnumerable<T> ResolveAll<T>() where T : AggregateRootEntity
        {
            return Queryable.Where(this._dbContext.Query<T>(), x => !x.Deleted).AsEnumerable();
        }
        #endregion

        #region # 统一事务处理保存更改 —— void Commit()
        /// <summary>
        /// 统一事务处理保存更改
        /// </summary>
        public void Commit()
        {
            try
            {
                //提交事务
                this._dbContext.SaveChanges();
            }
            catch
            {
                this.RollBack();
                throw;
            }
        }
        #endregion

        #region # 统一事务处理保存更改（异步） —— async Task CommitAsync()
        /// <summary>
        /// 统一事务处理保存更改（异步）
        /// </summary>
        public async Task CommitAsync()
        {
            try
            {
                //提交事务
                await Task.Run(() => this._dbContext.SaveChanges());
            }
            catch
            {
                this.RollBack();
                throw;
            }
        }
        #endregion

        #region # 统一事务回滚取消更改 —— void RollBack()
        /// <summary>
        /// 统一事务回滚取消更改
        /// </summary>
        public void RollBack()
        {

        }
        #endregion

        #region # 执行SQL命令（无需Commit） —— void ExecuteSqlCommand(string sql...
        /// <summary>
        /// 执行SQL命令（无需Commit）
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <exception cref="ArgumentNullException">SQL语句为空</exception>
        public void ExecuteSqlCommand(string sql, params object[] parameters)
        {
            throw new NotSupportedException("RavenDB不支持SQL命令！");
        }
        #endregion

        #region # 释放资源 —— void Dispose()
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (this._dbContext != null)
            {
                this._dbContext.Dispose();
            }
        }
        #endregion


        //Protected

        #region # 根据条件获取唯一实体对象（修改时用） —— T Single<T>(...
        /// <summary>
        /// 根据条件获取唯一实体对象（修改时用）
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>实体对象集合</returns>
        /// <exception cref="ArgumentNullException">条件表达式为空</exception>
        /// <exception cref="NullReferenceException">查询不到任何实体对象</exception>
        /// <exception cref="NotSupportedException">无法将表达式转换SQL语句</exception>
        /// <exception cref="InvalidOperationException">查询到1个以上的实体对象</exception>
        protected T Single<T>(Expression<Func<T, bool>> predicate) where T : AggregateRootEntity
        {
            #region # 验证参数

            if (predicate == null)
            {
                throw new ArgumentNullException("predicate", @"条件表达式不可为空！");
            }
            if (!this.Exists<T>(predicate))
            {
                throw new NullReferenceException(string.Format("给定的条件\"{0}\"中查询不到任何{1}实体对象！", predicate, typeof(T).Name));
            }
            if (this.Count(predicate) > 1)
            {
                throw new InvalidOperationException(string.Format("给定的条件\"{0}\"中查询到1个以上的{1}实体对象！", predicate, typeof(T).Name));
            }

            #endregion

            return this.FindAll<T>().SingleAsync(predicate).Result;
        }
        #endregion

        #region # 根据条件获取实体对象集合（修改时用） —— IRavenQueryable<T> ResolveRange<T>(...
        /// <summary>
        /// 根据条件获取实体对象集合（修改时用）
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>实体对象集合</returns>
        /// <exception cref="ArgumentNullException">条件表达式为空</exception>
        /// <exception cref="NotSupportedException">无法将表达式转换SQL语句</exception>
        /// <exception cref="InvalidOperationException">查询到1个以上的实体对象</exception>
        protected IRavenQueryable<T> ResolveRange<T>(Expression<Func<T, bool>> predicate) where T : AggregateRootEntity
        {
            #region # 验证参数

            if (predicate == null)
            {
                throw new ArgumentNullException("predicate", @"条件表达式不可为空！");
            }

            #endregion

            return this.FindAll<T>().Where(predicate);
        }
        #endregion


        //Private

        #region # 获取全部实体对象集合 —— IRavenQueryable<T> FindAll<T>()
        /// <summary>
        /// 获取全部实体对象集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns>实体对象集合</returns>
        private IRavenQueryable<T> FindAll<T>() where T : AggregateRootEntity
        {
            return this._dbContext.Query<T>().Where(x => !x.Deleted);
        }
        #endregion

        #region # 是否存在实体对象 —— bool Exists<T>(Guid id)
        /// <summary>
        /// 是否存在实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">标识Id</param>
        /// <returns>是否存在</returns>
        private bool Exists<T>(Guid id) where T : AggregateRootEntity
        {
            return this._dbContext.Load<T>(id) != null;
        }
        #endregion

        #region # 是否存在实体对象 —— bool Exists<T>(string number)
        /// <summary>
        /// 是否存在实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="number">编号</param>
        /// <returns>是否存在</returns>
        private bool Exists<T>(string number) where T : AggregateRootEntity
        {
            return this.Exists<T>(x => x.Number == number);
        }
        #endregion

        #region # 是否存在实体对象 —— bool Exists<T>(Expression<Func<T, bool>> predicate)
        /// <summary>
        /// 是否存在实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <returns>是否存在</returns>
        private bool Exists<T>(Expression<Func<T, bool>> predicate) where T : AggregateRootEntity
        {
            Task<bool> exists = this.FindAll<T>().AnyAsync(predicate);

            return exists.Result;
        }
        #endregion

        #region # 实体对象个数 —— int Count<T>(Expression<Func<T, bool>> predicate)
        /// <summary>
        /// 实体对象个数
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <returns>实体对象个数</returns>
        private int Count<T>(Expression<Func<T, bool>> predicate) where T : AggregateRootEntity
        {
            Task<int> exists = this.FindAll<T>().CountAsync(predicate);

            return exists.Result;
        }
        #endregion
    }
}
