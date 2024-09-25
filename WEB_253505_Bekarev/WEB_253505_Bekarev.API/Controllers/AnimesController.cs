using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_253505_Bekarev.API.Data;
using WEB_253505_Bekarev.API.Services.AnimeService;
using WEB_253505_Bekarev.Domain.Entities;
using WEB_253505_Bekarev.Domain.Models;

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
        public async Task<ActionResult<ResponseData<ListModel<Anime>>>> GetAnimes(string? category, int pageNo = 1, int pageSize = 5)
        {
            return Ok(await _animeService.GetProductListAsync(category, pageNo, pageSize));
        }

        // GET: api/Animes/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ResponseData<Anime>>> GetAnime(int id)
        {
            return Ok(await _animeService.GetProductByIdAsync(id));
        }

        // PUT: api/Animes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> PutAnime(int id, Anime anime)
        {
            await _animeService.UpdateProductAsync(id, anime);
            return NoContent();
        }

        // POST: api/Animes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = "admin")]
        public async Task<ActionResult<ResponseData<Anime>>> PostAnime(Anime anime)
        {
            return Ok(await _animeService.CreateProductAsync(anime));
        }

        // DELETE: api/Animes/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> DeleteAnime(int id)
        {
            await _animeService.DeleteProductAsync(id);
            return NoContent();
        }

        private bool AnimeExists(int id)
        {
            throw new NotImplementedException();
        }
    }
}
