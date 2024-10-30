using Asp.Versioning;
using AutoMapper;
using AutoMapper.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Linq.Expressions;
using WakaWaka.API.DataAccess.DTO;
using WakaWaka.API.DataAccessLayer.DataContext;
using WakaWaka.API.DataAccessLayer.Interfaces;
using WakaWaka.API.Domain.Models.restaurant;
using WakaWaka.API.Models.Hotel;

namespace WakaWaka.API.Controllers
{
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ControllerName("Hotel")]
    [ApiVersion(1.0, Deprecated =true)]
   
    
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IBaseRepository<Hotel> _hotelRepository;
        private readonly IReviewRepository<HotelReview> _reviewRepository;
        private readonly IMapper _mapper;

        public HotelsController(IBaseRepository<Hotel> hotelRepository, IReviewRepository<HotelReview> reviewRepository, IMapper mapper)
        {
            _hotelRepository = hotelRepository;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{hotelId}")]
        [MapToApiVersion("1.0")]
        
        public async Task<IActionResult> GetHotelByIdAsync(int hotelId)
        {
            try
            {
                var checkHotel = await _hotelRepository.GetByIDAsync(hotelId);
                if (checkHotel == null)
                {
                    return NotFound();
                }
                else
                {
                    var hotel = _mapper.Map<HotelGetDTO>(checkHotel);
                    return Ok(hotel);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Source + ex.Message);
            }
        }
    


        [HttpPost]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddNewHotel([FromBody] HotelCreateDTO newHotel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mapNewHhotel = _mapper.Map<Hotel>(newHotel);
                    await _hotelRepository.CreateAsync(mapNewHhotel);

                    var hotel = _mapper.Map<HotelGetDTO>(mapNewHhotel);
                    return Ok(hotel);
                    //return CreatedAtAction(nameof(GetHotelByIdAsync), new { hotelId = mapNewHhotel.Id }, hotel);
                    //return CreatedAtAction(actionName: "GetHotelByIdAsync", routeValues: new { hotelId = mapNewHhotel.Id }, value: hotel);

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

        [HttpGet("Hotels")]
        //    [MapToApiVersion(1)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAllHotels()
        {
            try
            {
                var getHotels = await _hotelRepository.GetAllAsync();
                var hotels = _mapper.Map<List<HotelGetDTO>>(getHotels);
                return Ok(hotels);

            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Source + ex.Message);
            }
        }

        [HttpDelete]
        [Route("{hotelId}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> DeleteHotel(int hotelId)
        {
            try
            {
                var getHotel = await _hotelRepository.DeleteAsync(hotelId);
                return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Source + ex.Message);
            }
        }

        [HttpPut]
        [Route("{hotelId}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateHotel([FromBody] HotelCreateDTO updateHotel, int hotelId)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var mapHotel = _mapper.Map<Hotel>(updateHotel);
                    await _hotelRepository.UpdateAsync(mapHotel, hotelId);

                    var updatedHotel = _mapper.Map<HotelGetDTO>(mapHotel);
                    return Ok(updatedHotel);
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

        //[HttpPost("Multiple-Hotels")]
        //public async Task<IEnumerable<IActionResult>> AddMultipleHotelEntity([FromBody] IEnumerable<HotelCreateDTO> newHotel)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var mapHotel = _mapper.Map<List<Hotel>>(newHotel);
        //            await _hotelRepository.CreateMultipleAsync(mapHotel);


        //            var result = new List<IActionResult>();
        //            foreach (var hotel in mapHotel)
        //            {
        //                if (ModelState.IsValid)
        //                {
        //                    var hotels = _mapper.Map<List<HotelGetDTO>>(hotel);
        //                    result.Add(Ok(hotels));
        //                    //return Ok(hotel);
        //                }
        //                else
        //                {
        //                    return new List<IActionResult> { BadRequest() };
        //                }
        //            }
        //            return result;
        //            //return Ok(hotels);
        //        }
        //        else
        //        {
        //            return new List<IActionResult> { BadRequest() };
        //            //return BadRequest();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new List<IActionResult> { StatusCode(StatusCodes.Status500InternalServerError, ex.Source + ex.Message) };
        //        //return StatusCode(StatusCodes.Status500InternalServerError, ex.Source + ex.Message);
        //    }
        //}
        [HttpPost("Multiple-Hotels")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddMultipleHotelEntity([FromBody] IEnumerable<HotelCreateDTO> newHotels)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var hotelsToCreate = _mapper.Map<List<Hotel>>(newHotels);
                    await _hotelRepository.CreateMultipleAsync(hotelsToCreate);

                    var hotels = new List<object>();
                    foreach (var hotel in hotelsToCreate)
                    {
                        var hotelGetDto = _mapper.Map<HotelGetDTO>(hotel);
                        var hotelObject = new
                        {
                            Hotel = hotelGetDto,
                            IsSuccessful = ModelState.IsValid,
                            Message = ModelState.IsValid ? "Hotel Added successfully." : " Unsuccessful..Please check your input values"
                        };
                        hotels.Add(hotelObject);
                    }

                    return Ok(hotels);
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
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetByFilteredCondition(string? city, string? state, string? country, int? rating, int? price, int? maxPrice)
        {
            try
            {               
                Expression<Func<Hotel, bool>> filterCondition = hotel =>
                (string.IsNullOrEmpty(city) || hotel.City == city) &&
                (string.IsNullOrEmpty(state) || hotel.State == state) &&
                (string.IsNullOrEmpty(country) || hotel.Country == country) &&
                (!rating.HasValue || hotel.Rating == (Rating)rating.Value) &&
                (!price.HasValue || hotel.Price == price) &&
                (!maxPrice.HasValue || hotel.Price < maxPrice);

                var hotels = await _hotelRepository.GetAllFilteredAsync(filterCondition);
                if(hotels != null)
                {
                    var hotelGetDTO = _mapper.Map<List<HotelGetDTO>>(hotels);
                    return Ok(hotelGetDTO);
                }
                else 
                { 
                    return NotFound(); 
                }
                
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Source + ex.Message);
            }
        }


        [HttpGet("by-search-query")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetBySearchQuery(string? searchQuery)
        {
            try
            {
                Expression<Func<Hotel, object>> searchCondition = hotel =>
                hotel.Name.Contains(searchQuery) ||
                hotel.Keywords.Contains(searchQuery) ||
                hotel.Area.Contains(searchQuery) ||
                hotel.Reviews.Any(review => review.Comment.Contains(searchQuery)) ||
                hotel.State.Contains(searchQuery) ||
                hotel.Slug.Contains(searchQuery) ||
                hotel.Country.Contains(searchQuery) ||
                hotel.City.Contains(searchQuery) ||
                hotel.Address.Contains(searchQuery) ||
                hotel.Hotel_Email.Contains(searchQuery);
                


                var hotels = await _hotelRepository.GetAllSortedAsync(searchCondition);
                if(hotels !=null)
                {
                    var mapHotels = _mapper.Map<List<HotelGetDTO>>(hotels);
                    return Ok(mapHotels);
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

        [HttpGet("paged-hotels")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetPagedHotels(int pageNumber, int pageSize)
        {
            try
            {
                if(pageNumber < 1 || pageSize < 10 ||pageSize> 10)
                {
                    pageNumber = 1;
                    pageSize = 10;
                }
                var hotels = await _hotelRepository.GetAllPagedAsync(pageNumber, pageSize);

                if (!hotels.Any())
                {
                    return NotFound();
                }

                var hotelGetDTOs = _mapper.Map<List<HotelGetDTO>>(hotels);

                return Ok(hotelGetDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Source + ex.Message);
            }
        }

        [HttpPost("Create-Hotel-Reviews")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CreateHotelReview([FromBody] HotelReviewCreateDTO newReview)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var map = _mapper.Map<HotelReview>(newReview);
                    await _reviewRepository.CreateReviewAsync(map);
                    
                    var review = _mapper.Map<HotelReviewGetDTO>(map);
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

        [HttpPost("Get-Hotel-Review-By-ID")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetHotelReviewByID(int hotelId, int reviewId)
        {
            try
            {
                var review = await _reviewRepository.GetReviewByIdAsync(hotelId, reviewId);
                if(review == null)
                {
                    return NotFound();
                }
                else
                {
                    var mapReview = _mapper.Map<HotelReviewGetDTO>(review);
                    return Ok(mapReview);
                }
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Source + ex.Message);
            }
        }

        [HttpGet("Get-All-Hotel-Reviews")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAllHotelReviews(int hotelId)
        {
            try
            {
                var review = await _reviewRepository.GetAllReviewsAsync(hotelId);
                var mapReview = _mapper.Map<List<HotelReviewGetDTO>>(review);
                return Ok(mapReview);

            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Source + ex.Message);
            }
        }


        [HttpPut("Update-Hotel-Review")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateHotelReview([FromBody] HotelReviewCreateDTO updateReview, int hotelId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mapReview = _mapper.Map<HotelReview>(updateReview);
                    mapReview.Hotel_Id = hotelId;
                    await _reviewRepository.UpdateReviewAsync(mapReview, hotelId);

                    var getReviewDTO = _mapper.Map<HotelReviewGetDTO>(mapReview);
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

        [HttpDelete("Remove-Hotel-ReviewBy-Id")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> DeleteHotelReview(int reviewId, int hotelId)
        {
            try
            {
                var review = await _reviewRepository.DeleteReviewAsync(reviewId, hotelId);
                return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Source + ex.Message);
            }
        }

    }
}
