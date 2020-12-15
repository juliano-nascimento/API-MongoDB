using System;
using Api.Data.Collections;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfectadoController : ControllerBase
    {
        Data.MongoDB _mongoDB;

        IMongoCollection<Infectado> _infectadoCollection;

        public InfectadoController(Data.MongoDB mongoDB)
        {
            _mongoDB = mongoDB;
            _infectadoCollection = _mongoDB.DB.GetCollection<Infectado>(typeof(Infectado).Name.ToLower());
        }

        [HttpPost]
        public ActionResult SalvarInfectado([FromBody] InfectadoDto dto)
        {
            var infectado = new Infectado(dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);

            _infectadoCollection.InsertOne(infectado);

            return StatusCode(201, "Infectado Adicionado com sucesso. ");
        }

        [HttpGet]
        public ActionResult ObterInfectados()
        {
            var infectados = _infectadoCollection.Find(Builders<Infectado>.Filter.Empty).ToList();
            return Ok(infectados);
        }

        [HttpPut]
        public ActionResult AtualizarInfectado([FromBody] InfectadoDto dto)
        {
            _infectadoCollection.UpdateOne(Builders<Infectado>.Filter.Where(i => i.DataNascimento == dto.DataNascimento), Builders<Infectado>.Update.Set("Sexo", dto.Sexo));

            return Ok("Atualizado com sucesso. ");
        }

        [HttpDelete("{DataNasc}")]
        public ActionResult DeletarInfectado(DateTime dataNasc)
        {
            _infectadoCollection.DeleteOne(Builders<Infectado>.Filter.Where(i => i.DataNascimento == dataNasc));

            return Ok("Deletado com sucesso. ");
        }
    }
}