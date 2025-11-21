using part_2.Models;

namespace part_2.Services
{
    public class ClaimValidationService
    {
        public static bool IsClaimValid(Claim claim)
        {
            // Validate hours: must be between 1 and 40
            if (claim.Hours < 1 || claim.Hours > 40)
                return false;

            // Validate rate: must be between 10 and 200
            if (claim.Rate < 10 || claim.Rate > 200)
                return false;

            // Check for supporting documents
            if (string.IsNullOrEmpty(claim.SupportingDocuments))
                return false;

            

            
            return true;
        }
    }
}
