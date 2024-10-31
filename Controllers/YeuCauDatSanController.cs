﻿using Azure.Messaging;
using Microsoft.AspNetCore.Mvc;
using QLSanBong_API.Models; // Namespace chứa các lớp Models
using QLSanBong_API.Services.IService; // Namespace chứa các interface dịch vụ
using System.Collections.Generic;

namespace QLSanBong_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class YeuCauDatSanController : ControllerBase
    {
        private readonly IYeuCauDatSanService _yeuCauDatSanService;

        public YeuCauDatSanController(IYeuCauDatSanService yeuCauDatSanService)
        {
            _yeuCauDatSanService = yeuCauDatSanService;
        }

        [HttpGet("getall")]
        public ActionResult<IEnumerable<YeuCauDatSan>> GetAll()
        {
            var result = _yeuCauDatSanService.GetAll();
            return Ok(result);
        }

        [HttpGet("getbyid")]
        public ActionResult<YeuCauDatSan> GetById([FromQuery]int id)
        {
            var result = _yeuCauDatSanService.GetByID(id);
            if (result == null)
            {
                return NotFound($"Yêu cầu đặt sân với ID {id} không tồn tại.");
            }
            return Ok(result);
        }

        [HttpPost("getby")]
        public ActionResult<YeuCauDatSan> GetBy([FromBody] GetYCDSRequest request)
        {
            var result = _yeuCauDatSanService.GetBy(request);
            if (result == null)
            {
                return NotFound($"Yêu cầu đặt sân với ID {request.Id} không tồn tại.");
            }
            return Ok(result);
        }


        [HttpPost("add")]
        public ActionResult Add([FromBody] YeuCauDatSanAdd yeuCauDatSanAdd)
        {
            // Gọi hàm Add từ service
            _yeuCauDatSanService.Add(yeuCauDatSanAdd);
            return CreatedAtAction(nameof(GetById), yeuCauDatSanAdd);
        }

        [HttpPut("update")]
        public ActionResult Update([FromBody] UpdateYCDSRequest request)
        {
            try
            {
                // Gọi hàm Update từ service
                _yeuCauDatSanService.Update(request);
                return Ok("Success");
            }
            catch (Exception ex) // Thay KeyNotFoundException bằng Exception để bắt tất cả các loại lỗi
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("delete")]
        public ActionResult Delete([FromQuery] int id)
        {
            try
            {
                _yeuCauDatSanService.Delete(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
