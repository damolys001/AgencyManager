using DataFactory.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;


namespace DataFactory.Services
{
    public class ServiceManager<T> where T : class
    {
        private readonly AgencyManagerContext _db = new AgencyManagerContext();
        public T Add(T entity, ref string msg)
        {
            try
            {
                _db.Set<T>().Add(entity);
                _db.SaveChanges();
                return entity;
            }
            catch (DbEntityValidationException e)
            {
                JUtility.ErrorLog.LogApplicationError(e);
                return null;
            }
            catch (Exception ex)
            {
                JUtility.ErrorLog.LogApplicationError(ex);
                if (ex.InnerException != null && ex.InnerException.InnerException != null)
                {
                    msg = ex.InnerException.InnerException.ToString();
                    if (msg.Contains("duplicate"))
                    {
                        msg = "Record already exists";
                        return null;
                    }
                }
                msg = ex.Message;
                return null;
            }
        }
        public int Add(T entity)
        {
            try
            {
                _db.Set<T>().Add(entity);
                _db.SaveChanges();
                return 1;
            }
            catch (DbEntityValidationException e)
            {
                JUtility.ErrorLog.LogApplicationError(e);
                return 0;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.InnerException != null)
                {
                    if (ex.InnerException.InnerException.ToString().Contains("duplicate"))
                    {
                        return -3;
                    }
                }
                JUtility.ErrorLog.LogApplicationError(ex);
                return 0;
            }
        }
        public int AddRange(List<T> entity)
        {
            try
            {
                _db.Set<T>().AddRange(entity);
                _db.SaveChanges();
                return 1;
            }
            catch (DbEntityValidationException e)
            {
                JUtility.ErrorLog.LogApplicationError(e);
                return 0;
            }
            catch (Exception ex)
            {
                JUtility.ErrorLog.LogApplicationError(ex);
                if (ex.InnerException != null && ex.InnerException.InnerException != null)
                {
                    if (ex.InnerException.InnerException.ToString().Contains("duplicate"))
                    {
                        return -3;
                    }
                }
                return -1;
            }
        }
        public T GetSingle(long? id)
        {
            return _db.Set<T>().Find(id);

        }
        public bool Update(T entity)
        {
            try
            {
                _db.Set<T>().Attach(entity);
                _db.Entry(entity).State = EntityState.Modified;
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                JUtility.ErrorLog.LogApplicationError(ex);
                return false;
            }
        }
        public bool UpdateRange(List<T> entity)
        {
            try
            {
                foreach (var item in entity)
                {
                    _db.Set<T>().Attach(item);
                    _db.Entry(item).State = EntityState.Modified;
                }
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                JUtility.ErrorLog.LogApplicationError(ex);
                return false;
            }
        }
        public bool Delete(long id)
        {
            try
            {
                var item = _db.Set<T>().Find(id);
                _db.Set<T>().Remove(item);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                JUtility.ErrorLog.LogApplicationError(ex);
                return false;
            }
        }
        public bool DeleteBy(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var item = _db.Set<T>().Find(predicate);
                // _db.Set<T>().Remove(item);
                //_db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                JUtility.ErrorLog.LogApplicationError(ex);
                return false;
            }
        }
        public IQueryable<T> GetAll()
        {
            var db = new AgencyManagerContext();
            return db.Set<T>();
        }
        public IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            return includeProperties.Aggregate<Expression<Func<T, object>>, IQueryable<T>>(_db.Set<T>(), (current, includeProperty) => current.Include(includeProperty));
        }
        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _db.Set<T>().Where(predicate);
        }
        public T FindSingleBy(Expression<Func<T, bool>> predicate)
        {
            return _db.Set<T>().FirstOrDefault(predicate);

        }
    }
}
