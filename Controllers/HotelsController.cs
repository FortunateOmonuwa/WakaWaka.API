using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WakaWaka.API.DataAccess.DTO;
using WakaWaka.API.DataAccessLayer.DataContext;
using WakaWaka.API.DataAccessLayer.Interfaces;
using WakaWaka.API.Domain.Models.Hotel;
using WakaWaka.API.Models.Hotel;

namespace WakaWaka.API.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<IActionResult> AddNewHotel([FromBody] HotelCreateDTO newHotel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mapNewHhotel = _mapper.Map<Hotel>(newHotel);
                    await _hotelRepository.CreateAsync(mapNewHhotel);

                    var hotel = _mapper.Map<HotelGetDTO>(mapNewHhotel);
                    return CreatedAtAction(nameof(GetHotelByIdAsync), new { hotelId = mapNewHhotel.Id }, hotel);

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
    }
}
