using FileShare.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FileShare.Services;

public class BaseService<T> where T : class
{
    protected readonly DataContext _dataContext;
    protected readonly DbSet<T> dbSet;

    public BaseService(DataContext dataContext)
    {
        _dataContext = dataContext;
        dbSet = dataContext.Set<T>();
    }

    public void Add(T entity)
    {
        dbSet.Add(entity);
        _dataContext.SaveChanges();
    }

    public void Update(T entity)
    {
        dbSet.Update(entity);
        _dataContext.SaveChanges();
    }

    public void Remove(T entity)
    {
        dbSet.Remove(entity);
        _dataContext.SaveChanges();
    }

    public T? FindByIdOrDefault(int id)
    {
        return dbSet.Find(id);
    }

    public IQueryable<T> Get(Expression<Func<T, bool>> predicate)
    {
        return dbSet.Where(predicate);
    }

    public IQueryable<T> GetAll()
    {
        return dbSet;
    }

    public T? GetFirstOrDefault(Expression<Func<T, bool>> predicate)
    {
        return dbSet.FirstOrDefault(predicate);
    }

    public bool Exists(Expression<Func<T, bool>> predicate)
    {
        return dbSet.Any(predicate);
    }
}

public interface IBaseService<T> where T : class
{
    void Add(T entity);
    void Update(T entity);
    void Remove(T entity);
    T? FindByIdOrDefault(int id);
    IQueryable<T> Get(Expression<Func<T, bool>> predicate);
    IQueryable<T> GetAll();
    T? GetFirstOrDefault(Expression<Func<T, bool>> predicate);
    bool Exists(Expression<Func<T, bool>> predicate);
}
