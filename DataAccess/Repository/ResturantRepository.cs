using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WakaWaka.API.DataAccessLayer.DataContext;
using WakaWaka.API.DataAccessLayer.Interfaces;
using WakaWaka.API.Models.Restaurant;
using WakaWaka.API.Models.Resturant;

namespace WakaWaka.API.DataAccessLayer.Repository
{
    public class RestaurantRepository : IBaseRepository<Restaurant>, IReviewRepository<RestaurantReview>
    {
        private readonly WakaContext _context;
        public RestaurantRepository(WakaContext context)
        {
            _context = context;
        }
        public async Task<Restaurant> CreateAsync(Restaurant restaurant)
        {
            try
            {
                if (restaurant == null)
                {
                    throw new ArgumentNullException(nameof(restaurant));
                }
                else
                {
                    var restaurantName = await _context.Restaurants.FirstOrDefaultAsync(h => h.Name == restaurant.Name);
                    if (restaurantName != null)
                    {
                        throw new Exception($"Restaurant with name {restaurantName.Name} already exists");
                    }

                    await _context.Restaurants.AddAsync(restaurant);
                    await _context.SaveChangesAsync();
                    return restaurant;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Source}/n {ex.Message}");
            }
        }

        public async Task<Restaurant> GetByIDAsync(int id)
        {
            try
            {
                var restaurant = await _context.Restaurants.Include(r => r.Reviews).FirstOrDefaultAsync(h => h.Id == id);
                if (restaurant == null)
                {
                    throw new Exception($"Restaurant with ID:{id} doesn't exist");
                }
                else
                {
                    return restaurant;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Source}/n {ex.Message}");
            }
        }

        public async Task<Restaurant> UpdateAsync(Restaurant restaurant , int restaurantId)
        {
            try
            {
                if (restaurant == null)
                {
                    throw new ArgumentNullException(nameof(restaurant));
                }
                else
                {
                    var existingResturant = await _context.Restaurants.FirstOrDefaultAsync(h => h.Id == restaurantId);
                    if (existingResturant == null)
                    {
                        throw new ArgumentNullException(nameof(restaurant));
                    }
                    else
                    {
                        var resturantName = await _context.Restaurants.FirstOrDefaultAsync(h => h.Name == restaurant.Name);
                        if (resturantName != null)
                        {
                            throw new Exception($"Restaurant with name {resturantName.Name} already exists");
                        }
                        existingResturant.Id = restaurantId;
                        existingResturant.Name = restaurant.Name;
                        existingResturant.Rating = restaurant.Rating;
                        _context.Restaurants.Update(restaurant);
                        await _context.SaveChangesAsync();
                        return restaurant;
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
                var restaurant = await _context.Restaurants.Include(r => r.Reviews).FirstOrDefaultAsync(h => h.Id == id);
                if (restaurant == null)
                {
                    return false;
                }
                else
                {
                    _context.Restaurants.Remove(restaurant);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Source}/n {ex.Message}");
            }
        }


        public async Task<IQueryable<Restaurant>> GetAllAsync()
        {
            try
            {
                var restaurants = await _context.Restaurants.ToListAsync();
                if (restaurants != null)
                {
                    return restaurants.AsQueryable();
                }
                else
                {
                    throw new Exception("No restaurants exists on the database");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Source}/n {ex.Message}");
            }
        }
        public async Task<IEnumerable<Restaurant>> CreateMultipleAsync(IEnumerable<Restaurant> restaurants)
        {
            try
            {
                if (restaurants == null)
                {
                    throw new ArgumentNullException(nameof(restaurants));
                }
                else
                {
                    foreach (var restaurant in restaurants)
                    {
                        var restaurantName = await _context.Restaurants.FindAsync(restaurant.Name);
                        if (restaurantName != null)
                        {
                            throw new Exception($"Restaurant with name {restaurantName.Name} already exists");
                        }
                        await _context.Restaurants.AddAsync(restaurant);
                        await _context.SaveChangesAsync();
                    }

                    await _context.SaveChangesAsync();
                    return restaurants.ToList();

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Source}/n {ex.Message}");
            }
        }
        public async Task<IQueryable<Restaurant>> GetAllFilteredAsync(Expression<Func<Restaurant, bool>> filterCondition)
        {
            try
            {
                var restaurants = _context.Restaurants.AsQueryable();
                if (filterCondition == null)
                {
                    return restaurants;
                }
                else
                {
                    restaurants = restaurants.Where(filterCondition);
                    return await Task.FromResult(restaurants);

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Source}/n {ex.Message}");
            }
        }

        public async Task<IQueryable<Restaurant>> GetAllPagedAsync(int pageNumber, int pageSize)
        {
            try
            {
                if (pageNumber <= 0 || pageSize <= 0)
                {
                    throw new Exception("Page number and page size must be greater than 0.");
                }

                var restaurants = _context.Restaurants
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .AsQueryable();

                return restaurants;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Source}/n {ex.Message}");
            }
        }

        public async Task<IQueryable<Restaurant>> GetAllSortedAsync(Expression<Func<Restaurant, object>> randomSearchQuery)
        {
            try
            {
                var restaurants = _context.Restaurants.AsQueryable();
                if (randomSearchQuery == null)
                {
                    return restaurants;
                }
                else
                {
                    restaurants = restaurants.OrderBy(randomSearchQuery);
                    return restaurants;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Source}/n {ex.Message}");
            }
        }


        //public async Task<IQueryable<Restaurant>> GetSortedByQueryAsync(Expression<Func<Restaurant, object>> orderByQuery)
        //{
        //    try
        //    {
        //        var restaurants = _context.Restaurants.AsQueryable();
        //        if (orderByQuery == null)
        //        {
        //            return restaurants;
        //        }
        //        else
        //        {
        //            return orderByQuery.ToString() switch
        //            {
        //                "name" => restaurants.OrderBy(h => h.Name),
        //                "Rating" => restaurants.OrderBy(h => h.Rating),
        //                "Price" => restaurants.OrderBy(h => h.Price),
        //                _ => restaurants,
        //            };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"{ex.Source}/n {ex.Message}");
        //    }
        //}

        public async Task<RestaurantReview> GetReviewByIdAsync(int reviewId, int restaurantId)
        {
            try
            {
                var restaurant = await GetByIDAsync(restaurantId);
                if (restaurant == null)
                {
                    throw new ArgumentNullException(nameof(restaurantId));
                }
                else
                {
                    var review = restaurant.Reviews.FirstOrDefault(r => r.Id == reviewId);
                    if (review == null)
                    {
                        return null;
                    }
                    else
                    {
                        return review;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Source} /n {ex.Message}");
            }
        }

        public async Task<RestaurantReview> CreateReviewAsync(RestaurantReview review)
        {
            try
            {
                var restaurant = await _context.Restaurants.FindAsync(review.Restaurant_Id);
                if (restaurant != null)
                {
                    restaurant.Reviews.Add(review);
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

        public async Task<RestaurantReview> UpdateReviewAsync(RestaurantReview review, int reviewId)
        {
            try
            {
                var restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.Id == review.Restaurant_Id);

                if (restaurant != null)
                {
                    var existingReview = restaurant.Reviews.FirstOrDefault(r => r.Id == reviewId);
                    if (existingReview == null)
                    {
                        return null;
                    }

                    existingReview.Comment = review.Comment;
                    existingReview.Author = review.Author;
                    existingReview.Restaurant_Id = review.Restaurant_Id;
                    existingReview.Id = reviewId;

                    restaurant.Reviews.Add(review);
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
        public async Task<bool> DeleteReviewAsync(int reviewId, int restaurantId)
        {
            try
            {
                var restaurant = await GetByIDAsync(restaurantId);
                if (restaurant == null)
                {
                    throw new ArgumentNullException(nameof(restaurantId));
                }
                else
                {
                    var review = restaurant.Reviews.FirstOrDefault(r => r.Id == reviewId);
                    if (review == null)
                    {
                        return false;
                    }
                    else
                    {
                        restaurant.Reviews.Remove(review);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Source} /n {ex.Message}");
            }
        }

        public async Task<IQueryable<RestaurantReview>> GetAllReviewsAsync(int restaurantId)
        {
            try
            {
                var restaurant = await GetByIDAsync(restaurantId);
                if (restaurant != null)
                {
                    var reviews = restaurant.Reviews.AsQueryable();
                    return reviews;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Source} /n {ex.Message}");
            }
        }
    }

}
