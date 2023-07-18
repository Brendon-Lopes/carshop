namespace Carshop.Application.DTOs;

public class GetAllCarsFilteredAndPaginatedResponse
{
    public IEnumerable<CarResponse> Cars { get; init; } = null!;
    public int TotalPages { get; init; }
    public int CurrentPage { get; init; }
}