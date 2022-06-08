using API.Data;
using API.Entity.Monitoreo;
using API.Smart_Heart.Models;
using API.Smart_Heart.Models.Monitoreo.RecordECG;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Smart_Heart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordECGsController : ControllerBase
    {
        private readonly CosmosContextSistema _contextCosmos;
        private readonly DBContextSistema _contextSQL;
        private readonly IConfiguration _config;

        public RecordECGsController(CosmosContextSistema contextCosmos, DBContextSistema contextSQL, IConfiguration config)
        {
            _config = config;
            _contextCosmos = contextCosmos;
            _contextSQL = contextSQL;

        }

        // GET: api/RecordEcgs/1
        [Authorize(Roles = "Médico")]
        [HttpGet("{idPaciente}")]
        public async Task<IEnumerable<RecordECGViewModelApp>> Listar([FromRoute] int idPaciente)
        {

            var paciente = await _contextSQL.Pacientes.FirstOrDefaultAsync(c => c.Id_paciente == idPaciente);

            if(paciente == null)
            {
                return Enumerable.Empty<RecordECGViewModelApp>();
            }

            var usuario = await _contextSQL.Usuarios.FirstOrDefaultAsync(c => c.idusuario == paciente.idusuario);

            var recordsECG = await _contextCosmos.RecordsECG.WithPartitionKey(usuario.idusuario.ToString()).ToListAsync();//paciente.idusuario.ToString()).ToList();

            var recordsECGClean = recordsECG.Where(c => c.data != null).ToList();

            //var recordsECG = _contextCosmos.RecordsECG.ToList();
            var tempRecords = recordsECGClean.Select(x => new RecordECGViewModelApp
            {
                id = x.id,
                userID = x.userID,
                readDate = DateTime.Parse(x.readDate),
                data = FormatDataECG(x.data),
                labelResult = labelResults(x.countNSR, x.countWindow),
                subLabel = sublabelResults(x.countNSR, x.countARR, x.countCHF, x.countWindow),
                type = x.type,
                commentUser = x.commentUser

            });

            return tempRecords.Reverse();
        }

        //[HttpGet("[action]")]
        //[Authorize(Roles = "Médico")]
        //public async Task<IEnumerable<RecordECGViewModel>> VisualizarECG([FromRoute] int idPaciente) { }

        private List<DataECGViewModel> FormatDataECG(List<DataECG> dataECG)
        {
            List<DataECGViewModel> listDataClean = new List<DataECGViewModel>();
            DataECGViewModel dataItem;

            foreach (var item in dataECG)
            {
                dataItem = new DataECGViewModel();
                dataItem.result = item.result;
                dataItem.labelResult = item.result;
                dataItem.order = item.order;

                List<double> dataECGfloat = new List<double>();

                foreach (var item2 in item.dataECG)
                {
                    dataECGfloat.Add(item2 / 1000.0);
                }

                dataItem.dataECG = dataECGfloat;
                    //dataECG1.ToList().ForEach(c => c /= 100) as List<float>;

                listDataClean.Add(dataItem);
            }

            return listDataClean;
        }

        private List<double> FormatDataECG2(List<DataECG> dataECG)
        {
            List<double> listDataClean = new List<double>();
          
            foreach (var item in dataECG)
            {

                foreach (var item2 in item.dataECG)
                {
                    listDataClean.Add(item2 / 1000.0);
                }

                //dataECG1.ToList().ForEach(c => c /= 100) as List<float>;

            }

            return listDataClean;
        }


        // GET: api/RecordEcgs/mobile
        [Authorize(Roles = "Paciente")]
        [HttpGet("mobile")]
        public async Task<IEnumerable<RecordECGViewModelApp>> ListarECGMobile()
        {
            var idusuario = HelperController.readToken(HttpContext, _config["Jwt:Key"]);

            var recordsECG = await _contextCosmos.RecordsECG.WithPartitionKey(idusuario.ToString()).ToListAsync();//paciente.idusuario.ToString()).ToList();

            var recordsECGClean = recordsECG.Where(c => c.data != null).ToList();

            var salida = "";
            //var recordsECG = _contextCosmos.RecordsECG.ToList();
            
            var records = recordsECGClean.Select(x => new RecordECGViewModelApp
            {
                id = x.id,
                userID = x.userID,
                readDate = DateTime.Parse(x.readDate),
                data = FormatDataECGSub(x.data, x.countNSR, x.countWindow),
                labelResult = labelResults(x.countNSR,x.countWindow),
                subLabel = sublabelResults(x.countNSR, x.countARR, x.countCHF, x.countWindow),
                //type = x.type,
                //commentUser = x.commentUser
            });

            return records.Reverse();
        }

        // GET: api/RecordEcgs/mobile
        [Authorize(Roles = "Paciente")]
        [HttpPost("mobile/[action]")]
        public async Task<ActionResult<IEnumerable<RecordECGViewModelApp>>> FilterDayECGMobile([FromBody] DateFilterModel dateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //var idusuario = 11;
            var idusuario = HelperController.readToken(HttpContext, _config["Jwt:Key"]);

            var recordsECG = await _contextCosmos.RecordsECG.WithPartitionKey(idusuario.ToString()).ToListAsync();

            var recordsECGClean = recordsECG.Where(c => c.data != null && DateTime.Parse(c.readDate).ToShortDateString() == dateModel.datefilter.ToShortDateString()).ToList();

            var records = recordsECGClean.Select(x => new RecordECGViewModelApp
            {
                id = x.id,
                userID = x.userID,
                readDate = DateTime.Parse(x.readDate),
                data = FormatDataECGSub(x.data, x.countNSR, x.countWindow),
                labelResult = labelResults(x.countNSR, x.countWindow),
                subLabel = sublabelResults(x.countNSR, x.countARR, x.countCHF, x.countWindow),
                //type = x.type,
                //commentUser = x.commentUser
            });

            return Ok(records.Reverse());
        }

        [Authorize(Roles = "Paciente")]
        [HttpGet("mobile/[action]")]
        public async Task<List<DateResultModel>> datesAbnormalities()
        {
            //var idusuario = 12;
            var idusuario = HelperController.readToken(HttpContext, _config["Jwt:Key"]);

            var recordsECG = await _contextCosmos.RecordsECG.WithPartitionKey(idusuario.ToString()).ToListAsync();//paciente.idusuario.ToString()).ToList();

            var recordsECGClean = recordsECG.Where(c => c.data != null & c.countWindow != 0).ToList();

            //var recordsECG = _contextCosmos.RecordsECG.ToList();

            var records = recordsECGClean.Select(x => new RecordECGFilterRawModel
            {
                id = x.id,
                userID = x.userID,
                readDate = DateTime.Parse(x.readDate).ToShortDateString(),
                labelResult = labelResults(x.countNSR, x.countWindow),
                subLabel = sublabelResults(x.countNSR, x.countARR, x.countCHF, x.countWindow),
                type = x.type,
            });

            var listDates = records.Select(s => s.readDate).Distinct().ToList();
            
            List<DateResultModel> listDateResultModel = new List<DateResultModel>();
            DateResultModel dateResultModel;

            foreach (var datefilter in listDates)
            {
                dateResultModel = new DateResultModel
                {
                    dateResult = datefilter.ToString(),
                    isAbnormal = ExistAnormal(records, datefilter.ToString())
                };
                listDateResultModel.Add(dateResultModel);
            }

            return listDateResultModel;
        }

        private bool ExistAnormal(IEnumerable<RecordECGFilterRawModel> recordECGFilterRaws, string datefilter)
        {
            var records = recordECGFilterRaws;

            var demo = records.Where(c => c.readDate == datefilter && c.labelResult == "Resultados Anormales").ToList();

            if (demo.Count > 0)
            {
                return true;
            }

            return false;
        }

        // GET: api/RecordEcgs/mobile
        [Authorize(Roles = "Paciente")]
        [HttpGet("mobile/{idRecord}")]
        public async Task<RecordOneECGViewModelApp> ReadRecordMobile([FromRoute] string idRecord)
        {
            //var idusuario = HelperController.readToken(HttpContext, _config["Jwt:Key"]);

            var recordECG = await _contextCosmos.RecordsECG.FirstOrDefaultAsync(r => r.id == idRecord);//paciente.idusuario.ToString()).ToList();

            //var recordsECGClean = recordECG.(c => c.data != null).ToList();

            //var recordsECG = _contextCosmos.RecordsECG.ToList();
            
            return new RecordOneECGViewModelApp
            {
                id = recordECG.id,
                userID = recordECG.userID,
                readDate = DateTime.Parse(recordECG.readDate),
                data = FormatDataECG2(recordECG.data),
                labelResult = labelResults(recordECG.countNSR, recordECG.countWindow),
                subLabel = sublabelResults(recordECG.countNSR, recordECG.countARR, recordECG.countCHF, recordECG.countWindow),
                type = recordECG.type,
                commentUser = recordECG.commentUser,
            };


        }

        private List<DataECGViewModel> FormatDataECGSub(List<DataECG> dataECG,int countNSR, int countWindow)
        {
            List<DataECGViewModel> listDataClean = new List<DataECGViewModel>();
            DataECGViewModel dataItem;
            int index = 0;
            int indexa = 0;
            int count = 0;
            foreach (var item in dataECG)
            {
                count++;
                dataItem = new DataECGViewModel();
                dataItem.result = item.result;
                dataItem.labelResult = item.result;
                dataItem.order = item.order;

                List<double> dataECGfloat = new List<double>();

                foreach (var item2 in item.dataECG)
                {
                    dataECGfloat.Add(item2 / 1000.0);
                }

                dataItem.dataECG = dataECGfloat;
                
                listDataClean.Add(dataItem);

                if (indexa == 1)
                {
                    break;
                }

                if (conditionNormal(countNSR, countWindow))
                {
                    index++;
                }
                else 
                {
                    listDataClean = new List<DataECGViewModel>();
                    listDataClean.Add(dataItem);
                    indexa++;
                }


                if (index == 2)
                {
                    break;
                }

            }


            return listDataClean;
        }

        private bool conditionNormal(int countNSR, int countWindow)
        {
            double _countNSR = countNSR;
            double _countWindow = countWindow;

            if (_countNSR > _countWindow * 0.8)
            {
                return true;
            }

            return false;
        }

        private string labelResults(int countNSR, int countWindow)
        {
            double _countNSR = countNSR;
            double _countWindow = countWindow;

            if (_countNSR > _countWindow * 0.8)
            {
                return "Resultados Estables";
            }

            return "Resultados Anormales";
        }

        private string sublabelResults(int countNSR, int countARR,int countCHF ,int countWindow)
        {
         
            if (countNSR > countWindow * 0.8)
            {
                return "Ritmo Sinusal Normal";
            }
            if (countARR > countWindow * 0.8)
            {
                return "Ritmo Arritmico";
            } 
            if (countCHF > countWindow * 0.8)
            {
                return "Problema de Insuficiencia Cardíaca";
            }

            return "Ritmo variado";
        }


    }
}
