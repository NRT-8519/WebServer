using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebServer.Authentication;
using WebServer.Models.ClinicData.Entities;
using WebServer.Models.DTOs;
using WebServer.Models.UserData;
using WebServer.Models.UserData.Relations;
using WebServer.Services;

namespace WebServer.Controllers
{
    [Authorize]
    [Route("api/users/patients")]
    [ApiController]
    public class PatientController : Controller<Patient, PatientBasicDTO, PatientDetailsDTO>
    {
        public PatientController(ILogger<PatientController> logger, IDbService<Patient, PatientBasicDTO, PatientDetailsDTO> service) : base(logger, service)
        {
        }

        [HttpGet("all")]
        [Authorize(Roles = "ADMINISTRATOR,DOCTOR")]
        public override async Task<IActionResult> GetAll()
        {
            var result = await ((PatientService)service).FindAll();
            if (result.Any())
            {
                List<PatientBasicDTO> DTOs = new();
                foreach (var patient in result)
                {
                    DTOs.Add(new PatientBasicDTO
                    {
                        FirstName = patient.PersonalData.FirstName,
                        MiddleName = patient.PersonalData.MiddleName,
                        LastName = patient.PersonalData.LastName,
                        UUID = patient.PatientUUID,
                        AssignedDoctor = new DoctorBasicDTO
                        {
                            FirstName = patient.AssignedDoctor.PersonalData.FirstName,
                            MiddleName = patient.AssignedDoctor.PersonalData.MiddleName,
                            LastName = patient.AssignedDoctor.PersonalData.LastName,
                            UUID = patient.AssignedDoctor.DoctorUUID
                        }
                    });
                }
                logger.LogInformation("Fetched all patients basic information.");
                return Ok(DTOs);
            }
            else
            {
                logger.LogInformation("Patients database empty.");
                return NoContent();
            }
        }

        [HttpGet("all/basic")]
        [Authorize(Roles = "ADMINISTRATOR,DOCTOR")]
        public async Task<IActionResult> GetAllBasic()
        {
            var result = await ((PatientService)service).FindAll();
            if (result.Any())
            {
                List<PatientBasicDTO> DTOs = new();
                foreach (var patient in result)
                {
                    DTOs.Add(new PatientBasicDTO 
                    { 
                        FirstName = patient.PersonalData.FirstName, 
                        MiddleName = patient.PersonalData.MiddleName,
                        LastName = patient.PersonalData.LastName,
                        UUID = patient.PatientUUID,
                        AssignedDoctor = new DoctorBasicDTO
                        {
                            FirstName = patient.AssignedDoctor.PersonalData.FirstName,
                            MiddleName = patient.AssignedDoctor.PersonalData.MiddleName,
                            LastName = patient.AssignedDoctor.PersonalData.LastName,
                            UUID = patient.AssignedDoctor.DoctorUUID
                        }
                    });
                }
                logger.LogInformation("Fetched all patients basic information.");
                return Ok(DTOs);
            }
            else
            {
                logger.LogInformation("Patients database empty.");
                return NoContent();
            }
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "ADMINISTRATOR")]
        public override async Task<IActionResult> GetById(int id)
        {
            var doctor = await ((PatientService)service).FindById(id);
            if (doctor != default)
            {
                return Ok(doctor);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{UUID}")]
        [Authorize(Roles = "ADMINISTRATOR,DOCTOR,PATIENT")]
        public override async Task<IActionResult> GetByUUID(Guid UUID)
        {
            var result = await ((PatientService)service).FindByUUID(UUID);
            
            if (result != default)
            {
                List<EmailDTO> Emails = new();
                List<PhoneNumberDTO> PhoneNumbers = new();

                foreach(UserEmail email in result.Emails)
                {
                    Emails.Add(new EmailDTO { Email = email.Email });
                }

                foreach (UserPhoneNumber phone in result.PhoneNumbers)
                {
                    PhoneNumbers.Add(new PhoneNumberDTO { PhoneNumber = phone.PhoneNumber });
                }

                PatientDetailsDTO patient = new()
                {
                    UUID = result.PatientUUID,
                    FirstName = result.PersonalData.FirstName,
                    MiddleName = result.PersonalData.MiddleName,
                    LastName = result.PersonalData.LastName,
                    Title = result.PersonalData.Title,
                    Emails = Emails,
                    PhoneNumbers = PhoneNumbers,
                    DateOfBirth = result.PersonalData.DateOfBirth,
                    Gender = result.PersonalData.Gender,
                    SSN = result.PersonalData.SSN,
                    PasswordExpiryDate = result.PasswordExpiryDate,
                    IsDisabled = result.IsDisabled,
                    IsExpired = result.IsExpired,
                    AssignedDoctor = new DoctorBasicDTO
                    {
                        FirstName = result.AssignedDoctor.PersonalData.FirstName,
                        MiddleName = result.AssignedDoctor.PersonalData.MiddleName,
                        LastName = result.AssignedDoctor.PersonalData.LastName,
                        UUID = result.AssignedDoctor.DoctorUUID
                    }
                };
                return Ok(patient);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("add")]
        [Authorize(Roles = "ADMINISTRATOR")]
        public override async Task<IActionResult> Add([FromBody] Patient model)
        {
            var user = await ((PatientService)service).Insert(model);

            if (user != 0)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("edit")]
        [Authorize(Roles = "ADMINISTRATOR,DOCTOR")]
        public override async Task<IActionResult> Edit([FromBody] PatientDetailsDTO model)
        {
            var user = await ((PatientService)service).Update(model);

            if (user != 0)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("remove")]
        [Obsolete]
        public override async Task<IActionResult> Remove(PatientDetailsDTO entity)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("remove/{id}")]
        [Obsolete]
        public override async Task<IActionResult> RemoveById(int id)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("remove/{UUID}")]
        [Obsolete]
        public override async Task<IActionResult> RemoveByUUID(Guid UUID)
        {
            throw new NotImplementedException();
        }
    }
}
