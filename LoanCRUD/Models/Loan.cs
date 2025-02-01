using Google.Cloud.Firestore;

namespace LoanCRUD.Models
{
    [FirestoreData]
    public class Loan
    {
        [FirestoreDocumentId]
        public string? Id { get; set; }

        [FirestoreProperty]
        public string UserId { get; set; }

        [FirestoreProperty]
        public double Amount { get; set; }

        [FirestoreProperty]
        public string Purpose { get; set; }

        [FirestoreProperty]
        public int TermInMonths { get; set; }
    }
}