using System.Numerics;
using IMDB_Movie_API.Data;
using IMDB_Movie_API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace IMDB_Movie_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly DataContext _context;
        public MoviesController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("Create")]
        public IActionResult CreateMovies(Movie _movie)
        {
            try
            {
                var isMovieExist = _context.Movies.Where(c => c.Title == _movie.Title && c.DateOfRelease == _movie.DateOfRelease).ToList().Count > 0;
                if (isMovieExist)
                    return Ok("Already Exist");
                if(String.IsNullOrEmpty(_movie.Title) || _movie.DateOfRelease == null)
                    return Ok("Can not update");
                var producer = _movie.Producer;
                var isProducerExist = _context.Producers.Where(x => x.ProducerId == producer).ToList().Count > 0;
                if (!isProducerExist)
                    return NotFound("Producer does not exist");

                var actors = _movie.Actors;
                var notExistActors = actors.Select(x => x.actorId).ToList().Except(_context.Actors.Select(x => x.ActorId).ToList()).ToList();
                if(notExistActors.Any())
                    return NotFound(string.Format("Actors with Id({0}) does not exist", string.Join(", ", notExistActors)));

                var assignedActors = _movie.Actors;
                _movie.Actors = new List<ActorMovies>();
                foreach (var assignedActor in assignedActors)
                {
                    _movie.Actors.Add(new ActorMovies
                    {
                        actorId = assignedActor.actorId,
                        movieId = assignedActor.movieId,    
                    });
                }
                
                _context.Movies.Add(_movie);
                _context.SaveChanges();

                return Ok(GetDataById(_movie.MovieId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("Create/Actor")]
        public IActionResult CreateActor(Actor _actor)
        {
            try
            {
                if(_actor.ActorName.All(x=> Char.IsLetter(x)))
                    return BadRequest("Please enter valid actor name");
                var gender = new String[3] { "male", "female", "others" };
                if (!gender.Contains(_actor.Gender.ToLower()))
                    return BadRequest(string.Format("Please enter valid gender name({0})", string.Join(", ", gender)));
                var isActorExist = _context.Actors.Where(c => c.DOB == _actor.DOB &&
                                                            c.ActorName == _actor.ActorName &&
                                                            c.Gender == _actor.Gender).ToList().Count > 0;
                if (isActorExist)
                    return Ok("Already Exist");
                _context.Actors.Add(_actor);
                _context.SaveChanges();

                return Ok(_actor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("Create/Producer")]
        public IActionResult CreateProducer(Producer _producer)
        {
            try
            {
                if (_producer.ProducerName.All(x => Char.IsLetter(x)))
                    return BadRequest("Please enter valid producer name");
                var isProducerExist = _context.Producers.Where(c => c.ProducerName == _producer.ProducerName).ToList().Count > 0;
                if (isProducerExist)
                    return Ok("Already Exist");
                _context.Producers.Add(_producer);
                _context.SaveChanges();

                return Ok(_producer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("Update/{movieId}")]
        public IActionResult UpdateMovie(long movieId, Movie movieDetails)
        {
            try
            {
                var _movie = _context.Movies.Where(x => x.MovieId == movieId).FirstOrDefault();
               _movie.Title = String.IsNullOrEmpty(movieDetails.Title) ? _movie.Title : movieDetails.Title;
                _movie.DateOfRelease = movieDetails.DateOfRelease == null ? _movie.DateOfRelease : (DateOnly)movieDetails.DateOfRelease;
                _movie.Producer = movieDetails.Producer == 0 ? _movie.Producer : (long)movieDetails.Producer;
                _movie.Actors = movieDetails.Actors == null ? _movie.Actors : movieDetails.Actors;
                var producer = _movie.Producer;
                var isProducerExist = _context.Producers.Where(x => x.ProducerId == producer).ToList().Count > 0;
                if (!isProducerExist)
                    return NotFound("Producer does not exist");

                var actorsList = new List<ActorMovies>(_movie.Actors);
                var notExistActors = actorsList.Select(x => x.actorId).ToList().Except(_context.Actors.Select(x => x.ActorId).ToList()).ToList();
                if (notExistActors.Any())
                    return NotFound(string.Format("Actors with Id({0}) does not exist", string.Join(", ", notExistActors)));
                _movie.Actors.Clear();
                 var newActors = actorsList.Select(x => x.actorId).ToList().Except(_context.ActorMovies.Where(x=>x.movieId == movieId).Select(x => x.actorId).ToList());
                foreach (var assignedActor in newActors)
                {
                    _movie.Actors.Add(new ActorMovies
                    {
                        actorId = assignedActor,
                        movieId = movieId,
                    });
                }
                _context.Movies.Attach(_movie);
                _context.Entry(_movie).State = EntityState.Modified;
                _context.SaveChanges();
                return Ok(GetDataById(movieId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        public List<dynamic> GetDataById(long movieId)
        {
            try
            {
                var lst = new List<dynamic>();
                var data = _context.Movies.Where(x => x.MovieId == movieId).ToList();
                var producerData = _context.Producers.ToList();
                var actorsData = _context.Actors.ToList();
                var actorMoviesData = _context.ActorMovies.Where(x => x.movieId == movieId).ToList();

                foreach (var item in data)
                {
                    lst.Add(new
                    {
                        Title = item.Title,
                        DateOfRelease = item.DateOfRelease,
                        Actor = actorsData.Select(x => actorMoviesData.Where(p => p.actorId == x.ActorId).Select(y => new { ActorId = x.ActorId, ActorName = x.ActorName })),
                        producerId = producerData.Where(prop => prop.ProducerId == item.Producer).FirstOrDefault(),
                    });
                }
                return lst;
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public IActionResult GetAllMovies()
        {
            try
            {
                var lst = new List<dynamic>();
                var data = _context.Movies.ToList(); 
                    var producerData = _context.Producers.ToList();
                var actorsData = _context.Actors.ToList();
                var actorMoviesData = _context.ActorMovies.ToList();

                foreach (var item in data)
                {
                    lst.Add(new
                    {
                        Title = item.Title,
                        DateOfRelease = item.DateOfRelease,
                        Actor = actorsData.Select(x=>actorMoviesData.Where(p=>p.actorId == x.ActorId).Select(y => new { ActorId=x.ActorId, ActorName = x.ActorName})),
                        producerId = producerData.Where(prop=>prop.ProducerId == item.Producer).FirstOrDefault(),
                    });
                }
                return Ok(lst);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
