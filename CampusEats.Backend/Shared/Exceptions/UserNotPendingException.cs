namespace CampusEats.Shared.Exceptions;

public class UserNotPendingException(int userId) 
    : Exception($"User with ID {userId} is not a pending student request.");