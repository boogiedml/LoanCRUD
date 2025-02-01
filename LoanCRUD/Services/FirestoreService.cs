using Google.Cloud.Firestore;
using LoanCRUD.Models;

namespace LoanCRUD.Services
{
    public class FirestoreService
    {
        private readonly FirestoreDb _db;
        private const string CollectionName = "loans";

        public FirestoreService(string projectId)
        {
            _db = FirestoreDb.Create(projectId);
        }

        public async Task<string> CreateLoanAsync(Loan loan)
        {
            var docRef = await _db.Collection(CollectionName).AddAsync(loan);
            return docRef.Id;
        }

        public async Task<Loan> GetLoanAsync(string userId, string id)
        {
            var docRef = _db.Collection(CollectionName).Document(id);
            var snapshot = await docRef.GetSnapshotAsync();
            var loan = snapshot.ConvertTo<Loan>();
            return loan.UserId == userId ? loan : null;
        }

        public async Task<List<Loan>> GetAllLoansForUserAsync(string userId)
        {
            var snapshot = await _db.Collection(CollectionName)
                .WhereEqualTo("UserId", userId)
                .GetSnapshotAsync();
            return snapshot.Documents.Select(d => d.ConvertTo<Loan>()).ToList();
        }

        public async Task UpdateLoanAsync(string userId, string id, Loan loan)
        {
            var docRef = _db.Collection(CollectionName).Document(id);
            var snapshot = await docRef.GetSnapshotAsync();
            var existingLoan = snapshot.ConvertTo<Loan>();

            if (existingLoan.UserId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to update this loan.");
            }

            await docRef.SetAsync(loan, SetOptions.MergeAll);
        }

        public async Task DeleteLoanAsync(string userId, string id)
        {
            var docRef = _db.Collection(CollectionName).Document(id);
            var snapshot = await docRef.GetSnapshotAsync();
            var existingLoan = snapshot.ConvertTo<Loan>();

            if (existingLoan.UserId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to delete this loan.");
            }

            await docRef.DeleteAsync();
        }
    }
}

