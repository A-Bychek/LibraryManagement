using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Application.QueryModels.Borrowings;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Shared;

namespace LibraryManagement.Application.Interfaces.Repositories
{
    public interface IBorrowingRepository
    {
        // check for required methods
        public Task<Borrowing> GetByIdAsync(long borrowingId, CancellationToken cancellationToken = default);
        public Task<Borrowing> AddAsync(Borrowing borrowing, CancellationToken cancellationToken = default);
        public Task<Borrowing> UpdateAsync(Borrowing borrowing, CancellationToken cancellationToken = default);
        public Task<Borrowing> DeleteAsync(Borrowing borrowing, CancellationToken cancellationToken = default);
        public Task<ICollection<Borrowing>> GetAllAsync(CancellationToken cancellationToken = default);
        public Task<PagedResult<Borrowing>> FindAsync(BorrowingSearchArgs args, CancellationToken cancellationToken = default);
    }

}

