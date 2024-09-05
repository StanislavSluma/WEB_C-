using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_253505_Bekarev.API.Data;
using WEB_253505_Bekarev.API.Services.AnimeService;
using WEB_253505_Bekarev.Domain.Entities;

namespace WEB_253505_Bekarev.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimesController : ControllerBase
    {
        private readonly IAnimeService _animeService;

        public AnimesController(IAnimeService animeService)
        {
            _animeService = animeService;
        }

        // GET: api/Animes
        [HttpGet]
        [Route("{category?}")]
        public async Task<ActionResult<IEnumerable<Anime>>> GetAnimes(string? category, int pageNo = 1, int pageSize = 5)
        {
            return Ok(await _animeService.GetProductListAsync(category, pageNo, pageSize));
        }

        // GET: api/Animes/5
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Anime>> GetAnime(int id)
        {
            throw new NotImplementedException();
        }

        // PUT: api/Animes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnime(int id, Anime anime)
        {
            throw new NotImplementedException();
        }

        // POST: api/Animes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Anime>> PostAnime(Anime anime)
        {
            throw new NotImplementedException();
        }

        // DELETE: api/Animes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnime(int id)
        {
            throw new NotImplementedException();
        }

        private bool AnimeExists(int id)
        {
            throw new NotImplementedException();
        }
    }
}
