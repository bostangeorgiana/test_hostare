namespace CampusEats.Shared.Exceptions;

public class UserNotKitchenStaffException(int userId) 
    : Exception($"User with ID {userId} is not a kitchen staff.");