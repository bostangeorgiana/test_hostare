namespace CampusEats.Shared.Exceptions;

public class DuplicateEmailException(string email) 
    : Exception($"The email '{email}' is already registered.");