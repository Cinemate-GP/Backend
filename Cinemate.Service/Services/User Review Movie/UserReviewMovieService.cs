using Azure;
using Cinemate.Core.Contracts.User_Rate_Movie;
using Cinemate.Core.Contracts.User_Review_Movie;
using Cinemate.Core.Contracts.User_Watched_Movie;
using Cinemate.Core.Entities;
using Cinemate.Core.Entities.Auth;
using Cinemate.Core.Errors.ProfileError;
using Cinemate.Core.Repository_Contract;
using Cinemate.Core.Service_Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Cinemate.Service.Services.User_Review_Movie
{
    public class UserReviewMovieService : IUserReviewMovieService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;
		private const string SentimentAnalysisUrl = "http://cinemate-sa-api.h6bkc5a2dveqedfm.italynorth.azurecontainer.io:5000/review/sentiment";
		public UserReviewMovieService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, HttpClient httpClient)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
        }
		public async Task<OperationResult> AddUserReviewMovieAsync(UserReviewMovieResponse request, CancellationToken cancellationToken = default)
		{
			try
			{
				var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
				if (string.IsNullOrEmpty(userId))
					return OperationResult.Failure("Unauthorized user.");

				if (!string.IsNullOrEmpty(request.UserId))
				{
					var userRepo = _unitOfWork.Repository<ApplicationUser>().GetQueryable();
					var requestedUser = await userRepo.FirstOrDefaultAsync(u => u.UserName == request.UserId, cancellationToken);

					if (requestedUser != null)
						userId = requestedUser.Id;
				}
				var sentimentResult = await AnalyzeSentimentAsync(request.ReviewBody, cancellationToken);
				var entity = new UserReviewMovie
				{
					UserId = userId,
					TMDBId = request.TMDBId,
					ReviewedOn = DateTime.Now,
					ReviewBody = request.ReviewBody,
					ReviewType = sentimentResult?.Sentiment,
					ReviewConfidence = sentimentResult?.Confidence.HasValue == true ? (decimal)sentimentResult.Confidence : null
				};

				await _unitOfWork.Repository<UserReviewMovie>().AddAsync(entity);
				await _unitOfWork.CompleteAsync();
				return OperationResult.Success("Review added successfully.");
			}
			catch (Exception ex)
			{
				return OperationResult.Failure($"Failed to add review: {ex.Message}");
			}
		}
		public async Task<OperationResult> DeleteUserReviewMovieAsync(UserReviewDeleteMovieResponse request, CancellationToken cancellationToken = default)
		{
			try
			{
				var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
				if (string.IsNullOrEmpty(userId))
					return OperationResult.Failure("Unauthorized user.");

				if (!string.IsNullOrEmpty(request.UserId))
				{
					var userRepo = _unitOfWork.Repository<ApplicationUser>().GetQueryable();
					var requestedUser = await userRepo.FirstOrDefaultAsync(u => u.UserName == request.UserId, cancellationToken);

					if (requestedUser != null)
						userId = requestedUser.Id;
				}

				var review = await _unitOfWork.Repository<UserReviewMovie>()
					.GetQueryable()
					.FirstOrDefaultAsync(r =>
						r.ReviewId == request.ReviewId &&
						r.TMDBId == request.TMDBId &&
						r.UserId == userId,
						cancellationToken);

				if (review == null)
					return OperationResult.Failure("Review not found.");

				_unitOfWork.Repository<UserReviewMovie>().Delete(review);
				await _unitOfWork.CompleteAsync();

				return OperationResult.Success("Review deleted successfully.");
			}
			catch (Exception ex)
			{
				return OperationResult.Failure($"Failed to delete review: {ex.Message}");
			}
		}

		public async Task<IEnumerable<UserReviewMovieResponseBack>> GetUserReviewMoviesAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Repository<UserReviewMovie>()
                        .GetQueryable()
                        .Include(ul => ul.User)
                        .Include(ul => ul.Movie)
						.ThenInclude(m => m.UserRates)						
						.Select(ul => new UserReviewMovieResponseBack
						{
                            UserId = ul.UserId,
                            ReviewId = ul.ReviewId,
							ReviewBody = ul.ReviewBody,
                            Title = ul.Movie.Title ?? string.Empty,
                            TMDBId = ul.Movie.TMDBId,
                            Poster_path = ul.Movie.PosterPath,
							Stars = ul.Movie.UserRates.Where(urate => urate.UserId == ul.UserId).Select(urate => urate.Stars).FirstOrDefault() ?? 0
						})
                        .ToListAsync(cancellationToken);

        }
		private async Task<SentimentAnalysisResponse?> AnalyzeSentimentAsync(string reviewText, CancellationToken cancellationToken)
		{
			try
			{
				var requestBody = new SentimentAnalysisRequest { Review = reviewText };
				var jsonContent = JsonSerializer.Serialize(requestBody);
				var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
				var response = await _httpClient.PostAsync(SentimentAnalysisUrl, content, cancellationToken);
				var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
				if (response.IsSuccessStatusCode)
				{
					var result = JsonSerializer.Deserialize<SentimentAnalysisResponse>(responseJson, new JsonSerializerOptions
					{
						PropertyNameCaseInsensitive = true,
						AllowTrailingCommas = true,
						ReadCommentHandling = JsonCommentHandling.Skip
					}) ?? throw new JsonException($"Deserialization failed. Response: {responseJson}");
					return result;
				}
				else
				{
					var errorResponse = await response.Content.ReadAsStringAsync(cancellationToken);
					throw new HttpRequestException($"API request failed with status {response.StatusCode}");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Sentiment analysis failed: {ex.Message}");
				Console.WriteLine($"Stack trace: {ex.StackTrace}");
				return null;
			}
		}
	}	
	public class SentimentAnalysisRequest
    {
        [JsonPropertyName("review")]
        public string Review { get; set; } = string.Empty;
    }
    public class SentimentAnalysisResponse
    {
        [JsonPropertyName("sentiment")]
        public string? Sentiment { get; set; }
        
        [JsonPropertyName("confidence")]
        public double? Confidence { get; set; }
        
        [JsonPropertyName("probabilities")]
        public SentimentProbabilities Probabilities { get; set; } = new();
    }
    public class SentimentProbabilities
    {
        [JsonPropertyName("positive")]
        public double Positive { get; set; }
        
        [JsonPropertyName("negative")]
        public double Negative { get; set; }
    }
}
