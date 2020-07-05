using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelFinder.Business.Abstract;
using HotelFinder.Business.Concrete;
using HotelFinder.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelFinder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private IHotelService _hotelService;
        public HotelsController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var getHotels = await _hotelService.GetAllHotels();
            return Ok(getHotels);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetHotelById(int id)
        {
            var getByHotel = await _hotelService.GetHotelById(id);
            if (getByHotel != null)
            { //otel bulunmuşssa 200 dondur
                return Ok(getByHotel);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetHotelByName(string name)
        {

            var hotelName = await _hotelService.GetHotelByName(name);
            if (hotelName != null)
            {
                return Ok(hotelName);
            }
            return NotFound();

        }

        //Create Hotel
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                var hotelPost = await _hotelService.CreateHotel(hotel);
                return CreatedAtAction("Get", new { id = hotelPost.Id }, hotelPost); //Dönen kısmın header kısmında oluşturulan otelın hangi url de oldugunu belirtmek icin CreatedAtAction kullandık
            }
            return BadRequest(ModelState); //400+ validation error 
        }
        //Update Hotel
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Hotel hotel)
        {
            if (await _hotelService.GetHotelById(hotel.Id) != null)
            {
                return Ok(await _hotelService.UpdateHotel(hotel));
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _hotelService.GetHotelById(id) != null)
            {
                await _hotelService.DeleteHotel(id);
                return Ok();

            }
            return NotFound();
        }
    }
}
