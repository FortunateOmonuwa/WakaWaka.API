using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WakaWaka.API.DataAccessLayer.DataContext;
using WakaWaka.API.DataAccessLayer.Interfaces;
using WakaWaka.API.Domain.Models.Hotel;
using WakaWaka.API.Models.Hotel;

namespace WakaWaka.API.DataAccessLayer.Repository
{
    public class HotelRepository : IBaseRepository<Hotel>, IReviewRepository<HotelReview>
    {
        private readonly WakaContext _context;
        public HotelRepository(WakaContext context)
        {
            _context = context;
        }
        public async Task<Hotel> CreateAsync(Hotel entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity));
                }
                else
                {
                    var hotelName = await _context.Hotels.FirstOrDefaultAsync(h => h.Name == entity.Name);
                    if (hotelName != null)
                    {
                        throw new Exception($"Hotel with name {hotelName.Name} already exists");
                    }

                    await _context.Hotels.AddAsync(entity);
                    await _context.SaveChangesAsync();
                    return entity;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Source}/n {ex.Message}");
            }
        }

        public async Task<Hotel> GetByIDAsync(int id)
        {
            try
            {
                var hotel = await _context.Hotels.Include(r => r.Reviews).FirstOrDefaultAsync(h => h.Id == id);
                if (hotel == null)
                {
                    throw new Exception($"Hotel with ID:{id} doesn't exist");
                }
                else
                {
                    return hotel;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Source}/n {ex.Message}");
            }
        }

        public async Task<Hotel> UpdateAsync(Hotel entity, int hotelId)
        {
            try
            {
                if(entity == null)
                {
                    throw new ArgumentNullException(nameof(entity));
                }
                else
                {
                    var hotel = await _context.Hotels.FirstOrDefaultAsync(h => h.Id == hotelId);
                    if (hotel == null)
                    {
                        throw new ArgumentNullException(nameof(entity));
                    }
                    else
                    {
                        var hotelName = await _context.Hotels.FirstOrDefaultAsync(h => h.Name == entity.Name);
                        if (hotelName != null)
                        {
                            throw new Exception($"Hotel with name {hotelName.Name} already exists");
                        }
                        hotel.Id = hotelId;
                        hotel.Name = entity.Name;
                        hotel.Rating = entity.Rating;
                        _context.Hotels.Update(entity);
                        await _context.SaveChangesAsync();
                        return entity;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Source}/n {ex.Message}");
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var hotel = await _context.Hotels.Include(r => r.Reviews).FirstOrDefaultAsync(h => h.Id == id);
                if(hotel == null)
                {
                    return false;
                }
                else
                {
                    _context.Hotels.Remove(hotel);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"{ex.Source}/n {ex.Message}");
            }
        }


        public async Task<IQueryable<Hotel>> GetAllAsync()
        {
            try
            {
                var hotels = await _context.Hotels.ToListAsync();
                if (hotels != null) 
                {
                    return hotels.AsQueryable();
                }
                else
                {
                    throw new Exception("No hotel exists on the database");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Source}/n {ex.Message}");
            }
        }
        public async Task<IEnumerable<Hotel>> CreateMultipleAsync(IEnumerable<Hotel> hotels)
        {
            try
            {
               if(hotels == null)
               {
                   throw new ArgumentNullException(nameof(hotels));
               }
               else
               {
                    foreach(var hotel in hotels)
                    {
                        var hotelName = await _context.Hotels.FindAsync(hotel.Name);
                        if(hotelName != null)
                        {
                            throw new Exception($"Hotel with name {hotelName.Name} already exists");
                        }
                        await _context.Hotels.AddAsync(hotel);
                        await _context.SaveChangesAsync();
                    }

                    await _context.SaveChangesAsync();
                    return hotels.ToList();
                    
               }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Source}/n {ex.Message}");
            }
        }
        public async Task<IQueryable<Hotel>> GetAllFilteredAsync(Expression<Func<Hotel, bool>> filterCondition)
        {
            try
            {
                var hotels = _context.Hotels.AsQueryable();
                if(filterCondition == null)
                {
                    return hotels;
                }
                else
                {
                    hotels = hotels.Where(filterCondition);
                    return await Task.FromResult(hotels);
                    
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"{ex.Source}/n {ex.Message}");
            }
        }

        public async Task<IQueryable<Hotel>> GetAllPagedAsync(int pageNumber, int pageSize)
        {
            try
            {
                if (pageNumber <= 0 || pageSize <= 0)
                {
                    throw new Exception("Page number and page size must be greater than 0.");
                }

                var hotels = _context.Hotels
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .AsQueryable();

                return hotels;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Source}/n {ex.Message}");
            }
        }

        public async Task<IQueryable<Hotel>> GetAllSortedAsync(Expression<Func<Hotel, object>> randomSearchQuery)
        {
            try
            {
                var hotels = _context.Hotels.AsQueryable();
                if(randomSearchQuery == null)
                {
                    return hotels;
                }
                else
                {
                    hotels = hotels.OrderBy(randomSearchQuery);
                    return hotels;
                }
            }
            catch( Exception ex )
            {
                throw new Exception($"{ex.Source}/n {ex.Message}");
            }
        }


        public async Task<IQueryable<Hotel>> GetSortedByQueryAsync(Expression<Func<Hotel, object>> orderByQuery)
        {
            try
            {
                var hotels = _context.Hotels.AsQueryable();
                if(orderByQuery == null)
                {
                    return hotels;
                }
                else
                {
                    return orderByQuery.ToString() switch
                    {
                        "name" => hotels.OrderBy(h => h.Name),
                        "Rating" => hotels.OrderBy(h => h.Rating),
                        "Price" => hotels.OrderBy(h => h.Price),
                        _ => hotels,
                    };
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"{ex.Source}/n {ex.Message}");
            }
        }

        public async Task<HotelReview> GetReviewAsync(int reviewId, int hotelId)
        {
            try
            {
                var hotel = await GetByIDAsync(hotelId);
                if(hotel == null)
                {
                    throw new ArgumentNullException(nameof(hotelId));
                }
                else
                {
                    var review = hotel.Reviews.FirstOrDefault(r => r.Id == reviewId);
                    if(review == null)
                    {
                        return null;
                    }
                    else
                    {
                        return review;
                    }

                }
            }
            catch(Exception ex)
            {
                throw new Exception($"{ex.Source} /n {ex.Message}");
            }
        }

        public async Task<HotelReview> CreateReviewAsync(HotelReview review)
        {
            try
            {
                var hotel = await _context.Hotels.FindAsync(review.Hotel_Id);
                if(hotel != null)
                {
                    hotel.Reviews.Add(review);
                    await _context.SaveChangesAsync();
                    return review;
                }
                else
                {
                    throw new ArgumentNullException(nameof(review));
                }
            }
            catch( Exception ex )
            {
                throw new Exception($"{ex.Source} /n {ex.Message}");
            }
        }

        public async Task<HotelReview> UpdateReviewAsync(HotelReview review, int reviewId)
        {
            try
            {
                var hotel = await _context.Hotels.FirstOrDefaultAsync(r => r.Id == review.Hotel_Id);
                
                if (hotel != null)
                {
                    var existingReview = hotel.Reviews.FirstOrDefault(r => r.Id == reviewId);
                    if(existingReview == null)
                    {
                        return null;
                    }

                    existingReview.Comment = review.Comment;
                    existingReview.Author = review.Author;
                    existingReview.Hotel_Id = review.Hotel_Id;
                    existingReview.Id = reviewId;

                    hotel.Reviews.Add(review);
                    await _context.SaveChangesAsync();
                    return review;
                }
                else
                {
                    throw new ArgumentNullException(nameof(review));
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Source} /n {ex.Message}");
            }
        }
        public async Task<bool> DeleteReviewAsync(int reviewId, int hotelId)
        {
            try
            {
                var hotel = await GetByIDAsync(hotelId);
                if(hotel == null)
                {
                    throw new ArgumentNullException(nameof(hotelId));
                }
                else
                {
                    var review = hotel.Reviews.FirstOrDefault(r => r.Id == reviewId);
                    if(review == null)
                    {
                        return false;
                    }
                    else
                    {
                        hotel.Reviews.Remove(review);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"{ex.Source} /n {ex.Message}");
            }
        }

        public async Task<IQueryable<HotelReview>> GetAllReviewsAsync(int hotelId)
        {
            try
            {
                var hotel = await GetByIDAsync(hotelId);
                if(hotel != null)
                {
                    var reviews = hotel.Reviews.AsQueryable();
                    return reviews;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"{ex.Source} /n {ex.Message}");
            }
        }
    }

}
