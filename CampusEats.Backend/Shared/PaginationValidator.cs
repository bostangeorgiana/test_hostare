namespace CampusEats.Shared;

public class PaginationValidator
{
    private const string ErrorMessage = "Invalid page or page size";

    public static void Validate(int page, int pageSize)
    {
        // Both must be positive natural numbers
        if (page < 1 || pageSize < 1)
            throw new ArgumentException(ErrorMessage);

        // Page size must be multiple of 10
        if (pageSize % 10 != 0)
            throw new ArgumentException(ErrorMessage);

        // This also protects 0, negative, decimals (not possible for int)
    }
}