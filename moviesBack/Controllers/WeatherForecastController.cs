using Microsoft.AspNetCore.Mvc;

namespace moviesBack.Controllers;

[ApiController]
[Route("[controller]")]
public class MoviesController : ControllerBase
{
    // Lista filmów z przypisanymi unikalnymi Id
    private static readonly List<Movie> Movies = new()
    {
        new Movie { Id = 1, Title = "Incepcja", Director = "Christopher Nolan", Year = 2010, Description = "Zawiła historia o manipulacji snami i szpiegostwie korporacyjnym." },
        new Movie { Id = 2, Title = "Matrix", Director = "The Wachowskis", Year = 1999, Description = "Haker odkrywa, że rzeczywistość to symulacja i dołącza do buntu przeciwko władcom AI." },
        new Movie { Id = 3, Title = "Interstellar", Director = "Christopher Nolan", Year = 2014, Description = "Podróż przez kosmos, by uratować ludzkość przed umierającą Ziemią." },
        new Movie { Id = 4, Title = "Parasite", Director = "Bong Joon-ho", Year = 2019, Description = "Czarna satyra na temat walki klasowej i nierówności społecznych." },
        new Movie { Id = 5, Title = "Ojciec chrzestny", Director = "Francis Ford Coppola", Year = 1972, Description = "Saga o mafii, rodzinie, lojalności i władzy." },
        new Movie { Id = 6, Title = "Mroczny rycerz", Director = "Christopher Nolan", Year = 2008, Description = "Mroczny thriller kryminalny, w którym Batman staje do walki ze swoim największym wrogiem, Jokerem." },
        new Movie { Id = 7, Title = "Pulp Fiction", Director = "Quentin Tarantino", Year = 1994, Description = "Seria przeplatających się opowieści o przestępczości w Los Angeles." },
        new Movie { Id = 8, Title = "Forrest Gump", Director = "Robert Zemeckis", Year = 1994, Description = "Mężczyzna z niskim IQ wpływa na wiele ważnych wydarzeń w historii USA." },
        new Movie { Id = 9, Title = "Fight Club", Director = "David Fincher", Year = 1999, Description = "Bezsenny pracownik biurowy zakłada nielegalny klub walki z handlowcem mydłem." },
        new Movie { Id = 10, Title = "Skazani na Shawshank", Director = "Frank Darabont", Year = 1994, Description = "Dwaj więźniowie nawiązują głęboką przyjaźń, znajdując pocieszenie i ostateczne odkupienie." },
        new Movie { Id = 11, Title = "Władca Pierścieni: Drużyna Pierścienia", Director = "Peter Jackson", Year = 2001, Description = "Młody hobbit wyrusza w niebezpieczną podróż, by zniszczyć potężny pierścień." },
        new Movie { Id = 12, Title = "The Social Network", Director = "David Fincher", Year = 2010, Description = "Historia powstania Facebooka i toczących się wokół niego batalii prawnych." },
        new Movie { Id = 13, Title = "Gladiator", Director = "Ridley Scott", Year = 2000, Description = "Zdradzony generał rzymski szuka zemsty na skorumpowanym cesarzu, który zamordował jego rodzinę." },
        new Movie { Id = 14, Title = "Milczenie owiec", Director = "Jonathan Demme", Year = 1991, Description = "Młoda agentka FBI prosi o pomoc genialnego, ale szalonego psychiatrę, by schwytać seryjnego mordercę." },
        new Movie { Id = 15, Title = "Gwiezdne wojny: Epizod IV - Nowa nadzieja", Director = "George Lucas", Year = 1977, Description = "Młody chłopak z farmy staje się częścią grupy rebeliantów walczących z złem imperium." }

    };

    private readonly ILogger<MoviesController> _logger;

    public MoviesController(ILogger<MoviesController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetMovies")]
    public IActionResult Get()
    {
        _logger.LogInformation("Received request for movies.");

        var result = new
        {
            Movies = Movies,
            TotalMovies = Movies.Count
        };

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var movie = Movies.FirstOrDefault(m => m.Id == id);
        if (movie == null)
        {
            return NotFound(new { Message = "Film not found." });
        }

        Movies.Remove(movie);
        _logger.LogInformation($"Movie with ID '{id}' was deleted.");

        return NoContent();
    }

    [HttpPost]
    public IActionResult Push([FromBody] Movie newMovie)
    {
        if (newMovie == null)
        {
            return BadRequest(new { Message = "Invalid movie data." });
        }

        if (string.IsNullOrWhiteSpace(newMovie.Title))
        {
            return BadRequest(new { Message = "Movie title is required." });
        }

        newMovie.Id = Movies.Max(m => m.Id) + 1;

        Movies.Insert(0, newMovie);

        _logger.LogInformation($"New movie added: {newMovie.Title} (ID: {newMovie.Id})");

        return CreatedAtAction(nameof(Get), new { id = newMovie.Id }, newMovie);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] Movie updatedMovie)
    {
        if (updatedMovie == null)
        {
            return BadRequest(new { Message = "Invalid movie data." });
        }

        var movie = Movies.FirstOrDefault(m => m.Id == id);
        if (movie == null)
        {
            return NotFound(new { Message = "Movie not found." });
        }

        movie.Title = updatedMovie.Title;
        movie.Director = updatedMovie.Director;
        movie.Year = updatedMovie.Year;
        movie.Description = updatedMovie.Description;

        _logger.LogInformation($"Movie with ID '{id}' was updated.");

        return Ok(movie);
    }

    [HttpPost("bulk")]
    public IActionResult PushMovies([FromBody] List<Movie> newMovies)
    {
        if (newMovies == null || !newMovies.Any())
        {
            return BadRequest(new { Message = "Invalid movie data." });
        }

        // Przypisanie nowych ID w kolejności, ignorując przekazane ID
        int nextId = Movies.Max(m => m.Id) + 1;
        foreach (var movie in newMovies)
        {
            if (string.IsNullOrWhiteSpace(movie.Title))
            {
                return BadRequest(new { Message = "Movie title is required." });
            }

            movie.Id = nextId++;
        }

        // Dodanie nowych filmów do listy
        Movies.InsertRange(0, newMovies);

        _logger.LogInformation($"Added {newMovies.Count} new movies.");

        return CreatedAtAction(nameof(Get), new { id = newMovies.First().Id }, newMovies);
    }

}
