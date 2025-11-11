using LibraryManagement.Application.QueryModels.Borrowings;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class BorrowingRepository: IBorrowingRepository
    {
        public Task<Borrowing> GetByIdAsync(long borrowingId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public Task<Borrowing> AddAsync(Borrowing borrowing, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public Task<Borrowing> UpdateAsync(Borrowing borrowing, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteAsync(Borrowing borrowing, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public Task<ICollection<Borrowing>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public Task<Borrowing> FindAsync(long borrowingId, BorrowingSearchArgs args, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
