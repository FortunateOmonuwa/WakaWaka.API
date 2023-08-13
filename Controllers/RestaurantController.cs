using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using WakaWaka.API.DataAccess.DTO;
using WakaWaka.API.DataAccessLayer.Interfaces;
using WakaWaka.API.Domain.Models.restaurant;
using WakaWaka.API.Models.Restaurant;
using WakaWaka.API.Models.Resturant;

namespace WakaWaka.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IBaseRepository<Restaurant> _restaurantRepository;
        private readonly IReviewRepository<RestaurantReview> _reviewRepository;
        private readonly IMapper _mapper;

        public RestaurantController(IBaseRepository<Restaurant> restaurantRepository, IReviewRepository<RestaurantReview> reviewRepository, IMapper mapper)
        {
            _restaurantRepository = restaurantRepository;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }


        [HttpGet]
        [Route("{restaurantId}")]
        public async Task<IActionResult> GetRestaurantByIdAsync(int restaurantId)
        {
            try
            {
                var checkRestaurant = await _restaurantRepository.GetByIDAsync(restaurantId);
                if (checkRestaurant == null)
                {
                    return NotFound();
                }
                else
                {
                    var restaurant = _mapper.Map<RestaurantGetDTO>(checkRestaurant);
                    return Ok(restaurant);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Source + ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddNewRestaurant([FromBody] RestaurantCreateDTO newRestaurant)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mapNewRestaurant = _mapper.Map<Restaurant>(newRestaurant);
                    await _restaurantRepository.CreateAsync(mapNewRestaurant);

                    var restaurant = _mapper.Map<RestaurantGetDTO>(mapNewRestaurant);
                    return Ok(restaurant);
                    //return CreatedAtAction(nameof(GetrestaurantByIdAsync), new { restaurantId = mapNewHrestaurant.Id }, restaurant);
                    //return CreatedAtAction(actionName: "GetrestaurantByIdAsync", routeValues: new { restaurantId = mapNewHrestaurant.Id }, value: restaurant);

                }
                else
                {
                    return BadRequest("Invalid data provided");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Source + ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRestaurants()
        {
            try
            {
                var getRestaurants = await _restaurantRepository.GetAllAsync();
                var restaurants = _mapper.Map<List<RestaurantGetDTO>>(getRestaurants);
                return Ok(restaurants);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Source + ex.Message);
            }
        }

        [HttpDelete]
        [Route("{restaurantId}")]
        public async Task<IActionResult> DeleteRestaurant(int restaurantId)
        {
            try
            {
                var getRestaurant = await _restaurantRepository.DeleteAsync(restaurantId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Source + ex.Message);
            }
        }

        [HttpPut]
        [Route("{restaurantId}")]
        public async Task<IActionResult> UpdateRestaurant([FromBody] RestaurantCreateDTO updateRestaurant, int restaurantId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mapRestaurant = _mapper.Map<Restaurant>(updateRestaurant);
                    await _restaurantRepository.UpdateAsync(mapRestaurant, restaurantId);

                    var updatedRestaurant = _mapper.Map<RestaurantGetDTO>(mapRestaurant);
                    return Ok(updatedRestaurant);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Source + ex.Message);
            }
        }

        [HttpPost("Multiple-Restaurants")]
        public async Task<IActionResult> AddMultipleRestaurantEntity([FromBody] IEnumerable<RestaurantCreateDTO> newRestaurants)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var restaurantsToCreate = _mapper.Map<List<Restaurant>>(newRestaurants);
                    await _restaurantRepository.CreateMultipleAsync(restaurantsToCreate);

                    var restaurants = new List<object>();
                    foreach (var restaurant in restaurantsToCreate)
                    {
                        var restaurantGetDto = _mapper.Map<RestaurantGetDTO>(restaurant);
                        var restaurantObject = new
                        {
                            restaurant = restaurantGetDto,
                            IsSuccessful = ModelState.IsValid,
                            Message = ModelState.IsValid ? "Restaurant Added successfully." : " Unsuccessful..Please check your input values"
                        };
                        restaurants.Add(restaurantObject);
                    }

                    return Ok(restaurants);
                }
                else
                {
                    return BadRequest("Invalid input data.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Source + ex.Message);
            }
        }


        [HttpGet("by-filter-condition")]
        public async Task<IActionResult> GetByFilteredCondition(string? city, string? state, string? country, int? rating, int? price, int? maxPrice)
        {
            try
            {
                Expression<Func<Restaurant, bool>> filterCondition = restaurant =>
                (string.IsNullOrEmpty(city) || restaurant.City == city) &&
                (string.IsNullOrEmpty(state) || restaurant.State == state) &&
                (string.IsNullOrEmpty(country) || restaurant.Country == country) &&
                (!rating.HasValue || restaurant.Rating == (Rating)rating.Value) &&
                (!price.HasValue || restaurant.Price == price) &&
                (!maxPrice.HasValue || restaurant.Price < maxPrice);

                var restaurants = await _restaurantRepository.GetAllFilteredAsync(filterCondition);
                if (restaurants != null)
                {
                    var restaurantGetDTO = _mapper.Map<List<RestaurantGetDTO>>(restaurants);
                    return Ok(restaurantGetDTO);
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Source + ex.Message);
            }
        }


        [HttpGet("by-search-query")]
        public async Task<IActionResult> GetBySearchQuery(string? searchQuery)
        {
            try
            {
                Expression<Func<Restaurant, object>> searchCondition = restaurant =>
                restaurant.Name.Contains(searchQuery) ||
                restaurant.Keywords.Contains(searchQuery) ||
                restaurant.Area.Contains(searchQuery) ||
                restaurant.Reviews.Any(review => review.Comment.Contains(searchQuery)) ||
                restaurant.State.Contains(searchQuery) ||
                restaurant.Slug.Contains(searchQuery) ||
                restaurant.Country.Contains(searchQuery) ||
                restaurant.City.Contains(searchQuery) ||
                restaurant.Address.Contains(searchQuery) ||
                restaurant.Restaurant_Email.Contains(searchQuery);


                var restaurants = await _restaurantRepository.GetAllSortedAsync(searchCondition);
                if (restaurants != null)
                {
                    var mapRestaurants = _mapper.Map<List<RestaurantGetDTO>>(restaurants);
                    return Ok(mapRestaurants);
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Source + ex.Message);
            }
        }

        [HttpGet("paged-restaurants")]
        public async Task<IActionResult> GetPagedRestaurants(int pageNumber, int pageSize)
        {
            try
            {
                if (pageNumber < 1 || pageSize < 10 || pageSize > 10)
                {
                    pageNumber = 1;
                    pageSize = 10;
                }
                var restaurants = await _restaurantRepository.GetAllPagedAsync(pageNumber, pageSize);

                if (!restaurants.Any())
                {
                    return NotFound();
                }

                var restaurantGetDTOs = _mapper.Map<List<RestaurantGetDTO>>(restaurants);

                return Ok(restaurantGetDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Source + ex.Message);
            }
        }

        [HttpPost("Create-Restaurant-Reviews")]
        public async Task<IActionResult> CreaterestaurantReview([FromBody] RestaurantReviewCreateDTO newReview)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var map = _mapper.Map<RestaurantReview>(newReview);
                    await _reviewRepository.CreateReviewAsync(map);

                    var review = _mapper.Map<RestaurantReviewGetDTO>(map);
                    return Ok(review);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Source + ex.Message);
            }
        }

        [HttpPost("Get-Restaurant-Review-By-ID")]
        public async Task<IActionResult> GetRestaurantReviewByID(int restaurantId, int reviewId)
        {
            try
            {
                var review = await _reviewRepository.GetReviewByIdAsync(restaurantId, reviewId);
                if (review == null)
                {
                    return NotFound();
                }
                else
                {
                    var mapReview = _mapper.Map<RestaurantReviewGetDTO>(review);
                    return Ok(mapReview);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Source + ex.Message);
            }
        }

        [HttpGet("Get-All-Restaurant-Reviews")]
        public async Task<IActionResult> GetAllRestaurantReviews(int restaurantId)
        {
            try
            {
                var review = await _reviewRepository.GetAllReviewsAsync(restaurantId);
                var mapReview = _mapper.Map<List<RestaurantReviewGetDTO>>(review);
                return Ok(mapReview);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Source + ex.Message);
            }
        }


        [HttpPut("Update-Restaurant-Review")]
        public async Task<IActionResult> UpdateRestaurantReview([FromBody] RestaurantReviewCreateDTO updateReview, int restaurantId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mapReview = _mapper.Map<RestaurantReview>(updateReview);
                    mapReview.Restaurant_Id = restaurantId;
                    await _reviewRepository.UpdateReviewAsync(mapReview, restaurantId);

                    var getReviewDTO = _mapper.Map<RestaurantReviewGetDTO>(mapReview);
                    return Ok(getReviewDTO);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Source + ex.Message);
            }
        }

        [HttpDelete("Remove-Restaurant-ReviewBy-Id")]
        public async Task<IActionResult> DeleteRestaurantReview(int reviewId, int restaurantId)
        {
            try
            {
                var review = await _reviewRepository.DeleteReviewAsync(reviewId, restaurantId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Source + ex.Message);
            }
        }
    }
}
