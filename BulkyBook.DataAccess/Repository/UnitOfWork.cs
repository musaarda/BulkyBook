using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repository.IRepository;

namespace BulkyBook.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;

    public ICategoryRepository Category { get; private set; }

    public ICoverTypeRepository CoverType {get; private set; }

    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;
        Category = new CategoryRepository(_db);
        CoverType = new CoverTypeRepository(_db);
    }

    public void Save()
    {
        _db.SaveChanges();
    }
}
