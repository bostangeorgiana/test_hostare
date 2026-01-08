namespace CampusEats.Shared.Exceptions;

public class UserNotAdminException(int userId) 
    : Exception($"User with ID {userId} is not an admin.");