using Cinemate.Core.Errors.ProfileError;
using Cinemate.Repository.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;
using System.Security.Claims;

public class MovieRecommendationService : IMovieRecommendationService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;


  

    public MovieRecommendationService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<IEnumerable<MovieRecommendationResponse>>> GetRecommendedMoviesAsync(
        MovieRecommendationRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            //var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            //if (string.IsNullOrEmpty(userId))
            //    return OperationResult.Failure("User is not authenticated.");

            var response = await _httpClient.PostAsJsonAsync(
                "your-ml-service-url/recommend",
                request,
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return Result.Failure<IEnumerable<MovieRecommendationResponse>>(
                    new Error("Recommendation.Failed", "Failed to get recommendations", (int)response.StatusCode));
            }

            var recommendations = await response.Content.ReadFromJsonAsync<IEnumerable<MovieRecommendationResponse>>(cancellationToken: cancellationToken);

            if (recommendations == null)
            {
                return Result.Failure<IEnumerable<MovieRecommendationResponse>>(
                    new Error("Recommendation.Empty", "No recommendations received", null));
            }

            return Result.Success(recommendations);
        }
        catch (Exception ex)
        {
            return Result.Failure<IEnumerable<MovieRecommendationResponse>>(
                new Error("Recommendation.Exception", ex.Message, null));
        }
    }
}
