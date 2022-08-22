using HotelListing.Data;
using HotelListing.IRepository;

namespace HotelListing.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly DatabaseContext _context;
    private IGenericRepository<Country> _countries;
    private IGenericRepository<Hotel> _hotels;

    public IGenericRepository<Country> Countries => _countries ??= new GenericRepository<Country>(_context);
    public IGenericRepository<Hotel> Hotels => _hotels ??= new GenericRepository<Hotel>(_context);


    public UnitOfWork(DatabaseContext context)
    {
        this._context = context;
    }
    
    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}