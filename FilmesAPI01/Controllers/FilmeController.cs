using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;
using Microsoft.AspNetCore.Mvc;


namespace FilmesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{
    private FilmeContext _context;
    private IMapper _mapper;

    public FilmeController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AdicionaFilme([FromBody] CreateFilmeDto filmeDto)
    {
        Filme filme = _mapper.Map<Filme>(filmeDto);
        _context.Filmes.Add(filme);
        _context.SaveChanges();
        return CreatedAtAction(nameof(RecuperaFilmePorId), new { id = filme.Id }, filme);
    }

    [HttpGet]
    public IEnumerable<Filme> RecuperaFilmes(int skip = 0, int take = 50)
    {
        return _context.Filmes.Skip(skip).Take(take);

    }

    [HttpGet("{id}")]
    public IActionResult RecuperaFilmePorId(int id)
    {
        Filme filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme != null) return NotFound();
        {
          ReadFilmeDto filmeDto = _mapper.Map<ReadFilmeDto>(filme);  
          return Ok(filmeDto);
        }
        return NotFound();
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult AtualizaFilme(int id, [FromBody] UpdateFilmeDto filmeDto)
    {
        Filme filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme != null)
        {
            _mapper.Map(filmeDto, filme);
            _context.SaveChanges();
            return NoContent();
        }
        return NotFound();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeletaFilme(int id)
    {
        Filme filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme != null)
        {
            _context.Remove(filme);
            _context.SaveChanges();
            return NoContent();
        }
        return NotFound();
    }

}
