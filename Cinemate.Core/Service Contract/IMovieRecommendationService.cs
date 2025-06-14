public record MovieRecommendationRequest
{
    public string UserId { get; init; }
    public int Age { get; init; }
}

public record MovieRecommendationResponse
{
    public int TMDBId { get; init; }
    public double Score { get; init; }
}

public interface IMovieRecommendationService
{
    Task<Result<IEnumerable<MovieRecommendationResponse>>> GetRecommendedMoviesAsync(
        MovieRecommendationRequest request,
        CancellationToken cancellationToken = default);
}