﻿using FileShare.Models;
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

    public T? FindByIdOrDefault(int id)
    {
        return dbSet.Find(id);
    }

    public IQueryable<T> GetAll()
    {
        return dbSet;
    }

    public bool Exists(Expression<Func<T, bool>> predicate)
    {
        return dbSet.Any(predicate);
    }
}

public interface IBaseService<T> where T : class
{
    public void Add(T entity);
    public T? FindByIdOrDefault(int id);
    public IQueryable<T> GetAll();
    public bool Exists(Expression<Func<T, bool>> predicate);
}