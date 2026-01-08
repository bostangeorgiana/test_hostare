namespace CampusEats.Shared.Exceptions;

public class EntityNotFoundException(string entity, object key)
    : Exception($"{entity} with identifier '{key}' was not found.");